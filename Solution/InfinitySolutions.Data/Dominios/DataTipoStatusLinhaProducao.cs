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

 public class DataTipoStatusLinhaProducao: DataBase
{
#region AÇÕES

public Retorno Incluir(TipoStatusLinhaProducao Entity)
{
try
{
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("INSERT INTO TB_TIPO_STATUS_LINHA_PRODUCAO( ");
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

public Retorno Excluir(TipoStatusLinhaProducao Entity)
{
try
{
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("DELETE FROM TB_TIPO_STATUS_LINHA_PRODUCAO WHERE CODIGO = @CODIGO");
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

public Retorno Alterar(TipoStatusLinhaProducao Entity)
{
try
{
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("UPDATE TB_TIPO_STATUS_LINHA_PRODUCAO SET ");
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
List<TipoStatusLinhaProducao> TipoStatusLinhaProducaos = new List<TipoStatusLinhaProducao>();
int Limite = (Pagina - 1) * QntPagina;
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("SELECT "); 
CommandSQL.AppendLine("TB_TIPO_STATUS_LINHA_PRODUCAO.CODIGO, ");
CommandSQL.AppendLine("TB_TIPO_STATUS_LINHA_PRODUCAO.DESCRICAO ");
CommandSQL.AppendLine("FROM TB_TIPO_STATUS_LINHA_PRODUCAO ");

CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
Command = CriaComandoSQL(CommandSQL.ToString());
Abrir();
Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
Command.Parameters.AddWithValue("@LIMITE", Limite);
Reader = Command.ExecuteReader();
while (Reader.Read())
{
TipoStatusLinhaProducaos.Add(FillEntity(Reader));
}
return new Retorno(TipoStatusLinhaProducaos);
}
catch(Exception ex)
{
throw ex;
}
finally { Fechar(); }
}

public Retorno Pesquisar(TipoStatusLinhaProducao Entity, int Pagina, int QntPagina)
{
try
{
List<TipoStatusLinhaProducao> TipoStatusLinhaProducaos = new List<TipoStatusLinhaProducao>();
int Limite = (Pagina - 1) * QntPagina;
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("SELECT "); 
CommandSQL.AppendLine("TB_TIPO_STATUS_LINHA_PRODUCAO.CODIGO, ");
CommandSQL.AppendLine("TB_TIPO_STATUS_LINHA_PRODUCAO.DESCRICAO ");
CommandSQL.AppendLine("FROM TB_TIPO_STATUS_LINHA_PRODUCAO ");

CommandSQL.AppendLine("WHERE (TB_TIPO_STATUS_LINHA_PRODUCAO.DESCRICAO LIKE '%" + Entity.Descricao + "%' )");
CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
Command = CriaComandoSQL(CommandSQL.ToString());
Abrir();
Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
Command.Parameters.AddWithValue("@LIMITE", Limite);
Reader = Command.ExecuteReader();
while (Reader.Read())
{
TipoStatusLinhaProducaos.Add(FillEntity(Reader));
}
return new Retorno(TipoStatusLinhaProducaos);
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
List<TipoStatusLinhaProducao> TipoStatusLinhaProducaos = new List<TipoStatusLinhaProducao>();
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("SELECT "); 
CommandSQL.AppendLine("TB_TIPO_STATUS_LINHA_PRODUCAO.CODIGO, ");
CommandSQL.AppendLine("TB_TIPO_STATUS_LINHA_PRODUCAO.DESCRICAO ");
CommandSQL.AppendLine("FROM TB_TIPO_STATUS_LINHA_PRODUCAO ");

Command = CriaComandoSQL(CommandSQL.ToString());
Abrir();
Reader = Command.ExecuteReader();
while (Reader.Read())
{
TipoStatusLinhaProducaos.Add(FillEntity(Reader));
}
return new Retorno(TipoStatusLinhaProducaos);
}
catch(Exception ex)
{
throw ex;
}
finally { Fechar(); }
}

public Retorno Consultar(TipoStatusLinhaProducao Entity )
{
try
{
TipoStatusLinhaProducao TipoStatusLinhaProducao = new TipoStatusLinhaProducao ();
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("SELECT "); 
CommandSQL.AppendLine("TB_TIPO_STATUS_LINHA_PRODUCAO.CODIGO, ");
CommandSQL.AppendLine("TB_TIPO_STATUS_LINHA_PRODUCAO.DESCRICAO ");
CommandSQL.AppendLine("FROM TB_TIPO_STATUS_LINHA_PRODUCAO ");

CommandSQL.AppendLine("WHERE TB_TIPO_STATUS_LINHA_PRODUCAO.CODIGO = @CODIGO ");
Command = CriaComandoSQL(CommandSQL.ToString());
Abrir();
Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo );
Reader = Command.ExecuteReader();
while (Reader.Read())
{
TipoStatusLinhaProducao = FillEntity(Reader);
}
return new Retorno(TipoStatusLinhaProducao);
}
catch(Exception ex)
{
throw ex;
}
finally { Fechar(); }
}

#endregion

#region METODOS AUXILIARES

public Retorno VerificarExistencia(TipoStatusLinhaProducao Entity )
{
try
{
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("SELECT 1 FROM TB_TIPO_STATUS_LINHA_PRODUCAO "); 
CommandSQL.AppendLine("WHERE TB_TIPO_STATUS_LINHA_PRODUCAO.DESCRICAO = @DESCRICAO ");
CommandSQL.AppendLine("AND TB_TIPO_STATUS_LINHA_PRODUCAO.CODIGO <> @CODIGO ");
Command = CriaComandoSQL(CommandSQL.ToString());
Abrir();
Command.Parameters.AddWithValue("@DESCRICAO", Entity.Descricao );
Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo );
Reader = Command.ExecuteReader();
while (Reader.Read())
{
return new Retorno(false, String.Format(Mensagens.MSG_04, "TipoStatusLinhaProducao", "Descricao"));
}
return new Retorno(true);
}
catch(Exception ex)
{
throw ex;
}
finally { Fechar(); }
}

private TipoStatusLinhaProducao FillEntity(IDataReader reader)
{
TipoStatusLinhaProducao TipoStatusLinhaProducao = new TipoStatusLinhaProducao();
try
{
TipoStatusLinhaProducao.Codigo = ConverterValorReader(reader,"CODIGO",0);
TipoStatusLinhaProducao.Descricao = ConverterValorReader(reader,"DESCRICAO",String.Empty);
}
catch(Exception ex) { throw ex; }
return TipoStatusLinhaProducao;
}

#endregion

}
}

