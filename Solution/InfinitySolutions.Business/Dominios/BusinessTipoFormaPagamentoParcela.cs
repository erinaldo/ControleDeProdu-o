using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using InfinitySolutions.Data;
using InfinitySolutions.Entity;
using InfinitySolutions.Commom;

namespace InfinitySolutions.Business
{

public class BusinessTipoFormaPagamentoParcela: BusinessBase
{
#region AÇÕES

public Retorno Salvar(TipoFormaPagamentoParcela Entity)
{
try
{
Retorno retorno = PreenchimentoObrigatorio(Entity);
if(retorno.IsValido)
{
if (Entity.Codigo == 0)
retorno = new DataTipoFormaPagamentoParcela().Incluir(Entity);
else
retorno = new DataTipoFormaPagamentoParcela().Alterar(Entity);
}
return retorno;
}
catch(Exception ex)
{
return Retorno.CriarRetornoExcecao(ex);
}
}

public Retorno Excluir(TipoFormaPagamentoParcela Entity)
{
try
{
return new DataTipoFormaPagamentoParcela().Excluir(Entity);
}
catch(Exception ex)
{
return Retorno.CriarRetornoExcecao(ex);
}
}

#endregion

#region CONSULTAS

public Retorno Listar(int Pagina, int QntPagina)
{
try
{
return new DataTipoFormaPagamentoParcela().Listar(Pagina,QntPagina);
}
catch(Exception ex)
{
return Retorno.CriarRetornoExcecao(ex);
}
}

public Retorno Pesquisar(TipoFormaPagamentoParcela Entity, int Pagina, int QntPagina)
{
try
{
return new DataTipoFormaPagamentoParcela().Pesquisar(Entity,Pagina, QntPagina);
}
catch(Exception ex)
{
return Retorno.CriarRetornoExcecao(ex);
}
}

public Retorno Listar()
{
try
{
return new DataTipoFormaPagamentoParcela().Listar();
}
catch(Exception ex)
{
return Retorno.CriarRetornoExcecao(ex);
}
}

public Retorno Consultar(TipoFormaPagamentoParcela Entity )
{
try
{
return new DataTipoFormaPagamentoParcela().Consultar(Entity);
}
catch(Exception ex)
{
return Retorno.CriarRetornoExcecao(ex);
}
}

#endregion

#region METODOS AUXILIARES

public Retorno PreenchimentoObrigatorio(TipoFormaPagamentoParcela Entity)
{

if(String.IsNullOrEmpty(Entity.Descricao))
return new Retorno(false,String.Format(Mensagens.MSG_01,"Descricao"));

return new Retorno(true);
}

private Retorno VerificarExistencia(TipoFormaPagamentoParcela Entity )
{
try
{
return new DataTipoFormaPagamentoParcela().VerificarExistencia(Entity);
}
catch(Exception ex)
{
return Retorno.CriarRetornoExcecao(ex);
}
}

#endregion

}
}

