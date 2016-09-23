using System.Data.Entity.ModelConfiguration;
using Arquitetura.Dominio.Aggregates.ConfiguracaoAgg;

namespace Arquitetura.Infraestrutura.UnitOfWork.Mapping
{
    class ConfiguracaoServidorEmailEntityConfiguration : EntityTypeConfiguration<ConfiguracaoServidorEmail>
    {
        public ConfiguracaoServidorEmailEntityConfiguration()
        {
            // Configurando propriedades e chaves
            this.HasKey(c => c.Id);

            this.Property(c => c.Id)
                .HasColumnName("ID_CSIS")
                .IsRequired();

            this.Property(c => c.UtilizarEnvioDeEmail)
                .HasColumnName("UTILIZAR_CSIS")
                .IsRequired();

            this.Property(c => c.Conta)
                .HasColumnName("CONTA_CSIS")
                .HasMaxLength(320);

            this.Ignore(c => c.Senha);
            this.Property(c => c.SenhaCriptografada)
                .HasColumnName("SENHA_CSIS");

            this.Property(c => c.Smtp)
                .HasColumnName("SMTP_CSIS")
                .HasMaxLength(320);

            this.Property(c => c.Porta)
                .HasColumnName("PORTA_CSIS");

            this.Property(c => c.Ssl)
                .HasColumnName("SSL_CSIS")
                .IsRequired();

            this.Property(c => c.PastaRaiz)
                .HasColumnName("PASTA_CSIS");

            //Configurando a Tabela
            this.ToTable("cfgsis");
        }
    }
}
