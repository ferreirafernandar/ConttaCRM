using Arquitetura.Dominio.Aggregates.Base;
using Arquitetura.Dominio.Aggregates.Enums;
using Arquitetura.Dominio.Base;
using Arquitetura.Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arquitetura.Dominio.Aggregates.ClienteAgg
{
    public class Cliente:Entity,IValidator
    {
        public string NomeFantasia { get; set; }
        public string RazaoSocial { get; set; }
        public string Cnpj { get; set; }
        public string InscricaoEstadual { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
        public string Skype { get; set; }
        public string NomeResponsavel { get; set; }
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public eEstado? Estado { get; set; }
        public string Cep { get; set; }
        public int? MatrizId { get; set; }
        public eTipoEmpresa? TipoEmpresa { get; set; }

        
        public IEnumerable<string[]> Validate()
        {
            var validationResults = new List<string[]>();

            if (string.IsNullOrWhiteSpace(NomeFantasia))
            {
                validationResults.Add(new string[] { "O nome fantasia é obrigatório.", "NomeFantasia" });
            }
            else if (NomeFantasia.Length > 255)
            {
                validationResults.Add(new string[] { "O máximo de caracteres neste campo é 255.", "NomeFantasia" });
            }

            if(string.IsNullOrWhiteSpace(RazaoSocial))
            {
                validationResults.Add(new string[] { "A razão social é obrigatória.", "RazaoSocial"});
            }
            else if (RazaoSocial.Length > 255)
            {
                validationResults.Add(new string[] { "O máximo de caracteres neste campo é 255.", "RazaoSocial" });
            }

            if(string.IsNullOrWhiteSpace(Cnpj))
            {
                validationResults.Add(new string[] { "O CNPJ é obrigatório.", "Cnpj" });
            }

            if (string.IsNullOrWhiteSpace(InscricaoEstadual))
            {
                validationResults.Add(new string[] { "A inscrição estadual é obrigatória.", "InscricaoEstadual" });
            }
            if (string.IsNullOrWhiteSpace(Email))
            {
                validationResults.Add(new string[] { "E-mail é obrigatório.", "Email" });
            }
            else if (!Util.IsValidEmail(Email))
            {
                validationResults.Add(new string[] { "E-mail inválido.", "Email" });
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
            if (string.IsNullOrWhiteSpace(NomeResponsavel))
            {
                validationResults.Add(new string[] { "O nome do responsável é obrigatório.", "NomeResponsavel" });
            }
            else if (NomeResponsavel.Length > 255)
            {
                validationResults.Add(new string[] { "O máximo de caracteres neste campo é 255.", "NomeResponsavel" });
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
                validationResults.Add(new string[] { "O máximo de caracteres neste campo é 150.", "Complemento" });
            }

            if (string.IsNullOrWhiteSpace(Bairro))
            {
                validationResults.Add(new string[] { "O bairro é obrigatório.", "Bairro" });
            }
            else if (Bairro.Length > 150)
            {
                validationResults.Add(new string[] { "O máximo de caracteres neste campo é 150.", "Bairro" });
            }

            if (string.IsNullOrWhiteSpace(Cidade))
            {
                validationResults.Add(new string[] { "Cidade é obrigatório.", "Cidade" });
            }
            else if (Cidade.Length > 100)
            {
                validationResults.Add(new string[] { "Cidade máximo 100 caracteres.", "Cidade" });
            }
            if (!Estado.HasValue)
            {
                validationResults.Add(new string[] { "O estado é obrigatório.", "Estado" });
            }
            if (!string.IsNullOrWhiteSpace(Cep))
            {
                if (Cep.Replace(".", "").Replace("-", "").Trim().Length != 8)
                {
                    validationResults.Add(new string[] { "CEP inválido.", "Cep" });
                }
            }
            if (!TipoEmpresa.HasValue)
            {
                validationResults.Add(new string[] { "Informe o tipo da empresa.", "TipoEmpresa" });
            }
            else if(TipoEmpresa == eTipoEmpresa.Filial && !MatrizId.HasValue)
            {
                validationResults.Add(new string[] { "Informe a matriz para essa empresa.", "MatrizId" });
            }
            return validationResults;
        }
    }
}
