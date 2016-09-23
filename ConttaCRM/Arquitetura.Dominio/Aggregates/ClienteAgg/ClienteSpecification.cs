using Arquitetura.Domino.Base.Specification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arquitetura.Dominio.Aggregates.ClienteAgg
{
    public static class ClienteSpecification
    {
        public static Specification<Cliente> ConsultaTexto(string texto)
        {
            Specification<Cliente> spec = new DirectSpecification<Cliente>(c => true);

            if (!string.IsNullOrWhiteSpace(texto))
            {
                spec &= new DirectSpecification<Cliente>(c =>
                    c.NomeFantasia.ToUpper().Contains(texto.ToUpper()) ||
                    c.RazaoSocial.ToUpper().Contains(texto.ToUpper()));
            }

            return spec;
        }

        public static Specification<Cliente> ConsultaClientePorCnpj(string cnpj)
        {
            Specification<Cliente> spec = new DirectSpecification<Cliente>(c => c.Cnpj.Equals(cnpj));
            return spec;
        }
    }
}
