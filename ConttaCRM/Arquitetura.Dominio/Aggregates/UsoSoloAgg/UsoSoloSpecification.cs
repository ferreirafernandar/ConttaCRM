using Arquitetura.Domino.Base.Specification;

namespace Arquitetura.Dominio.Aggregates.UsoSoloAgg
{
    public static class UsoSoloSpecification
    {
        public static Specification<UsoSolo> ConsultaTexto(string texto)
        {
            Specification<UsoSolo> spec = new DirectSpecification<UsoSolo>(c => true);

            return spec;
        }
    }
}