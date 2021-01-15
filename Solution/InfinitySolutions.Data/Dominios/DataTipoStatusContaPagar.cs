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

 public class DataTipoStatusContaPagar: DataBase
{
#region AÇÕES

public Retorno Incluir(TipoStatusContaPagar Entity)
{
try
{
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("INSERT INTO TB_TIPO_STATUS_CONTA_PAGAR( ");
CommandSQL.AppendLine("DESCRICAO) ");
CommandSQL.AppendLine("VALUES (");
CommandSQL.AppendLine("@DESCRICAO) ");
Command = CriaComandoSQL(CommandSQL.ToString());
Command.Parameters.AddWithValue("@DESCRICAO",Entity.Descricao);
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

public Retorno Excluir(TipoStatusContaPagar Entity)
{
try
{
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("DELETE FROM TB_TIPO_STATUS_CONTA_PAGAR WHERE CODIGO = @CODIGO");
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

public Retorno Alterar(TipoStatusContaPagar Entity)
{
try
{
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("UPDATE TB_TIPO_STATUS_CONTA_PAGAR SET ");
CommandSQL.AppendLine("DESCRICAO = @DESCRICAO ");
CommandSQL.AppendLine("WHERE CODIGO = @CODIGO");
Command = CriaComandoSQL(CommandSQL.ToString());
Command.Parameters.AddWithValue("@CODIGO",Entity.Codigo);
Command.Parameters.AddWithValue("@DESCRICAO",Entity.Descricao);
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
List<TipoStatusContaPagar> TipoStatusContaPagars = new List<TipoStatusContaPagar>();
int Limite = (Pagina - 1) * QntPagina;
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("SELECT "); 
CommandSQL.AppendLine("TB_TIPO_STATUS_CONTA_PAGAR.CODIGO, ");
CommandSQL.AppendLine("TB_TIPO_STATUS_CONTA_PAGAR.DESCRICAO ");
CommandSQL.AppendLine("FROM TB_TIPO_STATUS_CONTA_PAGAR ");

CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
Command = CriaComandoSQL(CommandSQL.ToString());
Abrir();
Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
Command.Parameters.AddWithValue("@LIMITE", Limite);
Reader = Command.ExecuteReader();
while (Reader.Read())
{
TipoStatusContaPagars.Add(FillEntity(Reader));
}
return new Retorno(TipoStatusContaPagars);
}
catch(Exception ex)
{
throw ex;
}
finally { Fechar(); }
}

public Retorno Pesquisar(TipoStatusContaPagar Entity, int Pagina, int QntPagina)
{
try
{
List<TipoStatusContaPagar> TipoStatusContaPagars = new List<TipoStatusContaPagar>();
int Limite = (Pagina - 1) * QntPagina;
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("SELECT "); 
CommandSQL.AppendLine("TB_TIPO_STATUS_CONTA_PAGAR.CODIGO, ");
CommandSQL.AppendLine("TB_TIPO_STATUS_CONTA_PAGAR.DESCRICAO ");
CommandSQL.AppendLine("FROM TB_TIPO_STATUS_CONTA_PAGAR ");

CommandSQL.AppendLine("WHERE (TB_TIPO_STATUS_CONTA_PAGAR.DESCRICAO LIKE '%" + Entity.Descricao + "%' )");
CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
Command = CriaComandoSQL(CommandSQL.ToString());
Abrir();
Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
Command.Parameters.AddWithValue("@LIMITE", Limite);
Reader = Command.ExecuteReader();
while (Reader.Read())
{
TipoStatusContaPagars.Add(FillEntity(Reader));
}
return new Retorno(TipoStatusContaPagars);
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
List<TipoStatusContaPagar> TipoStatusContaPagars = new List<TipoStatusContaPagar>();
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("SELECT "); 
CommandSQL.AppendLine("TB_TIPO_STATUS_CONTA_PAGAR.CODIGO, ");
CommandSQL.AppendLine("TB_TIPO_STATUS_CONTA_PAGAR.DESCRICAO ");
CommandSQL.AppendLine("FROM TB_TIPO_STATUS_CONTA_PAGAR ");

Command = CriaComandoSQL(CommandSQL.ToString());
Abrir();
Reader = Command.ExecuteReader();
while (Reader.Read())
{
TipoStatusContaPagars.Add(FillEntity(Reader));
}
return new Retorno(TipoStatusContaPagars);
}
catch(Exception ex)
{
throw ex;
}
finally { Fechar(); }
}

public Retorno Consultar(TipoStatusContaPagar Entity )
{
try
{
TipoStatusContaPagar TipoStatusContaPagar = new TipoStatusContaPagar ();
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("SELECT "); 
CommandSQL.AppendLine("TB_TIPO_STATUS_CONTA_PAGAR.CODIGO, ");
CommandSQL.AppendLine("TB_TIPO_STATUS_CONTA_PAGAR.DESCRICAO ");
CommandSQL.AppendLine("FROM TB_TIPO_STATUS_CONTA_PAGAR ");

CommandSQL.AppendLine("WHERE TB_TIPO_STATUS_CONTA_PAGAR.CODIGO = @CODIGO ");
Command = CriaComandoSQL(CommandSQL.ToString());
Abrir();
Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo );
Reader = Command.ExecuteReader();
while (Reader.Read())
{
TipoStatusContaPagar = FillEntity(Reader);
}
return new Retorno(TipoStatusContaPagar);
}
catch(Exception ex)
{
throw ex;
}
finally { Fechar(); }
}

#endregion

#region METODOS AUXILIARES

public Retorno VerificarExistencia(TipoStatusContaPagar Entity )
{
try
{
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("SELECT 1 FROM TB_TIPO_STATUS_CONTA_PAGAR "); 
CommandSQL.AppendLine("WHERE TB_TIPO_STATUS_CONTA_PAGAR.DESCRICAO = @DESCRICAO ");
CommandSQL.AppendLine("AND TB_TIPO_STATUS_CONTA_PAGAR.CODIGO <> @CODIGO ");
Command = CriaComandoSQL(CommandSQL.ToString());
Abrir();
Command.Parameters.AddWithValue("@DESCRICAO", Entity.Descricao );
Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo );
Reader = Command.ExecuteReader();
while (Reader.Read())
{
return new Retorno(false, String.Format(Mensagens.MSG_04, "TipoStatusContaPagar", "Descricao"));
}
return new Retorno(true);
}
catch(Exception ex)
{
throw ex;
}
finally { Fechar(); }
}

private TipoStatusContaPagar FillEntity(IDataReader reader)
{
TipoStatusContaPagar TipoStatusContaPagar = new TipoStatusContaPagar();
try
{
TipoStatusContaPagar.Codigo = ConverterValorReader(reader,"CODIGO",0);
TipoStatusContaPagar.Descricao = ConverterValorReader(reader,"DESCRICAO",String.Empty);
}
catch(Exception ex) { throw ex; }
return TipoStatusContaPagar;
}

#endregion

}
}

