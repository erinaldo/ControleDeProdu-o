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

public class BusinessTipoFormaPagamentoContaReceber: BusinessBase
{
#region AÇÕES

public Retorno Salvar(TipoFormaPagamentoContaReceber Entity)
{
try
{
Retorno retorno = PreenchimentoObrigatorio(Entity);
if(retorno.IsValido)
{
if (Entity.Codigo == 0)
retorno = new DataTipoFormaPagamentoContaReceber().Incluir(Entity);
else
retorno = new DataTipoFormaPagamentoContaReceber().Alterar(Entity);
}
return retorno;
}
catch(Exception ex)
{
return Retorno.CriarRetornoExcecao(ex);
}
}

public Retorno Excluir(TipoFormaPagamentoContaReceber Entity)
{
try
{
return new DataTipoFormaPagamentoContaReceber().Excluir(Entity);
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
return new DataTipoFormaPagamentoContaReceber().Listar(Pagina,QntPagina);
}
catch(Exception ex)
{
return Retorno.CriarRetornoExcecao(ex);
}
}

public Retorno Pesquisar(TipoFormaPagamentoContaReceber Entity, int Pagina, int QntPagina)
{
try
{
return new DataTipoFormaPagamentoContaReceber().Pesquisar(Entity,Pagina, QntPagina);
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
return new DataTipoFormaPagamentoContaReceber().Listar();
}
catch(Exception ex)
{
return Retorno.CriarRetornoExcecao(ex);
}
}

public Retorno Consultar(TipoFormaPagamentoContaReceber Entity )
{
try
{
return new DataTipoFormaPagamentoContaReceber().Consultar(Entity);
}
catch(Exception ex)
{
return Retorno.CriarRetornoExcecao(ex);
}
}

#endregion

#region METODOS AUXILIARES

public Retorno PreenchimentoObrigatorio(TipoFormaPagamentoContaReceber Entity)
{

if(String.IsNullOrEmpty(Entity.Descricao))
return new Retorno(false,String.Format(Mensagens.MSG_01,"Descricao"));

return new Retorno(true);
}

private Retorno VerificarExistencia(TipoFormaPagamentoContaReceber Entity )
{
try
{
return new DataTipoFormaPagamentoContaReceber().VerificarExistencia(Entity);
}
catch(Exception ex)
{
return Retorno.CriarRetornoExcecao(ex);
}
}

#endregion

}
}

