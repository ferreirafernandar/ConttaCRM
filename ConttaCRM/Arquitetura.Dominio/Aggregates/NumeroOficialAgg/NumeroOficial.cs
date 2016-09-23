using Arquitetura.Dominio.Aggregates.Enums;
using Arquitetura.Dominio.Aggregates.ResponsavelAgg;
using Arquitetura.Dominio.Base;
using Arquitetura.Dominio.Enums;
using System;
using System.Collections.Generic;

namespace Arquitetura.Dominio.Aggregates.NumeroOficialAgg
{
    public class NumeroOficial : Entity, IValidator
    {
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
        public int? ResponsavelId { get; set; }
        public DateTime? DataCadastro { get; set; }

        public IEnumerable<string[]> Validate()
        {
            var validationResults = new List<string[]>();

            if (string.IsNullOrWhiteSpace(Atividade))
            {
                validationResults.Add(new string[] { "A atividade é obrigatória.", "Atividade" });
            }
            else if (Atividade.Length > 255)
            {
                validationResults.Add(new string[] { "O máximo de caracteres neste campo é 255.", "Atividade" });
            }
            if (!SituacaoLocal.HasValue)
            {
                validationResults.Add(new string[] { "A situação é obrigatória.", "SituacaoLocal" });
            }
            return validationResults;
        }
    }
}