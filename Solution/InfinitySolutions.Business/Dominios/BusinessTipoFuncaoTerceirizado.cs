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

public class BusinessTipoFuncaoTerceirizado: BusinessBase
{
#region AÇÕES

public Retorno Salvar(TipoFuncaoTerceirizado Entity)
{
try
{
Retorno retorno = PreenchimentoObrigatorio(Entity);
if(retorno.IsValido)
{
if (Entity.Codigo == 0)
retorno = new DataTipoFuncaoTerceirizado().Incluir(Entity);
else
retorno = new DataTipoFuncaoTerceirizado().Alterar(Entity);
}
return retorno;
}
catch(Exception ex)
{
return Retorno.CriarRetornoExcecao(ex);
}
}

public Retorno Excluir(TipoFuncaoTerceirizado Entity)
{
try
{
return new DataTipoFuncaoTerceirizado().Excluir(Entity);
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
return new DataTipoFuncaoTerceirizado().Listar(Pagina,QntPagina);
}
catch(Exception ex)
{
return Retorno.CriarRetornoExcecao(ex);
}
}

public Retorno Pesquisar(TipoFuncaoTerceirizado Entity, int Pagina, int QntPagina)
{
try
{
return new DataTipoFuncaoTerceirizado().Pesquisar(Entity,Pagina, QntPagina);
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
return new DataTipoFuncaoTerceirizado().Listar();
}
catch(Exception ex)
{
return Retorno.CriarRetornoExcecao(ex);
}
}

public Retorno Consultar(TipoFuncaoTerceirizado Entity )
{
try
{
return new DataTipoFuncaoTerceirizado().Consultar(Entity);
}
catch(Exception ex)
{
return Retorno.CriarRetornoExcecao(ex);
}
}

#endregion

#region METODOS AUXILIARES

public Retorno PreenchimentoObrigatorio(TipoFuncaoTerceirizado Entity)
{

if(String.IsNullOrEmpty(Entity.Nome))
return new Retorno(false,String.Format(Mensagens.MSG_01,"Nome"));

return new Retorno(true);
}

private Retorno VerificarExistencia(TipoFuncaoTerceirizado Entity )
{
try
{
return new DataTipoFuncaoTerceirizado().VerificarExistencia(Entity);
}
catch(Exception ex)
{
return Retorno.CriarRetornoExcecao(ex);
}
}

#endregion

}
}

