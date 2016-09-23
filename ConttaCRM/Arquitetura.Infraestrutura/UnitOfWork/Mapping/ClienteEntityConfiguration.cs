using Arquitetura.Dominio.Aggregates.ClienteAgg;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arquitetura.Infraestrutura.UnitOfWork.Mapping
{
    public class ClienteEntityConfiguration : EntityTypeConfiguration<Cliente>
    {
        public ClienteEntityConfiguration()
        {
            this.HasKey(c => c.Id);

            this.Property(c => c.Id)
               .HasColumnName("ID_CCLI")
               .IsRequired();

            this.Property(c => c.NomeFantasia)
                .HasColumnName("NOMEFANTASIA_CCLI")
                .IsRequired()
                .HasMaxLength(255);

            this.Property(c => c.RazaoSocial)
                .HasColumnName("RAZAOSOCIAL_CCLI")
                .IsRequired()
                .HasMaxLength(255);

            this.Property(c => c.Cnpj)
                .HasColumnName("CNPJ_CCLI")
                .IsRequired()
                .HasMaxLength(14);

            this.Property(c => c.InscricaoEstadual)
                .HasColumnName("INSCRICAO_CCLI")
                .IsRequired()
                .HasMaxLength(15);

            this.Property(c => c.Email)
                .HasColumnName("EMAIL_CCLI")
                .IsRequired()
                .HasMaxLength(255);

            this.Property(c => c.Telefone)
                .HasColumnName("TELEFONE_CCLI")
                .IsRequired()
                .HasMaxLength(15);

            this.Property(c => c.Celular)
                .HasColumnName("CELULAR_CCLI")
                .IsRequired()
                .HasMaxLength(15);

            this.Property(c => c.Rua)
                .HasColumnName("RUA_CCLI")
                .IsRequired()
                .HasMaxLength(255);

            this.Property(c => c.Numero)
                .HasColumnName("NUMERO_CCLI")
                .HasMaxLength(11);

            this.Property(c => c.Complemento)
               .HasColumnName("COMPLEMENTO_CCLI")
               .HasMaxLength(255);

            this.Property(c => c.Bairro)
                .HasColumnName("BAIRRO_CCLI")
                .IsRequired()
                .HasMaxLength(150);

            this.Property(c => c.Cidade)
                .HasColumnName("CIDADE_CCLI")
                .IsRequired()
                .HasMaxLength(100);

            this.Property(c => c.Estado)
                .HasColumnName("ESTADO_CCLI");

            this.Property(c => c.Cep)
                .HasColumnName("CEP_CCLI")
                .HasMaxLength(10);

            this.Property(c => c.MatrizId)
                .HasColumnName("MATRIZID_CCLI");

            this.Property(c => c.TipoEmpresa)
                .HasColumnName("TIPOEMPRESA_CCLI");

            this.Property(c => c.Skype)
                .HasColumnName("SKYPE_CCLI")
                .HasMaxLength(100);

            this.Property(c => c.NomeResponsavel)
                 .HasColumnName("NOMERESPONSAVEL_CCLI")
                .IsRequired()
                .HasMaxLength(255);

            this.ToTable("cadcli");

        }
    }
}
