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

    public class DataErro : DataBase, IData<Erro>
    {
        public Retorno Salvar(Erro Entity)
        {
            try
            {
                //Util.Print(Entity);

                CommandSQL = new StringBuilder();
                CommandSQL.Append("INSERT INTO TB_ERRO( ");
                CommandSQL.Append("DESCRICAO, ");
                CommandSQL.Append("CASO_DE_USO, ");
                CommandSQL.Append("CAMADA, ");
                CommandSQL.Append("FUNCIONALIDADE, ");
                CommandSQL.Append("IMAGEM, ");
                CommandSQL.Append("DATA, ");
                CommandSQL.Append("ENTIDADE) ");
                CommandSQL.Append("VALUES ( ");
                CommandSQL.Append("@DESCRICAO, ");
                CommandSQL.Append("@CASO_DE_USO, ");
                CommandSQL.Append("@CAMADA, ");
                CommandSQL.Append("@FUNCIONALIDADE, ");
                CommandSQL.Append("@IMAGEM, ");
                CommandSQL.Append("@DATA, ");
                CommandSQL.Append("@ENTIDADE) ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@DESCRICAO", Entity.Descricao);
                Command.Parameters.AddWithValue("@CASO_DE_USO", Entity.CasoDeUso);
                Command.Parameters.AddWithValue("@CAMADA", Entity.Camada);
                Command.Parameters.AddWithValue("@FUNCIONALIDADE", Entity.Funcionalidade);
                Command.Parameters.AddWithValue("@IMAGEM", Entity.Imagem);
                Command.Parameters.AddWithValue("@DATA", DateTime.Now);
                Command.Parameters.AddWithValue("@ENTIDADE", Entity.Entidade);
                Abrir();
                Command.ExecuteNonQuery();
                return new Retorno(true, String.Format(Mensagens.MSG_02, "Salvo ", "Erro"));
            }
            catch (Exception ex)
            {
                Erro erro = new Erro()
                {
                    Descricao = ex.Message,
                    Imagem = Guid.NewGuid().ToString(),
                    CasoDeUso = EnumCasoDeUso.MANTER_ERRO,
                    Camada = EnumCamada.DATA,
                    Funcionalidade = EnumFuncionalidade.SALVAR,
                    Entidade = "Erro"
                };
                if (Internet.Conectado())
                {
                    ISEmail.EnviarErro(erro);
                }

                return new Retorno(false, String.Format(Mensagens.MSG_05, ex.Message, "Salvar Erro"));
            }
            finally { Fechar(); }
        }

        public Retorno Excluir(Erro Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.Append("DELETE FROM TB_ERRO WHERE ID = @ID");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@ID", Entity.Id);
                Abrir();
                Command.ExecuteNonQuery();
                return new Retorno(true, String.Format(Mensagens.MSG_02, "Excluido ", "Erro"));
            }
            catch (Exception ex)
            {
                Erro erro = new Erro()
                {
                    Descricao = ex.Message,
                    Imagem = Guid.NewGuid().ToString(),
                    CasoDeUso = EnumCasoDeUso.MANTER_ERRO,
                    Camada = EnumCamada.DATA,
                    Funcionalidade = EnumFuncionalidade.EXCLUIR,
                    Entidade = "Erro"
                };
                if (Internet.Conectado())
                {
                    ISEmail.EnviarErro(erro);
                }

                return new Retorno(false, String.Format(Mensagens.MSG_05, ex.Message, "Excluir Erro"));
            }
            finally { Fechar(); }
        }

        public Retorno Listar(int Pagina, int QntPagina)
        {
            try
            {
                List<Erro> Erros = new List<Erro>();
                int Limite = (Pagina - 1) * QntPagina;
                CommandSQL = new StringBuilder();
                CommandSQL.Append("SELECT ");
                CommandSQL.Append("TB_ERRO.ID, ");
                CommandSQL.Append("TB_ERRO.DESCRICAO, ");
                CommandSQL.Append("TB_ERRO.CASO_DE_USO, ");
                CommandSQL.Append("TB_ERRO.CAMADA, ");
                CommandSQL.Append("TB_ERRO.FUNCIONALIDADE, ");
                CommandSQL.Append("TB_ERRO.ENTIDADE ");
                CommandSQL.Append("FROM TB_ERRO ");

                CommandSQL.Append("LIMIT @QNT_PAGINA OFFSET @LIMITE");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@QNT_PAGINA", QntPagina);
                Command.Parameters.AddWithValue("@LIMITE", Limite);
                MySqlDataReader reader = Command.ExecuteReader();
                while (reader.Read())
                {
                    Erros.Add(FillEntity(reader));
                }
                return new Retorno(Erros);
            }
            catch (Exception ex)
            {
                Erro erro = new Erro()
                {
                    Descricao = ex.Message,
                    Imagem = Guid.NewGuid().ToString(),
                    CasoDeUso = EnumCasoDeUso.MANTER_ERRO,
                    Camada = EnumCamada.DATA,
                    Funcionalidade = EnumFuncionalidade.LISTAR_PAGINADO,
                    Entidade = "Erro"
                };
                if (Internet.Conectado())
                {
                    ISEmail.EnviarErro(erro);
                }

                return new Retorno(false, String.Format(Mensagens.MSG_05, ex.Message, "Listar Erro"));
            }
            finally { Fechar(); }
        }

        public Retorno Listar()
        {
            try
            {
                List<Erro> Erros = new List<Erro>();
                CommandSQL = new StringBuilder();
                CommandSQL.Append("SELECT ");
                CommandSQL.Append("TB_ERRO.ID, ");
                CommandSQL.Append("TB_ERRO.DESCRICAO, ");
                CommandSQL.Append("TB_ERRO.CASO_DE_USO, ");
                CommandSQL.Append("TB_ERRO.CAMADA, ");
                CommandSQL.Append("TB_ERRO.FUNCIONALIDADE, ");
                CommandSQL.Append("TB_ERRO.ENTIDADE ");
                CommandSQL.Append("FROM TB_ERRO ");

                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                MySqlDataReader reader = Command.ExecuteReader();
                while (reader.Read())
                {
                    Erros.Add(FillEntity(reader));
                }
                return new Retorno(Erros);
            }
            catch (Exception ex)
            {
                Erro erro = new Erro()
                {
                    Descricao = ex.Message,
                    Imagem = Guid.NewGuid().ToString(),
                    CasoDeUso = EnumCasoDeUso.MANTER_ERRO,
                    Camada = EnumCamada.DATA,
                    Funcionalidade = EnumFuncionalidade.LISTAR,
                    Entidade = "Erro"
                };
                if (Internet.Conectado())
                {
                    ISEmail.EnviarErro(erro);
                }

                return new Retorno(false, String.Format(Mensagens.MSG_05, ex.Message, "Listar Erro"));
            }
            finally { Fechar(); }
        }

        public Retorno Carregar(Erro Entity)
        {
            try
            {
                Erro Erro = new Erro();
                CommandSQL = new StringBuilder();
                CommandSQL.Append("SELECT ");
                CommandSQL.Append("TB_ERRO.ID, ");
                CommandSQL.Append("TB_ERRO.DESCRICAO, ");
                CommandSQL.Append("TB_ERRO.CASO_DE_USO, ");
                CommandSQL.Append("TB_ERRO.CAMADA, ");
                CommandSQL.Append("TB_ERRO.FUNCIONALIDADE, ");
                CommandSQL.Append("TB_ERRO.ENTIDADE ");
                CommandSQL.Append("FROM TB_ERRO ");

                CommandSQL.Append("WHERE TB_ERRO.ID = @ID ");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Abrir();
                Command.Parameters.AddWithValue("@ID", Entity.Id);
                MySqlDataReader reader = Command.ExecuteReader();
                while (reader.Read())
                {
                    Erro = FillEntity(reader);
                }
                return new Retorno(Erro);
            }
            catch (Exception ex)
            {
                Erro erro = new Erro()
                {
                    Descricao = ex.Message,
                    Imagem = Guid.NewGuid().ToString(),
                    CasoDeUso = EnumCasoDeUso.MANTER_ERRO,
                    Camada = EnumCamada.DATA,
                    Funcionalidade = EnumFuncionalidade.CARREGAR,
                    Entidade = "Erro"
                };
                if (Internet.Conectado())
                {
                    ISEmail.EnviarErro(erro);
                }

                return new Retorno(false, String.Format(Mensagens.MSG_05, ex.Message, "Carregar Erro"));
            }
            finally { Fechar(); }
        }

        public Retorno Alterar(Erro Entity)
        {
            try
            {
                CommandSQL = new StringBuilder();
                CommandSQL.Append("UPDATE TB_ERRO SET ");
                CommandSQL.Append("DESCRICAO = @DESCRICAO, ");
                CommandSQL.Append("CASO_DE_USO = @CASO_DE_USO, ");
                CommandSQL.Append("CAMADA = @CAMADA, ");
                CommandSQL.Append("FUNCIONALIDADE = @FUNCIONALIDADE, ");
                CommandSQL.Append("ENTIDADE = @ENTIDADE ");
                CommandSQL.Append("WHERE ID = @ID");
                Command = CriaComandoSQL(CommandSQL.ToString());
                Command.Parameters.AddWithValue("@ID", Entity.Id);
                Command.Parameters.AddWithValue("@DESCRICAO", Entity.Id);
                Command.Parameters.AddWithValue("@CASO_DE_USO", Entity.Id);
                Command.Parameters.AddWithValue("@CAMADA", Entity.Id);
                Command.Parameters.AddWithValue("@FUNCIONALIDADE", Entity.Id);
                Command.Parameters.AddWithValue("@ENTIDADE", Entity.Id);
                Abrir();
                Command.ExecuteNonQuery();
                return new Retorno(true, String.Format(Mensagens.MSG_02, "Alterado ", "Erro"));
            }
            catch (Exception ex)
            {
                Erro erro = new Erro()
                {
                    Descricao = ex.Message,
                    Imagem = Guid.NewGuid().ToString(),
                    CasoDeUso = EnumCasoDeUso.MANTER_ERRO,
                    Camada = EnumCamada.DATA,
                    Funcionalidade = EnumFuncionalidade.ALTERAR,
                    Entidade = "Erro"
                };
                if (Internet.Conectado())
                {
                    ISEmail.EnviarErro(erro);
                }

                return new Retorno(false, String.Format(Mensagens.MSG_05, ex.Message, "Alterar Erro"));
            }
            finally { Fechar(); }
        }

        public Erro FillEntity(IDataReader reader)
        {
            Erro Erro = new Erro();
            try
            {
                Erro.Id = reader["ID"].ConverteValor<int>(0);
                Erro.Descricao = reader["DESCRICAO"].ConverteValor<string>(String.Empty);
                Erro.CasoDeUso = (EnumCasoDeUso)reader["CASO_DE_USO"].ConverteValor<int>(0);
                Erro.Camada = (EnumCamada)reader["CAMADA"].ConverteValor<int>(0);
                Erro.Funcionalidade = (EnumFuncionalidade)reader["FUNCIONALIDADE"].ConverteValor<int>(0);
                Erro.Entidade = reader["ENTIDADE"].ConverteValor<string>(String.Empty);
            }
            catch (Exception) { throw; }
            return Erro;
        }

    }
}

