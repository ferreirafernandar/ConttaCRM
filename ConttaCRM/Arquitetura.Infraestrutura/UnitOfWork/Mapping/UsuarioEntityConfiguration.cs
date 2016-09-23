using System.Data.Entity.ModelConfiguration;
using Arquitetura.Dominio.Aggregates.UsuarioAgg;

namespace Arquitetura.Infraestrutura.UnitOfWork.Mapping
{
    class UsuarioEntityConfiguration : EntityTypeConfiguration<Usuario>
    {
        public UsuarioEntityConfiguration()
        {
            // Configurando propriedades e chaves
            this.HasKey(c => c.Id);

            this.Property(c => c.Id)
                .HasColumnName("ID_CUSU")
                .IsRequired();

            this.Property(c => c.NomeUsuario)
                .HasColumnName("NOMEUSUARIO_CUSU")
                .IsRequired()
                .HasMaxLength(100);

            this.Property(c => c.Email)
                .HasColumnName("EMAIL_CUSU")
                .IsRequired()
                .HasMaxLength(300);

            this.Property(c => c.Senha)
                .HasColumnName("SENHA_CUSU")
                .IsRequired()
                .HasMaxLength(255);

            this.Property(c => c.Nome)
                .HasColumnName("NOME_CUSU")
                .IsRequired()
                .HasMaxLength(500);

            this.Property(c => c.Cpf)
                .HasColumnName("CPF_CUSU")
                .IsRequired()
                .HasMaxLength(15);

            this.Property(c => c.Endereco)
                .HasColumnName("ENDERECO_CUSU")
                .IsRequired()
                .HasMaxLength(500);

            this.Property(c => c.Complemento)
                .HasColumnName("COMPLEMENTO_CUSU")
                .HasMaxLength(500);

            this.Property(c => c.Numero)
                .HasColumnName("NUMERO_CUSU")
                .HasMaxLength(11);

            this.Property(c => c.Bairro)
                .HasColumnName("BAIRRO_CUSU")
                .IsRequired()
                .HasMaxLength(255);

            this.Property(c => c.Cidade)
                .HasColumnName("CIDADE_CUSU")
                .IsRequired()
                .HasMaxLength(255);

            this.Property(c => c.Estado)
                .HasColumnName("ESTADO_CUSU");

            this.Property(c => c.Cep)
                .HasColumnName("CEP_CUSU")
                .HasMaxLength(18);

            this.Property(c => c.Telefone)
                .HasColumnName("TELEFONE_CUSU")
                .IsRequired()
                .HasMaxLength(15);

            this.Property(c => c.Celular)
                .HasColumnName("CELULAR_CUSU")
                .IsRequired()
                .HasMaxLength(15);

            this.Property(c => c.Sexo)
                .HasColumnName("SEXO_CUSU");

            this.Property(c => c.Ativo)
                .IsRequired()
                .HasColumnName("ATIVO_CUSU");

            this.Property(c => c.TipoUsuario)
                .HasColumnName("TIPOUSUARIO_CUSU");

            this.Property(c => c.ClienteId)
                .HasColumnName("CLI_ID");

            this.HasOptional(c => c.Cliente)
                .WithMany()
                .HasForeignKey(c => c.ClienteId)
                .WillCascadeOnDelete(false);
               
            //Configurando a Tabela
            this.ToTable("cadusu");
        }
    }
}
