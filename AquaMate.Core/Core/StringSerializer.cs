/*
 *  AquaMate, home aquariums manager.
 *  Copyright (C) 2019-2026 by Sergey V. Zhdanovskih.
 *  
 *  Licensed under the GNU General Public License (GPL) v3.
 *  See LICENSE file in the project root for full license information.
 */

using System;
using System.Globalization;
using System.Text;

namespace AquaMate.Core
{
    /// <summary>
    /// Extremely simple linear serializer of objects into strings.
    /// </summary>
    public static class StringSerializer
    {
        private static bool IsDecimal(object obj)
        {
            return (obj != null) && IsDecimal(obj.GetType());
        }

        private static bool IsDecimal(Type type)
        {
            if (type == null)
                return false;

            switch (Type.GetTypeCode(type)) {
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return true;
            }

            return false;
        }

        private static readonly NumberFormatInfo STD_NFI = new NumberFormatInfo {
            NumberDecimalSeparator = ".",
            NumberGroupSeparator = ""
        };

        public static string Serialize(object obj)
        {
            if (obj == null)
                return string.Empty;

            var str = new StringBuilder();
            //str.Append("{");

            var objType = obj.GetType();

            var typeProps = objType.GetProperties();
            foreach (var prop in typeProps) {
                var propValue = prop.GetValue(obj, null);

                if (str.Length > 1)
                    str.Append(";");

                IFormatProvider fmt = IsDecimal(propValue) ? STD_NFI : null;
                string strVal = Convert.ToString(propValue, fmt);
                str.Append(prop.Name + "=" + strVal);
            }

            //str.Append("}");
            return str.ToString();
        }

        public static T Deserialize<T>(string str) where T : class, new()
        {
            var objType = typeof(T);
            return Deserialize<T>(objType, str);
        }

        public static T Deserialize<T>(Type objType, string str) where T : class, new()
        {
            T result = new T();
            Deserialize(objType, result, str);
            return result;
        }

        public static object Deserialize(Type objType, string str)
        {
            object result = Activator.CreateInstance(objType);
            Deserialize(objType, result, str);
            return result;
        }

        public static void Deserialize(Type objType, object obj, string str)
        {
            if (!string.IsNullOrEmpty(str)) {
                string[] props = str.Split(new char[] { ';', '=' }, Int32.MaxValue, StringSplitOptions.None);

                int i = 0;
                while (i < props.Length) {
                    string propName = props[i];
                    string propValue = (++i < props.Length) ? props[i] : string.Empty;

                    var propInfo = objType.GetProperty(propName);
                    if (propInfo != null) {
                        IFormatProvider fmt = IsDecimal(propInfo.PropertyType) ? STD_NFI : null;
                        propInfo.SetValue(obj, Convert.ChangeType(propValue, propInfo.PropertyType, fmt), null);
                    }

                    i += 1;
                }
            }
        }
    }
}
