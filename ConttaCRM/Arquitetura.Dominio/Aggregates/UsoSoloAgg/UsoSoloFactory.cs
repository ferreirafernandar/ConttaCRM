using Arquitetura.Dominio.Aggregates.Enums;
using Arquitetura.Dominio.Enums;
using System;

namespace Arquitetura.Dominio.Aggregates.UsoSoloAgg
{
    public static class UsoSoloFactory
    {
        public static UsoSolo CreateUsoSolo(
        int iptu,
        bool imovelRual,
        string enderecoRural,
        string complemento,
        string rua,
        string quadra,
        string lote,
        string bairro,
        string cnae,
        bool escritorio,
        string observacoes,
        DateTime? dataCadastro,
        int? responsavelId)
        {
            var usoSolo = new UsoSolo();

            usoSolo.Iptu = iptu;
            usoSolo.ImovelRual = imovelRual;
            usoSolo.EnderecoRural = enderecoRural;
            usoSolo.Complemento = complemento;
            usoSolo.Rua = rua;
            usoSolo.Quadra = quadra;
            usoSolo.Lote = lote;
            usoSolo.Bairro = bairro;
            usoSolo.Cnae = cnae;
            usoSolo.Escritorio = escritorio;
            usoSolo.Observacoes = observacoes;
            usoSolo.DataCadastro = dataCadastro;
            usoSolo.ResponsavelId = responsavelId;

            return usoSolo;
        }
    }
}