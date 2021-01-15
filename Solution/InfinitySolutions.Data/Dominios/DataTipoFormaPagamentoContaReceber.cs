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

 public class DataTipoFormaPagamentoContaReceber: DataBase
{
#region AÇÕES

public Retorno Incluir(TipoFormaPagamentoContaReceber Entity)
{
try
{
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("INSERT INTO TB_TIPO_FORMA_PAGAMENTO_CONTA_RECEBER( ");
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

public Retorno Excluir(TipoFormaPagamentoContaReceber Entity)
{
try
{
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("DELETE FROM TB_TIPO_FORMA_PAGAMENTO_CONTA_RECEBER WHERE CODIGO = @CODIGO");
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

public Retorno Alterar(TipoFormaPagamentoContaReceber Entity)
{
try
{
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("UPDATE TB_TIPO_FORMA_PAGAMENTO_CONTA_RECEBER SET ");
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
List<TipoFormaPagamentoContaReceber> TipoFormaPagamentoContaRecebers = new List<TipoFormaPagamentoContaReceber>();
int Limite = (Pagina - 1) * QntPagina;
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("SELECT "); 
CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO_CONTA_RECEBER.CODIGO, ");
CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO_CONTA_RECEBER.DESCRICAO ");
CommandSQL.AppendLine("FROM TB_TIPO_FORMA_PAGAMENTO_CONTA_RECEBER ");

CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
Command = CriaComandoSQL(CommandSQL.ToString());
Abrir();
Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
Command.Parameters.AddWithValue("@LIMITE", Limite);
Reader = Command.ExecuteReader();
while (Reader.Read())
{
TipoFormaPagamentoContaRecebers.Add(FillEntity(Reader));
}
return new Retorno(TipoFormaPagamentoContaRecebers);
}
catch(Exception ex)
{
throw ex;
}
finally { Fechar(); }
}

public Retorno Pesquisar(TipoFormaPagamentoContaReceber Entity, int Pagina, int QntPagina)
{
try
{
List<TipoFormaPagamentoContaReceber> TipoFormaPagamentoContaRecebers = new List<TipoFormaPagamentoContaReceber>();
int Limite = (Pagina - 1) * QntPagina;
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("SELECT "); 
CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO_CONTA_RECEBER.CODIGO, ");
CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO_CONTA_RECEBER.DESCRICAO ");
CommandSQL.AppendLine("FROM TB_TIPO_FORMA_PAGAMENTO_CONTA_RECEBER ");

CommandSQL.AppendLine("WHERE (TB_TIPO_FORMA_PAGAMENTO_CONTA_RECEBER.DESCRICAO LIKE '%" + Entity.Descricao + "%' )");
CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
Command = CriaComandoSQL(CommandSQL.ToString());
Abrir();
Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
Command.Parameters.AddWithValue("@LIMITE", Limite);
Reader = Command.ExecuteReader();
while (Reader.Read())
{
TipoFormaPagamentoContaRecebers.Add(FillEntity(Reader));
}
return new Retorno(TipoFormaPagamentoContaRecebers);
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
List<TipoFormaPagamentoContaReceber> TipoFormaPagamentoContaRecebers = new List<TipoFormaPagamentoContaReceber>();
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("SELECT "); 
CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO_CONTA_RECEBER.CODIGO, ");
CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO_CONTA_RECEBER.DESCRICAO ");
CommandSQL.AppendLine("FROM TB_TIPO_FORMA_PAGAMENTO_CONTA_RECEBER ");

Command = CriaComandoSQL(CommandSQL.ToString());
Abrir();
Reader = Command.ExecuteReader();
while (Reader.Read())
{
TipoFormaPagamentoContaRecebers.Add(FillEntity(Reader));
}
return new Retorno(TipoFormaPagamentoContaRecebers);
}
catch(Exception ex)
{
throw ex;
}
finally { Fechar(); }
}

public Retorno Consultar(TipoFormaPagamentoContaReceber Entity )
{
try
{
TipoFormaPagamentoContaReceber TipoFormaPagamentoContaReceber = new TipoFormaPagamentoContaReceber ();
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("SELECT "); 
CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO_CONTA_RECEBER.CODIGO, ");
CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO_CONTA_RECEBER.DESCRICAO ");
CommandSQL.AppendLine("FROM TB_TIPO_FORMA_PAGAMENTO_CONTA_RECEBER ");

CommandSQL.AppendLine("WHERE TB_TIPO_FORMA_PAGAMENTO_CONTA_RECEBER.CODIGO = @CODIGO ");
Command = CriaComandoSQL(CommandSQL.ToString());
Abrir();
Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo );
Reader = Command.ExecuteReader();
while (Reader.Read())
{
TipoFormaPagamentoContaReceber = FillEntity(Reader);
}
return new Retorno(TipoFormaPagamentoContaReceber);
}
catch(Exception ex)
{
throw ex;
}
finally { Fechar(); }
}

#endregion

#region METODOS AUXILIARES

public Retorno VerificarExistencia(TipoFormaPagamentoContaReceber Entity )
{
try
{
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("SELECT 1 FROM TB_TIPO_FORMA_PAGAMENTO_CONTA_RECEBER "); 
CommandSQL.AppendLine("WHERE TB_TIPO_FORMA_PAGAMENTO_CONTA_RECEBER.DESCRICAO = @DESCRICAO ");
CommandSQL.AppendLine("AND TB_TIPO_FORMA_PAGAMENTO_CONTA_RECEBER.CODIGO <> @CODIGO ");
Command = CriaComandoSQL(CommandSQL.ToString());
Abrir();
Command.Parameters.AddWithValue("@DESCRICAO", Entity.Descricao );
Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo );
Reader = Command.ExecuteReader();
while (Reader.Read())
{
return new Retorno(false, String.Format(Mensagens.MSG_04, "TipoFormaPagamentoContaReceber", "Descricao"));
}
return new Retorno(true);
}
catch(Exception ex)
{
throw ex;
}
finally { Fechar(); }
}

private TipoFormaPagamentoContaReceber FillEntity(IDataReader reader)
{
TipoFormaPagamentoContaReceber TipoFormaPagamentoContaReceber = new TipoFormaPagamentoContaReceber();
try
{
TipoFormaPagamentoContaReceber.Codigo = ConverterValorReader(reader,"CODIGO",0);
TipoFormaPagamentoContaReceber.Descricao = ConverterValorReader(reader,"DESCRICAO",String.Empty);
}
catch(Exception ex) { throw ex; }
return TipoFormaPagamentoContaReceber;
}

#endregion

}
}

