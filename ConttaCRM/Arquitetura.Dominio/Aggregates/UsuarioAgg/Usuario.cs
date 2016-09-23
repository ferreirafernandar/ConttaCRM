using Arquitetura.Dominio.Aggregates.Base;
using Arquitetura.Dominio.Aggregates.Enums;
using Arquitetura.Dominio.Aggregates.ClienteAgg;
using Arquitetura.Dominio.Enums;
using Arquitetura.Dominio.Base;
using System;
using System.Collections.Generic;
using System.IO;

namespace Arquitetura.Dominio.Aggregates.UsuarioAgg
{
    public class Usuario : Entity, IValidator
    {
        #region Construtor

        public Usuario()
        {
        }

        #endregion

        #region Propriedades

        public string NomeUsuario { get; set; }

        public string Email { get; set; }

        public string Senha { get; set; }

        public string Nome { get; set; }

        public string Cpf { get; set; }

        public string Endereco { get; set; }

        public string Complemento { get; set; }

        public string Numero { get; set; }

        public string Bairro { get; set; }

        public string Cidade { get; set; }

        public eEstado? Estado { get; set; }

        public string Cep { get; set; }

        public string Telefone { get; set; }

        public string Celular { get; set; }

        public eSexo? Sexo { get; set; }

        public bool Ativo { get; set; }

        public eTipoUsuario? TipoUsuario { get; set; }

        public int? ClienteId { get; set; }

        public virtual Cliente Cliente { get; set; }

        #endregion

        #region Validação

        public IEnumerable<string[]> Validate()
        {
            var validationResults = new List<string[]>();

            if (string.IsNullOrWhiteSpace(NomeUsuario))
            {
                validationResults.Add(new string[] { "Usuário é obrigatório.", "NomeUsuario" });
            }
            else if (NomeUsuario.Length > 50)
            {
                validationResults.Add(new string[] { "Usuário máximo 50 caracteres.", "NomeUsuario" });
            }

            if (string.IsNullOrWhiteSpace(Email))
            {
                validationResults.Add(new string[] { "E-mail é obrigatório.", "Email" });
            }
            else if (!Util.IsValidEmail(Email))
            {
                validationResults.Add(new string[] { "E-mail inválido.", "Email" });
            }

            if (string.IsNullOrWhiteSpace(Senha))
            {
                validationResults.Add(new string[] { "Senha é obrigatório.", "Senha" });
            }

            if (string.IsNullOrWhiteSpace(Nome))
            {
                validationResults.Add(new string[] { "Nome é obrigatório.", "Nome" });
            }
            else if (Nome.Length > 100)
            {
                validationResults.Add(new string[] { "Nome máximo 100 caracteres.", "Nome" });
            }

            if (string.IsNullOrWhiteSpace(Telefone))
            {
                validationResults.Add(new string[] { "Telefone é obrigatório.", "Telefone" });
            }
            else if (Telefone.Length < 10 || Telefone.Length > 15)
            {
                validationResults.Add(new string[] { "Telefone inválido.", "Telefone" });
            }

            if (string.IsNullOrWhiteSpace(Celular))
            {
                validationResults.Add(new string[] { "Celular é obrigatório.", "Celular" });
            }
            else if (Celular.Length < 10 || Celular.Length > 15)
            {
                validationResults.Add(new string[] { "Celular inválido.", "Celular" });
            }

            if (string.IsNullOrWhiteSpace(Cpf))
            {
                validationResults.Add(new string[] { "CPF/CNPJ é obrigatório", "Cpf" });
            }
            //else if (Cpf.Length == 14)
            //{
            //    if (!Util.ValidarCPF(Cpf))
            //    {
            //        validationResults.Add(new string[] { "CPF inválido", "Cpf" });
            //    }
            //}
            //else if (Cpf.Length == 18)
            //{
            //    if (!Util.ValidarCPF(Cpf))
            //    {
            //        validationResults.Add(new string[] { "CNPJ inválido", "Cpf" });
            //    }
            //}
            //else
            //{
            //    validationResults.Add(new string[] { "CPF/CNPJ inválido", "Cpf" });
            //}

            if (string.IsNullOrWhiteSpace(Endereco))
            {
                validationResults.Add(new string[] { "Endereço é obrigatório.", "Endereco" });
            }
            else if (Endereco.Length > 50)
            {
                validationResults.Add(new string[] { "Endereço máximo 50 caracteres.", "Endereco" });
            }

            if (Complemento != null && Complemento.Length > 50)
            {
                validationResults.Add(new string[] { "Complemento máximo 50 caracteres.", "Complemento" });
            }

            if (!string.IsNullOrWhiteSpace(Numero))
            {
                if (Numero.Length > 20)
                {
                    validationResults.Add(new string[] { "Número máximo 20 caracteres.", "Numero" });
                }
            }

            if (string.IsNullOrWhiteSpace(Bairro))
            {
                validationResults.Add(new string[] { "Bairro é obrigatório.", "Bairro" });
            }
            else if (Bairro.Length > 50)
            {
                validationResults.Add(new string[] { "Bairro máximo 50 caracteres.", "Bairro" });
            }

            if (string.IsNullOrWhiteSpace(Cidade))
            {
                validationResults.Add(new string[] { "Cidade é obrigatório.", "Cidade" });
            }
            else if (Cidade.Length > 50)
            {
                validationResults.Add(new string[] { "Cidade máximo 100 caracteres.", "Cidade" });
            }

            if (!Estado.HasValue)
            {
                validationResults.Add(new string[] { "Estado é obrigatório.", "Estado" });
            }

            if (string.IsNullOrWhiteSpace(Cidade))
            {
                validationResults.Add(new string[] { "Cidade é obrigatório.", "Cidade" });
            }
            else if (Cidade.Length > 50)
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

            if (!TipoUsuario.HasValue)
            {
                validationResults.Add(new string[] { "Informe o tipo de usuário.", "TipoUsuario" });
            }
            return validationResults;
        }

        #endregion
    }
}
