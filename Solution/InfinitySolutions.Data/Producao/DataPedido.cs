using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using InfinitySolutions.Entity;
using InfinitySolutions.Commom;
using MySql.Data.MySqlClient;
using InfinitySolutions.Entity.Enum;

namespace InfinitySolutions.Data
{
    public class DataPedido : DataBase
    {
        #region AÇÕES

        public Retorno Incluir(Pedido Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("INSERT INTO TB_PEDIDO( ");
                CommandSQL.AppendLine("NUMERO_PEDIDO_CLIENTE, ");
                CommandSQL.AppendLine("NUMERO_PARCELAS, ");
                CommandSQL.AppendLine("OBSERVACAO, ");
                CommandSQL.AppendLine("OBSERVACAO_NOTA_FISCAL, ");
                CommandSQL.AppendLine("VALOR, ");
                CommandSQL.AppendLine("DATA_PREVISAO_ENTREGA, ");
                CommandSQL.AppendLine("DATA_INCLUSAO, ");
                CommandSQL.AppendLine("DESCONTO, ");
                CommandSQL.AppendLine("PORCENTAGEM_LUCRO, ");
                CommandSQL.AppendLine("CODIGO_CLIENTE, ");
                CommandSQL.AppendLine("CODIGO_TABELA_PRECO, ");
                CommandSQL.AppendLine("CODIGO_TIPO_FASE, ");
                CommandSQL.AppendLine("CODIGO_TIPO_FORMA_PAGAMENTO, ");
                CommandSQL.AppendLine("DATA_PAGAMENTO) ");
                CommandSQL.AppendLine("VALUES (");
                CommandSQL.AppendLine("@NUMERO_PEDIDO_CLIENTE, ");
                CommandSQL.AppendLine("@NUMERO_PARCELAS, ");
                CommandSQL.AppendLine("@OBSERVACAO, ");
                CommandSQL.AppendLine("@OBSERVACAO_NOTA_FISCAL, ");
                CommandSQL.AppendLine("@VALOR, ");
                CommandSQL.AppendLine("@DATA_PREVISAO_ENTREGA, ");
                CommandSQL.AppendLine("@DATA_INCLUSAO, ");
                CommandSQL.AppendLine("@DESCONTO, ");
                CommandSQL.AppendLine("@PORCENTAGEM_LUCRO, ");
                CommandSQL.AppendLine("@CODIGO_CLIENTE, ");
                CommandSQL.AppendLine("@CODIGO_TABELA_PRECO, ");
                CommandSQL.AppendLine("@CODIGO_TIPO_FASE, ");
                CommandSQL.AppendLine("@CODIGO_TIPO_FORMA_PAGAMENTO, ");
                CommandSQL.AppendLine("@DATA_PAGAMENTO); ");

                CommandSQL.AppendLine("SELECT LAST_INSERT_ID()");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@NUMERO_PEDIDO_CLIENTE", Entity.NumeroPedidoCliente);
                Command.Parameters.AddWithValue("@NUMERO_PARCELAS", Entity.NumeroParcelas);
                Command.Parameters.AddWithValue("@OBSERVACAO", Entity.Observacao != null ? (object)Entity.Observacao.ToUpper() : DBNull.Value);
                Command.Parameters.AddWithValue("@OBSERVACAO_NOTA_FISCAL", Entity.ObservacaoNotaFiscal != null ? (object)Entity.ObservacaoNotaFiscal.ToUpper() : DBNull.Value);
                Command.Parameters.AddWithValue("@VALOR", Entity.Valor);
                Command.Parameters.AddWithValue("@DATA_PREVISAO_ENTREGA", Entity.DataPrevisaoEntrega);
                Command.Parameters.AddWithValue("@DATA_INCLUSAO", DateTime.Now);
                Command.Parameters.AddWithValue("@DESCONTO", Entity.Desconto.HasValue ? (object)Entity.Desconto.Value : DBNull.Value);
                Command.Parameters.AddWithValue("@PORCENTAGEM_LUCRO", Entity.PorcentagemLucro.HasValue ? (object)Entity.PorcentagemLucro.Value : DBNull.Value);
                Command.Parameters.AddWithValue("@CODIGO_CLIENTE", Entity.Cliente.Codigo);
                Command.Parameters.AddWithValue("@CODIGO_TABELA_PRECO", Entity.TabelaPreco.Codigo);
                Command.Parameters.AddWithValue("@CODIGO_TIPO_FASE", Entity.TipoFase.Codigo);
                Command.Parameters.AddWithValue("@CODIGO_TIPO_FORMA_PAGAMENTO", Entity.TipoFormaPagamento.Codigo);
                Command.Parameters.AddWithValue("@DATA_PAGAMENTO", Entity.DataPagamento.HasValue ? (object)Entity.DataPagamento.Value : DBNull.Value);

                Abrir();
                Entity.Codigo = Command.ExecuteScalar().ConverteValor(0);

                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("INSERT INTO TB_PEDIDO_TIPO_FASE( ");
                CommandSQL.AppendLine("CODIGO_PEDIDO, ");
                CommandSQL.AppendLine("CODIGO_TIPO_FASE, ");
                CommandSQL.AppendLine("DATA) ");
                CommandSQL.AppendLine("VALUES ( ");
                CommandSQL.AppendLine("@CODIGO_PEDIDO, ");
                CommandSQL.AppendLine("@CODIGO_TIPO_FASE, ");
                CommandSQL.AppendLine("@DATA) ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@CODIGO_PEDIDO", Entity.Codigo);
                Command.Parameters.AddWithValue("@CODIGO_TIPO_FASE", Entity.TipoFase.Codigo);
                Command.Parameters.AddWithValue("@DATA", DateTime.Now);

                Command.ExecuteNonQuery();

                return new Retorno(true, String.Format(Mensagens.MSG_02, "Salvo"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno AlterarDataEntrega(Pedido pedido)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("UPDATE TB_PEDIDO SET ");
                CommandSQL.AppendLine("DATA_PREVISAO_ENTREGA = @DATA_PREVISAO_ENTREGA ");
                CommandSQL.AppendLine("WHERE CODIGO = @CODIGO");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@DATA_PREVISAO_ENTREGA", pedido.DataPrevisaoEntrega);
                Abrir();
                Command.ExecuteNonQuery();
                return new Retorno(true, String.Format(Mensagens.MSG_02, "Alterado "));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno ExcluirPedidoProduto(int codigoPedidoProduto)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("DELETE FROM TB_PEDIDO_PRODUTO WHERE CODIGO = @CODIGO");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@CODIGO", codigoPedidoProduto);
                Abrir();
                Command.ExecuteNonQuery();
                return new Retorno(true, String.Format(Mensagens.MSG_02, "Excluido "));
            }
            catch (Exception ex)
            {
                if (((MySqlException)ex).Number == 1451)
                    return new Retorno(false, Mensagens.MSG_16);
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno MudarTipoFase(int codigo, int codigoTipoFase)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("INSERT INTO TB_PEDIDO_TIPO_FASE( ");
                CommandSQL.AppendLine("CODIGO_PEDIDO, ");
                CommandSQL.AppendLine("CODIGO_TIPO_FASE, ");
                CommandSQL.AppendLine("DATA) ");
                CommandSQL.AppendLine("VALUES ( ");
                CommandSQL.AppendLine("@CODIGO_PEDIDO, ");
                CommandSQL.AppendLine("@CODIGO_TIPO_FASE, ");
                CommandSQL.AppendLine("@DATA) ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@CODIGO_PEDIDO", codigo);
                Command.Parameters.AddWithValue("@CODIGO_TIPO_FASE", codigoTipoFase);
                Command.Parameters.AddWithValue("@DATA", DateTime.Now);

                Abrir();
                Command.ExecuteNonQuery();
                return new Retorno(true, String.Format(Mensagens.MSG_02, "Salvo"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno IncluirPedidoProduto(Produto produto, Pedido pedido)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("INSERT INTO TB_PEDIDO_PRODUTO( ");
                CommandSQL.AppendLine("QUANTIDADE, ");
                CommandSQL.AppendLine("VALOR, ");
                CommandSQL.AppendLine("DESCONTO, ");
                CommandSQL.AppendLine("NUMERO_CLIENTE, ");
                CommandSQL.AppendLine("VALOR_UNITARIO, ");
                CommandSQL.AppendLine("CODIGO_PEDIDO, ");
                CommandSQL.AppendLine("CODIGO_PRODUTO, ");
                CommandSQL.AppendLine("CODIGO_TIPO_COR, ");
                CommandSQL.AppendLine("CODIGO_TIPO_TAMANHO, ");
                CommandSQL.AppendLine("NUMERO_PEDIDO_CLIENTE, ");
                CommandSQL.AppendLine("LINHA_PEDIDO_CLIENTE) ");
                CommandSQL.AppendLine("VALUES (");
                CommandSQL.AppendLine("@QUANTIDADE, ");
                CommandSQL.AppendLine("@VALOR, ");
                CommandSQL.AppendLine("@DESCONTO, ");
                CommandSQL.AppendLine("@NUMERO_CLIENTE, ");
                CommandSQL.AppendLine("@VALOR_UNITARIO, ");
                CommandSQL.AppendLine("@CODIGO_PEDIDO, ");
                CommandSQL.AppendLine("@CODIGO_PRODUTO, ");
                CommandSQL.AppendLine("@CODIGO_TIPO_COR, ");
                CommandSQL.AppendLine("@CODIGO_TIPO_TAMANHO, ");
                CommandSQL.AppendLine("@NUMERO_PEDIDO_CLIENTE, ");
                CommandSQL.AppendLine("@LINHA_PEDIDO_CLIENTE); ");

                CommandSQL.AppendLine("SELECT LAST_INSERT_ID()");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@QUANTIDADE", produto.Quantidade);
                Command.Parameters.AddWithValue("@VALOR", produto.Valor);
                Command.Parameters.AddWithValue("@DESCONTO", produto.Desconto.HasValue ? (object)produto.Desconto : DBNull.Value);
                Command.Parameters.AddWithValue("@NUMERO_CLIENTE", produto.NumeroProdutoCliente.HasValue ? (object)produto.NumeroProdutoCliente : DBNull.Value);
                Command.Parameters.AddWithValue("@VALOR_UNITARIO", produto.ValorUnitario.HasValue ? (object)produto.ValorUnitario : DBNull.Value);
                Command.Parameters.AddWithValue("@CODIGO_PEDIDO", pedido.Codigo);
                Command.Parameters.AddWithValue("@CODIGO_PRODUTO", produto.Codigo);
                Command.Parameters.AddWithValue("@CODIGO_TIPO_COR", produto.TipoCor.Codigo);
                Command.Parameters.AddWithValue("@CODIGO_TIPO_TAMANHO", produto.TipoTamanho.Codigo);
                Command.Parameters.AddWithValue("@NUMERO_PEDIDO_CLIENTE", produto.NumeroPedidoCliente);
                Command.Parameters.AddWithValue("@LINHA_PEDIDO_CLIENTE", produto.LinhaPedidoCliente);

                Abrir();
                produto.CodigoPedidoProduto = Command.ExecuteScalar().ConverteValor(0);

                return new Retorno(true, String.Format(Mensagens.MSG_02, "Salvo"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno SalvarDataPagamento(Pedido pedido)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("UPDATE TB_PEDIDO SET ");
                CommandSQL.AppendLine("DATA_PAGAMENTO = @DATA_PAGAMENTO ");
                CommandSQL.AppendLine("WHERE CODIGO = @CODIGO ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@DATA_PAGAMENTO", pedido.DataPagamento);
                Command.Parameters.AddWithValue("@CODIGO", pedido.Codigo);

                Abrir();
                Command.ExecuteNonQuery();
                return new Retorno(true, String.Format(Mensagens.MSG_02, "Salvo"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno AlterarPedidoProduto(Produto produto, Pedido pedido)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("UPDATE TB_PEDIDO_PRODUTO SET ");
                CommandSQL.AppendLine("QUANTIDADE = @QUANTIDADE, ");
                CommandSQL.AppendLine("VALOR = @VALOR, ");
                CommandSQL.AppendLine("DESCONTO = @DESCONTO, ");
                CommandSQL.AppendLine("NUMERO_CLIENTE = @NUMERO_CLIENTE, ");
                CommandSQL.AppendLine("VALOR_UNITARIO = @VALOR_UNITARIO, ");
                CommandSQL.AppendLine("NUMERO_PEDIDO_CLIENTE = @NUMERO_PEDIDO_CLIENTE, ");
                CommandSQL.AppendLine("LINHA_PEDIDO_CLIENTE = @LINHA_PEDIDO_CLIENTE ");
                CommandSQL.AppendLine("WHERE CODIGO = @CODIGO ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@QUANTIDADE", produto.Quantidade);
                Command.Parameters.AddWithValue("@VALOR", produto.Valor);
                Command.Parameters.AddWithValue("@DESCONTO", produto.Desconto.HasValue ? (object)produto.Desconto : DBNull.Value);
                Command.Parameters.AddWithValue("@NUMERO_CLIENTE", produto.NumeroProdutoCliente.HasValue ? (object)produto.NumeroProdutoCliente : DBNull.Value);
                Command.Parameters.AddWithValue("@VALOR_UNITARIO", produto.ValorUnitario.HasValue ? (object)produto.ValorUnitario : DBNull.Value);
                Command.Parameters.AddWithValue("@NUMERO_PEDIDO_CLIENTE", produto.NumeroPedidoCliente.HasValue ? (object)produto.NumeroPedidoCliente : DBNull.Value);
                Command.Parameters.AddWithValue("@LINHA_PEDIDO_CLIENTE", produto.LinhaPedidoCliente.HasValue ? (object)produto.LinhaPedidoCliente : DBNull.Value);
                Command.Parameters.AddWithValue("@CODIGO", produto.CodigoPedidoProduto);

                Abrir();
                Command.ExecuteNonQuery();
                return new Retorno(true, String.Format(Mensagens.MSG_02, "Salvo"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno IncluirPedidoProdutoPronto(decimal quantidadePronta, int codigoFase, int codigoPedidoProduto, string codigoNota = "")
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("INSERT INTO TB_PEDIDO_PRODUTO_PRONTO( ");
                CommandSQL.AppendLine("DATA, ");
                CommandSQL.AppendLine("QUANTIDADE, ");
                CommandSQL.AppendLine("CODIGO_TIPO_FASE, ");
                CommandSQL.AppendLine("CODIGO_PEDIDO_PRODUTO, ");
                CommandSQL.AppendLine("CODIGO_NOTA) ");
                CommandSQL.AppendLine("VALUES ( ");
                CommandSQL.AppendLine("@DATA, ");
                CommandSQL.AppendLine("@QUANTIDADE, ");
                CommandSQL.AppendLine("@CODIGO_TIPO_FASE, ");
                CommandSQL.AppendLine("@CODIGO_PEDIDO_PRODUTO, ");
                CommandSQL.AppendLine("@CODIGO_NOTA) ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@DATA", DateTime.Now);
                Command.Parameters.AddWithValue("@QUANTIDADE", quantidadePronta);
                Command.Parameters.AddWithValue("@CODIGO_TIPO_FASE", codigoFase);
                Command.Parameters.AddWithValue("@CODIGO_PEDIDO_PRODUTO", codigoPedidoProduto);
                Command.Parameters.AddWithValue("@CODIGO_NOTA", codigoNota);

                Abrir();
                Command.ExecuteNonQuery();
                return new Retorno(true, String.Format(Mensagens.MSG_02, "Salvo"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Excluir(Pedido Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("DELETE FROM TB_PEDIDO WHERE CODIGO = @CODIGO");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Abrir();
                Command.ExecuteNonQuery();
                return new Retorno(true, String.Format(Mensagens.MSG_02, "Excluido "));
            }
            catch (Exception ex)
            {
                if (((MySqlException)ex).Number == 1451)
                    return new Retorno(false, Mensagens.MSG_16);
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Alterar(Pedido Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("UPDATE TB_PEDIDO SET ");
                CommandSQL.AppendLine("NUMERO_PEDIDO_CLIENTE = @NUMERO_PEDIDO_CLIENTE, ");
                CommandSQL.AppendLine("NUMERO_PARCELAS = @NUMERO_PARCELAS, ");
                CommandSQL.AppendLine("OBSERVACAO = @OBSERVACAO, ");
                CommandSQL.AppendLine("OBSERVACAO_NOTA_FISCAL = @OBSERVACAO_NOTA_FISCAL, ");
                CommandSQL.AppendLine("VALOR = @VALOR, ");
                CommandSQL.AppendLine("DATA_PREVISAO_ENTREGA = @DATA_PREVISAO_ENTREGA, ");
                CommandSQL.AppendLine("DATA_INCLUSAO = @DATA_INCLUSAO, ");
                CommandSQL.AppendLine("DESCONTO = @DESCONTO, ");
                CommandSQL.AppendLine("PORCENTAGEM_LUCRO = @PORCENTAGEM_LUCRO, ");
                CommandSQL.AppendLine("CODIGO_CLIENTE = @CODIGO_CLIENTE, ");
                CommandSQL.AppendLine("CODIGO_TABELA_PRECO = @CODIGO_TABELA_PRECO, ");
                CommandSQL.AppendLine("CODIGO_TIPO_FASE = @CODIGO_TIPO_FASE, ");
                CommandSQL.AppendLine("CODIGO_TIPO_FORMA_PAGAMENTO = @CODIGO_TIPO_FORMA_PAGAMENTO, ");
                CommandSQL.AppendLine("DATA_PAGAMENTO = @DATA_PAGAMENTO ");
                CommandSQL.AppendLine("WHERE CODIGO = @CODIGO");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@NUMERO_PEDIDO_CLIENTE", Entity.NumeroPedidoCliente);
                Command.Parameters.AddWithValue("@NUMERO_PARCELAS", Entity.NumeroParcelas);
                Command.Parameters.AddWithValue("@OBSERVACAO", Entity.Observacao != null ? (object)Entity.Observacao.ToUpper() : DBNull.Value);
                Command.Parameters.AddWithValue("@OBSERVACAO_NOTA_FISCAL", Entity.ObservacaoNotaFiscal != null ? (object)Entity.ObservacaoNotaFiscal.ToUpper() : DBNull.Value);
                Command.Parameters.AddWithValue("@VALOR", Entity.Valor);
                Command.Parameters.AddWithValue("@DATA_PREVISAO_ENTREGA", Entity.DataPrevisaoEntrega);
                Command.Parameters.AddWithValue("@DATA_INCLUSAO", DateTime.Now);
                Command.Parameters.AddWithValue("@DESCONTO", Entity.Desconto);
                Command.Parameters.AddWithValue("@PORCENTAGEM_LUCRO", Entity.PorcentagemLucro);
                Command.Parameters.AddWithValue("@CODIGO_CLIENTE", Entity.Cliente.Codigo);
                Command.Parameters.AddWithValue("@CODIGO_TABELA_PRECO", Entity.TabelaPreco.Codigo);
                Command.Parameters.AddWithValue("@CODIGO_TIPO_FASE", Entity.TipoFase.Codigo);
                Command.Parameters.AddWithValue("@CODIGO_TIPO_FORMA_PAGAMENTO", Entity.TipoFormaPagamento.Codigo);
                Command.Parameters.AddWithValue("@DATA_PAGAMENTO", Entity.DataPagamento);
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Abrir();
                Command.ExecuteNonQuery();
                return new Retorno(true, String.Format(Mensagens.MSG_02, "Alterado "));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        #endregion

        #region CONSULTAS

        public Retorno Listar(int Pagina, int QntPagina)
        {
            try
            {
                List<Pedido> Pedidos = new List<Pedido>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_PEDIDO.CODIGO AS CODIGO_PEDIDO, ");
                CommandSQL.AppendLine("TB_PEDIDO.NUMERO_PEDIDO_CLIENTE, ");
                CommandSQL.AppendLine("TB_PEDIDO.NUMERO_PARCELAS, ");
                CommandSQL.AppendLine("TB_PEDIDO.OBSERVACAO, ");
                CommandSQL.AppendLine("TB_PEDIDO.OBSERVACAO_NOTA_FISCAL, ");
                CommandSQL.AppendLine("TB_PEDIDO.VALOR, ");
                CommandSQL.AppendLine("TB_CLIENTE.CODIGO AS CODIGO_CLIENTE, ");
                CommandSQL.AppendLine("TB_CLIENTE.NOME, ");
                CommandSQL.AppendLine("TB_CLIENTE.NOME_FANTASIA, ");
                CommandSQL.AppendLine("TB_CLIENTE.CNPJ, ");
                CommandSQL.AppendLine("TB_CLIENTE.CONTATO, ");
                CommandSQL.AppendLine("TB_TABELA_PRECO.CODIGO AS CODIGO_TABELA_PRECO, ");
                CommandSQL.AppendLine("TB_TABELA_PRECO.DESCRICAO AS DESCRICAO_TABELA_PRECO, ");
                CommandSQL.AppendLine("TB_TABELA_PRECO.IMPOSTO, ");
                CommandSQL.AppendLine("TB_TABELA_PRECO.LUCRO, ");
                CommandSQL.AppendLine("TB_TABELA_PRECO.DATA_INICIO, ");
                CommandSQL.AppendLine("TB_TABELA_PRECO.DATA_FIM, ");
                CommandSQL.AppendLine("TB_TIPO_FASE.CODIGO AS CODIGO_TIPO_FASE, ");
                CommandSQL.AppendLine("TB_TIPO_FASE.DESCRICAO AS DESCRICAO_TIPO_FASE, ");
                CommandSQL.AppendLine("TB_FICHA_TECNICA.CODIGO AS CODIGO_FICHA_TECNICA, ");
                CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO.CODIGO AS CODIGO_TIPO_FORMA_PAGAMENTO, ");
                CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO.DESCRICAO AS DESCRICAO_TIPO_FORMA_PAGAMENTO ");
                CommandSQL.AppendLine("FROM TB_PEDIDO ");
                CommandSQL.AppendLine("INNER JOIN TB_CLIENTE ON ");
                CommandSQL.AppendLine("TB_PEDIDO.CODIGO_CLIENTE = TB_CLIENTE.CODIGO ");
                CommandSQL.AppendLine("INNER JOIN TB_TABELA_PRECO ON ");
                CommandSQL.AppendLine("TB_PEDIDO.CODIGO_TABELA_PRECO = TB_TABELA_PRECO.CODIGO ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_FASE ON ");
                CommandSQL.AppendLine("TB_PEDIDO.CODIGO_TIPO_FASE = TB_TIPO_FASE.CODIGO ");
                CommandSQL.AppendLine("INNER JOIN TB_FICHA_TECNICA ON ");
                CommandSQL.AppendLine("TB_PEDIDO.CODIGO_FICHA_TECNICA = TB_FICHA_TECNICA.CODIGO ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_FORMA_PAGAMENTO ON ");
                CommandSQL.AppendLine("TB_PEDIDO.CODIGO_TIPO_FORMA_PAGAMENTO = TB_TIPO_FORMA_PAGAMENTO.CODIGO ");

                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Pedidos.Add(FillEntity(Reader));
                }
                return new Retorno(Pedidos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Pesquisar(Pedido Entity, int Pagina, int QntPagina)
        {
            try
            {
                List<Pedido> Pedidos = new List<Pedido>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_PEDIDO.CODIGO AS CODIGO_PEDIDO, ");
                CommandSQL.AppendLine("TB_PEDIDO.NUMERO_PEDIDO_CLIENTE, ");
                CommandSQL.AppendLine("TB_PEDIDO.NUMERO_PARCELAS, ");
                CommandSQL.AppendLine("TB_PEDIDO.OBSERVACAO, ");
                CommandSQL.AppendLine("TB_PEDIDO.OBSERVACAO_NOTA_FISCAL, ");
                CommandSQL.AppendLine("TB_PEDIDO.VALOR, ");
                CommandSQL.AppendLine("TB_CLIENTE.CODIGO AS CODIGO_CLIENTE, ");
                CommandSQL.AppendLine("TB_CLIENTE.NOME, ");
                CommandSQL.AppendLine("TB_CLIENTE.NOME_FANTASIA, ");
                CommandSQL.AppendLine("TB_CLIENTE.CNPJ, ");
                CommandSQL.AppendLine("TB_CLIENTE.CONTATO, ");
                CommandSQL.AppendLine("TB_TABELA_PRECO.CODIGO AS CODIGO_TABELA_PRECO, ");
                CommandSQL.AppendLine("TB_TABELA_PRECO.DESCRICAO AS DESCRICAO_TABELA_PRECO, ");
                CommandSQL.AppendLine("TB_TABELA_PRECO.IMPOSTO, ");
                CommandSQL.AppendLine("TB_TABELA_PRECO.LUCRO, ");
                CommandSQL.AppendLine("TB_TABELA_PRECO.DATA_INICIO, ");
                CommandSQL.AppendLine("TB_TABELA_PRECO.DATA_FIM, ");
                CommandSQL.AppendLine("TB_TIPO_FASE.CODIGO AS CODIGO_TIPO_FASE, ");
                CommandSQL.AppendLine("TB_TIPO_FASE.DESCRICAO AS DESCRICAO_TIPO_FASE, ");
                CommandSQL.AppendLine("TB_FICHA_TECNICA.CODIGO AS CODIGO_FICHA_TECNICA, ");
                CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO.CODIGO AS CODIGO_TIPO_FORMA_PAGAMENTO, ");
                CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO.DESCRICAO AS DESCRICAO_TIPO_FORMA_PAGAMENTO ");
                CommandSQL.AppendLine("FROM TB_PEDIDO ");
                CommandSQL.AppendLine("INNER JOIN TB_CLIENTE ON ");
                CommandSQL.AppendLine("TB_PEDIDO.CODIGO_CLIENTE = TB_CLIENTE.CODIGO ");
                CommandSQL.AppendLine("INNER JOIN TB_TABELA_PRECO ON ");
                CommandSQL.AppendLine("TB_PEDIDO.CODIGO_TABELA_PRECO = TB_TABELA_PRECO.CODIGO ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_FASE ON ");
                CommandSQL.AppendLine("TB_PEDIDO.CODIGO_TIPO_FASE = TB_TIPO_FASE.CODIGO ");
                CommandSQL.AppendLine("INNER JOIN TB_FICHA_TECNICA ON ");
                CommandSQL.AppendLine("TB_PEDIDO.CODIGO_FICHA_TECNICA = TB_FICHA_TECNICA.CODIGO ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_FORMA_PAGAMENTO ON ");
                CommandSQL.AppendLine("TB_PEDIDO.CODIGO_TIPO_FORMA_PAGAMENTO = TB_TIPO_FORMA_PAGAMENTO.CODIGO ");

                CommandSQL.AppendLine("WHERE (TB_PEDIDO.NUMERO_PEDIDO_CLIENTE LIKE '%" + Entity.NumeroPedidoCliente + "%' )");
                CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Pedidos.Add(FillEntity(Reader));
                }
                return new Retorno(Pedidos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Listar(TipoFase tipoFase)
        {
            try
            {
                List<Pedido> Pedidos = new List<Pedido>();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("T.CODIGO AS CODIGO_PEDIDO, ");
                CommandSQL.AppendLine("T.DATA_PREVISAO_ENTREGA, ");

                CommandSQL.AppendLine("					(SELECT CODIGO FROM TB_TIPO_FASE WHERE ORDEM = ");
                CommandSQL.AppendLine("					(SELECT MAX(TF.ORDEM) FROM TB_PEDIDO_TIPO_FASE PTF ");
                CommandSQL.AppendLine("					INNER JOIN TB_TIPO_FASE TF ON TF.CODIGO = PTF.CODIGO_TIPO_FASE ");
                CommandSQL.AppendLine("					WHERE PTF.CODIGO_PEDIDO = T.CODIGO)) AS CODIGO_TIPO_FASE,  ");

                CommandSQL.AppendLine("					(SELECT DESCRICAO FROM TB_TIPO_FASE WHERE ORDEM = ");
                CommandSQL.AppendLine("					(SELECT MAX(TF.ORDEM) FROM TB_PEDIDO_TIPO_FASE PTF ");
                CommandSQL.AppendLine("					INNER JOIN TB_TIPO_FASE TF ON TF.CODIGO = PTF.CODIGO_TIPO_FASE ");
                CommandSQL.AppendLine("					WHERE PTF.CODIGO_PEDIDO = T.CODIGO)) AS DESCRICAO_TIPO_FASE,  ");

                CommandSQL.AppendLine("TB_CLIENTE.NOME, ");
                CommandSQL.AppendLine("TB_CLIENTE.NOME_FANTASIA ");
                CommandSQL.AppendLine("FROM TB_PEDIDO T ");
                CommandSQL.AppendLine("LEFT JOIN TB_CLIENTE ON T.CODIGO_CLIENTE = TB_CLIENTE.CODIGO ");
                CommandSQL.AppendLine("INNER JOIN TB_PEDIDO_PRODUTO PP ON PP.CODIGO_PEDIDO = T.CODIGO");

                if (tipoFase.Codigo != (int)EnumTipoFuncao.ADMINISTRATIVO)
                {
                    var juncaoSQL = String.Empty;

                    CommandSQL.AppendLine("WHERE");

                    if (tipoFase.Codigo != (int)EnumTipoFase.CORTE)
                    {
                        juncaoSQL = "AND";

                        CommandSQL.AppendLine("			T.CODIGO IN( ");
                        CommandSQL.AppendLine("							SELECT PP.CODIGO_PEDIDO FROM TB_PEDIDO_PRODUTO PP ");
                        CommandSQL.AppendLine("							INNER JOIN TB_PEDIDO_PRODUTO_PRONTO PPP ON PPP.CODIGO_PEDIDO_PRODUTO = PP.CODIGO ");
                        CommandSQL.AppendLine("							INNER JOIN TB_TIPO_FASE F ON F.CODIGO = PPP.CODIGO_TIPO_FASE ");
                        CommandSQL.AppendLine("							WHERE F.ORDEM = @TIPO_FASE_ORDEM_ANTERIOR ");
                        CommandSQL.AppendLine("						) ");
                    }

                    //CommandSQL.AppendLine("			OR ( ");
                    //CommandSQL.AppendLine("					(SELECT MAX(TF.ORDEM) FROM TB_PEDIDO_TIPO_FASE PTF ");
                    //CommandSQL.AppendLine("					INNER JOIN TB_TIPO_FASE TF ON TF.CODIGO = PTF.CODIGO_TIPO_FASE ");
                    //CommandSQL.AppendLine("					WHERE PTF.CODIGO_PEDIDO = T.CODIGO) = @TIPO_FASE_INICIAL_PRODUCAO ");
                    //CommandSQL.AppendLine("			   ) ");
                    //CommandSQL.AppendLine("		 ) ");

                    CommandSQL.AppendLine(juncaoSQL);

                    CommandSQL.AppendLine("	( ");
                    CommandSQL.AppendLine("		SELECT IFNULL(SUM(PPP.QUANTIDADE), 0) FROM TB_PEDIDO_PRODUTO PP ");
                    CommandSQL.AppendLine("		INNER JOIN TB_PEDIDO_PRODUTO_PRONTO PPP ON PPP.CODIGO_PEDIDO_PRODUTO = PP.CODIGO ");
                    CommandSQL.AppendLine("		INNER JOIN TB_TIPO_FASE F ON F.CODIGO = PPP.CODIGO_TIPO_FASE ");
                    CommandSQL.AppendLine("		WHERE F.ORDEM = @TIPO_FASE_ORDEM AND PP.CODIGO_PEDIDO = T.CODIGO ");
                    CommandSQL.AppendLine("	) < ");

                    CommandSQL.AppendLine("	( ");
                    CommandSQL.AppendLine("		SELECT IFNULL(SUM(PP.QUANTIDADE), 0) FROM TB_PEDIDO_PRODUTO PP ");
                    CommandSQL.AppendLine("		WHERE PP.CODIGO_PEDIDO = T.CODIGO ");
                    CommandSQL.AppendLine("	) ");

                    CommandSQL.AppendLine(" AND ( ");
                    CommandSQL.AppendLine(" 		(SELECT MAX(TF.ORDEM) FROM TB_PEDIDO_TIPO_FASE PTF ");
                    CommandSQL.AppendLine(" 		INNER JOIN TB_TIPO_FASE TF ON TF.CODIGO = PTF.CODIGO_TIPO_FASE ");
                    CommandSQL.AppendLine(" 		WHERE PTF.CODIGO_PEDIDO = T.CODIGO) <> @ORCAMENTO ");
                    CommandSQL.AppendLine("    ) ");
                }

                CommandSQL.AppendLine("GROUP BY T.CODIGO ");
                CommandSQL.AppendLine("ORDER BY CODIGO_TIPO_FASE, T.DATA_PREVISAO_ENTREGA ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@CODIGO_TIPO_FASE", tipoFase.Codigo);
                Command.Parameters.AddWithValue("@TIPO_FASE_ORDEM_ANTERIOR", tipoFase.Ordem - 1);
                Command.Parameters.AddWithValue("@TIPO_FASE_ORDEM", tipoFase.Ordem);
                Command.Parameters.AddWithValue("@TIPO_FASE_INICIAL_PRODUCAO", (int)EnumTipoFase.CORTE);
                Command.Parameters.AddWithValue("@ORCAMENTO", (int)EnumTipoFase.ORCAMENTO);
                Abrir();
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Pedidos.Add(new Pedido
                    {
                        Codigo = ConverterValorReader(Reader, "CODIGO_PEDIDO", 0),
                        DataPrevisaoEntrega = ConverterValorReader(Reader, "DATA_PREVISAO_ENTREGA", DateTime.MinValue),
                        TipoFase = new TipoFase
                        {
                            Codigo = ConverterValorReader(Reader, "CODIGO_TIPO_FASE", 0),
                            Descricao = ConverterValorReader(Reader, "DESCRICAO_TIPO_FASE", String.Empty)
                        },
                        Cliente = new Cliente { Nome = ConverterValorReader(Reader, "NOME", String.Empty), NomeFantasia = ConverterValorReader(Reader, "NOME_FANTASIA", String.Empty) }
                    });
                }
                return new Retorno(Pedidos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno Consultar(Pedido Entity)
        {
            try
            {
                Pedido Pedido = new Pedido();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_PEDIDO.CODIGO AS CODIGO_PEDIDO, ");
                CommandSQL.AppendLine("TB_PEDIDO.DATA_PREVISAO_ENTREGA, ");
                CommandSQL.AppendLine("TB_PEDIDO.NUMERO_PARCELAS, ");
                CommandSQL.AppendLine("TB_PEDIDO.OBSERVACAO, ");
                CommandSQL.AppendLine("TB_PEDIDO.OBSERVACAO_NOTA_FISCAL, ");
                CommandSQL.AppendLine("TB_PEDIDO.DESCONTO, ");
                CommandSQL.AppendLine("TB_PEDIDO.PORCENTAGEM_LUCRO, ");
                CommandSQL.AppendLine("TB_PEDIDO.DATA_PAGAMENTO, ");
                CommandSQL.AppendLine("TB_PEDIDO.VALOR, ");
                CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO.DIAS, ");
                CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO.DATA_COMBINADA, ");
                CommandSQL.AppendLine("TB_CLIENTE.CODIGO AS CODIGO_CLIENTE, ");
                CommandSQL.AppendLine("TB_CLIENTE.NOME, ");
                CommandSQL.AppendLine("TB_CLIENTE.CNPJ, ");
                CommandSQL.AppendLine("TB_CLIENTE.CONTATO, ");
                CommandSQL.AppendLine("TB_CLIENTE.IE, ");
                CommandSQL.AppendLine("TB_ENDERECO.RUA, ");
                CommandSQL.AppendLine("TB_ENDERECO.CIDADE, ");
                CommandSQL.AppendLine("TB_ENDERECO.BAIRRO, ");
                CommandSQL.AppendLine("TB_ENDERECO.NUMERO, ");
                CommandSQL.AppendLine("TB_ENDERECO.UF, ");
                CommandSQL.AppendLine("TB_ENDERECO.CEP, ");
                CommandSQL.AppendLine("TB_TABELA_PRECO.CODIGO AS CODIGO_TABELA_PRECO, ");
                CommandSQL.AppendLine("TB_TABELA_PRECO.ESPECIAL, ");
                //CommandSQL.AppendLine("TB_TABELA_PRECO.DESCRICAO AS DESCRICAO_TABELA_PRECO, ");
                CommandSQL.AppendLine("TV.IMPOSTO, ");
                CommandSQL.AppendLine("TV.LUCRO, ");
                //CommandSQL.AppendLine("TB_TABELA_PRECO.DATA_INICIO, ");
                //CommandSQL.AppendLine("TB_TABELA_PRECO.DATA_FIM, ");

                CommandSQL.AppendLine("(SELECT CODIGO FROM TB_TIPO_FASE WHERE ORDEM = ");
                CommandSQL.AppendLine("(SELECT MAX(TB_TIPO_FASE.ORDEM) ");
                CommandSQL.AppendLine("FROM TB_PEDIDO_TIPO_FASE INNER JOIN TB_TIPO_FASE ON TB_TIPO_FASE.CODIGO = TB_PEDIDO_TIPO_FASE.CODIGO_TIPO_FASE WHERE CODIGO_PEDIDO = TB_PEDIDO.CODIGO)) AS CODIGO_TIPO_FASE, ");

                CommandSQL.AppendLine("(SELECT DESCRICAO FROM TB_TIPO_FASE WHERE ORDEM = ");
                CommandSQL.AppendLine("(SELECT MAX(TB_TIPO_FASE.ORDEM) ");
                CommandSQL.AppendLine("FROM TB_PEDIDO_TIPO_FASE INNER JOIN TB_TIPO_FASE ON TB_TIPO_FASE.CODIGO = TB_PEDIDO_TIPO_FASE.CODIGO_TIPO_FASE WHERE CODIGO_PEDIDO = TB_PEDIDO.CODIGO)) AS DESCRICAO_TIPO_FASE, ");

                CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO.CODIGO AS CODIGO_TIPO_FORMA_PAGAMENTO ");

                CommandSQL.AppendLine("FROM TB_PEDIDO ");
                CommandSQL.AppendLine("INNER JOIN TB_CLIENTE ON ");
                CommandSQL.AppendLine("TB_PEDIDO.CODIGO_CLIENTE = TB_CLIENTE.CODIGO ");
                CommandSQL.AppendLine("INNER JOIN TB_ENDERECO ON ");
                CommandSQL.AppendLine("TB_CLIENTE.CODIGO_ENDERECO = TB_ENDERECO.CODIGO ");

                CommandSQL.AppendLine("INNER JOIN TB_TABELA_PRECO ON ");
                CommandSQL.AppendLine("TB_PEDIDO.CODIGO_TABELA_PRECO = TB_TABELA_PRECO.CODIGO ");
                CommandSQL.AppendLine("LEFT JOIN TB_TABELA_PRECO_VALORES TV ON TV.CODIGO_TABELA_PRECO = TB_TABELA_PRECO.CODIGO ");
                CommandSQL.AppendLine("AND TB_TABELA_PRECO.DATA BETWEEN TV.DATA_INICIO AND IFNULL(TV.DATA_FIM, '9999-12-31 23:59:59.997') ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_FASE ON ");
                CommandSQL.AppendLine("TB_PEDIDO.CODIGO_TIPO_FASE = TB_TIPO_FASE.CODIGO ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_FORMA_PAGAMENTO ON ");
                CommandSQL.AppendLine("TB_PEDIDO.CODIGO_TIPO_FORMA_PAGAMENTO = TB_TIPO_FORMA_PAGAMENTO.CODIGO ");

                CommandSQL.AppendLine("WHERE TB_PEDIDO.CODIGO = @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    Pedido.Codigo = ConverterValorReader(Reader, "CODIGO_PEDIDO", 0);
                    Pedido.NumeroParcelas = ConverterValorReader(Reader, "NUMERO_PARCELAS", 0);
                    Pedido.DataPrevisaoEntrega = ConverterValorReader(Reader, "DATA_PREVISAO_ENTREGA", DateTime.MinValue);
                    Pedido.Observacao = ConverterValorReader(Reader, "OBSERVACAO", String.Empty);
                    Pedido.ObservacaoNotaFiscal = ConverterValorReader(Reader, "OBSERVACAO_NOTA_FISCAL", String.Empty);
                    Pedido.Desconto = ConverterValorReader<decimal?>(Reader, "DESCONTO", null);
                    Pedido.Valor = ConverterValorReader(Reader, "VALOR", 0M);
                    Pedido.PorcentagemLucro = ConverterValorReader(Reader, "PORCENTAGEM_LUCRO", 0M);
                    Pedido.DataPagamento = ConverterValorReader<DateTime?>(Reader, "DATA_PAGAMENTO", null);
                    Pedido.Cliente.Codigo = ConverterValorReader(Reader, "CODIGO_CLIENTE", 0);
                    Pedido.Cliente.Nome = ConverterValorReader(Reader, "NOME", String.Empty);
                    Pedido.Cliente.Cnpj = ConverterValorReader(Reader, "CNPJ", 0M);
                    Pedido.Cliente.Contato = ConverterValorReader(Reader, "CONTATO", String.Empty);
                    Pedido.Cliente.Ie = ConverterValorReader(Reader, "IE", 0M);

                    Pedido.Cliente.Endereco.Rua = ConverterValorReader(Reader, "RUA", String.Empty);
                    Pedido.Cliente.Endereco.Cidade = ConverterValorReader(Reader, "CIDADE", String.Empty);
                    Pedido.Cliente.Endereco.Bairro = ConverterValorReader(Reader, "BAIRRO", String.Empty);
                    Pedido.Cliente.Endereco.Numero = ConverterValorReader(Reader, "NUMERO", String.Empty);
                    Pedido.Cliente.Endereco.Uf = ConverterValorReader(Reader, "UF", String.Empty);
                    Pedido.Cliente.Endereco.Cep = ConverterValorReader(Reader, "CEP", String.Empty);

                    Pedido.TabelaPreco.Codigo = ConverterValorReader(Reader, "CODIGO_TABELA_PRECO", 0);
                    Pedido.TabelaPreco.Especial = ConverterValorReader(Reader, "ESPECIAL", false);
                    Pedido.TabelaPreco.Imposto = ConverterValorReader(Reader, "IMPOSTO", 0M);
                    Pedido.TabelaPreco.Lucro = ConverterValorReader(Reader, "LUCRO", 0M);
                    Pedido.FichaTecnica.Codigo = ConverterValorReader(Reader, "CODIGO_FICHA_TECNICA", 0);
                    Pedido.FichaTecnica.Foto = ConverterValorReader(Reader, "FOTO", new byte[] { });
                    Pedido.TipoFase.Codigo = ConverterValorReader(Reader, "CODIGO_TIPO_FASE", 0);
                    Pedido.TipoFase.Descricao = ConverterValorReader(Reader, "DESCRICAO_TIPO_FASE", String.Empty);
                    Pedido.TipoFormaPagamento.Codigo = ConverterValorReader(Reader, "CODIGO_TIPO_FORMA_PAGAMENTO", 0);
                    Pedido.TipoFormaPagamento.Dias = ConverterValorReader(Reader, "DIAS", 0);
                    Pedido.TipoFormaPagamento.DataCombinada = ConverterValorReader(Reader, "DATA_COMBINADA", false);

                }
                return new Retorno(Pedido);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Fechar();
            }
        }

        public Retorno ConsultarPedidoProduto(int codigo, int codigoTipoFase)
        {
            try
            {
                var produtos = new List<Produto>();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("TB_PEDIDO_PRODUTO.CODIGO AS CODIGO_PEDIDO_PRODUTO, ");
                CommandSQL.AppendLine("TB_PEDIDO_PRODUTO.QUANTIDADE, ");
                CommandSQL.AppendLine("TB_PEDIDO_PRODUTO.NUMERO_CLIENTE, ");
                CommandSQL.AppendLine("TB_PEDIDO_PRODUTO.VALOR_UNITARIO, ");
                CommandSQL.AppendLine("TB_PEDIDO_PRODUTO.NUMERO_PEDIDO_CLIENTE, ");
                CommandSQL.AppendLine("TB_PEDIDO_PRODUTO.LINHA_PEDIDO_CLIENTE, ");
                CommandSQL.AppendLine("TB_PRODUTO.CODIGO AS CODIGO_PRODUTO, ");
                CommandSQL.AppendLine("TB_PRODUTO.DESCRICAO AS DESCRICAO_PRODUTO, ");
                CommandSQL.AppendLine("TB_PRODUTO.NUMERO_PRODUTO_CLIENTE, ");
                CommandSQL.AppendLine("TB_FICHA_TECNICA.NCM, ");

                CommandSQL.AppendLine("(SELECT SUM(QUANTIDADE) FROM TB_PEDIDO_PRODUTO_PRONTO ");
                CommandSQL.AppendLine("WHERE TB_PEDIDO_PRODUTO_PRONTO.CODIGO_PEDIDO_PRODUTO = TB_PEDIDO_PRODUTO.CODIGO ");
                CommandSQL.AppendLine("AND CODIGO_TIPO_FASE = @CODIGO_TIPO_FASE) AS QUANTIDADE_PRONTA, ");

                CommandSQL.AppendLine("(SELECT SUM(QUANTIDADE) FROM TB_PEDIDO_PRODUTO_PRONTO ");
                CommandSQL.AppendLine("WHERE TB_PEDIDO_PRODUTO_PRONTO.CODIGO_PEDIDO_PRODUTO = TB_PEDIDO_PRODUTO.CODIGO ");
                CommandSQL.AppendLine("AND CODIGO_TIPO_FASE = @CODIGO_TIPO_FASE -1) AS QUANTIDADE_TIPO_FASE_AMTERIOR_PRONTA, ");

                CommandSQL.AppendLine("(SELECT VALOR FROM TB_PRODUTO_VALOR P ");
                CommandSQL.AppendLine("WHERE TB_PEDIDO.DATA_INCLUSAO > DATA_INICIO AND TB_PEDIDO.DATA_INCLUSAO < IFNULL(DATA_FIM, '9999-12-31 23:59:59.997') ");
                CommandSQL.AppendLine("AND P.CODIGO_PRODUTO = TB_PRODUTO.CODIGO LIMIT 1) AS VALOR, ");

                CommandSQL.AppendLine("TB_TIPO_COR.CODIGO AS CODIGO_TIPO_COR, ");
                CommandSQL.AppendLine("TB_TIPO_COR.DESCRICAO AS DESCRICAO_TIPO_COR, ");
                CommandSQL.AppendLine("TB_TIPO_TAMANHO.CODIGO AS CODIGO_TIPO_TAMANHO, ");
                CommandSQL.AppendLine("TB_TIPO_TAMANHO.DESCRICAO AS DESCRICAO_TIPO_TAMANHO ");
                CommandSQL.AppendLine("FROM TB_PEDIDO_PRODUTO ");
                CommandSQL.AppendLine("INNER JOIN TB_PRODUTO ON TB_PRODUTO.CODIGO = TB_PEDIDO_PRODUTO.CODIGO_PRODUTO ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_COR ON TB_TIPO_COR.CODIGO = TB_PEDIDO_PRODUTO.CODIGO_TIPO_COR ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_TAMANHO ON TB_TIPO_TAMANHO.CODIGO = TB_PEDIDO_PRODUTO.CODIGO_TIPO_TAMANHO ");
                CommandSQL.AppendLine("INNER JOIN TB_PEDIDO ON TB_PEDIDO.CODIGO = TB_PEDIDO_PRODUTO.CODIGO_PEDIDO ");
                CommandSQL.AppendLine("LEFT JOIN TB_FICHA_TECNICA ON TB_FICHA_TECNICA.CODIGO = TB_PRODUTO.CODIGO_FICHA_TECNICA ");

                CommandSQL.AppendLine("WHERE TB_PEDIDO_PRODUTO.CODIGO_PEDIDO = @CODIGO_PEDIDO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@CODIGO_PEDIDO", codigo);
                Command.Parameters.AddWithValue("@CODIGO_TIPO_FASE", codigoTipoFase);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    produtos.Add(new Produto
                    {
                        Codigo = ConverterValorReader(Reader, "CODIGO_PRODUTO", 0),
                        NumeroProdutoCliente = ConverterValorReader(Reader, "NUMERO_PRODUTO_CLIENTE", 0) == 0 ? ConverterValorReader(Reader, "NUMERO_CLIENTE", 0) : ConverterValorReader(Reader, "NUMERO_PRODUTO_CLIENTE", 0),
                        CodigoPedidoProduto = ConverterValorReader(Reader, "CODIGO_PEDIDO_PRODUTO", 0),
                        QuantidadePronta = ConverterValorReader(Reader, "QUANTIDADE_PRONTA", 0M),
                        QuantidadeAnteriorPronta = ConverterValorReader(Reader, "QUANTIDADE_TIPO_FASE_AMTERIOR_PRONTA", 0M),
                        Descricao = ConverterValorReader(Reader, "DESCRICAO_PRODUTO", String.Empty),
                        NumeroPedidoCliente = ConverterValorReader<decimal?>(Reader, "NUMERO_PEDIDO_CLIENTE", null),
                        LinhaPedidoCliente = ConverterValorReader<decimal?>(Reader, "LINHA_PEDIDO_CLIENTE", null),
                        TipoCor = new TipoCor
                        {
                            Codigo = ConverterValorReader(Reader, "CODIGO_TIPO_COR", 0),
                            Descricao = ConverterValorReader(Reader, "DESCRICAO_TIPO_COR", String.Empty)
                        },
                        TipoTamanho = new TipoTamanho
                        {
                            Codigo = ConverterValorReader(Reader, "CODIGO_TIPO_TAMANHO", 0),
                            Descricao = ConverterValorReader(Reader, "DESCRICAO_TIPO_TAMANHO", String.Empty)
                        },
                        Quantidade = ConverterValorReader(Reader, "QUANTIDADE", 0M),
                        Valor = ConverterValorReader(Reader, "VALOR", 0M),
                        ValorUnitario = ConverterValorReader(Reader, "VALOR_UNITARIO", 0M),
                        FichaTecnica = new FichaTecnica
                        {
                            Ncm = ConverterValorReader(Reader, "NCM", String.Empty)
                        }
                    });
                }
                return new Retorno(produtos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno ConsultarQuantidadeProdutosFaltantes(int codigoTipoFase, int codigoPedido)
        {
            try
            {
                var produtos = new List<Produto>();
                CommandSQL = new StringBuilder();

                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("(SELECT COALESCE(SUM(QUANTIDADE), 0) FROM TB_PEDIDO_PRODUTO WHERE CODIGO_PEDIDO = @CODIGO_PEDIDO) - (COALESCE(SUM(TB_PEDIDO_PRODUTO_PRONTO.QUANTIDADE), 0)) ");
                CommandSQL.AppendLine("FROM TB_PEDIDO_PRODUTO_PRONTO ");
                CommandSQL.AppendLine("LEFT JOIN TB_PEDIDO_PRODUTO ON TB_PEDIDO_PRODUTO.CODIGO = TB_PEDIDO_PRODUTO_PRONTO.CODIGO_PEDIDO_PRODUTO ");
                CommandSQL.AppendLine("LEFT JOIN TB_PEDIDO ON TB_PEDIDO.CODIGO = TB_PEDIDO_PRODUTO.CODIGO_PEDIDO ");
                CommandSQL.AppendLine("WHERE CODIGO_PEDIDO = @CODIGO_PEDIDO AND TB_PEDIDO_PRODUTO_PRONTO.CODIGO_TIPO_FASE = @CODIGO_TIPO_FASE ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@CODIGO_PEDIDO", codigoPedido);
                Command.Parameters.AddWithValue("@CODIGO_TIPO_FASE", codigoTipoFase);

                return new Retorno(Command.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno ConsultarPedidoProdutoPronto(int codigoPedido, int codigoProduto)
        {
            try
            {
                var faseProduzido = new decimal[6] { 0, 0, 0, 0, 0, 0 };
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("SUM(PPP.QUANTIDADE) AS QUANTIDADE_PRONTA, ");
                CommandSQL.AppendLine("P.CODIGO, ");
                CommandSQL.AppendLine("TF.CODIGO AS CODIGO_TIPO_FASE, ");
                CommandSQL.AppendLine("TF.DESCRICAO AS DESCRICAO_TIPO_FASE, ");
                CommandSQL.AppendLine("PR.DESCRICAO AS DESCRICAO_PRODUTO, ");
                CommandSQL.AppendLine("TT.DESCRICAO AS DESCRICAO_TIPO_TAMANHO, ");
                CommandSQL.AppendLine("TC.DESCRICAO AS DESCRICAO_TIPO_COR ");
                CommandSQL.AppendLine("FROM TB_PEDIDO_PRODUTO_PRONTO PPP ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_FASE TF ON TF.CODIGO = PPP.CODIGO_TIPO_FASE ");
                CommandSQL.AppendLine("INNER JOIN TB_PEDIDO_PRODUTO PP ON PP.CODIGO = PPP.CODIGO_PEDIDO_PRODUTO ");
                CommandSQL.AppendLine("INNER JOIN TB_PRODUTO PR ON PR.CODIGO = PP.CODIGO_PRODUTO ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_TAMANHO TT ON TT.CODIGO = PP.CODIGO_TIPO_TAMANHO ");
                CommandSQL.AppendLine("INNER JOIN TB_TIPO_COR TC ON TC.CODIGO = PP.CODIGO_TIPO_COR ");
                CommandSQL.AppendLine("INNER JOIN TB_PEDIDO P ON P.CODIGO = PP.CODIGO_PEDIDO ");
                CommandSQL.AppendLine("WHERE P.CODIGO = @CODIGO_PEDIDO AND PP.CODIGO = @CODIGO_PEDIDO_PRODUTO ");
                CommandSQL.AppendLine("GROUP BY P.CODIGO, PPP.CODIGO_TIPO_FASE ");
                CommandSQL.AppendLine("ORDER BY CODIGO_TIPO_FASE ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@CODIGO_PEDIDO", codigoPedido);
                Command.Parameters.AddWithValue("@CODIGO_PEDIDO_PRODUTO", codigoProduto);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    faseProduzido[ConverterValorReader(Reader, "CODIGO_TIPO_FASE", 0) - 1] = ConverterValorReader(Reader, "QUANTIDADE_PRONTA", 0M);
                }

                return new Retorno(faseProduzido);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        public Retorno ConsultarPedidoProduto(int codigoPedidoProduto)
        {
            try
            {
                var produto = new Produto();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT P.CODIGO AS CODIGO_PRODUTO, ");
                CommandSQL.AppendLine("P.DESCRICAO AS DESCRICAO_PRODUTO, ");
                CommandSQL.AppendLine("P.NUMERO_PRODUTO_CLIENTE, ");
                CommandSQL.AppendLine("F.CODIGO AS CODIGO_FICHA_TECNICA, ");
                CommandSQL.AppendLine("F.MODELO, ");
                CommandSQL.AppendLine("F.TIPO, ");
                CommandSQL.AppendLine("F.NCM, ");
                CommandSQL.AppendLine("F.DESCRICAO AS DESCRICAO_FICHA_TECNICA, ");
                CommandSQL.AppendLine("F.FOTO ");
                CommandSQL.AppendLine("FROM TB_PEDIDO_PRODUTO PP ");
                CommandSQL.AppendLine("INNER JOIN TB_PRODUTO P ON P.CODIGO = PP.CODIGO_PRODUTO ");
                CommandSQL.AppendLine("LEFT JOIN TB_FICHA_TECNICA F ON F.CODIGO = P.CODIGO_FICHA_TECNICA ");
                CommandSQL.AppendLine("WHERE PP.CODIGO = @CODIGO_PEDIDO_PRODUTO  ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@CODIGO_PEDIDO_PRODUTO", codigoPedidoProduto);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    produto.Codigo = ConverterValorReader(Reader, "CODIGO_PRODUTO", 0);
                    produto.Descricao = ConverterValorReader(Reader, "DESCRICAO_PRODUTO", String.Empty);
                    produto.NumeroProdutoCliente = ConverterValorReader(Reader, "NUMERO_PRODUTO_CLIENTE", 0M);
                    produto.FichaTecnica.Codigo = ConverterValorReader(Reader, "CODIGO_FICHA_TECNICA", 0);
                    produto.FichaTecnica.Modelo = ConverterValorReader(Reader, "MODELO", String.Empty);
                    produto.FichaTecnica.Tipo = ConverterValorReader(Reader, "TIPO", String.Empty);
                    produto.FichaTecnica.Ncm = ConverterValorReader(Reader, "NCM", String.Empty);
                    produto.FichaTecnica.Descricao = ConverterValorReader(Reader, "DESCRICAO_FICHA_TECNICA", String.Empty);
                    produto.FichaTecnica.Foto = ConverterValorReader(Reader, "FOTO", new byte[] { });

                }
                return new Retorno(produto);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Fechar();
            }
        }


        public Retorno ConsultarDoPedido(int codigoPedido)
        {
            try
            {
                var produtos = new List<Produto>();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT CODIGO ");

                CommandSQL.AppendLine("FROM TB_PEDIDO_PRODUTO ");
                CommandSQL.AppendLine("WHERE CODIGO_PEDIDO = @CODIGO_PEDIDO ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@CODIGO_PEDIDO", codigoPedido);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    produtos.Add(new Produto { CodigoPedidoProduto = ConverterValorReader(Reader, "CODIGO", 0) });
                }
                return new Retorno(produtos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Fechar();
            }
        }

        public Retorno ConsultarNumeroNfe(int codigoPedido)
        {
            try
            {
                var produtos = new List<Produto>();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("CODIGO_NOTA ");
                CommandSQL.AppendLine("FROM TB_PEDIDO_PRODUTO_PRONTO ");
                CommandSQL.AppendLine("INNER JOIN TB_PEDIDO_PRODUTO ON TB_PEDIDO_PRODUTO.CODIGO = TB_PEDIDO_PRODUTO_PRONTO.CODIGO_PEDIDO_PRODUTO ");
                CommandSQL.AppendLine("INNER JOIN TB_PEDIDO ON TB_PEDIDO.CODIGO = TB_PEDIDO_PRODUTO.CODIGO_PEDIDO ");

                CommandSQL.AppendLine("WHERE TB_PEDIDO.CODIGO = @CODIGO_PEDIDO ");
                CommandSQL.AppendLine("AND CODIGO_NOTA IS NOT NULL AND CODIGO_NOTA <> '' ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@CODIGO_PEDIDO", codigoPedido);

                return new Retorno(Command.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }


        public Retorno ListarParaDominio()
        {
            try
            {
                var pedidos = new List<Pedido>();
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT ");
                CommandSQL.AppendLine("CODIGO ");
                CommandSQL.AppendLine("FROM TB_PEDIDO ");

                CommandSQL.AppendLine("ORDER BY CODIGO ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    pedidos.Add(new Pedido { Codigo = ConverterValorReader(Reader, "CODIGO", 0) });
                }
                return new Retorno(pedidos);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        #endregion

        #region METODOS AUXILIARES

        public Retorno VerificarExistencia(Pedido Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.AppendLine("SELECT 1 FROM TB_PEDIDO ");
                CommandSQL.AppendLine("WHERE TB_PEDIDO.NUMERO_PEDIDO_CLIENTE = @NUMERO_PEDIDO_CLIENTE ");
                CommandSQL.AppendLine("AND TB_PEDIDO.CODIGO <> @CODIGO ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@NUMERO_PEDIDO_CLIENTE", Entity.NumeroPedidoCliente);
                Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo);
                Reader = Command.ExecuteReader();
                while (Reader.Read())
                {
                    return new Retorno(false, String.Format(Mensagens.MSG_04, "Pedido", "NumeroPedidoCliente"));
                }
                return new Retorno(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally { Fechar(); }
        }

        private Pedido FillEntity(IDataReader reader)
        {
            Pedido Pedido = new Pedido();
            try
            {
                Pedido.Codigo = ConverterValorReader(reader, "CODIGO_PEDIDO", 0);
                Pedido.NumeroPedidoCliente = ConverterValorReader(reader, "NUMERO_PEDIDO_CLIENTE", 0M);
                Pedido.NumeroParcelas = ConverterValorReader(reader, "NUMERO_PARCELAS", 0);
                Pedido.Observacao = ConverterValorReader(reader, "OBSERVACAO", String.Empty);
                Pedido.ObservacaoNotaFiscal = ConverterValorReader(reader, "OBSERVACAO_NOTA_FISCAL", String.Empty);
                Pedido.Valor = ConverterValorReader(reader, "VALOR", 0M);
                Pedido.Cliente.Codigo = ConverterValorReader(reader, "CODIGO_CLIENTE", 0);
                Pedido.Cliente.Nome = ConverterValorReader(reader, "NOME", String.Empty);
                Pedido.Cliente.NomeFantasia = ConverterValorReader(reader, "NOME_FANTASIA", String.Empty);
                Pedido.Cliente.Cnpj = ConverterValorReader(reader, "CNPJ", 0M);
                Pedido.Cliente.Contato = ConverterValorReader(reader, "CONTATO", String.Empty);
                Pedido.TabelaPreco.Codigo = ConverterValorReader(reader, "CODIGO_TABELA_PRECO", 0);
                Pedido.TabelaPreco.Descricao = ConverterValorReader(reader, "DESCRICAO_TABELA_PRECO", String.Empty);
                Pedido.TabelaPreco.Imposto = ConverterValorReader(reader, "IMPOSTO", 0M);
                Pedido.TabelaPreco.Lucro = ConverterValorReader(reader, "LUCRO", 0M);
                Pedido.TipoFase.Codigo = ConverterValorReader(reader, "CODIGO_TIPO_FASE", 0);
                Pedido.TipoFase.Descricao = ConverterValorReader(reader, "DESCRICAO_TIPO_FASE", String.Empty);
                Pedido.FichaTecnica.Codigo = ConverterValorReader(reader, "CODIGO_FICHA_TECNICA", 0);
                Pedido.TipoFormaPagamento.Codigo = ConverterValorReader(reader, "CODIGO_TIPO_FORMA_PAGAMENTO", 0);
                Pedido.TipoFormaPagamento.Descricao = ConverterValorReader(reader, "DESCRICAO_TIPO_FORMA_PAGAMENTO", String.Empty);
            }
            catch (Exception ex) { throw ex; }
            return Pedido;
        }

        #endregion
    }
}

