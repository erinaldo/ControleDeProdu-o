using InfinitySolutions.Commom;
using InfinitySolutions.Commom.Dtos;
using InfinitySolutions.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InfinitySolutions.Presentation.Models
{
    public class TerceirizadoModel
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


        private Terceirizado _terceirizado;
        public Terceirizado Terceirizado
        {
            get
            {
                if (_terceirizado == null)
                    _terceirizado = new Terceirizado();

                return _terceirizado;
            }
            set { _terceirizado = value; }
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