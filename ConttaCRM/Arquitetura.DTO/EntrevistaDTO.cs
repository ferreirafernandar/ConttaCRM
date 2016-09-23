using Arquitetura.Dominio.Aggregates.SocioAgg;
using System;
using System.Collections.Generic;

namespace Arquitetura.DTO
{
    public class EntrevistaDTO
    {
        public int Id { get; set; }
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
        public int? ClienteId { get; set; }
        public int? UsuarioId { get; set; }
        public int? ResponsalvelId { get; set; }
        public DateTime? DataCadastro { get; set; }
        public bool CopiaRg { get; set; }
        public bool CopiaCpf { get; set; }
        public bool CopiaEndereco { get; set; }
        public bool CopiaCnh { get; set; }
        public bool CopiaCasamento { get; set; }
        public List<Socio> ListaDeSocios { get; private set; }
    }
}