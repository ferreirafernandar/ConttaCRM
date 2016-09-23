using Arquitetura.Dominio.Aggregates.Enums;
using System;

namespace Arquitetura.DTO
{
    public class NumeroOficialListDTO
    {
        public int Id { get; set; }
        public string Requerente { get; set; }
        public string Rg { get; set; }
        public string Telefone { get; set; }
    }
}