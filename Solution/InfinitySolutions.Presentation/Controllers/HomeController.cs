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
    [ValidadorLogin]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Sair()
        {
            Session[Constantes.USUARIO_LOGADO] = null;
            return RedirectToAction("Index", "Login");
        }
    }
}
