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
    [ValidadorLogin(Order = 1, TipoPermissao = EnumTipoPermissao.PRODUTO)]
    public class ProdutoController : BaseController
    {
        public ActionResult Index()
        {
            return View(CarregarModel());
        }

        public ActionResult Create(Produto produto)
        {
            try
            {
                var model = CarregarModel();

                if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                    produto.FichaTecnica.Foto = ConverterEmByte(Request.Files[0]);

                model.Retorno = new BusinessProduto().Salvar(produto);

                if (model.Retorno.IsValido)
                    return RedirectToAction("Index", "Produto");

                return base.View("Index", model);
            }
            catch
            {
                return View();
            }
        }

        private ProdutoModel CarregarModel()
        {

            var produtoModel = new ProdutoModel
            {
                Retorno = new BusinessProduto().Listar()
            };

            if (produtoModel.Retorno.IsValido)
            {
                var retorno = new BusinessProduto().CarregarDominios();

                if (retorno.IsValido)
                    produtoModel.Dominios = retorno.Entity as DominiosDto;
                else
                    produtoModel.Retorno = retorno;
            }

            return produtoModel;
        }


        public JsonResult ConsultarMateriaPrima(int codigoMateriaPrima)
        {
            var retorno = new BusinessProduto().ConsultarMateriaPrima(codigoMateriaPrima);

            return Json(retorno, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Excluir(int codigo)
        {
            var retorno = new BusinessProduto().Excluir(new Produto { Codigo = codigo });
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Consultar(int codigo)
        {
            var retorno = new BusinessProduto().Consultar(new Produto { Codigo = codigo });
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }
    }
}