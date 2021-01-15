using InfinitySolutions.Business;
using InfinitySolutions.Commom.Dtos;
using InfinitySolutions.Entity;
using InfinitySolutions.Presentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InfinitySolutions.Presentation.Controllers.PRODUCAO
{
    [ValidadorLogin(Order = 1, TipoPermissao = EnumTipoPermissao.TERCEIRIZADO)]
    public class TerceirizadoController : Controller
    {

        public ActionResult Index()
        {
            return View(CarregarModel());
        }

        public ActionResult Salvar(Terceirizado terceirizado)
        {
            try
            {
                var model = CarregarModel();
                model.Retorno = new BusinessTerceirizado().Salvar(terceirizado);

                if (model.Retorno.IsValido)
                    return RedirectToAction("Index", "Terceirizado");

                return base.View("Index", model);
            }
            catch
            {
                return View();
            }
        }
        public JsonResult ConsultarDadosTerceirizado(int codigo)
        {
            var retorno = new BusinessTerceirizado().Consultar(new Terceirizado { Codigo = codigo });
            return Json(retorno.Entity as Cliente, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Excluir(int codigo)
        {
            var retorno = new BusinessTerceirizado().Excluir(new Terceirizado { Codigo = codigo });
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Consultar(int codigo)
        {
            var retorno = new BusinessTerceirizado().Consultar(new Terceirizado { Codigo = codigo });
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        private TerceirizadoModel CarregarModel()
        {
            var terceirizadoModel = new TerceirizadoModel { Retorno = new BusinessTerceirizado().Listar() };

            var retorno = new BusinessTerceirizado().CarregarDominios();

            if (retorno.IsValido)
                terceirizadoModel.Dominios = retorno.Entity as DominiosDto;
            else
                terceirizadoModel.Retorno = retorno;

            return terceirizadoModel;
        }
    }
}