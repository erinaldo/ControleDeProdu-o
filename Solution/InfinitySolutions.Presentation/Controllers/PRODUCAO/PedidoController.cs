using InfinitySolutions.Business;
using InfinitySolutions.Commom;
using InfinitySolutions.Commom.Dtos;
using InfinitySolutions.Entity;
using InfinitySolutions.Entity.Enum;
using InfinitySolutions.Entity.Nfe;
using InfinitySolutions.Presentation.Models;
using RestSharp;
using Rotativa;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace InfinitySolutions.Presentation.Controllers
{

    [ValidadorLogin(Order = 1, TipoPermissao = EnumTipoPermissao.PEDIDO)]
    public class PedidoController : BaseController
    {
        public ActionResult Index()
        {
            return View(CarregarModel());
        }
        public ActionResult Salvar(Pedido pedido)
        {
            /* TODO: OPÇÃO DE PESQUISA POR NOME DO PRODUTO. COLOCAR COMPONENTE */
            /* TODO: ARRUMAR MUDANÇA DE STATUS PEDIDO QUANDO FOR FATURADO DIRETAMENTE (MUDAR PARA FATURADO E NAO PARA A PROXIMA FASE */

            try
            {
                var model = CarregarModel();
                model.Retorno = new BusinessPedido().Salvar(pedido);

                if (model.Retorno.IsValido)
                    return RedirectToAction("Index", "Pedido");

                return base.View("Index", model);
            }
            catch
            {
                return View();
            }
        }

        public ActionResult FichaTecnica()
        {
            try
            {
                return base.View("FichaTecnica", new FichaTecnicaModel());
            }
            catch
            {
                return View();
            }
        }

        public ActionResult ImprimirFichaTecnica(int codigoProduto)
        {
            var retorno = new BusinessPedido().ConsultarProduto(codigoProduto, null);
            var produto = retorno.Entity as Produto;

            //return View("FichaTecnicaImpressao", new FichaTecnicaModel { Produto = produto });

            var pdf = new ViewAsPdf
            {
                ViewName = "FichaTecnicaImpressao",
                Model = new FichaTecnicaModel { Produto = produto },
                FileName = String.Format("FICHATECNICA_{0}.pdf", produto.Descricao)
            };

            return pdf;
        }

        public ActionResult ImprimirPedido(int codigoPedido)
        {
            var retorno = new BusinessPedido().Consultar(new Pedido { Codigo = codigoPedido }, RecuperarFuncao().Codigo);
            var pedido = retorno.Entity as Pedido;

            //return View("PedidoImpressao", new PedidoModel { Pedido = pedido });

            var pdf = new ViewAsPdf
            {
                ViewName = "PedidoImpressao",
                Model = new PedidoModel { Pedido = pedido },
                FileName = String.Format("PEDIDO_{0}.pdf", pedido.Codigo.ToString("0000000"))
            };

            return pdf;
        }

        public JsonResult SalvarPedidoProdutoPronto(decimal quantidadePronta, int codigoPedidoProduto, int codigoPedido)
        {
            var retorno = new BusinessPedido().SalvarPedidoProdutoPronto(quantidadePronta, RecuperarFuncao().Codigo, codigoPedidoProduto, codigoPedido);
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Consultar(int codigo)
        {
            var retorno = new BusinessPedido().Consultar(new Pedido { Codigo = codigo }, RecuperarFuncao().Codigo);
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ConsultarProduto(int? codigoProduto, decimal? numeroProdutoCliente)
        {
            var retorno = new BusinessPedido().ConsultarProduto(codigoProduto ?? 0, numeroProdutoCliente);
            (retorno.Entity as Produto).FichaTecnica.Foto = new byte[] { };
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ConsultarPedidoProduto(int codigoPedidoProduto)
        {
            var retorno = new BusinessPedido().ConsultarPedidoProduto(codigoPedidoProduto);
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ConsultarTabelaPreco(int codigoTabelaPreco)
        {
            var retorno = new BusinessPedido().ConsultarTabelaPreco(codigoTabelaPreco);
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ConsultarFormaPagamento(int codigoFormaPagamento)
        {
            var retorno = new BusinessPedido().ConsultarFormaPagamento(codigoFormaPagamento);
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Excluir(int codigo)
        {
            var retorno = new BusinessPedido().Excluir(new Pedido { Codigo = codigo });
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ListarTerceirizados()
        {
            var retorno = new BusinessPedido().ListarTerceirizados();
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SalvarCor(string descricao)
        {
            var retorno = new BusinessPedido().SalvarCor(descricao);
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SalvarTamanho(string descricao)
        {
            var retorno = new BusinessPedido().SalvarTamanho(descricao);
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SalvarTipoFormaPagamento(string descricao, bool contemParcelas)
        {
            var retorno = new BusinessPedido().SalvarTipoFormaPagamento(descricao, contemParcelas);
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SalvarFichaTecnica(int codigoFichaTecnica, string modelo, string tipo, string ncm, string descricao)
        {
            var retorno = new BusinessPedido().SalvarFichaTecnica(new FichaTecnica { Codigo = codigoFichaTecnica, Modelo = modelo, Tipo = tipo, Ncm = ncm, Descricao = descricao });
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }


        public JsonResult MontarLinhaProducaoPedido(int codigoPedido, bool adicionarLinhaProducao, List<LinhaProducao> linhaProducaoTerceirizados)
        {
            var retorno = new BusinessPedido().MontarLinhaProducaoPedido(new Pedido { Codigo = codigoPedido }, adicionarLinhaProducao
                , linhaProducaoTerceirizados ?? new List<LinhaProducao>(), RecuperarFuncao().Codigo);
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        public JsonResult EmitirNfe(Pedido Pedido, List<Produto> produtos, bool ambienteProducao)
        {
            var retorno = new BusinessPedido().EmitirNfe(Pedido, Pedido.Frete, produtos, ambienteProducao);

            if (retorno.IsValido)
            {
                var emitirNfe = retorno.Entity as Tuple<EmitirNfe, Pedido>;
                var emitirNfeJson = ConverterObjetoParaJson(emitirNfe.Item1);

                var restClientNotaFiscal = new RestClient("https://webmaniabr.com/api/1/nfe/emissao/");
                var request = new RestRequest(Method.POST);
                request.AddHeader("x-access-token-secret", "r9eJsACbCh0hWV0CdTp11ndfvdW31DoB7JJRJQYvsxdJuToE");
                request.AddHeader("x-access-token", "2360-M7KiSccuVwI9W5bcUZ0DsG9JOBLb9vYLmtjIiq5huPVbJuzH");
                request.AddHeader("x-consumer-secret", "9uU6OMob0PKjnGV6WmKp26AyfXsf3fH8My73lGiaf4jZ9ul7");
                request.AddHeader("x-consumer-key", "pjztrwvoX8P4zGCPdyPvHsCHsw56y2bz");
                request.AddHeader("content-type", "application/json");
                request.AddHeader("cache-control", "no-cache");

                request.AddParameter("undefined", emitirNfeJson, ParameterType.RequestBody);

                IRestResponse response = restClientNotaFiscal.Execute(request);
                var retornoNfe = ConverterJsonParaObjecto<retorno>(response.Content);
                retorno.IsValido = retornoNfe.status == "aprovado" || retornoNfe.status == "processamento";
                retorno.Mensagem = retornoNfe.log != null && retornoNfe.log.aProt != null && !String.IsNullOrEmpty(retornoNfe.log.aProt[0].xMotivo) ? retornoNfe.log.aProt[0].xMotivo : String.IsNullOrEmpty(retornoNfe.error) ? retornoNfe.log.xMotivo : retornoNfe.error;

                if (retorno.IsValido)
                {
                    retorno.Entity = retornoNfe.danfe;

                    if (ambienteProducao)
                    {
                        var retornoRegistrarFaturamento = new BusinessPedido().RegistrarFaturamento(emitirNfe.Item2, produtos, retornoNfe.chave);

                        if (!retornoRegistrarFaturamento.IsValido)
                            retorno = retornoRegistrarFaturamento;
                    }
                }

            }
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }
        public JsonResult EnviarPorEmail(int codigoPedido, string nomeCliente, string notaFiscal)
        {
            var retorno = new BusinessPedido().EnviarPorEmail(new Pedido { Codigo = codigoPedido, Cliente = new Cliente { Nome = nomeCliente }, NumeroNfe = notaFiscal });

            return Json(retorno, JsonRequestBehavior.AllowGet);
        }


        private PedidoModel CarregarModel()
        {
            var usuario = Session[Constantes.USUARIO_LOGADO] as Login;
            var pedidoModel = new PedidoModel
            {
                Retorno = new BusinessPedido().Listar(usuario.TiposFuncoes)
            };

            if (pedidoModel.Retorno.IsValido)
            {
                var retorno = new BusinessPedido().CarregarDominios();

                if (retorno.IsValido)
                    pedidoModel.Dominios = retorno.Entity as DominiosDto;
                else
                    pedidoModel.Retorno = retorno;
            }

            return pedidoModel;
        }
    }
}