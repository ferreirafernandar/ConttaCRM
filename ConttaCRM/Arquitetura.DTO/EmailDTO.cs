using System;

namespace Arquitetura.DTO
{
    public class EmailDTO
    {
        public string De { get; set; }

        public string NomeDe { get; set; }

        public string Para { get; set; }

        public string NomePara { get; set; }

        public string Assunto { get; set; }

        public string Mensagem { get; set; }

        public ConfiguracaoServidorEmailDTO ConfiguracaoServidorEmailDTO { get; set; }
    }
}
