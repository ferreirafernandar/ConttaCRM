using Arquitetura.Dominio.Aggregates.ResponsavelAgg;
using System;

namespace Arquitetura.DTO
{
    public class UsoSoloDTO
    {
        public int Id { get; set; }
        public int Iptu { get; set; }
        public bool ImovelRual { get; set; }
        public string EnderecoRural { get; set; }
        public string Complemento { get; set; }
        public string Rua { get; set; }
        public string Quadra { get; set; }
        public string Lote { get; set; }
        public string Bairro { get; set; }
        public string Cnae { get; set; }
        public bool Escritorio { get; set; }
        public string Observacoes { get; set; }
        public virtual Responsavel Responsavel { get; set; }
        public int ResponsavelId { get; set; }
        public DateTime? DataCadastro { get; set; }
    }
}