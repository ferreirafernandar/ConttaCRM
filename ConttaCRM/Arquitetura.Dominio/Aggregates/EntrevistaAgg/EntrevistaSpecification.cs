using Arquitetura.Dominio.Aggregates.EntrevistaAgg;
using Arquitetura.Domino.Base.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arquitetura.Dominio.Aggregates.EntrevistaAgg
{
    public static class EntrevistaSpecification
    {
        public static Specification<Entrevista> ConsultaTexto(string texto)
        {
            Specification<Entrevista> spec = new DirectSpecification<Entrevista>(c => true);

            if (!string.IsNullOrWhiteSpace(texto))
            {
                spec &= new DirectSpecification<Entrevista>(c =>
                    c.NomeDaEmpresa1.ToUpper().Contains(texto.ToUpper()) ||
                    c.NomeFantasia.ToUpper().Contains(texto.ToUpper()));
            }

            return spec;
        }
    }
}
