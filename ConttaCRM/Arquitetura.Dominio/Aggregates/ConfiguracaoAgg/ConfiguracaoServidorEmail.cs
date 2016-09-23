using Arquitetura.Dominio.Aggregates.Base;
using Arquitetura.Dominio.Base;
using System.Collections.Generic;
using System.IO;

namespace Arquitetura.Dominio.Aggregates.ConfiguracaoAgg
{
    public class ConfiguracaoServidorEmail : Entity, IValidator
    {
        #region Propriedades

        public bool UtilizarEnvioDeEmail { get; set; }

        public string Conta { get; set; }

        public string SenhaCriptografada { get; private set; }
        public string Senha 
        { 
            get
            {
                if (SenhaCriptografada != null)
                {
                    return Encryption.DecryptToString(SenhaCriptografada);
                }
                return null;
            }
            set
            {
                if (value != null)
                {
                    SenhaCriptografada = Encryption.Encrypt(value);
                }
                else
                {
                    SenhaCriptografada = null;
                }
            }
        }

        public string Smtp { get; set; }

        public int? Porta { get; set; }

        public bool Ssl { get; set; }

        public string PastaRaiz { get; set; }

        #endregion

        #region Validação

        public IEnumerable<string[]> Validate()
        {
            var validationResults = new List<string[]>();

            if (UtilizarEnvioDeEmail)
            {
                if (string.IsNullOrEmpty(Conta))
                {
                    validationResults.Add(new string[] { "Conta é obrigatório.", "Conta" });
                }
                else if (Conta.Length > 360)
                {
                    validationResults.Add(new string[] { "Conta máximo 360 caracteres.", "Conta" });
                }

                if (string.IsNullOrEmpty(Senha))
                {
                    validationResults.Add(new string[] { "Senha é obrigatório.", "Senha" });
                }

                if (string.IsNullOrEmpty(Smtp))
                {
                    validationResults.Add(new string[] { "Smtp é obrigatório.", "Smtp" });
                }
                else if (Smtp.Length > 360)
                {
                    validationResults.Add(new string[] { "Smtp máximo 360 caracteres.", "Smtp" });
                }

                if (!Porta.HasValue)
                {
                    validationResults.Add(new string[] { "Porta é obrigatório.", "Porta" });
                }
                else if (Porta <= 0)
                {
                    validationResults.Add(new string[] { "Porta deve ser maior que zero.", "Porta" });
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(Conta) && Conta.Length > 360)
                {
                    validationResults.Add(new string[] { "Conta máximo 360 caracteres.", "Conta" });
                }

                if (!string.IsNullOrEmpty(Smtp) && Smtp.Length > 360)
                {
                    validationResults.Add(new string[] { "Smtp máximo 360 caracteres.", "Smtp" });
                }

                if (Porta.HasValue && Porta <= 0)
                {
                    validationResults.Add(new string[] { "Porta deve ser maior que zero.", "Porta" });
                }
            }

            if (Id != 0)
            {
                if (string.IsNullOrWhiteSpace(PastaRaiz))
                {
                    validationResults.Add(new string[] { "Pasta Raiz é obrigatório.", "PastaRaiz" });
                }
            }

            return validationResults;
        }

        #endregion
    }
}
