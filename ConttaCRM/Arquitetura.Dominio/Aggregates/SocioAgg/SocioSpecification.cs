using Arquitetura.Domino.Base.Specification;

namespace Arquitetura.Dominio.Aggregates.SocioAgg
{
    public static class SocioSpecification
    {
        public static Specification<Socio> ConsultaTexto(string texto)
        {
            Specification<Socio> spec = new DirectSpecification<Socio>(c => true);

            if (!string.IsNullOrWhiteSpace(texto))
            {
                spec &= new DirectSpecification<Socio>(c =>
                    c.Nome.ToUpper().Contains(texto.ToUpper()));
            }

            return spec;
        }

        public static Specification<Socio> ConsultaResponsavelPorCpf(string cpf)
        {
            Specification<Socio> spec = new DirectSpecification<Socio>(c => c.Cpf.Equals(cpf));
            return spec;
        }
    }
}