using Arquitetura.Dominio.Aggregates.Enums;
using Arquitetura.Dominio.Aggregates.ResponsavelAgg;
using Arquitetura.Dominio.Aggregates.UsoSoloAgg;
using Arquitetura.Dominio.Base;
using Arquitetura.Dominio.Enums;
using System;
using System.Collections.Generic;

namespace Arquitetura.Dominio.Aggregates.UsoSoloAgg
{
    public class UsoSolo : Entity, IValidator
    {
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
        public int? ResponsavelId { get; set; }
        public DateTime? DataCadastro { get; set; }

        public IEnumerable<string[]> Validate()
        {
            var validationResults = new List<string[]>();

            if (Iptu <= 0)
            {
                validationResults.Add(new string[] { "O IPTU é obrigatório.", "Iptu" });
            }
            return validationResults;
        }
    }
}