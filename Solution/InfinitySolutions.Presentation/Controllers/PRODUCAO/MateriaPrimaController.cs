using InfinitySolutions.Business;
using InfinitySolutions.Commom.Dtos;
using InfinitySolutions.Entity;
using InfinitySolutions.Entity.Enum;
using InfinitySolutions.Presentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InfinitySolutions.Presentation.Controllers
{

    [ValidadorLogin(Order = 1, TipoPermissao = EnumTipoPermissao.MATERIA_PRIMA)]
    public class MateriaPrimaController : Controller
    {
        public ActionResult Index()
        {
            return View(CarregarModel());
        }

        public ActionResult Salvar(MateriaPrima materiaPrima)
        {
            try
            {
                var model = CarregarModel();
                model.Retorno = new BusinessMateriaPrima().Salvar(materiaPrima);

                if (model.Retorno.IsValido)
                    return RedirectToAction("Index", "MateriaPrima");

                return base.View("Index", model);
            }
            catch
            {
                return View();
            }
        }

        private MateriaPrimaModel CarregarModel()
        {
            var tabelaPrecoModel = new MateriaPrimaModel
            {
                Retorno = new BusinessMateriaPrima().Listar()
            };

            if (tabelaPrecoModel.Retorno.IsValido)
            {
                var retorno = new BusinessMateriaPrima().CarregarDominios();

                if (retorno.IsValido)
                    tabelaPrecoModel.Dominios = retorno.Entity as DominiosDto;
                else
                    tabelaPrecoModel.Retorno = retorno;
            }

            return tabelaPrecoModel;
        }


        public JsonResult Excluir(int codigo)
        {
            var retorno = new BusinessMateriaPrima().Excluir(new MateriaPrima { Codigo = codigo });
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Consultar(int codigo)
        {
            var retorno = new BusinessMateriaPrima().Consultar(new MateriaPrima { Codigo = codigo });
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

    }
}