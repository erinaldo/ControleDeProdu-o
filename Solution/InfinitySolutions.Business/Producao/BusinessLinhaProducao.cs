using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using InfinitySolutions.Data;
using InfinitySolutions.Entity;
using InfinitySolutions.Commom;
using InfinitySolutions.Commom.Dtos;
using System.Transactions;

namespace InfinitySolutions.Business
{

    public class BusinessLinhaProducao : BusinessBase
    {
        public const int HORA_INICIO_TRABALHO = 7;
        public const int MINUTO_INICIO_TRABALHO = 20;
        public const int QUANTIDADE_HORAS_TRABALHO_DIA = 9;
        public const int HORA_FIM_TRABALHO = 17;

        public const int HORA_ALMOÇO = 12;
        public const int QUANTIDADE_HORAS_ALMOÇO = 1;

        #region AÇÕES

        public Retorno Salvar(LinhaProducao Entity)
        {
            try
            {
                Retorno retorno = PreenchimentoObrigatorio(Entity);
                if (retorno.IsValido)
                {
                    if (Entity.Codigo == 0)
                        retorno = new DataLinhaProducao().Incluir(Entity);
                    else
                        retorno = new DataLinhaProducao().Alterar(Entity);
                }
                return retorno;
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Terceirizar(LinhaProducao linhaProducao)
        {
            try
            {
                return new DataLinhaProducao().Terceirizar(linhaProducao);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Listar(DateTime dataInicio, DateTime dataFim)
        {
            try
            {
                var retorno = new DataLinhaProducao().Listar(dataInicio, dataFim);

                if (retorno.IsValido)
                {
                    retorno.Entity = CriarExibicaoLinhaProducao(retorno.Entity as List<LinhaProducao>);
                }

                return retorno;
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Excluir(LinhaProducao Entity)
        {
            try
            {
                return new DataLinhaProducao().Excluir(Entity);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno MontarLinhaProducaoPedido(Pedido pedido, bool adicionarLinhaProducao, List<LinhaProducao> linhaProducaoTerceirizados)
        {
            try
            {
                var retorno = ListarFuncionariosComDataDisponivel();

                if (retorno.IsValido)
                {
                    var funcionarios = retorno.Entity as List<Funcionario>;
                    var dataInicioPedido = VerificarDataInicioPedido(funcionarios);
                    var dataUltimaProducao = dataInicioPedido;

                    if (linhaProducaoTerceirizados != null)
                        pedido.Produtos = pedido.Produtos.Where(p => !linhaProducaoTerceirizados.Any(pt => pt.Produto.CodigoPedidoProduto == p.CodigoPedidoProduto)).ToList();

                    foreach (var produto in pedido.Produtos)
                    {
                        retorno = new BusinessProduto().ConsultarTempoProducaoHoraFuncionario(produto);

                        if (retorno.IsValido)
                        {
                            produto.QuantidadeProducaoHoraFuncionario = retorno.Entity.ConverteValor(0M);
                            var totalHorasProduto = Math.Round((produto.Quantidade / produto.QuantidadeProducaoHoraFuncionario).ConverteValor(0D), 1);

                            while (Math.Round(totalHorasProduto, 1) > 0)
                            {
                                var funcionariosLivresNoDia = funcionarios.Where(f => f.LinhasProducao.Any(l => l.DataPrevisaoFim.Date <= dataUltimaProducao.Date)).ToList();
                                var horasRestanteDiaPorFuncionario = DefinirHorasRestanteDiaPorFuncionario(totalHorasProduto, funcionariosLivresNoDia.Count());
                                var horasDoDia = 0D;

                                foreach (var funcionarioLivreNoDia in funcionariosLivresNoDia)
                                {
                                    horasDoDia = DefinirHorasDoDia(horasRestanteDiaPorFuncionario, dataUltimaProducao, ref totalHorasProduto);
                                    funcionarioLivreNoDia.HorasProducao += horasDoDia;
                                }

                                dataUltimaProducao = DefinirDataFinalProducao(dataUltimaProducao, horasDoDia);
                            }

                            DefinirLinhaProducaoFuncionarios(funcionarios, produto, dataInicioPedido);
                        }
                    }
                    pedido.Funcionarios = funcionarios;
                    var linhasProducao = linhaProducaoTerceirizados;

                    funcionarios.ForEach(f => { linhasProducao.AddRange(f.LinhasProducao.Where(l => l.Produto.Codigo > 0).ToList()); });

                    if (adicionarLinhaProducao)
                        return new BusinessLinhaProducao().Salvar(linhasProducao);
                    else
                    {
                        pedido.LinhaProducaoExibicao = CriarExibicaoLinhaProducao(linhasProducao);
                        retorno.Entity = pedido;
                    }
                }

                return retorno;
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Salvar(List<LinhaProducao> linhasProducao)
        {
            try
            {
                var retorno = new Retorno();
                using (var transaction = new TransactionScope())
                {
                    foreach (var linhaProducao in linhasProducao)
                    {
                        if (linhaProducao.Produto.CodigoPedidoProduto != 0)
                        {
                            if (linhaProducao.EhTerceirizado)
                                linhaProducao.Quantidade = linhaProducao.Produto.Quantidade.Value;

                            retorno = Salvar(linhaProducao);
                            if (!retorno.IsValido)
                                break;
                        }
                    }

                    if (retorno.IsValido)
                        transaction.Complete();
                }

                return retorno;
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        private static LinhaProducaoExibicaoDto CriarExibicaoLinhaProducao(List<LinhaProducao> linhasProducao)
        {
            var linhasProducaoAgrupada = linhasProducao.GroupBy(p => p.Produto.CodigoPedidoProduto).ToList();

            var linhaProducaoExibicaoDto = new LinhaProducaoExibicaoDto();
            linhasProducaoAgrupada.ForEach(la =>
            {
                var linhaProducaoProdutoDto = new LinhaProducaoProdutoDto { Produto = la.FirstOrDefault().Produto };

                la.ToList().ForEach(l => { linhaProducaoProdutoDto.Funcionarios.Add(l.Funcionario); });

                linhaProducaoProdutoDto.Codigo = la.FirstOrDefault().Codigo;
                linhaProducaoProdutoDto.DataPrevisaoInicio = la.FirstOrDefault().DataPrevisaoInicio;
                linhaProducaoProdutoDto.DataPrevisaoFim = la.FirstOrDefault().DataPrevisaoFim;
                linhaProducaoProdutoDto.EhTerceirizado = la.FirstOrDefault().EhTerceirizado;
                linhaProducaoProdutoDto.Terceirizado = la.FirstOrDefault().Terceirizado;
                linhaProducaoProdutoDto.TipoStatusLinhaProducao = la.FirstOrDefault().TipoStatusLinhaProducao;
                linhaProducaoProdutoDto.Pedido = la.FirstOrDefault().Pedido;

                linhaProducaoExibicaoDto.LinhaProducaoProdutos.Add(linhaProducaoProdutoDto);

            });
            return linhaProducaoExibicaoDto;
        }

        private static DateTime DefinirDataFinalProducao(DateTime dataVerificacao, double horasDoDia)
        {
            var passouParaProximoDia = (dataVerificacao.AddHours(horasDoDia).Hour >= HORA_FIM_TRABALHO) || horasDoDia == QUANTIDADE_HORAS_TRABALHO_DIA;

            if (horasDoDia >= QUANTIDADE_HORAS_TRABALHO_DIA || passouParaProximoDia)
                dataVerificacao = dataVerificacao.AddDays(DefinirQuantidadeDiaParaDiaProximoDiaUtil(dataVerificacao));

            if (passouParaProximoDia)
                dataVerificacao = new DateTime(dataVerificacao.Year, dataVerificacao.Month, dataVerificacao.Day, HORA_INICIO_TRABALHO, MINUTO_INICIO_TRABALHO, 0);
            else
            {
                if (horasDoDia == QUANTIDADE_HORAS_TRABALHO_DIA)
                    dataVerificacao = new DateTime(dataVerificacao.Year, dataVerificacao.Month, dataVerificacao.Day, HORA_FIM_TRABALHO, 0, 0);

                dataVerificacao = dataVerificacao.AddHours(horasDoDia);
            }

            return dataVerificacao;
        }

        public Retorno ConfirmarProducao(LinhaProducao linhaProducao)
        {
            try
            {
                return new DataLinhaProducao().ConfirmarProducao(linhaProducao);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        #endregion

        #region CONSULTAS

        public Retorno Listar(int Pagina, int QntPagina)
        {
            try
            {
                return new DataLinhaProducao().Listar(Pagina, QntPagina);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Pesquisar(LinhaProducao Entity, int Pagina, int QntPagina)
        {
            try
            {
                return new DataLinhaProducao().Pesquisar(Entity, Pagina, QntPagina);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        public Retorno Consultar(LinhaProducao Entity)
        {
            try
            {
                return new DataLinhaProducao().Consultar(Entity);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        #endregion

        #region METODOS AUXILIARES

        public Retorno PreenchimentoObrigatorio(LinhaProducao Entity)
        {

            if (Entity.DataPrevisaoInicio == DateTime.MinValue)
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Data Previsao Inicio"));

            if (Entity.DataPrevisaoFim == DateTime.MinValue)
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Data Previsao Fim"));

            if (Entity.Quantidade == 0M && Entity.Produto.Quantidade == 0M)
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Quantidade"));

            if (Entity.Produto.CodigoPedidoProduto == 0)
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Produto do Pedido"));

            if (Entity.DataPrevisaoInicio > Entity.DataPrevisaoFim)
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Data Previsao Inicio não pode ser Maior que a Data Previsao Fim"));

            if (Entity.EhTerceirizado && Entity.Terceirizado.Codigo == 0)
                return new Retorno(false, String.Format(Mensagens.MSG_01, "Colaborador"));

            return new Retorno(true);
        }

        private Retorno VerificarExistencia(LinhaProducao Entity)
        {
            try
            {
                return new DataLinhaProducao().VerificarExistencia(Entity);
            }
            catch (Exception ex)
            {
                return Retorno.CriarRetornoExcecao(ex);
            }
        }

        private static Retorno ListarFuncionariosComDataDisponivel()
        {
            var retorno = new BusinessFuncionario().Listar();

            if (retorno.IsValido)
            {
                var funcionarios = retorno.Entity as List<Funcionario>;

                foreach (var funcionario in funcionarios)
                {
                    retorno = new DataLinhaProducao().ConsultarDataDisponivel(funcionario);

                    if (retorno.IsValido)
                        funcionario.LinhasProducao.Add(retorno.Entity as LinhaProducao);
                    else
                        return retorno;
                }
                return new Retorno(funcionarios);
            }

            return retorno;
        }

        private DateTime VerificarDataInicioPedido(List<Funcionario> funcionarios)
        {
            var dataInicio = DateTime.Now;
            var contemDiaNaoUtil = false;

            if (funcionarios.FirstOrDefault().LinhasProducao.FirstOrDefault().DataPrevisaoFim != DateTime.MinValue && funcionarios.FirstOrDefault().LinhasProducao.FirstOrDefault().DataPrevisaoFim >= DateTime.Now)
                dataInicio = funcionarios.FirstOrDefault().LinhasProducao.FirstOrDefault().DataPrevisaoFim;
            else
            {
                /* TODO: CRIAR CONTROLE DE FERIADOS */
                while (dataInicio.DayOfWeek == DayOfWeek.Saturday || dataInicio.DayOfWeek == DayOfWeek.Sunday)
                {
                    contemDiaNaoUtil = true;
                    dataInicio = dataInicio.AddDays(1);
                }
            }

            if (dataInicio.Hour == HORA_ALMOÇO)
                dataInicio = dataInicio.AddHours(QUANTIDADE_HORAS_ALMOÇO);

            var passouParaProximoDia = dataInicio.Hour >= HORA_FIM_TRABALHO;

            if (passouParaProximoDia)
                dataInicio = dataInicio.AddDays(1);

            if (passouParaProximoDia || contemDiaNaoUtil || dataInicio.Hour < HORA_INICIO_TRABALHO)
                dataInicio = new DateTime(dataInicio.Year, dataInicio.Month, dataInicio.Day, HORA_INICIO_TRABALHO, MINUTO_INICIO_TRABALHO, 0);

            return dataInicio;
        }

        private static double DefinirHorasRestanteDiaPorFuncionario(double totalHorasProduto, int quantidadeFuncionariosLivresNoDia)
        {
            if (totalHorasProduto > (QUANTIDADE_HORAS_TRABALHO_DIA * quantidadeFuncionariosLivresNoDia))
                return QUANTIDADE_HORAS_TRABALHO_DIA;
            else
                return Math.Round(totalHorasProduto / quantidadeFuncionariosLivresNoDia, 1);
        }

        private static double DefinirHorasDoDia(double horasRestantesDoDia, DateTime dataExecucao, ref double totalHorasProduto)
        {
            if (horasRestantesDoDia == 0)
            {
                totalHorasProduto = 0;
                return 0;
            }

            var horasRestantesNoDia = Math.Round((new DateTime(dataExecucao.Year, dataExecucao.Month, dataExecucao.Day, HORA_FIM_TRABALHO, 0, 0) - dataExecucao).TotalMinutes / 60, 1);

            var horasDescontar = horasRestantesNoDia;

            if (horasRestantesNoDia > horasRestantesDoDia || horasRestantesNoDia < 0.1)
                horasDescontar = horasRestantesDoDia;
            else if (horasRestantesNoDia > QUANTIDADE_HORAS_TRABALHO_DIA)
                horasDescontar = QUANTIDADE_HORAS_TRABALHO_DIA;

            totalHorasProduto -= horasDescontar;

            if (dataExecucao.Hour < HORA_ALMOÇO && dataExecucao.AddHours(horasRestantesDoDia).Hour >= HORA_ALMOÇO)
                horasDescontar += QUANTIDADE_HORAS_ALMOÇO;

            return horasDescontar;
        }

        private static int DefinirQuantidadeDiaParaDiaProximoDiaUtil(DateTime dataDistribuicaoHoras)
        {
            var quantidadeParaProximoDiaUtil = 1;

            /* TODO: CRIAR CONTROLE DE FERIADOS */
            while (dataDistribuicaoHoras.AddDays(quantidadeParaProximoDiaUtil).DayOfWeek == DayOfWeek.Sunday
                || dataDistribuicaoHoras.AddDays(quantidadeParaProximoDiaUtil).DayOfWeek == DayOfWeek.Saturday)
                quantidadeParaProximoDiaUtil++;

            return quantidadeParaProximoDiaUtil;
        }

        private void DefinirLinhaProducaoFuncionarios(List<Funcionario> funcionarios, Produto produto, DateTime dataInicioPedido)
        {
            foreach (var funcionario in funcionarios)
            {
                var ultimaLinhaProducao = funcionario.LinhasProducao.LastOrDefault();

                var linhaProducao = new LinhaProducao();
                linhaProducao.Funcionario.Codigo = funcionario.Codigo;
                linhaProducao.Funcionario.Nome = funcionario.Nome;
                linhaProducao.Produto = produto;
                linhaProducao.Quantidade = produto.QuantidadeProducaoHoraFuncionario.Value * funcionario.HorasProducao.ConverteValor(0m);
                linhaProducao.DataPrevisaoInicio = DefinirDataPrevisaoInicio(ultimaLinhaProducao.DataPrevisaoFim != DateTime.MinValue ? ultimaLinhaProducao.DataPrevisaoFim : linhaProducao.DataPrevisaoInicio, dataInicioPedido);
                linhaProducao.DataPrevisaoFim = DefinirDataPrevisaoFim(linhaProducao.DataPrevisaoInicio, funcionario.HorasProducao);

                funcionario.HorasProducao = 0;
                funcionario.LinhasProducao.Add(linhaProducao);
            }
        }

        private DateTime DefinirDataPrevisaoInicio(DateTime dataPrevisaoInicio, DateTime dataInicioPedido)
        {
            if (dataPrevisaoInicio == DateTime.MinValue || dataPrevisaoInicio < DateTime.Now)
                dataPrevisaoInicio = DateTime.Now;

            if (dataPrevisaoInicio.Hour > HORA_FIM_TRABALHO)
                dataPrevisaoInicio = dataPrevisaoInicio.AddDays(DefinirQuantidadeDiaParaDiaProximoDiaUtil(dataPrevisaoInicio));

            if (dataPrevisaoInicio.Hour < HORA_INICIO_TRABALHO || dataPrevisaoInicio.Hour > HORA_FIM_TRABALHO)
                dataPrevisaoInicio = new DateTime(dataPrevisaoInicio.Year, dataPrevisaoInicio.Month, dataPrevisaoInicio.Day, HORA_INICIO_TRABALHO, MINUTO_INICIO_TRABALHO, 0);

            return dataPrevisaoInicio < dataInicioPedido ? dataInicioPedido : dataPrevisaoInicio;
        }

        private DateTime DefinirDataPrevisaoFim(DateTime dataPrevisaoInicio, double horasProducao)
        {
            var dataFimPrevisao = dataPrevisaoInicio;

            while (horasProducao > 0)
            {
                var horasDoDia = DefinirHorasDoDia(horasProducao, dataFimPrevisao, ref horasProducao);
                dataFimPrevisao = DefinirDataFinalProducao(dataFimPrevisao, horasDoDia);
            }

            return dataFimPrevisao;
        }

        #endregion

    }
}

