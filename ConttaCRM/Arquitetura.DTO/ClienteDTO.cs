using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Arquitetura.Dominio.Enums;
using Arquitetura.Dominio.Aggregates.Enums;


namespace Arquitetura.DTO
{
    public class ClienteDTO
    {
        public int Id { get; set; }
        public string NomeFantasia { get; set; }
        public string RazaoSocial { get; set; }
        public string Cnpj { get; set; }
        public string InscricaoEstadual { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Celular { get; set; }
        public string Skype { get; set; }
        public string NomeResponsavel { get; set; }
        public string Rua { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public eEstado? Estado { get; set; }
        public string Cep { get; set; }
        public int? MatrizId { get; set; }
        public eTipoEmpresa? TipoEmpresa { get; set; }
    }
}
