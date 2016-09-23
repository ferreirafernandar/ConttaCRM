using System.ComponentModel;

namespace Arquitetura.Dominio.Aggregates.Enums
{
    public enum eEstadoCivil
    {
        [Description("Solteiro(a)")]
        Solteiro = 0,

        [Description("Casado(a)")]
        Casado = 1,

        [Description("Divorciado(a)")]
        Divorciado = 2,

        [Description("Viúvo(a)")]
        Viuvo = 3,

        [Description("Separado(a)")]
        Separado = 4,

        [Description("Companheiro(a)")]
        Companheiro = 5
    }
}
