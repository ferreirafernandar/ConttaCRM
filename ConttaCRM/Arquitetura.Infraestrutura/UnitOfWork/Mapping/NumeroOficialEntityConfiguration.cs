using Arquitetura.Dominio.Aggregates.NumeroOficialAgg;
using System.Data.Entity.ModelConfiguration;

namespace Arquitetura.Infraestrutura.UnitOfWork.Mapping
{
    public class NumeroOficialEntityConfiguration : EntityTypeConfiguration<NumeroOficial>
    {
        public NumeroOficialEntityConfiguration()
        {
            this.HasKey(c => c.Id);

            this.Property(c => c.Id)
               .HasColumnName("ID_COFI")
               .IsRequired();

            this.Property(c => c.Requerente)
                .HasColumnName("REQUERENTE_COFI")
                .HasMaxLength(255)
                .IsRequired();

            this.Property(c => c.Rg)
               .HasColumnName("RG_COFI")
               .HasMaxLength(45)
               .IsRequired();

            this.Property(c => c.PossuiIptu)
                .HasColumnName("POSSUIIPTU_COFI");

            this.Property(c => c.Iptu)
                .HasColumnName("IPTU_COFI")
                .IsRequired();

            this.Property(c => c.Rua)
                .HasColumnName("RUA_COFI")
                .IsRequired()
                .HasMaxLength(255);

            this.Property(c => c.ExisteEdificacao)
                .HasColumnName("EXISTEEDIFICACAO_COFI");

            this.Property(c => c.Atividade)
                .HasColumnName("ATIVIDADE_COFI")
                .IsRequired()
                .HasMaxLength(255);

            this.Property(c => c.Telefone)
                .HasColumnName("TELEFONE_COFI")
                .IsRequired()
                .HasMaxLength(15);

            this.Property(c => c.SituacaoLocal)
                .HasColumnName("SITUACAOLOCAL_COFI")
                .IsRequired();

            this.Property(c => c.GerarNumeroOficial)
                .HasColumnName("GERARNUMEROOFICIAL_COFI");

            this.Property(c => c.NumeroOficialB)
                .HasColumnName("NUMEROOFICIALB_COFI");

            this.Property(c => c.NumeroOficialC)
                .HasColumnName("NUMEROOFICIALC_COFI");

            this.Property(c => c.Observacoes)
                .HasColumnName("OBSERVACOES_COFI");

            this.Property(c => c.DataCadastro)
               .HasColumnName("DATACADASTRO_COFI");

            this.Property(c => c.ResponsavelId)
               .HasColumnName("ID_CRES");

            this.HasOptional(c => c.Responsavel)
                .WithMany()
                .HasForeignKey(c => c.ResponsavelId)
                .WillCascadeOnDelete(false);

            this.ToTable("cadofi");
        }
    }
}