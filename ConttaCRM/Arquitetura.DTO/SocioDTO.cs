using Arquitetura.Dominio.Aggregates.Enums;

namespace Arquitetura.DTO
{
    public class SocioDTO
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public bool Administrador { get; set; }
        public string DataNascimento { get; set; }
        public string Rg { get; set; }
        public string OrgaoRG { get; set; }
        public string Cpf { get; set; }
        public string NomeMae { get; set; }
        public string NomePai { get; set; }
        public string Cnh { get; set; }
        public string Nacionalidade { get; set; }
        public string Naturalidade { get; set; }
        public eSexo? Sexo { get; set; }
        public eEstadoCivil? EstadoCivil { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
        public string Email { get; set; }
        public string Participacao { get; set; }
        public bool Assina { get; set; }
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Cep { get; set; }
        public string Referencia { get; set; }
        public eEstado? Estado { get; set; }
        public int EntrevistaId { get; set; }
    }
}