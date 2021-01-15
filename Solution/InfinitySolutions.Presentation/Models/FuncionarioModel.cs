using InfinitySolutions.Commom;
using InfinitySolutions.Commom.Dtos;
using InfinitySolutions.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InfinitySolutions.Presentation.Models
{
    public class FuncionarioModel
    {
        private Retorno _retorno;
        public Retorno Retorno
        {
            get
            {
                if (_retorno == null)
                    _retorno = new Retorno();

                return _retorno;
            }
            set { _retorno = value; }
        }


        private Funcionario _funcionario;
        public Funcionario Funcionario
        {
            get
            {
                if (_funcionario == null)
                    _funcionario = new Funcionario();

                return _funcionario;
            }
            set { _funcionario = value; }
        }

        private DominiosDto _dominios;
        public DominiosDto Dominios
        {
            get
            {
                if (_dominios == null)
                    _dominios = new DominiosDto();

                return _dominios;
            }
            set { _dominios = value; }
        }

    }
}