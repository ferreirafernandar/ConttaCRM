using Arquitetura.Dominio.Aggregates.UsoSoloAgg;
using System.Data.Entity.ModelConfiguration;

namespace Arquitetura.Infraestrutura.UnitOfWork.Mapping
{
    public class UsoSoloEntityConfiguration : EntityTypeConfiguration<UsoSolo>
    {
        public UsoSoloEntityConfiguration()
        {
            this.HasKey(c => c.Id);

            this.Property(c => c.Id)
               .HasColumnName("ID_CADUSO")
               .IsRequired();

            this.Property(c => c.ImovelRual)
                .HasColumnName("IMOVELRUAL_CADUSO");

            this.Property(c => c.Iptu)
               .HasColumnName("IPTU_CADUSO")
               .IsRequired();

            this.Property(c => c.EnderecoRural)
                .HasColumnName("ENDERECORUAL_CADUSO")
                .HasMaxLength(255);

            this.Property(c => c.Complemento)
                .HasColumnName("COMPLEMENTO_CADUSO")
                .HasMaxLength(255);

            this.Property(c => c.Rua)
                .HasColumnName("RUA_CADUSO")
                .HasMaxLength(255);

            this.Property(c => c.Quadra)
                .HasColumnName("QUADRA_CADUSO")
                .HasMaxLength(45);

            this.Property(c => c.Lote)
                .HasColumnName("LOTE_CADUSO")
                .HasMaxLength(45);

            this.Property(c => c.Bairro)
                .HasColumnName("BAIRRO_CADUSO")
                .HasMaxLength(100);

            this.Property(c => c.Cnae)
                .HasColumnName("CNAE_CADUSO")
                .HasMaxLength(45);

            this.Property(c => c.Escritorio)
                .HasColumnName("ESCRITORIO_CADUSO");

            this.Property(c => c.DataCadastro)
               .HasColumnName("DATACADASTRO_CADUSO");

            this.Property(c => c.ResponsavelId)
               .HasColumnName("ID_CRES");

            this.HasOptional(c => c.Responsavel)
                .WithMany()
                .HasForeignKey(c => c.ResponsavelId)
                .WillCascadeOnDelete(false);

            this.ToTable("caduso");
        }
    }
}