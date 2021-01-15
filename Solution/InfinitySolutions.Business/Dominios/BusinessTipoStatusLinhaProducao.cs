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

public class BusinessTipoStatusLinhaProducao: BusinessBase
{
#region AÇÕES

public Retorno Salvar(TipoStatusLinhaProducao Entity)
{
try
{
Retorno retorno = PreenchimentoObrigatorio(Entity);
if(retorno.IsValido)
{
if (Entity.Codigo == 0)
retorno = new DataTipoStatusLinhaProducao().Incluir(Entity);
else
retorno = new DataTipoStatusLinhaProducao().Alterar(Entity);
}
return retorno;
}
catch(Exception ex)
{
return Retorno.CriarRetornoExcecao(ex);
}
}

public Retorno Excluir(TipoStatusLinhaProducao Entity)
{
try
{
return new DataTipoStatusLinhaProducao().Excluir(Entity);
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
return new DataTipoStatusLinhaProducao().Listar(Pagina,QntPagina);
}
catch(Exception ex)
{
return Retorno.CriarRetornoExcecao(ex);
}
}

public Retorno Pesquisar(TipoStatusLinhaProducao Entity, int Pagina, int QntPagina)
{
try
{
return new DataTipoStatusLinhaProducao().Pesquisar(Entity,Pagina, QntPagina);
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
return new DataTipoStatusLinhaProducao().Listar();
}
catch(Exception ex)
{
return Retorno.CriarRetornoExcecao(ex);
}
}

public Retorno Consultar(TipoStatusLinhaProducao Entity )
{
try
{
return new DataTipoStatusLinhaProducao().Consultar(Entity);
}
catch(Exception ex)
{
return Retorno.CriarRetornoExcecao(ex);
}
}

#endregion

#region METODOS AUXILIARES

public Retorno PreenchimentoObrigatorio(TipoStatusLinhaProducao Entity)
{

if(String.IsNullOrEmpty(Entity.Descricao))
return new Retorno(false,String.Format(Mensagens.MSG_01,"Descricao"));

return new Retorno(true);
}

private Retorno VerificarExistencia(TipoStatusLinhaProducao Entity )
{
try
{
return new DataTipoStatusLinhaProducao().VerificarExistencia(Entity);
}
catch(Exception ex)
{
return Retorno.CriarRetornoExcecao(ex);
}
}

#endregion

}
}

