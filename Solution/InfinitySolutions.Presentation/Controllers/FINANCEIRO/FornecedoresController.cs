using InfinitySolutions.Business;
using InfinitySolutions.Entity;
using InfinitySolutions.Presentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InfinitySolutions.Presentation.Controllers.FINANCEIRO
{
    [ValidadorLogin(Order = 1, TipoPermissao = EnumTipoPermissao.FORNECEDOR)]
    public class FornecedoresController : Controller
    {
        public ActionResult Index()
        {
            return View(CarregarModel());
        }

        public ActionResult Salvar(Fornecedor fornecedor)
        {
            try
            {
                var model = CarregarModel();
                model.Retorno = new BusinessFornecedor().Salvar(fornecedor);

                if (model.Retorno.IsValido)
                    return RedirectToAction("Index", "Fornecedores");

                return base.View("Index", model);
            }
            catch
            {
                return View();
            }
        }
        public JsonResult ConsultarDadosFornecedor(int codigo)
        {
            var retorno = new BusinessFornecedor().Consultar(new Fornecedor { Codigo = codigo });
            return Json(retorno.Entity as Cliente, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Excluir(int codigo)
        {
            var retorno = new BusinessFornecedor().Excluir(new Fornecedor { Codigo = codigo });
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Consultar(int codigo)
        {
            var retorno = new BusinessFornecedor().Consultar(new Fornecedor { Codigo = codigo });
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        private FornecedorModel CarregarModel()
        {
            var fornecedorModel = new FornecedorModel { Retorno = new BusinessFornecedor().Listar() };
            return fornecedorModel;
        }
    }
}