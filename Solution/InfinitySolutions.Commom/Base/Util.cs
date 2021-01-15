using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel;
using InfinitySolutions.Entity;
using System.Configuration;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Json;

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

        public static string ConverteObjectParaJSon<T>(this T obj)
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

        public static T ConverteJSonParaObject<T>(this string jsonString)
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

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWow64Process(
            [In] IntPtr hProcess,
            [Out] out bool wow64Process
        );

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static bool VerificarSistema64Bits()
        {
            if ((Environment.OSVersion.Version.Major == 5 && Environment.OSVersion.Version.Minor >= 1) ||
                Environment.OSVersion.Version.Major >= 6)
            {
                using (Process p = Process.GetCurrentProcess())
                {
                    bool retVal;
                    if (!IsWow64Process(p.Handle, out retVal))
                    {
                        return false;
                    }
                    return retVal;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
