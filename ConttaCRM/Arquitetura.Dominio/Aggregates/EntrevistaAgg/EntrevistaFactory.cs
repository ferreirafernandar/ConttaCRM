using Arquitetura.Dominio.Aggregates.EntrevistaAgg;
using Arquitetura.Dominio.Aggregates.Enums;
using Arquitetura.Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arquitetura.Dominio.Aggregates.EntrevistaAgg
{
    public static class EntrevistaFactory
    {
        public static Entrevista CreateEntrevista(
        string nomeDaEmpresa1,
        string nomeDaEmpresa2,
        string nomeDaEmpresa3,
        int iptu,
        string nomeFantasia,
        string capitalSocial,
        string objetivo,
        string metragem,
        string pontoDeReferencia,
        string livroRegistroEmpregados,
        string inspencaoTrabalho,
        string livroTermoOcorrencia,
        string telefone,
        string email, 
        int? clienteId, 
        int? usuarioId,
        int? responsavelId,
        DateTime? dataCadastro,
        bool copiaRg,
        bool copiaCpf,
        bool copiaEndereco,
        bool copiaCnh,
        bool copiaCasamento)
        {
            var entrevista = new Entrevista();
            entrevista.NomeDaEmpresa1 = nomeDaEmpresa1;
            entrevista.NomeDaEmpresa2 = nomeDaEmpresa2;
            entrevista.NomeDaEmpresa3 = nomeDaEmpresa3;
            entrevista.Iptu = iptu;
            entrevista.NomeFantasia = nomeFantasia;
            entrevista.CapitalSocial = capitalSocial;
            entrevista.Objetivo = objetivo;
            entrevista.Metragem = metragem;
            entrevista.PontoDeReferencia = pontoDeReferencia;
            entrevista.LivroRegistroEmpregados = livroRegistroEmpregados;
            entrevista.InspencaoTrabalho = inspencaoTrabalho;
            entrevista.LivroTermoOcorrencia = livroTermoOcorrencia;
            entrevista.Telefone = telefone;
            entrevista.Email = email;
            entrevista.ClienteId = clienteId;
            entrevista.UsuarioId = usuarioId;
            entrevista.ResponsavelId = responsavelId;
            entrevista.DataCadastro = dataCadastro;
            entrevista.CopiaRg = copiaRg;
            entrevista.CopiaCpf = copiaCpf;
            entrevista.CopiaEndereco = copiaEndereco;
            entrevista.CopiaCnh = copiaCnh;
            entrevista.CopiaCasamento = copiaCasamento;

            return entrevista;
        }
    }
}
