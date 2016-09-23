using Arquitetura.Dominio.Aggregates.SocioAgg;
using System.Data.Entity.ModelConfiguration;

namespace Arquitetura.Infraestrutura.UnitOfWork.Mapping
{
    public class SocioEntityConfiguration : EntityTypeConfiguration<Socio>
    {
        public SocioEntityConfiguration()
        {
            this.HasKey(c => c.Id);

            this.Property(c => c.Id)
                .HasColumnName("ID_CADSOC")
                .IsRequired();

            this.Property(c => c.Nome)
                .HasColumnName("NOME_CADSOC")
                .IsRequired()
                .HasMaxLength(255);

            this.Property(c => c.Administrador)
                .HasColumnName("ADMINISTRADOR_CADSOC");

            this.Property(c => c.DataNascimento)
                .HasColumnName("DATANASCIMENTO_CADSOC")
                .IsRequired()
                .HasMaxLength(45);

            this.Property(c => c.Rg)
                .HasColumnName("RG_CADSOC")
                .IsRequired()
                .HasMaxLength(15);

            this.Property(c => c.OrgaoRG)
                .HasColumnName("ORGAORG_CADSOC")
                .IsRequired()
                .HasMaxLength(45);

            this.Property(c => c.Cpf)
                .HasColumnName("CPF_CADSOC")
                .IsRequired()
                .HasMaxLength(11);

            this.Property(c => c.NomePai)
                 .HasColumnName("NOMEPAI_CADSOC")
                 .HasMaxLength(255);

            this.Property(c => c.NomeMae)
                .HasColumnName("NOMEMAE_CADSOC")
                .IsRequired()
                .HasMaxLength(255);

            this.Property(c => c.Cnh)
                .HasColumnName("CNH_CADSOC")
                .HasMaxLength(15);

            this.Property(c => c.Nacionalidade)
                .HasColumnName("NACIONALIDADE_CADSOC")
                .IsRequired()
                .HasMaxLength(100);

            this.Property(c => c.Naturalidade)
                .HasColumnName("NATURALIDADE_CADSOC")
                .IsRequired()
                .HasMaxLength(100);

            this.Property(c => c.Sexo)
                .HasColumnName("SEXO_CADSOC");

            this.Property(c => c.EstadoCivil)
                .HasColumnName("ESTADOCIVIL_CADSOC");

            this.Property(c => c.Telefone)
                .HasColumnName("TELEFONE_CADSOC")
                .IsRequired()
                .HasMaxLength(15);

            this.Property(c => c.Celular)
                .HasColumnName("CELULAR_CADSOC")
                .IsRequired()
                .HasMaxLength(15);

            this.Property(c => c.Email)
                .HasColumnName("EMAIL_CADSOC")
                .IsRequired()
                .HasMaxLength(255);

            this.Property(c => c.Participacao)
                .HasColumnName("PARTICIPACAO_CADSOC")
                .IsRequired()
                .HasMaxLength(10);

            this.Property(c => c.Assina)
                .HasColumnName("ASSINA_CADSOC")
                .IsRequired();

            this.Property(c => c.Rua)
                .HasColumnName("RUA_CADSOC")
                .IsRequired()
                .HasMaxLength(255);

            this.Property(c => c.Numero)
                .HasColumnName("NUMERO_CADSOC")
                .HasMaxLength(11);

            this.Property(c => c.Complemento)
                .HasColumnName("COMPLEMENTO_CADSOC")
                .HasMaxLength(255);

            this.Property(c => c.Bairro)
                .HasColumnName("BAIRRO_CADSOC")
                .IsRequired()
                .HasMaxLength(100);

            this.Property(c => c.Cidade)
                .HasColumnName("CIDADE_CADSOC")
                .IsRequired()
                .HasMaxLength(100);

            this.Property(c => c.Cep)
                .HasColumnName("CEP_CADSOC")
                .HasMaxLength(20);

            this.Property(c => c.Referencia)
                .HasColumnName("REFERENCIA_CADSOC")
                .HasMaxLength(100);

            this.Property(c => c.Estado)
                .HasColumnName("ESTADO_CADSOC");

            this.Property(c => c.EntrevistaId)
                .HasColumnName("ID_CADENT")
                .IsRequired();

            this.HasRequired(c => c.Entrevista)
                .WithMany(c => c.ListaDeSocios)
                .HasForeignKey(c => c.EntrevistaId)
                .WillCascadeOnDelete(false);

            this.ToTable("cadsoc");
        }
    }
}