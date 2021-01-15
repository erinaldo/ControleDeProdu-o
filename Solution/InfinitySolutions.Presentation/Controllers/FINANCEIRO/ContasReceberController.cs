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
    [ValidadorLogin(Order = 1, TipoPermissao = EnumTipoPermissao.CONTA_RECEBER)]
    public class ContasReceberController : Controller
    {
        public ActionResult Index(DateTime? DataInicio, DateTime? DataFim)
        {
            return View(CarregarModel(DataInicio, DataFim));
        }
        public ActionResult Salvar(ContasReceberModel model)
        {
            try
            {
                var modelSalvar = CarregarModel(model.DataInicio, model.DataFim);
                modelSalvar.Retorno = new BusinessContaReceber().Salvar(model.ContaReceber);

                if (modelSalvar.Retorno.IsValido)
                    return RedirectToAction("Index", "ContasReceber");

                return base.View("Index", modelSalvar);
            }
            catch
            {
                return View();
            }
        }


        public JsonResult Excluir(int codigo)
        {
            var retorno = new BusinessContaReceber().Excluir(new ContaReceber { Codigo = codigo });
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Consultar(int codigo)
        {
            var retorno = new BusinessContaReceber().Consultar(new ContaReceber { Codigo = codigo });
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DarBaixaPagamento(int codigo)
        {
            var retorno = new BusinessContaReceber().DarBaixaPagamento(codigo);
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        private ContasReceberModel CarregarModel(DateTime? DataInicio, DateTime? DataFim)
        {
            var contaReceberModel = new ContasReceberModel();

            if (DataInicio.HasValue)
                contaReceberModel.DataInicio = DataInicio.Value;
            else
                contaReceberModel.DataInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (DataFim.HasValue)
                contaReceberModel.DataFim = DataFim.Value;
            else
                contaReceberModel.DataFim = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

            contaReceberModel.Retorno = new BusinessContaReceber().Listar(contaReceberModel.DataInicio, contaReceberModel.DataFim);

            if (contaReceberModel.Retorno.IsValido)
            {
                var retorno = new BusinessContaReceber().CarregarDominios();

                if (retorno.IsValido)
                    contaReceberModel.Dominios = retorno.Entity as DominiosDto;
                else
                    contaReceberModel.Retorno = retorno;
            }

            return contaReceberModel;
        }
    }
}