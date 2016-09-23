using System.ComponentModel;

namespace Arquitetura.Dominio.Enums
{
    public enum eSituacaoLocal
    {
        [Description("Situação 1")]
        Situacao1 = 0,

        [Description("Situação 2")]
        Situacao2 = 1,

        [Description("Situação 3")]
        Situacao3 = 2,

        [Description("Outra Situação")]
        OutraSituacao = 3
    }
}