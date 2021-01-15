using InfinitySolutions.Business;
using InfinitySolutions.Commom;
using InfinitySolutions.Entity;
using InfinitySolutions.Presentation.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InfinitySolutions.Presentation.Controllers
{
    [ValidadorLogin(Order = 1, TipoPermissao = EnumTipoPermissao.NOTAS_FISCAIS)]
    public class NotasFiscaisController : Controller
    {
        public ActionResult Index()
        {
            return View(CarregarModel());
        }

        public JsonResult Salvar(int codigo, string observacao)
        {
            var retorno = new BusinessNotaFiscal().Salvar(new NotaFiscal { Codigo = codigo, Observacao = observacao });
            return Json(retorno, JsonRequestBehavior.AllowGet);
        }

        private NotasFiscaisModel CarregarModel()
        {
            var usuario = Session[Constantes.USUARIO_LOGADO] as Login;
            var notasFiscaisModel = new NotasFiscaisModel
            {
                Retorno = new BusinessNotaFiscal().Listar()
            };

            return notasFiscaisModel;
        }
    }
}