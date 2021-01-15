using InfinitySolutions.Commom;
using InfinitySolutions.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace InfinitySolutions.Presentation.Controllers
{
    public class BaseController : Controller
    {
        protected Login RecuperarLogin()
        {
            return Session[Constantes.USUARIO_LOGADO] as Login;
        }

        protected TipoFuncao RecuperarFuncao()
        {
            return (Session[Constantes.USUARIO_LOGADO] as Login).TiposFuncoes.FirstOrDefault();
        }

        protected byte[] ConverterEmByte(HttpPostedFileBase postedFileBase)
        {
            using (var reader = new BinaryReader(postedFileBase.InputStream))
                return reader.ReadBytes(postedFileBase.ContentLength);
        }
        protected string ConverterObjetoParaJson<T>(T obj)
        {
            try
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
                MemoryStream ms = new MemoryStream();
                ser.WriteObject(ms, obj);
                string jsonString = Encoding.UTF8.GetString(ms.ToArray());
                ms.Close();
                return jsonString;
            }
            catch
            {
                throw;
            }
        }

        protected T ConverterJsonParaObjecto<T>(string jsonString)
        {
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
                T obj = (T)serializer.ReadObject(ms);
                return obj;
            }
            catch
            {
                throw;
            }
        }
    }
}