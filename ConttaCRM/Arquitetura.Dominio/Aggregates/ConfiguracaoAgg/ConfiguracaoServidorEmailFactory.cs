using Arquitetura.Dominio.Aggregates.Base;
using Arquitetura.Dominio.Base;

namespace Arquitetura.Dominio.Aggregates.ConfiguracaoAgg
{
    public static class ConfiguracaoServidorEmailFactory
    {
        public static ConfiguracaoServidorEmail CreateConfiguracaoServidorEmail(
            bool utilizarEnvioDeEmail,
            string conta,
            string senha,
            string smtp,
            int? porta,
            bool ssl,
            string pastaRaiz)
        {
            var configuracaoServidorEmail = new ConfiguracaoServidorEmail();

            configuracaoServidorEmail.UtilizarEnvioDeEmail = utilizarEnvioDeEmail;
            configuracaoServidorEmail.Porta = porta;
            configuracaoServidorEmail.Ssl = ssl;

            if (pastaRaiz != null)
            {
                configuracaoServidorEmail.PastaRaiz = pastaRaiz.Trim();
            }

            if (conta != null)
            {
                configuracaoServidorEmail.Conta = conta.Trim();
            }

            if (senha != null)
            {
                configuracaoServidorEmail.Senha = Encryption.Encrypt(senha);
            }

            if (smtp != null)
            {
                configuracaoServidorEmail.Smtp = smtp.Trim();
            }

            return configuracaoServidorEmail;
        }
    }
}
