using InfinitySolutions.Business;
using InfinitySolutions.Commom;
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

    [ValidadorLogin(Order = 1, TipoPermissao = EnumTipoPermissao.CLIENTE)]
    public class ClienteController : Controller
    {
        public ActionResult Index()
        {
            return View(CarregarModel());
        }
        public ActionResult Salvar(Cliente cliente)
        {
            try
            {
                var model = CarregarModel();
                model.Retorno = new BusinessCliente().Salvar(cliente);

                if (model.Retorno.IsValido)
                    return RedirectToAction("Index", "Cliente");

                return base.View("Index", model);
            }
            catch
            {
                return View();
            }
        }
        public JsonResult ConsultarDadosCliente(decimal cnpj)
        {
            var retorno = new BusinessCliente().ConsultarDados(cnpj);

            //retorno.Entity = (retorno.Entity as Cliente).ConverteObjectParaJSon();

            return Json(retorno.Entity as Cliente, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Excluir(int codigo)
        {
            var retorno = new BusinessCliente().Excluir(new Cliente { Codigo = codigo });
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Consultar(int codigo)
        {
            var retorno = new BusinessCliente().Consultar(new Cliente { Codigo = codigo });
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }


        private ClienteModel CarregarModel()
        {
            var clienteModel = new ClienteModel
            {
                Retorno = new BusinessCliente().Listar()
            };

            return clienteModel;
        }
    }
}