namespace Arquitetura.DTO
{
    public class ConfiguracaoServidorEmailDTO
    {
        public bool UtilizarEnvioDeEmail { get; set; }

        public string Conta { get; set; }

        public string Senha { get; set; }

        public string Smtp { get; set; }

        public int? Porta { get; set; }

        public bool Ssl { get; set; }

        public string PastaRaiz { get; set; }
    }
}
