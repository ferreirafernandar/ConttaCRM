using Arquitetura.Aplicacao.Base;
using Arquitetura.Dominio.ControladorDeSessao;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Arquitetura.Aplicacao.AOP
{
    internal class AutenticacaoCallHandler : ICallHandler
    {
        public int Order { get; set; }

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            bool autenticado = true;
            autenticado = ControladorDeSessao.EstaAutenticado();

            if (autenticado)
            {
                IMethodReturn result = getNext()(input, getNext);
                return result;
            }
            else
            {
                // TODO: criar tratamento de erro para autenticacao
                return input.CreateExceptionMethodReturn(new AppException("Usuário não autenticado."));
            }
        }
    }
}
