using Arquitetura.Dominio.Base;
using System;
using System.Collections.Generic;

namespace Arquitetura.Dominio.Aggregates.UsuarioAgg
{
    public class TokenSenha : Entity, IValidator
    {
        #region Construtor

        public TokenSenha()
        {
            Data = DateTime.Now;
        }

        #endregion

        #region Propriedades

        public int UsuarioId { get; set; }

        public virtual Usuario Usuario { get; set; }

        public string Token { get; private set; }

        public DateTime Data { get; set; }

        #endregion

        #region Métodos públicos

        public void GerarToken()
        {
            Token = Guid.NewGuid().ToString("N");
        }

        #endregion

        #region Validacao

        public IEnumerable<string[]> Validate()
        {
            var validationResults = new List<string[]>();

            if (UsuarioId <= 0)
                throw new Exception("UsuarioId inválido.");

            if (string.IsNullOrWhiteSpace(Token))
                throw new Exception("Token é obrigatório.");

            else if (Token.Length != 32)
                throw new Exception("Token deve ter 32 caracteres.");

            return validationResults;
        }

        #endregion
    }
}
