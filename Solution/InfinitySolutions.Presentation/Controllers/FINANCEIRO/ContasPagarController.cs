using InfinitySolutions.Business;
using InfinitySolutions.Commom.Dtos;
using InfinitySolutions.Entity;
using InfinitySolutions.Presentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InfinitySolutions.Presentation.Controllers
{
    [ValidadorLogin(Order = 1, TipoPermissao = EnumTipoPermissao.CONTA_PAGAR)]
    public class ContasPagarController : Controller
    {
        public ActionResult Index(DateTime? DataInicio, DateTime? DataFim)
        {
            return View(CarregarModel(DataInicio, DataFim));
        }
        public ActionResult Salvar(ContasPagarModel model)
        {
            try
            {
                var modelSalvar = CarregarModel(model.DataInicio, model.DataFim);
                modelSalvar.Retorno = new BusinessContaPagar().Salvar(model.ContaPagar);

                if (modelSalvar.Retorno.IsValido)
                    return RedirectToAction("Index", "ContasPagar");

                return base.View("Index", modelSalvar);
            }
            catch
            {
                return View();
            }
        }
        public ActionResult DarBaixaPagamento(Pagamento Pagamento, FormCollection form)
        {
            var retorno = new BusinessContaPagar().DarBaixaPagamento(Pagamento);
            return RedirectToAction("Index", "ContasPagar");
        }

        public JsonResult Excluir(int codigo)
        {
            var retorno = new BusinessContaPagar().Excluir(new ContaPagar { Codigo = codigo });
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Consultar(int codigo)
        {
            var retorno = new BusinessContaPagar().Consultar(new ContaPagar { Codigo = codigo });
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ConsultarFormaPagamento(int codigoFormaPagamento)
        {
            var retorno = new BusinessTipoContaPagar().Consultar(new TipoContaPagar { Codigo = codigoFormaPagamento });
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        private ContasPagarModel CarregarModel(DateTime? DataInicio, DateTime? DataFim)
        {
            var contaPagarModel = new ContasPagarModel();

            if (DataInicio.HasValue)
                contaPagarModel.DataInicio = DataInicio.Value;
            else
                contaPagarModel.DataInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (DataFim.HasValue)
                contaPagarModel.DataFim = DataFim.Value;
            else
                contaPagarModel.DataFim = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

            contaPagarModel.Retorno = new BusinessContaPagar().Listar(contaPagarModel.DataInicio, contaPagarModel.DataFim);

            if (contaPagarModel.Retorno.IsValido)
            {
                var retorno = new BusinessContaPagar().CarregarDominios();

                if (retorno.IsValido)
                    contaPagarModel.Dominios = retorno.Entity as DominiosDto;
                else
                    contaPagarModel.Retorno = retorno;
            }

            return contaPagarModel;
        }
    }
}