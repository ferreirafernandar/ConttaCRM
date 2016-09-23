using Arquitetura.Dominio.Aggregates.Base;
using Arquitetura.Dominio.Aggregates.Enums;
using Arquitetura.Dominio.Base;
using Arquitetura.Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arquitetura.Dominio.Aggregates.ResponsavelAgg
{
    public class Responsavel:Entity,IValidator
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public eSexo? Sexo { get; set; }
        public DateTime? DataCadastro { get; set; }
        public eTipoAbertura? TipoAbertura { get; set; }
        public bool EnviarEmail { get; set; }
        public string Rg { get; set; }
        public eEstadoCivil? EstadoCivil { get; set; }

        public IEnumerable<string[]> Validate()
        {
            var validationResults = new List<string[]>();

            if (string.IsNullOrWhiteSpace(Nome))
            {
                validationResults.Add(new string[] { "O nome é obrigatório.", "Nome" });
            }
            else if (Nome.Length > 255)
            {
                validationResults.Add(new string[] { "O máximo de caracteres neste campo é 255.", "Nome" });
            }

            if (string.IsNullOrWhiteSpace(Cpf))
            {
                validationResults.Add(new string[] { "O CPF é obrigatório.", "Cpf" });
            }

            if (string.IsNullOrWhiteSpace(Rg))
            {
                validationResults.Add(new string[] { "O RG é obrigatório.", "Rg" });
            }

            if (!EstadoCivil.HasValue)
            {
                validationResults.Add(new string[] { "O estado civil é obrigatório.", "EstadoCivil" });
            }
            if (string.IsNullOrWhiteSpace(Telefone))
            {
                validationResults.Add(new string[] { "O telefone é obrigatório.", "Telefone" });
            }
            else if (Telefone.Length < 10 || Telefone.Length > 15)
            {
                validationResults.Add(new string[] { "Telefone inválido.", "Telefone" });
            }

            if (string.IsNullOrWhiteSpace(Celular))
            {
                validationResults.Add(new string[] { "O celular é obrigatório.", "Celular" });
            }
            else if (Celular.Length < 10 || Celular.Length > 15)
            {
                validationResults.Add(new string[] { "Celular inválido.", "Celular" });
            }
            if (string.IsNullOrWhiteSpace(Email))
            {
                validationResults.Add(new string[] { "E-mail é obrigatório.", "Email" });
            }
            else if (!Util.IsValidEmail(Email))
            {
                validationResults.Add(new string[] { "E-mail inválido.", "Email" });
            }
            if (!Sexo.HasValue)
            {
                validationResults.Add(new string[] { "Informe o sexo.", "Sexo" });
            }
            
            return validationResults;
        }
    }
}
