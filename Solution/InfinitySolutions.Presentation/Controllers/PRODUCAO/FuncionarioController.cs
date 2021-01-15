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
    [ValidadorLogin(Order = 1, TipoPermissao = EnumTipoPermissao.FORNECEDOR)]
    public class FuncionarioController : Controller
    {
        public ActionResult Index()
        {
            return View(CarregarModel());
        }

        public ActionResult Salvar(Funcionario funcionario)
        {
            try
            {
                var model = CarregarModel();
                model.Retorno = new BusinessFuncionario().Salvar(funcionario);

                if (model.Retorno.IsValido)
                    return RedirectToAction("Index", "Funcionario");

                return base.View("Index", model);
            }
            catch
            {
                return View();
            }
        }
        public JsonResult ConsultarDadosFuncionario(int codigo)
        {
            var retorno = new BusinessFuncionario().Consultar(new Funcionario { Codigo = codigo });
            return Json(retorno.Entity as Cliente, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Excluir(int codigo)
        {
            var retorno = new BusinessFuncionario().Excluir(new Funcionario { Codigo = codigo });
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Consultar(int codigo)
        {
            var retorno = new BusinessFuncionario().Consultar(new Funcionario { Codigo = codigo });
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        private FuncionarioModel CarregarModel()
        {
            var funcionarioModel = new FuncionarioModel { Retorno = new BusinessFuncionario().Listar() };

            var retorno = new BusinessFuncionario().CarregarDominios();

            if (retorno.IsValido)
                funcionarioModel.Dominios = retorno.Entity as DominiosDto;
            else
                funcionarioModel.Retorno = retorno;


            return funcionarioModel;
        }
    }
}