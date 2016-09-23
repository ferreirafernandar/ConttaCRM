using Arquitetura.Dominio.Aggregates.ResponsavelAgg;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arquitetura.Infraestrutura.UnitOfWork.Mapping
{
    public class ResponsavelEntityConfiguration : EntityTypeConfiguration<Responsavel>
    {
        public ResponsavelEntityConfiguration()
        {
            this.HasKey(c => c.Id);

            this.Property(c => c.Id)
               .HasColumnName("ID_CRES")
               .IsRequired();

            this.Property(c => c.Nome)
                .HasColumnName("NOME_CRES")
                .IsRequired()
                .HasMaxLength(255);

            this.Property(c => c.Cpf)
                .HasColumnName("CPF_CRES")
                .IsRequired()
                .HasMaxLength(11);

            this.Property(c => c.Telefone)
                .HasColumnName("TELEFONE_CRES")
                .IsRequired()
                .HasMaxLength(15);

            this.Property(c => c.Celular)
                .HasColumnName("CELULAR_CRES")
                .IsRequired()
                .HasMaxLength(15);

            this.Property(c => c.Email)
                .HasColumnName("EMAIL_CRES")
                .IsRequired()
                .HasMaxLength(255);

            this.Property(c => c.Telefone)
                .HasColumnName("TELEFONE_CRES")
                .IsRequired()
                .HasMaxLength(15);

            this.Property(c => c.Celular)
                .HasColumnName("CELULAR_CRES")
                .IsRequired()
                .HasMaxLength(15);

            this.Property(c => c.DataCadastro)
                .HasColumnName("DATA_CRES");

            this.Property(c => c.Sexo)
                .HasColumnName("SEXO_CRES");

            this.Property(c => c.TipoAbertura)
                .HasColumnName("ABERTURA_CCLI");

            this.Property(c => c.EnviarEmail)
                .HasColumnName("ENVIAREMAIL_CCLI");

            this.Property(c => c.Rg)
                .HasColumnName("RG_CRES")
                .IsRequired()
                .HasMaxLength(15);

            this.Property(c => c.EstadoCivil)
               .HasColumnName("ESTADOCIVIL_CRES");

            this.ToTable("cadresp");

        }
    }
}
