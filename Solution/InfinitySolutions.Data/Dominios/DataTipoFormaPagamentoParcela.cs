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

 public class DataTipoFormaPagamentoParcela: DataBase
{
#region AÇÕES

public Retorno Incluir(TipoFormaPagamentoParcela Entity)
{
try
{
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("INSERT INTO TB_TIPO_FORMA_PAGAMENTO_PARCELA( ");
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

public Retorno Excluir(TipoFormaPagamentoParcela Entity)
{
try
{
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("DELETE FROM TB_TIPO_FORMA_PAGAMENTO_PARCELA WHERE CODIGO = @CODIGO");
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

public Retorno Alterar(TipoFormaPagamentoParcela Entity)
{
try
{
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("UPDATE TB_TIPO_FORMA_PAGAMENTO_PARCELA SET ");
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
List<TipoFormaPagamentoParcela> TipoFormaPagamentoParcelas = new List<TipoFormaPagamentoParcela>();
int Limite = (Pagina - 1) * QntPagina;
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("SELECT "); 
CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO_PARCELA.CODIGO, ");
CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO_PARCELA.DESCRICAO ");
CommandSQL.AppendLine("FROM TB_TIPO_FORMA_PAGAMENTO_PARCELA ");

CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
Command = CriaComandoSQL(CommandSQL.ToString());
Abrir();
Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
Command.Parameters.AddWithValue("@LIMITE", Limite);
Reader = Command.ExecuteReader();
while (Reader.Read())
{
TipoFormaPagamentoParcelas.Add(FillEntity(Reader));
}
return new Retorno(TipoFormaPagamentoParcelas);
}
catch(Exception ex)
{
throw ex;
}
finally { Fechar(); }
}

public Retorno Pesquisar(TipoFormaPagamentoParcela Entity, int Pagina, int QntPagina)
{
try
{
List<TipoFormaPagamentoParcela> TipoFormaPagamentoParcelas = new List<TipoFormaPagamentoParcela>();
int Limite = (Pagina - 1) * QntPagina;
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("SELECT "); 
CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO_PARCELA.CODIGO, ");
CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO_PARCELA.DESCRICAO ");
CommandSQL.AppendLine("FROM TB_TIPO_FORMA_PAGAMENTO_PARCELA ");

CommandSQL.AppendLine("WHERE (TB_TIPO_FORMA_PAGAMENTO_PARCELA.DESCRICAO LIKE '%" + Entity.Descricao + "%' )");
CommandSQL.AppendLine("LIMIT @QNT_PAGINA OFFSET @LIMITE");
Command = CriaComandoSQL(CommandSQL.ToString());
Abrir();
Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
Command.Parameters.AddWithValue("@LIMITE", Limite);
Reader = Command.ExecuteReader();
while (Reader.Read())
{
TipoFormaPagamentoParcelas.Add(FillEntity(Reader));
}
return new Retorno(TipoFormaPagamentoParcelas);
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
List<TipoFormaPagamentoParcela> TipoFormaPagamentoParcelas = new List<TipoFormaPagamentoParcela>();
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("SELECT "); 
CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO_PARCELA.CODIGO, ");
CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO_PARCELA.DESCRICAO ");
CommandSQL.AppendLine("FROM TB_TIPO_FORMA_PAGAMENTO_PARCELA ");

Command = CriaComandoSQL(CommandSQL.ToString());
Abrir();
Reader = Command.ExecuteReader();
while (Reader.Read())
{
TipoFormaPagamentoParcelas.Add(FillEntity(Reader));
}
return new Retorno(TipoFormaPagamentoParcelas);
}
catch(Exception ex)
{
throw ex;
}
finally { Fechar(); }
}

public Retorno Consultar(TipoFormaPagamentoParcela Entity )
{
try
{
TipoFormaPagamentoParcela TipoFormaPagamentoParcela = new TipoFormaPagamentoParcela ();
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("SELECT "); 
CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO_PARCELA.CODIGO, ");
CommandSQL.AppendLine("TB_TIPO_FORMA_PAGAMENTO_PARCELA.DESCRICAO ");
CommandSQL.AppendLine("FROM TB_TIPO_FORMA_PAGAMENTO_PARCELA ");

CommandSQL.AppendLine("WHERE TB_TIPO_FORMA_PAGAMENTO_PARCELA.CODIGO = @CODIGO ");
Command = CriaComandoSQL(CommandSQL.ToString());
Abrir();
Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo );
Reader = Command.ExecuteReader();
while (Reader.Read())
{
TipoFormaPagamentoParcela = FillEntity(Reader);
}
return new Retorno(TipoFormaPagamentoParcela);
}
catch(Exception ex)
{
throw ex;
}
finally { Fechar(); }
}

#endregion

#region METODOS AUXILIARES

public Retorno VerificarExistencia(TipoFormaPagamentoParcela Entity )
{
try
{
CommandSQL = new StringBuilder();
CommandSQL.AppendLine("SELECT 1 FROM TB_TIPO_FORMA_PAGAMENTO_PARCELA "); 
CommandSQL.AppendLine("WHERE TB_TIPO_FORMA_PAGAMENTO_PARCELA.DESCRICAO = @DESCRICAO ");
CommandSQL.AppendLine("AND TB_TIPO_FORMA_PAGAMENTO_PARCELA.CODIGO <> @CODIGO ");
Command = CriaComandoSQL(CommandSQL.ToString());
Abrir();
Command.Parameters.AddWithValue("@DESCRICAO", Entity.Descricao );
Command.Parameters.AddWithValue("@CODIGO", Entity.Codigo );
Reader = Command.ExecuteReader();
while (Reader.Read())
{
return new Retorno(false, String.Format(Mensagens.MSG_04, "TipoFormaPagamentoParcela", "Descricao"));
}
return new Retorno(true);
}
catch(Exception ex)
{
throw ex;
}
finally { Fechar(); }
}

private TipoFormaPagamentoParcela FillEntity(IDataReader reader)
{
TipoFormaPagamentoParcela TipoFormaPagamentoParcela = new TipoFormaPagamentoParcela();
try
{
TipoFormaPagamentoParcela.Codigo = ConverterValorReader(reader,"CODIGO",0);
TipoFormaPagamentoParcela.Descricao = ConverterValorReader(reader,"DESCRICAO",String.Empty);
}
catch(Exception ex) { throw ex; }
return TipoFormaPagamentoParcela;
}

#endregion

}
}

