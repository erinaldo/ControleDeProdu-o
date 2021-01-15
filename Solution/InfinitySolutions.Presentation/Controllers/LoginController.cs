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
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View(new LoginModel());
        }

        [HttpPost]
        public ActionResult Index(LoginModel model)
        {
            try
            {
                var retorno = new BusinessLogin().VerificarLogin(new Entity.Login { Usuario = model.Login.Usuario, Senha = model.Login.Senha });

                if (retorno.IsValido)
                {
                    Session[Constantes.USUARIO_LOGADO] = retorno.Entity;
                    return RedirectToAction("Index", "Pedido");
                }
                else
                {
                    model.Retorno = retorno;
                    return View(model);
                }
            }
            catch
            {
                return View();
            }
        }
    }
}
