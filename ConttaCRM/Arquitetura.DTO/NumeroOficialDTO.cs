using Arquitetura.Dominio.Aggregates.Enums;
using Arquitetura.Dominio.Aggregates.ResponsavelAgg;
using Arquitetura.Dominio.Enums;
using System;

namespace Arquitetura.DTO
{
    public class NumeroOficialDTO
    {
        public int Id { get; set; }
        public string Requerente { get; set; }
        public string Rg { get; set; }
        public bool PossuiIptu { get; set; }
        public int Iptu { get; set; }
        public string Rua { get; set; }
        public bool ExisteEdificacao { get; set; }
        public string Atividade { get; set; }
        public string Telefone { get; set; }
        public eSituacaoLocal? SituacaoLocal { get; set; }
        public eGerarNumeroOficial? GerarNumeroOficial { get; set; }
        public int NumeroOficialB { get; set; }
        public int NumeroOficialC { get; set; }
        public string Observacoes { get; set; }
        public virtual Responsavel Responsavel { get; set; }
        public int ResponsavelId { get; set; }
        public DateTime? DataCadastro { get; set; }
    }
}