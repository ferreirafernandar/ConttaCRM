using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Arquitetura.Domino.Base
{

    public class StringValue : Attribute
    {
        private string _value;
        private bool _allowNull;

        public StringValue(string value, bool allowNull = false)
        {
            _value = value;
            _allowNull = allowNull;
        }

        public string Value
        {
            get { return _value; }
        }

        public bool AllowNull
        {
            get { return _allowNull; }
        }
    }

    public static class StringEnum
    {
        public static string GetStringValue(Enum value)
        {
            string output = null;
            bool allowNull = false;

            if (value == null)
                return null;

            Type type = value.GetType();

            FieldInfo fi = type.GetField(value.ToString());
            StringValue[] attrs =
               fi.GetCustomAttributes(typeof(StringValue),
                                       false) as StringValue[];
            if (attrs.Length > 0)
            {
                output = attrs[0].Value;
                allowNull = attrs[0].AllowNull;
            }

            if (!allowNull && string.IsNullOrWhiteSpace(output))
                throw new Exception(string.Format("O enum do tipo {0} não possui atributo do tipo 'StringValue'.", type));

            return output;
        }

        public static T GetEnumValue<T>(string stringValue) //where T : struct
        {
            Type origType = typeof(T);
            Type type = origType;

            if (origType.IsNullable())
                type = Nullable.GetUnderlyingType(type);
            
            Array values = Enum.GetValues(type);

            foreach (var item in values)
            {
                string value = GetStringValue((Enum)item);
                if (value == stringValue)
                    return (T)item;
            }

            if (origType.IsNullable())
                return default(T);

            throw new Exception("Valor do tipo enumerado inválido.");
        }
    }
}
