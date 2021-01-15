using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using InfinitySolutions.Entity;
using InfinitySolutions.Commom;
using MySql.Data.MySqlClient;

namespace InfinitySolutions.Data
{

 public class DataTipoFuncaoTerceirizado: DataBase
{
#region AÇÕES

public Retorno Incluir(TipoFuncaoTerceirizado Entity)
{
try
{
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("INSERT INTO TB_TIPO_FUNCAO_TERCEIRIZADO( ");
CommandSQL.AppendLine("NOME) ");
CommandSQL.AppendLine("VALUES (");
CommandSQL.AppendLine("@NOME) ");
Command = CriaComandoSQL(CommandSQL.ToString());
Command.Parameters.AddWithValue("@NOME",Entity.Nome);
Abrir();
Command.ExecuteNonQuery();
return new Retorno(true,String.Format(Mensagens.MSG_02,"Salvo"));
}
catch(Exception ex)
{
throw ex;
}
finally { Fechar(); }
}

public Retorno Excluir(TipoFuncaoTerceirizado Entity)
{
try
{
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("DELETE FROM TB_TIPO_FUNCAO_TERCEIRIZADO WHERE CODIGO = @CODIGO");
Command = CriaComandoSQL(CommandSQL.ToString());
Command.Parameters.AddWithValue("@CODIGO",Entity.Codigo);
Abrir();
Command.ExecuteNonQuery();
return new Retorno(true,String.Format(Mensagens.MSG_02,"Excluido "));
}
catch(Exception ex)
{
if (((MySqlException)ex).Number == 1451)
return new Retorno(false, Mensagens.MSG_16);
throw ex;
}
finally { Fechar(); }
}

public Retorno Alterar(TipoFuncaoTerceirizado Entity)
{
try
{
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("UPDATE TB_TIPO_FUNCAO_TERCEIRIZADO SET ");
CommandSQL.AppendLine("NOME = @NOME ");
CommandSQL.AppendLine("WHERE CODIGO = @CODIGO");
Command = CriaComandoSQL(CommandSQL.ToString());
Command.Parameters.AddWithValue("@CODIGO",Entity.Codigo);
Command.Parameters.AddWithValue("@NOME",Entity.Nome);
Abrir();
Command.ExecuteNonQuery();
return new Retorno(true,String.Format(Mensagens.MSG_02,"Alterado "));
}
catch(Exception ex)
{
throw ex;
}
finally { Fechar(); }
}

#endregion

#region CONSULTAS

public Retorno Listar(int Pagina, int QntPagina)
{
try
{
List<TipoFuncaoTerceirizado> TipoFuncaoTerceirizados = new List<TipoFuncaoTerceirizado>();
int Limite = (Pagina - 1) * QntPagina;
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("SELECT "); 
CommandSQL.AppendLine("TB_TIPO_FUNCAO_TERCEIRIZADO.CODIGO, ");
CommandSQL.AppendLine("TB_TIPO_FUNCAO_TERCEIRIZADO.NOME ");
CommandSQL.AppendLine("FROM TB_TIPO_FUNCAO_TERCEIRIZADO ");

CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
Command = CriaComandoSQL(CommandSQL.ToString());
Abrir();
Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
Command.Parameters.AddWithValue("@LIMITE", Limite);
Reader = Command.ExecuteReader();
while (Reader.Read())
{
TipoFuncaoTerceirizados.Add(FillEntity(Reader));
}
return new Retorno(TipoFuncaoTerceirizados);
}
catch(Exception ex)
{
throw ex;
}
finally { Fechar(); }
}

public Retorno Pesquisar(TipoFuncaoTerceirizado Entity, int Pagina, int QntPagina)
{
try
{
List<TipoFuncaoTerceirizado> TipoFuncaoTerceirizados = new List<TipoFuncaoTerceirizado>();
int Limite = (Pagina - 1) * QntPagina;
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("SELECT "); 
CommandSQL.AppendLine("TB_TIPO_FUNCAO_TERCEIRIZADO.CODIGO, ");
CommandSQL.AppendLine("TB_TIPO_FUNCAO_TERCEIRIZADO.NOME ");
CommandSQL.AppendLine("FROM TB_TIPO_FUNCAO_TERCEIRIZADO ");

CommandSQL.AppendLine("WHERE (TB_TIPO_FUNCAO_TERCEIRIZADO.NOME LIKE '%" + Entity.Nome + "%' )");
CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
Command = CriaComandoSQL(CommandSQL.ToString());
Abrir();
Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
Command.Parameters.AddWithValue("@LIMITE", Limite);
Reader = Command.ExecuteReader();
while (Reader.Read())
{
TipoFuncaoTerceirizados.Add(FillEntity(Reader));
}
return new Retorno(TipoFuncaoTerceirizados);
}
catch(Exception ex)
{
throw ex;
}
finally { Fechar(); }
}

public Retorno Listar()
{
try
{
List<TipoFuncaoTerceirizado> TipoFuncaoTerceirizados = new List<TipoFuncaoTerceirizado>();
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("SELECT "); 
CommandSQL.AppendLine("TB_TIPO_FUNCAO_TERCEIRIZADO.CODIGO, ");
CommandSQL.AppendLine("TB_TIPO_FUNCAO_TERCEIRIZADO.NOME ");
CommandSQL.AppendLine("FROM TB_TIPO_FUNCAO_TERCEIRIZADO ");

Command = CriaComandoSQL(CommandSQL.ToString());
Abrir();
Reader = Command.ExecuteReader();
while (Reader.Read())
{
TipoFuncaoTerceirizados.Add(FillEntity(Reader));
}
return new Retorno(TipoFuncaoTerceirizados);
}
catch(Exception ex)
{
throw ex;
}
finally { Fechar(); }
}

public Retorno Consultar(TipoFuncaoTerceirizado Entity )
{
try
{
TipoFuncaoTerceirizado TipoFuncaoTerceirizado = new TipoFuncaoTerceirizado ();
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("SELECT "); 
CommandSQL.AppendLine("TB_TIPO_FUNCAO_TERCEIRIZADO.CODIGO, ");
CommandSQL.AppendLine("TB_TIPO_FUNCAO_TERCEIRIZADO.NOME ");
CommandSQL.AppendLine("FROM TB_TIPO_FUNCAO_TERCEIRIZADO ");

CommandSQL.AppendLine("WHERE TB_TIPO_FUNCAO_TERCEIRIZADO.CODIGO = @CODIGO ");
Command = CriaComandoSQL(CommandSQL.ToString());
Abrir();
Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo );
Reader = Command.ExecuteReader();
while (Reader.Read())
{
TipoFuncaoTerceirizado = FillEntity(Reader);
}
return new Retorno(TipoFuncaoTerceirizado);
}
catch(Exception ex)
{
throw ex;
}
finally { Fechar(); }
}

#endregion

#region METODOS AUXILIARES

public Retorno VerificarExistencia(TipoFuncaoTerceirizado Entity )
{
try
{
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("SELECT 1 FROM TB_TIPO_FUNCAO_TERCEIRIZADO "); 
CommandSQL.AppendLine("WHERE TB_TIPO_FUNCAO_TERCEIRIZADO.NOME = @NOME ");
CommandSQL.AppendLine("AND TB_TIPO_FUNCAO_TERCEIRIZADO.CODIGO <> @CODIGO ");
Command = CriaComandoSQL(CommandSQL.ToString());
Abrir();
Command.Parameters.AddWithValue("@NOME", Entity.Nome );
Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo );
Reader = Command.ExecuteReader();
while (Reader.Read())
{
return new Retorno(false, String.Format(Mensagens.MSG_04, "TipoFuncaoTerceirizado", "Nome"));
}
return new Retorno(true);
}
catch(Exception ex)
{
throw ex;
}
finally { Fechar(); }
}

private TipoFuncaoTerceirizado FillEntity(IDataReader reader)
{
TipoFuncaoTerceirizado TipoFuncaoTerceirizado = new TipoFuncaoTerceirizado();
try
{
TipoFuncaoTerceirizado.Codigo = ConverterValorReader(reader,"CODIGO",0);
TipoFuncaoTerceirizado.Nome = ConverterValorReader(reader,"NOME",String.Empty);
}
catch(Exception ex) { throw ex; }
return TipoFuncaoTerceirizado;
}

#endregion

}
}

