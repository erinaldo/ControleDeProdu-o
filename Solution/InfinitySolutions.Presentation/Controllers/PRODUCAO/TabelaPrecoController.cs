using InfinitySolutions.Business;
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

    [ValidadorLogin(Order = 1, TipoPermissao = EnumTipoPermissao.TABELA_PRECO)]
    public class TabelaPrecoController : Controller
    {
        public ActionResult Index()
        {
            return View(CarregarModel());
        }

        public ActionResult Salvar(TabelaPreco tabelaPreco)
        {
            try
            {
                var model = CarregarModel();
                model.Retorno = new BusinessTabelaPreco().Salvar(tabelaPreco);

                if (model.Retorno.IsValido)
                    return RedirectToAction("Index", "TabelaPreco");

                return base.View("Index", model);
            }
            catch
            {
                return View();
            }
        }

        private TabelaPrecoModel CarregarModel()
        {
            var tabelaPrecoModel = new TabelaPrecoModel
            {
                Retorno = new BusinessTabelaPreco().Listar()
            };

            return tabelaPrecoModel;
        }


        public JsonResult Excluir(int codigo)
        {
            var retorno = new BusinessTabelaPreco().Excluir(new TabelaPreco { Codigo = codigo });
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Consultar(int codigo)
        {
            var retorno = new BusinessTabelaPreco().Consultar(new TabelaPreco { Codigo = codigo });
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }
    }
}