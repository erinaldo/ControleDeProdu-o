using InfinitySolutions.Commom;
using InfinitySolutions.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InfinitySolutions.Presentation.Models
{
    public class LoginModel
    {
        public LoginModel()
        {
            Login = Login ?? new Login();
            Retorno = Retorno ?? new Retorno();
        }

        public Login Login { get; set; }
        public Retorno Retorno { get; set; }
    }
}