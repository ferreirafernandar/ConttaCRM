using Arquitetura.Dominio.Aggregates.Enums;
using Arquitetura.Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arquitetura.DTO
{
    public class EntrevistaListDTO
    {
        public int Id { get; set; }
        public string NomeDaEmpresa1 { get; set; }
        public string Telefone { get; set; }
        public DateTime? DataCadastro { get; set; }
    }
}
