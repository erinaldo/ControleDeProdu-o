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

 public class DataTipoStatusParcela: DataBase
{
#region AÇÕES

public Retorno Incluir(TipoStatusParcela Entity)
{
try
{
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("INSERT INTO TB_TIPO_STATUS_PARCELA( ");
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

public Retorno Excluir(TipoStatusParcela Entity)
{
try
{
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("DELETE FROM TB_TIPO_STATUS_PARCELA WHERE CODIGO = @CODIGO");
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

public Retorno Alterar(TipoStatusParcela Entity)
{
try
{
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("UPDATE TB_TIPO_STATUS_PARCELA SET ");
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
List<TipoStatusParcela> TipoStatusParcelas = new List<TipoStatusParcela>();
int Limite = (Pagina - 1) * QntPagina;
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("SELECT "); 
CommandSQL.AppendLine("TB_TIPO_STATUS_PARCELA.CODIGO, ");
CommandSQL.AppendLine("TB_TIPO_STATUS_PARCELA.DESCRICAO ");
CommandSQL.AppendLine("FROM TB_TIPO_STATUS_PARCELA ");

CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
Command = CriaComandoSQL(CommandSQL.ToString());
Abrir();
Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
Command.Parameters.AddWithValue("@LIMITE", Limite);
Reader = Command.ExecuteReader();
while (Reader.Read())
{
TipoStatusParcelas.Add(FillEntity(Reader));
}
return new Retorno(TipoStatusParcelas);
}
catch(Exception ex)
{
throw ex;
}
finally { Fechar(); }
}

public Retorno Pesquisar(TipoStatusParcela Entity, int Pagina, int QntPagina)
{
try
{
List<TipoStatusParcela> TipoStatusParcelas = new List<TipoStatusParcela>();
int Limite = (Pagina - 1) * QntPagina;
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("SELECT "); 
CommandSQL.AppendLine("TB_TIPO_STATUS_PARCELA.CODIGO, ");
CommandSQL.AppendLine("TB_TIPO_STATUS_PARCELA.DESCRICAO ");
CommandSQL.AppendLine("FROM TB_TIPO_STATUS_PARCELA ");

CommandSQL.AppendLine("WHERE (TB_TIPO_STATUS_PARCELA.DESCRICAO LIKE '%" + Entity.Descricao + "%' )");
CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
Command = CriaComandoSQL(CommandSQL.ToString());
Abrir();
Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
Command.Parameters.AddWithValue("@LIMITE", Limite);
Reader = Command.ExecuteReader();
while (Reader.Read())
{
TipoStatusParcelas.Add(FillEntity(Reader));
}
return new Retorno(TipoStatusParcelas);
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
List<TipoStatusParcela> TipoStatusParcelas = new List<TipoStatusParcela>();
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("SELECT "); 
CommandSQL.AppendLine("TB_TIPO_STATUS_PARCELA.CODIGO, ");
CommandSQL.AppendLine("TB_TIPO_STATUS_PARCELA.DESCRICAO ");
CommandSQL.AppendLine("FROM TB_TIPO_STATUS_PARCELA ");

Command = CriaComandoSQL(CommandSQL.ToString());
Abrir();
Reader = Command.ExecuteReader();
while (Reader.Read())
{
TipoStatusParcelas.Add(FillEntity(Reader));
}
return new Retorno(TipoStatusParcelas);
}
catch(Exception ex)
{
throw ex;
}
finally { Fechar(); }
}

public Retorno Consultar(TipoStatusParcela Entity )
{
try
{
TipoStatusParcela TipoStatusParcela = new TipoStatusParcela ();
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("SELECT "); 
CommandSQL.AppendLine("TB_TIPO_STATUS_PARCELA.CODIGO, ");
CommandSQL.AppendLine("TB_TIPO_STATUS_PARCELA.DESCRICAO ");
CommandSQL.AppendLine("FROM TB_TIPO_STATUS_PARCELA ");

CommandSQL.AppendLine("WHERE TB_TIPO_STATUS_PARCELA.CODIGO = @CODIGO ");
Command = CriaComandoSQL(CommandSQL.ToString());
Abrir();
Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo );
Reader = Command.ExecuteReader();
while (Reader.Read())
{
TipoStatusParcela = FillEntity(Reader);
}
return new Retorno(TipoStatusParcela);
}
catch(Exception ex)
{
throw ex;
}
finally { Fechar(); }
}

#endregion

#region METODOS AUXILIARES

public Retorno VerificarExistencia(TipoStatusParcela Entity )
{
try
{
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("SELECT 1 FROM TB_TIPO_STATUS_PARCELA "); 
CommandSQL.AppendLine("WHERE TB_TIPO_STATUS_PARCELA.DESCRICAO = @DESCRICAO ");
CommandSQL.AppendLine("AND TB_TIPO_STATUS_PARCELA.CODIGO <> @CODIGO ");
Command = CriaComandoSQL(CommandSQL.ToString());
Abrir();
Command.Parameters.AddWithValue("@DESCRICAO", Entity.Descricao );
Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo );
Reader = Command.ExecuteReader();
while (Reader.Read())
{
return new Retorno(false, String.Format(Mensagens.MSG_04, "TipoStatusParcela", "Descricao"));
}
return new Retorno(true);
}
catch(Exception ex)
{
throw ex;
}
finally { Fechar(); }
}

private TipoStatusParcela FillEntity(IDataReader reader)
{
TipoStatusParcela TipoStatusParcela = new TipoStatusParcela();
try
{
TipoStatusParcela.Codigo = ConverterValorReader(reader,"CODIGO",0);
TipoStatusParcela.Descricao = ConverterValorReader(reader,"DESCRICAO",String.Empty);
}
catch(Exception ex) { throw ex; }
return TipoStatusParcela;
}

#endregion

}
}

