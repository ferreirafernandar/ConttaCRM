using System.Data.Entity.ModelConfiguration;
using Arquitetura.Dominio.Aggregates.EntrevistaAgg;

namespace Arquitetura.Infraestrutura.UnitOfWork.Mapping
{
    class EntrevistaEntityConfiguration : EntityTypeConfiguration<Entrevista>
    {
        public EntrevistaEntityConfiguration()
        {
            // Configurando propriedades e chaves
            this.HasKey(c => c.Id);

            this.Property(c => c.Id)
                .HasColumnName("ID_CADENT")
                .IsRequired();

            this.Property(c => c.NomeDaEmpresa1)
                .HasColumnName("NOMEEMPRESA1_CADENT")
                .IsRequired()
                .HasMaxLength(255);

            this.Property(c => c.NomeDaEmpresa2)
                .HasColumnName("NOMEEMPRESA2_CADENT")
                .HasMaxLength(255);

            this.Property(c => c.NomeDaEmpresa3)
                .HasColumnName("NOMEEMPRESA3_CADENT")
                .HasMaxLength(255);

            this.Property(c => c.Iptu)
                .HasColumnName("IPTU_CADENT");

            this.Property(c => c.NomeFantasia)
                .HasColumnName("NOMEFANTASIA_CADENT")
                .IsRequired()
                .HasMaxLength(255);

            this.Property(c => c.CapitalSocial)
                .HasColumnName("CAPITALSOCIAL_CADENT")
                .IsRequired()
                .HasMaxLength(45);

            this.Property(c => c.Objetivo)
                .HasColumnName("OBJETIVO_CADENT")
                .IsRequired()
                .HasMaxLength(500);

            this.Property(c => c.Metragem)
                .HasColumnName("METRAGEM_CADENT")
                .IsRequired()
                .HasMaxLength(45);

            this.Property(c => c.PontoDeReferencia)
                .HasColumnName("PONTOREFERENCIA_CADENT");

            this.Property(c => c.LivroRegistroEmpregados)
                .HasColumnName("LIVROREGISTRO_CADENT");

            this.Property(c => c.InspencaoTrabalho)
                .HasColumnName("INSPENCAOTRABALHO_CADENT");

            this.Property(c => c.LivroTermoOcorrencia)
                .HasColumnName("LIVROTERMOOCORRENCIA_CADENT");

            this.Property(c => c.Telefone)
                .HasColumnName("TELEFONE_CADENT")
                .IsRequired()
                .HasMaxLength(15);

            this.Property(c => c.Email)
                .HasColumnName("EMAIL_CADENT")
                .IsRequired()
                .HasMaxLength(255);

            this.Property(c => c.ClienteId)
               .HasColumnName("ID_CCLI");

            this.Property(c => c.UsuarioId)
               .HasColumnName("ID_CUSU");

            this.Property(c => c.ResponsavelId)
               .HasColumnName("ID_CRES");

            this.Property(c => c.DataCadastro)
                .HasColumnName("DATA_CADENT");

            this.Property(c => c.CopiaRg)
               .HasColumnName("COPIARG_CADENT");

            this.Property(c => c.CopiaCpf)
               .HasColumnName("COPIACPF_CADENT");

            this.Property(c => c.CopiaEndereco)
               .HasColumnName("COPIAENDERECO_CADENT");

            this.Property(c => c.CopiaCnh)
               .HasColumnName("COPIACNH_CADENT");

            this.Property(c => c.CopiaCasamento)
               .HasColumnName("COPIACASAMENTO_ENT");

            this.HasOptional(c => c.Cliente)
                .WithMany()
                .HasForeignKey(c => c.ClienteId)
                .WillCascadeOnDelete(false);

            this.HasOptional(c => c.Usuario)
                .WithMany()
                .HasForeignKey(c => c.UsuarioId)
                .WillCascadeOnDelete(false);

            this.HasOptional(c => c.Responsavel)
                .WithMany()
                .HasForeignKey(c => c.ResponsavelId)
                .WillCascadeOnDelete(false);

            //Configurando a Tabela
            this.ToTable("cadent");
        }
    }
}
