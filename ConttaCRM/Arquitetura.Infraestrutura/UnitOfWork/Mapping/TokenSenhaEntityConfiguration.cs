using System.Data.Entity.ModelConfiguration;
using Arquitetura.Dominio.Aggregates.UsuarioAgg;

namespace Arquitetura.Infraestrutura.UnitOfWork.Mapping
{
    class TokenSenhaEntityConfiguration : EntityTypeConfiguration<TokenSenha>
    {
        public TokenSenhaEntityConfiguration()
        {
            // Configurando propriedades e chaves
            this.HasKey(c => c.Id);

            this.Property(c => c.Id)
                .HasColumnName("ID_CSEN")
                .IsRequired();

            this.Property(c => c.UsuarioId)
                .HasColumnName("ID_CUSU")
                .IsRequired();

            this.Property(c => c.Token)
                .HasColumnName("TOKEN_CSEN")
                .IsRequired();

            this.Property(c => c.Data)
                .HasColumnName("DATA_CSEN")
                .IsRequired();

            // Configurando associações
            this.HasRequired(c => c.Usuario)
                .WithMany()
                .HasForeignKey(c => c.UsuarioId);

            // Configurando tabela
            this.ToTable("cadsen");
        }
    }
}
