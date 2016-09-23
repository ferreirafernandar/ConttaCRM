using Arquitetura.Dominio.Aggregates.Enums;
using Arquitetura.Dominio.Enums;
using System;
using System.Collections.Generic;

namespace Arquitetura.DTO
{
    public class UsuarioDTO
    {
        public int Id { get; set; }

        public string NomeUsuario { get; set; }

        public string Email { get; set; }

        public string Senha { get; set; }

        public string Nome { get; set; }

        public string Cpf { get; set; }

        public string Endereco { get; set; }

        public string Complemento { get; set; }

        public string Numero { get; set; }

        public string Bairro { get; set; }

        public string Cidade { get; set; }

        public eEstado? Estado { get; set; }

        public string Cep { get; set; }

        public string Telefone { get; set; }

        public string Celular { get; set; }

        public eSexo? Sexo { get; set; }

        public bool Ativo { get; set; }

        public eTipoUsuario? TipoUsuario { get; set; }
        public int? ClienteId { get; set; }
    }
}
