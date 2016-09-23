using Arquitetura.Dominio.Aggregates.Enums;
using Arquitetura.Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arquitetura.Dominio.Aggregates.ClienteAgg
{
    public static class ClienteFactory
    {
        public static Cliente CreateCliente(
            string nomeFantasia,
            string razaoSocial,
            string cnpj,
            string inscricaoEstadual,
            string email,
            string telefone,
            string celular,
            string skype,
            string nomeResponsavel,
            string rua,
            string numero,
            string complemento,
            string bairro,
            string cidade,
            eEstado? estado,
            string cep,
            eTipoEmpresa? tipoEmpresa,
            int? matrizId
            )
        {
            var cliente = new Cliente();
                cliente.NomeFantasia = nomeFantasia;
                cliente.RazaoSocial = razaoSocial;
                cliente.Cnpj = cnpj;
                cliente.InscricaoEstadual = inscricaoEstadual;
                cliente.Email = email;
                cliente.Telefone = telefone;
                cliente.Cnpj = cnpj;
                cliente.Telefone = telefone;
                cliente.Celular = celular;
                cliente.Skype = skype;
                cliente.NomeResponsavel = nomeResponsavel;
                cliente.Rua = rua;
                cliente.Numero = numero;
                cliente.Complemento = complemento;
                cliente.Bairro = bairro;
                cliente.Cidade = cidade;
                cliente.Estado = estado;
                cliente.Cep = cep;
                cliente.TipoEmpresa = tipoEmpresa;

                if (tipoEmpresa == eTipoEmpresa.Matriz && matrizId.HasValue)
                {
                    cliente.MatrizId = null;
                }
                else
                {
                    cliente.MatrizId = matrizId;
                }
                return cliente;
        }
    }
}
