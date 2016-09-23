using Arquitetura.Dominio.Aggregates.Enums;
using Arquitetura.Dominio.Enums;
using System;

namespace Arquitetura.Dominio.Aggregates.NumeroOficialAgg
{
    public static class NumeroOficialFactory
    {
        public static NumeroOficial CreateNumeroOficial(
        string requerente,
        string rg,
        bool possuiIptu,
        int iptu,
        string rua,
        bool existeEdificacao,
        string atividade,
        string telefone,
        eSituacaoLocal? situacaoLocal,
        eGerarNumeroOficial? gerarNumeroOficial,
        int numeroOficialB,
        int numeroOficialC,
        string observacoes,
        DateTime? dataCadastro,
        int? responsavelId)
        {
            var numeroOficial = new NumeroOficial();

            numeroOficial.Requerente = requerente;
            numeroOficial.Rg = rg;
            numeroOficial.PossuiIptu = possuiIptu;
            numeroOficial.Iptu = iptu;
            numeroOficial.Rua = rua;
            numeroOficial.ExisteEdificacao = existeEdificacao;
            numeroOficial.Atividade = atividade;
            numeroOficial.Telefone = telefone;
            numeroOficial.SituacaoLocal = situacaoLocal;
            numeroOficial.GerarNumeroOficial = gerarNumeroOficial;
            numeroOficial.NumeroOficialB = numeroOficialB;
            numeroOficial.NumeroOficialC = numeroOficialC;
            numeroOficial.Observacoes = observacoes;
            numeroOficial.DataCadastro = dataCadastro;
            numeroOficial.ResponsavelId = responsavelId;

            return numeroOficial;
        }
    }
}