using Arquitetura.Domino.Base.Specification;

namespace Arquitetura.Dominio.Aggregates.NumeroOficialAgg
{
    public static class NumeroOficialSpecification
    {
        public static Specification<NumeroOficial> ConsultaTexto(string texto)
        {
            Specification<NumeroOficial> spec = new DirectSpecification<NumeroOficial>(c => true);

            if (!string.IsNullOrWhiteSpace(texto))
            {
                spec &= new DirectSpecification<NumeroOficial>(c =>
                    c.Requerente.ToUpper().Contains(texto.ToUpper()));
            }

            return spec;
        }
    }
}