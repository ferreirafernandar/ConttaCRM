using Arquitetura.Infraestrutura.Validator;
using System;
using System.Collections.Generic;

namespace Arquitetura.Aplicacao.Base
{
    public class AppException : Exception
    {
        #region Properties

        IEnumerable<ValidationResult> _validationErrors;
        /// <summary>
        /// Get or set the validation errors messages
        /// </summary>
        public IEnumerable<ValidationResult> ValidationErrors
        {
            get
            {
                return _validationErrors;
            }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Create new instance of Application validation errors exception
        /// </summary>
        /// <param name="validationErrors">The collection of validation errors</param>
        public AppException(IEnumerable<ValidationResult> validationErrors)
            : base("Verifique os dados informados.")
        {
            _validationErrors = validationErrors;
        }

        public AppException(string mensagem) : base(mensagem) { }

        #endregion
    }
}
