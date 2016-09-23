using Arquitetura.Dominio.Aggregates.Base;
using Arquitetura.Dominio.Aggregates.ClienteAgg;
using Arquitetura.Dominio.Aggregates.ResponsavelAgg;
using Arquitetura.Dominio.Aggregates.SocioAgg;
using Arquitetura.Dominio.Aggregates.UsuarioAgg;
using Arquitetura.Dominio.Base;
using System;
using System.Collections.Generic;

namespace Arquitetura.Dominio.Aggregates.EntrevistaAgg
{
    public class Entrevista : Entity, IValidator
    {
        public Entrevista()
        {
            ListaDeSocios = new List<Socio>();
        }

        public string NomeDaEmpresa1 { get; set; }
        public string NomeDaEmpresa2 { get; set; }
        public string NomeDaEmpresa3 { get; set; }
        public int Iptu { get; set; }
        public string NomeFantasia { get; set; }
        public string CapitalSocial { get; set; }
        public string Objetivo { get; set; }
        public string Metragem { get; set; }
        public string PontoDeReferencia { get; set; }
        public string LivroRegistroEmpregados { get; set; }
        public string InspencaoTrabalho { get; set; }
        public string LivroTermoOcorrencia { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public virtual Usuario Usuario { get; set; }
        public int? UsuarioId { get; set; }
        public virtual Cliente Cliente { get; set; }
        public int? ClienteId { get; set; }
        public virtual Responsavel Responsavel { get; set; }
        public int? ResponsavelId { get; set; }
        public DateTime? DataCadastro { get; set; }
        public bool CopiaRg { get; set; }
        public bool CopiaCpf { get; set; }
        public bool CopiaEndereco { get; set; }
        public bool CopiaCnh { get; set; }
        public bool CopiaCasamento { get; set; }

        // virtual Socio Socio { get; set; }
        public virtual ICollection<Socio> ListaDeSocios { get; private set; }

        public IEnumerable<string[]> Validate()
        {
            var validationResults = new List<string[]>();

            if (string.IsNullOrWhiteSpace(NomeDaEmpresa1))
            {
                validationResults.Add(new string[] { "O nome empresa 1 é obrigatório.", "NomeDaEmpresa1" });
            }
            else if (NomeDaEmpresa1.Length > 255)
            {
                validationResults.Add(new string[] { "O máximo de caracteres neste campo é de 255.", "NomeDaEmpresa1" });
            }
            if (!string.IsNullOrWhiteSpace(NomeDaEmpresa2))
            {
                if (NomeDaEmpresa2.Length > 255)
                {
                    validationResults.Add(new string[] { "O máximo de caracteres neste campo é de 255.", "NomeDaEmpresa2" });
                }
            }
            if (!string.IsNullOrWhiteSpace(NomeDaEmpresa3))
            {
                if (NomeDaEmpresa3.Length > 255)
                {
                    validationResults.Add(new string[] { "O máximo de caracteres neste campo é de 255.", "NomeDaEmpresa3" });
                }
            }
            if (string.IsNullOrWhiteSpace(NomeFantasia))
            {
                validationResults.Add(new string[] { "O nome fantasia é obrigatório.", "NomeFantasia" });
            }
            else if (NomeFantasia.Length > 255)
            {
                validationResults.Add(new string[] { "O máximo de caracteres neste campo é de 255.", "NomeFantasia" });
            }
            if (string.IsNullOrWhiteSpace(CapitalSocial))
            {
                validationResults.Add(new string[] { "Capital Social é obrigatório", "CapitalSocial" });
            }
            if (string.IsNullOrWhiteSpace(Objetivo))
            {
                validationResults.Add(new string[] { "Objetivo é obrigatório.", "Objetivo" });
            }
            else if (Objetivo.Length > 500)
            {
                validationResults.Add(new string[] { "O máximo de caracteres neste campo é de 500.", "Objetivo" });
            }
            if (string.IsNullOrWhiteSpace(Metragem))
            {
                validationResults.Add(new string[] { "Metragem da sala é obrigatório.", "Metragem" });
            }
            if (string.IsNullOrWhiteSpace(Telefone))
            {
                validationResults.Add(new string[] { "O telefone é obrigatório.", "Telefone" });
            }
            else if (Telefone.Length < 10 || Telefone.Length > 15)
            {
                validationResults.Add(new string[] { "Telefone inválido.", "Telefone" });
            }
            if (string.IsNullOrWhiteSpace(Email))
            {
                validationResults.Add(new string[] { "E-mail é obrigatório.", "Email" });
            }
            else if (!Util.IsValidEmail(Email))
            {
                validationResults.Add(new string[] { "E-mail inválido.", "Email" });
            }
            if (!ResponsavelId.HasValue)
            {
                validationResults.Add(new string[] { "O responsável é obrigatório.", "ResponsavelId" });
            }
            return validationResults;
        }
    }
}