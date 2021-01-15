using InfinitySolutions.Commom;
using InfinitySolutions.Entity;
using InfinitySolutions.Entity.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace InfinitySolutions.Presentation.Controllers
{
    public class ValidadorLoginAttribute : ActionFilterAttribute
    {
        public EnumTipoPermissao TipoPermissao { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var login = filterContext.HttpContext.Session[Constantes.USUARIO_LOGADO] as Login;

            if (login == null || ((int)TipoPermissao != 0 && !login.TiposPermissoes.Any(p => p.Codigo == (int)TipoPermissao)))
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    var retorno = new { mensagemDeErro = Constantes.MS_SESSAO, Mensagem = Constantes.MS_SESSAO };
                    filterContext.Result = new JsonResult { JsonRequestBehavior = JsonRequestBehavior.AllowGet, Data = retorno };
                }
                else
                {
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                    {
                        { "controller", "Login" },
                        { "action", "Index" }
                    });
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}