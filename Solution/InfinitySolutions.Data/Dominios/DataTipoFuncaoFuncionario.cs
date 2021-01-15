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

 public class DataTipoFuncaoFuncionario: DataBase
{
#region AÇÕES

public Retorno Incluir(TipoFuncaoFuncionario Entity)
{
try
{
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("INSERT INTO TB_TIPO_FUNCAO_FUNCIONARIO( ");
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

public Retorno Excluir(TipoFuncaoFuncionario Entity)
{
try
{
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("DELETE FROM TB_TIPO_FUNCAO_FUNCIONARIO WHERE CODIGO = @CODIGO");
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

public Retorno Alterar(TipoFuncaoFuncionario Entity)
{
try
{
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("UPDATE TB_TIPO_FUNCAO_FUNCIONARIO SET ");
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
List<TipoFuncaoFuncionario> TipoFuncaoFuncionarios = new List<TipoFuncaoFuncionario>();
int Limite = (Pagina - 1) * QntPagina;
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("SELECT "); 
CommandSQL.AppendLine("TB_TIPO_FUNCAO_FUNCIONARIO.CODIGO, ");
CommandSQL.AppendLine("TB_TIPO_FUNCAO_FUNCIONARIO.NOME ");
CommandSQL.AppendLine("FROM TB_TIPO_FUNCAO_FUNCIONARIO ");

CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
Command = CriaComandoSQL(CommandSQL.ToString());
Abrir();
Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
Command.Parameters.AddWithValue("@LIMITE", Limite);
Reader = Command.ExecuteReader();
while (Reader.Read())
{
TipoFuncaoFuncionarios.Add(FillEntity(Reader));
}
return new Retorno(TipoFuncaoFuncionarios);
}
catch(Exception ex)
{
throw ex;
}
finally { Fechar(); }
}

public Retorno Pesquisar(TipoFuncaoFuncionario Entity, int Pagina, int QntPagina)
{
try
{
List<TipoFuncaoFuncionario> TipoFuncaoFuncionarios = new List<TipoFuncaoFuncionario>();
int Limite = (Pagina - 1) * QntPagina;
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("SELECT "); 
CommandSQL.AppendLine("TB_TIPO_FUNCAO_FUNCIONARIO.CODIGO, ");
CommandSQL.AppendLine("TB_TIPO_FUNCAO_FUNCIONARIO.NOME ");
CommandSQL.AppendLine("FROM TB_TIPO_FUNCAO_FUNCIONARIO ");

CommandSQL.AppendLine("WHERE (TB_TIPO_FUNCAO_FUNCIONARIO.NOME LIKE '%" + Entity.Nome + "%' )");
CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
Command = CriaComandoSQL(CommandSQL.ToString());
Abrir();
Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
Command.Parameters.AddWithValue("@LIMITE", Limite);
Reader = Command.ExecuteReader();
while (Reader.Read())
{
TipoFuncaoFuncionarios.Add(FillEntity(Reader));
}
return new Retorno(TipoFuncaoFuncionarios);
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
List<TipoFuncaoFuncionario> TipoFuncaoFuncionarios = new List<TipoFuncaoFuncionario>();
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("SELECT "); 
CommandSQL.AppendLine("TB_TIPO_FUNCAO_FUNCIONARIO.CODIGO, ");
CommandSQL.AppendLine("TB_TIPO_FUNCAO_FUNCIONARIO.NOME ");
CommandSQL.AppendLine("FROM TB_TIPO_FUNCAO_FUNCIONARIO ");

Command = CriaComandoSQL(CommandSQL.ToString());
Abrir();
Reader = Command.ExecuteReader();
while (Reader.Read())
{
TipoFuncaoFuncionarios.Add(FillEntity(Reader));
}
return new Retorno(TipoFuncaoFuncionarios);
}
catch(Exception ex)
{
throw ex;
}
finally { Fechar(); }
}

public Retorno Consultar(TipoFuncaoFuncionario Entity )
{
try
{
TipoFuncaoFuncionario TipoFuncaoFuncionario = new TipoFuncaoFuncionario ();
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("SELECT "); 
CommandSQL.AppendLine("TB_TIPO_FUNCAO_FUNCIONARIO.CODIGO, ");
CommandSQL.AppendLine("TB_TIPO_FUNCAO_FUNCIONARIO.NOME ");
CommandSQL.AppendLine("FROM TB_TIPO_FUNCAO_FUNCIONARIO ");

CommandSQL.AppendLine("WHERE TB_TIPO_FUNCAO_FUNCIONARIO.CODIGO = @CODIGO ");
Command = CriaComandoSQL(CommandSQL.ToString());
Abrir();
Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo );
Reader = Command.ExecuteReader();
while (Reader.Read())
{
TipoFuncaoFuncionario = FillEntity(Reader);
}
return new Retorno(TipoFuncaoFuncionario);
}
catch(Exception ex)
{
throw ex;
}
finally { Fechar(); }
}

#endregion

#region METODOS AUXILIARES

public Retorno VerificarExistencia(TipoFuncaoFuncionario Entity )
{
try
{
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("SELECT 1 FROM TB_TIPO_FUNCAO_FUNCIONARIO "); 
CommandSQL.AppendLine("WHERE TB_TIPO_FUNCAO_FUNCIONARIO.NOME = @NOME ");
CommandSQL.AppendLine("AND TB_TIPO_FUNCAO_FUNCIONARIO.CODIGO <> @CODIGO ");
Command = CriaComandoSQL(CommandSQL.ToString());
Abrir();
Command.Parameters.AddWithValue("@NOME", Entity.Nome );
Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo );
Reader = Command.ExecuteReader();
while (Reader.Read())
{
return new Retorno(false, String.Format(Mensagens.MSG_04, "TipoFuncaoFuncionario", "Nome"));
}
return new Retorno(true);
}
catch(Exception ex)
{
throw ex;
}
finally { Fechar(); }
}

private TipoFuncaoFuncionario FillEntity(IDataReader reader)
{
TipoFuncaoFuncionario TipoFuncaoFuncionario = new TipoFuncaoFuncionario();
try
{
TipoFuncaoFuncionario.Codigo = ConverterValorReader(reader,"CODIGO",0);
TipoFuncaoFuncionario.Nome = ConverterValorReader(reader,"NOME",String.Empty);
}
catch(Exception ex) { throw ex; }
return TipoFuncaoFuncionario;
}

#endregion

}
}

