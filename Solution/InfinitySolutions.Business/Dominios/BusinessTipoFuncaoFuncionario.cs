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

public class BusinessTipoFuncaoFuncionario: BusinessBase
{
#region AÇÕES

public Retorno Salvar(TipoFuncaoFuncionario Entity)
{
try
{
Retorno retorno = PreenchimentoObrigatorio(Entity);
if(retorno.IsValido)
{
if (Entity.Codigo == 0)
retorno = new DataTipoFuncaoFuncionario().Incluir(Entity);
else
retorno = new DataTipoFuncaoFuncionario().Alterar(Entity);
}
return retorno;
}
catch(Exception ex)
{
return Retorno.CriarRetornoExcecao(ex);
}
}

public Retorno Excluir(TipoFuncaoFuncionario Entity)
{
try
{
return new DataTipoFuncaoFuncionario().Excluir(Entity);
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
return new DataTipoFuncaoFuncionario().Listar(Pagina,QntPagina);
}
catch(Exception ex)
{
return Retorno.CriarRetornoExcecao(ex);
}
}

public Retorno Pesquisar(TipoFuncaoFuncionario Entity, int Pagina, int QntPagina)
{
try
{
return new DataTipoFuncaoFuncionario().Pesquisar(Entity,Pagina, QntPagina);
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
return new DataTipoFuncaoFuncionario().Listar();
}
catch(Exception ex)
{
return Retorno.CriarRetornoExcecao(ex);
}
}

public Retorno Consultar(TipoFuncaoFuncionario Entity )
{
try
{
return new DataTipoFuncaoFuncionario().Consultar(Entity);
}
catch(Exception ex)
{
return Retorno.CriarRetornoExcecao(ex);
}
}

#endregion

#region METODOS AUXILIARES

public Retorno PreenchimentoObrigatorio(TipoFuncaoFuncionario Entity)
{

if(String.IsNullOrEmpty(Entity.Nome))
return new Retorno(false,String.Format(Mensagens.MSG_01,"Nome"));

return new Retorno(true);
}

private Retorno VerificarExistencia(TipoFuncaoFuncionario Entity )
{
try
{
return new DataTipoFuncaoFuncionario().VerificarExistencia(Entity);
}
catch(Exception ex)
{
return Retorno.CriarRetornoExcecao(ex);
}
}

#endregion

}
}

