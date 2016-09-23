using Arquitetura.Domino.Base.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arquitetura.Dominio.Aggregates.ResponsavelAgg
{
    public static class ResponsavelSpecification
    {
        public static Specification<Responsavel> ConsultaTexto(string texto)
        {
            Specification<Responsavel> spec = new DirectSpecification<Responsavel>(c => true);

            if (!string.IsNullOrWhiteSpace(texto))
            {
                spec &= new DirectSpecification<Responsavel>(c =>
                    c.Nome.ToUpper().Contains(texto.ToUpper()));
            }

            return spec;
        }

        public static Specification<Responsavel> ConsultaResponsavelPorCpf(string cpf)
        {
            Specification<Responsavel> spec = new DirectSpecification<Responsavel>(c => c.Cpf.Equals(cpf));
            return spec;
        }
    }
}
