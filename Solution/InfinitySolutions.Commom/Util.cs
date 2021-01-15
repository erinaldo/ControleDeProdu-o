using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using InfinitySolutions.Entity;
using System.Configuration;
using System.Drawing;

namespace InfinitySolutions.Commom
{
    public static class Util
    {
        public static string RecuperaDescricaoEnum(this object value)
        {
            Type objType = value.GetType();
            FieldInfo[] propriedades = objType.GetFields();
            FieldInfo field = propriedades.First(p => p.Name == value.ToString());
            return ((DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), false))[0].Description;
        }

        public static T ConverteValor<T>(this object value, T defaultValue)
        {
            if (value != null)
                value = value.ToString().Replace(".", ",");
            T result;
            try
            {
                var requestedType = typeof(T);
                var nullableType = Nullable.GetUnderlyingType(requestedType);

                if (nullableType != null)
                {
                    if (value == null)
                    {
                        result = defaultValue;
                    }
                    else
                    {
                        result = (T)Convert.ChangeType(value, nullableType);
                    }
                }
                else
                {
                    result = (T)Convert.ChangeType(value, requestedType);
                }
            }
            catch
            {
                result = defaultValue;
            }
            return result;
        }

        public static string ReduzString(this string palavra, int tamanho)
        {
            if (palavra.Length > tamanho)
                return palavra.Substring(0, tamanho);
            else
                return palavra;
        }

        public static Retorno VerificaMedidas(int Width, int Height)
        {
            try
            {
                if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["WIDTH"]) || String.IsNullOrEmpty(ConfigurationManager.AppSettings["HEIGHT"]))
                {
                    System.Configuration.Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                    config.AppSettings.Settings.Remove("WIDTH");
                    config.AppSettings.Settings.Add("WIDTH", Width.ToString());

                    config.AppSettings.Settings.Remove("HEIGHT");
                    config.AppSettings.Settings.Add("HEIGHT", Height.ToString());

                    config.Save(ConfigurationSaveMode.Modified);
                    ConfigurationManager.RefreshSection("appSettings");
                }
                return new Retorno(true);
            }
            catch (Exception ex)
            {
                Erro erro = new Erro()
                {
                    Descricao = ex.Message,
                    Imagem = Guid.NewGuid().ToString(),
                    CasoDeUso = EnumCasoDeUso.MANTER_MEDIDAS,
                    Camada = EnumCamada.COMMON,
                    Funcionalidade = EnumFuncionalidade.SALVAR,
                    Entidade = "Erro"
                };
                if (Internet.Conectado())
                {
                    ISEmail.EnviarErro(erro);
                }

                return new Retorno(false, String.Format(Mensagens.MSG_05, ex.Message, "Recuperar Medidas"));
            }
        }

        public static string Print(Erro Entity)
        {
            int Width = Convert.ToInt32(ConfigurationManager.AppSettings["WIDTH"]);
            int Height = Convert.ToInt32(ConfigurationManager.AppSettings["HEIGHT"]);

            Bitmap bmp = new Bitmap(Width, Height);

            Graphics g = Graphics.FromImage(bmp);
            g.CopyFromScreen(0, 0, 0, 0, new Size(Width, Height));

            string nomeArquivo = Guid.NewGuid().ToString();
            bmp.Save(Environment.CurrentDirectory + @"\" + Entity.Imagem + ".png");

            return nomeArquivo;
        }
    }
}
