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

public class BusinessTipoStatusContaPagar: BusinessBase
{
#region AÇÕES

public Retorno Salvar(TipoStatusContaPagar Entity)
{
try
{
Retorno retorno = PreenchimentoObrigatorio(Entity);
if(retorno.IsValido)
{
if (Entity.Codigo == 0)
retorno = new DataTipoStatusContaPagar().Incluir(Entity);
else
retorno = new DataTipoStatusContaPagar().Alterar(Entity);
}
return retorno;
}
catch(Exception ex)
{
return Retorno.CriarRetornoExcecao(ex);
}
}

public Retorno Excluir(TipoStatusContaPagar Entity)
{
try
{
return new DataTipoStatusContaPagar().Excluir(Entity);
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
return new DataTipoStatusContaPagar().Listar(Pagina,QntPagina);
}
catch(Exception ex)
{
return Retorno.CriarRetornoExcecao(ex);
}
}

public Retorno Pesquisar(TipoStatusContaPagar Entity, int Pagina, int QntPagina)
{
try
{
return new DataTipoStatusContaPagar().Pesquisar(Entity,Pagina, QntPagina);
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
return new DataTipoStatusContaPagar().Listar();
}
catch(Exception ex)
{
return Retorno.CriarRetornoExcecao(ex);
}
}

public Retorno Consultar(TipoStatusContaPagar Entity )
{
try
{
return new DataTipoStatusContaPagar().Consultar(Entity);
}
catch(Exception ex)
{
return Retorno.CriarRetornoExcecao(ex);
}
}

#endregion

#region METODOS AUXILIARES

public Retorno PreenchimentoObrigatorio(TipoStatusContaPagar Entity)
{

if(String.IsNullOrEmpty(Entity.Descricao))
return new Retorno(false,String.Format(Mensagens.MSG_01,"Descricao"));

return new Retorno(true);
}

private Retorno VerificarExistencia(TipoStatusContaPagar Entity )
{
try
{
return new DataTipoStatusContaPagar().VerificarExistencia(Entity);
}
catch(Exception ex)
{
return Retorno.CriarRetornoExcecao(ex);
}
}

#endregion

}
}

