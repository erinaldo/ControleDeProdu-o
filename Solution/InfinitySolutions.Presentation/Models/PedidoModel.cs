using InfinitySolutions.Commom;
using InfinitySolutions.Commom.Dtos;
using InfinitySolutions.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InfinitySolutions.Presentation.Models
{
    public class PedidoModel
    {
        public PedidoModel()
        {
            Pedido = Pedido ?? new Pedido();
            Retorno = Retorno ?? new Retorno();
            Dominios = Dominios ?? new DominiosDto();
            Produto = Produto ?? new Produto();
            TipoTamanho = TipoTamanho ?? new TipoTamanho();
            TipoCor = TipoCor ?? new TipoCor();
            FichaTecnica = FichaTecnica ?? new FichaTecnica();
            TipoFormaPagamento = TipoFormaPagamento ?? new TipoFormaPagamento();
        }

        public Pedido Pedido { get; set; }
        public Produto Produto { get; set; }
        public FichaTecnica FichaTecnica { get; set; }
        public TipoTamanho TipoTamanho { get; set; }
        public TipoCor TipoCor { get; set; }
        public TipoFormaPagamento TipoFormaPagamento { get; set; }
        public Retorno Retorno { get; set; }
        public DominiosDto Dominios { get; set; }
    }
}