using InfinitySolutions.Business;
using InfinitySolutions.Entity;
using InfinitySolutions.Presentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InfinitySolutions.Presentation.Controllers
{
    public class LinhaDeProducaoController : Controller
    {
        public ActionResult Index(DateTime? DataInicio, DateTime? DataFim)
        {
            return View(CarregarModel(DataInicio, DataFim));
        }

        public JsonResult ConfirmarProducao(int codigoLinhaProducao)
        {
            var retorno = new BusinessLinhaProducao().ConfirmarProducao(new LinhaProducao { Produto = new Produto { CodigoPedidoProduto = codigoLinhaProducao } });
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Terceirizar(LinhaProducao linhaProducaoTerceirizado)
        {
            var retorno = new BusinessLinhaProducao().Terceirizar(linhaProducaoTerceirizado);
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        private LinhaDeProducaoModel CarregarModel(DateTime? DataInicio, DateTime? DataFim)
        {
            var linhaDeProducao = new LinhaDeProducaoModel();

            if (DataInicio.HasValue)
                linhaDeProducao.DataInicio = DataInicio.Value;
            else
                linhaDeProducao.DataInicio = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            if (DataFim.HasValue)
                linhaDeProducao.DataFim = DataFim.Value;
            else
                linhaDeProducao.DataFim = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));

            linhaDeProducao.Retorno = new BusinessLinhaProducao().Listar(linhaDeProducao.DataInicio, linhaDeProducao.DataFim);

            return linhaDeProducao;
        }
    }
}