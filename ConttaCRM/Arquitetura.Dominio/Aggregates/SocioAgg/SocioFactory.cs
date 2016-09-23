using Arquitetura.Dominio.Aggregates.Enums;

namespace Arquitetura.Dominio.Aggregates.SocioAgg
{
    public static class SocioFactory
    {
        public static Socio CreateSocio(
             string nome,
             bool administrador,
             string dataNascimento,
             string rg,
             string ogaoRG,
             string cpf,
             string nomeMae,
             string nomePai,
             string cnh,
             string nacionalidade,
             string naturalidade,
             eSexo? sexo,
             eEstadoCivil? estadoCivil,
             string telefone,
             string celular,
             string email,
             string participacao,
             bool assina,
             string rua,
             string numero,
             string complemento,
             string bairro,
             string cidade,
             string cep,
             string referencia,
             eEstado? estado,
             int entrevistaId

             )
        {
            var socio = new Socio();
            socio.Nome = nome;
            socio.Administrador = administrador;
            socio.DataNascimento = dataNascimento;
            socio.Rg = rg;
            socio.OrgaoRG = ogaoRG;
            socio.Sexo = sexo;
            socio.Cpf = cpf;
            socio.NomeMae = nomeMae;
            socio.NomePai = nomePai;
            socio.Cnh = cnh;
            socio.Nacionalidade = nacionalidade;
            socio.Naturalidade = naturalidade;
            socio.Sexo = sexo;
            socio.EstadoCivil = estadoCivil;
            socio.Telefone = telefone;
            socio.Celular = celular;
            socio.Email = email;
            socio.Participacao = participacao;
            socio.Assina = assina;
            socio.Rua = rua;
            socio.Numero = numero;
            socio.Complemento = complemento;
            socio.Bairro = bairro;
            socio.Cidade = cidade;
            socio.Cep = cep;
            socio.Referencia = referencia;
            socio.Estado = estado;
            socio.EntrevistaId = entrevistaId;

            return socio;
        }
    }
}