using Arquitetura.Dominio.Aggregates.Enums;
using Arquitetura.Dominio.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arquitetura.Dominio.Aggregates.ResponsavelAgg
{
   public static class ResponsavelFactory
    {
        public static Responsavel CreateResponsavel(
            string nome,
            string cpf,
            string telefone,
            string celular,
            string email,
            eSexo? sexo,
            DateTime? dataCadastro,
            eTipoAbertura? tipoAbertura,
            bool enviarEmail,
            string rg,
            eEstadoCivil? estadoCivil 
            )
        {
            var responsavel = new Responsavel();
                responsavel.Nome = nome;
                responsavel.Cpf = cpf;
                responsavel.Telefone = telefone;
                responsavel.Celular = celular;
                responsavel.Email = email;
                responsavel.Sexo = sexo;
                responsavel.DataCadastro = dataCadastro;
                responsavel.TipoAbertura = tipoAbertura;
                responsavel.EnviarEmail = enviarEmail;
                responsavel.Rg = rg;
                responsavel.EstadoCivil = estadoCivil;

                return responsavel;
        }
    }
}
