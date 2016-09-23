using System.ComponentModel;

namespace Arquitetura.Dominio.Aggregates.Enums
{
    public enum eTipoAbertura
    {
        [Description("Empreendedor Individual")]
        EmpreendedorIndividual = 0,

        [Description("Empresário Individual")]
        EmpresarioIndividual = 1,

        [Description("Sociedade Limitada")]
        SociedadeLimitada = 2,

        [Description("EIRELI")]
        Eireli = 3,

        [Description("Consórcio")]
        Consorcio = 4,

        [Description("Cooperativa")]
        Cooperativa = 5,

        [Description("Sociedade Anônima")]
        SociedadeAnonima = 6,

        [Description("Outros Tipos Jurídicos")]
        OutrosTiposJuridicos = 7
    }
}
