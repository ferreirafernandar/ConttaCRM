using Arquitetura.Dominio.Aggregates.Base;
using Arquitetura.Dominio.Aggregates.EntrevistaAgg;
using Arquitetura.Dominio.Aggregates.Enums;
using Arquitetura.Dominio.Base;
using System.Collections.Generic;

namespace Arquitetura.Dominio.Aggregates.SocioAgg
{
    public class Socio : Entity, IValidator
    {
        public string Nome { get; set; }
        public bool Administrador { get; set; }
        public string DataNascimento { get; set; }
        public string Rg { get; set; }
        public string OrgaoRG { get; set; }
        public string Cpf { get; set; }
        public string NomeMae { get; set; }
        public string NomePai { get; set; }
        public string Cnh { get; set; }
        public string Nacionalidade { get; set; }
        public string Naturalidade { get; set; }
        public eSexo? Sexo { get; set; }
        public eEstadoCivil? EstadoCivil { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public string Participacao { get; set; }
        public bool Assina { get; set; }
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Cep { get; set; }
        public string Referencia { get; set; }
        public eEstado? Estado { get; set; }
        public virtual Entrevista Entrevista { get; set; }
        public int EntrevistaId { get; set; }

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
            if (string.IsNullOrWhiteSpace(DataNascimento))
            {
                validationResults.Add(new string[] { "A data de nascimento é obrigatória.", "DataNascimento" });
            }

            if (string.IsNullOrWhiteSpace(Rg))
            {
                validationResults.Add(new string[] { "O RG é obrigatório.", "Rg" });
            }

            if (string.IsNullOrWhiteSpace(OrgaoRG))
            {
                validationResults.Add(new string[] { "O orgão expeditor é obrigatório.", "OrgaoRG" });
            }

            if (string.IsNullOrWhiteSpace(Cpf))
            {
                validationResults.Add(new string[] { "O CPF é obrigatório.", "Cpf" });
            }

            if (string.IsNullOrWhiteSpace(NomeMae))
            {
                validationResults.Add(new string[] { "O nome da mãe é obrigatório.", "NomeMae" });
            }

            if (string.IsNullOrWhiteSpace(Nacionalidade))
            {
                validationResults.Add(new string[] { "A nacionalidade é obrigatória.", "Nacionalidade" });
            }

            if (string.IsNullOrWhiteSpace(Naturalidade))
            {
                validationResults.Add(new string[] { "A naturalidade é obrigatória.", "Naturalidade" });
            }

            if (!Sexo.HasValue)
            {
                validationResults.Add(new string[] { "Informe o sexo.", "Sexo" });
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

            if (string.IsNullOrWhiteSpace(Participacao))
            {
                validationResults.Add(new string[] { "A participação é obrigatório.", "Participacao" });
            }

            if (string.IsNullOrWhiteSpace(Rua))
            {
                validationResults.Add(new string[] { "O nome da rua é obrigatório.", "Rua" });
            }
            else if (Rua.Length > 255)
            {
                validationResults.Add(new string[] { "O máximo de caracteres neste campo é 255.", "Rua" });
            }
            if (!string.IsNullOrWhiteSpace(Numero))
            {
                if (Numero.Length > 11)
                {
                    validationResults.Add(new string[] { "Número máximo 11 caracteres.", "Numero" });
                }
            }
            if (Complemento != null && Complemento.Length > 255)
            {
                validationResults.Add(new string[] { "O máximo de caracteres neste campo é 255.", "Complemento" });
            }

            if (string.IsNullOrWhiteSpace(Bairro))
            {
                validationResults.Add(new string[] { "O bairro é obrigatório.", "Bairro" });
            }
            else if (Bairro.Length > 100)
            {
                validationResults.Add(new string[] { "O máximo de caracteres neste campo é 100.", "Bairro" });
            }

            if (string.IsNullOrWhiteSpace(Cidade))
            {
                validationResults.Add(new string[] { "Cidade é obrigatório.", "Cidade" });
            }
            else if (Cidade.Length > 100)
            {
                validationResults.Add(new string[] { "Cidade máximo 100 caracteres.", "Cidade" });
            }

            if (!string.IsNullOrWhiteSpace(Cep))
            {
                if (Cep.Replace(".", "").Replace("-", "").Trim().Length != 8)
                {
                    validationResults.Add(new string[] { "CEP inválido.", "Cep" });
                }
            }

            if (!Estado.HasValue)
            {
                validationResults.Add(new string[] { "O estado é obrigatório.", "Estado" });
            }

            return validationResults;
        }
    }
}