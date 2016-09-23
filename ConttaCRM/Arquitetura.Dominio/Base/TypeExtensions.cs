using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arquitetura.Domino.Base
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Verifica se é um tipo Nullable
        /// </summary>
        /// <param name="type">Tipo do objeto.</param>
        /// <returns>Verdadeiro caso seja um tipo Nullable, senão, falso.</returns>
        public static bool IsNullable(this Type type)
        {
            return (type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
        }
    }
}
