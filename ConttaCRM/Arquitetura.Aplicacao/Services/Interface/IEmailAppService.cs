using Arquitetura.DTO;
using System;
using System.Collections.Generic;

namespace Arquitetura.Aplicacao.Services.Interface
{
    public interface IEmailAppService : IDisposable
    {
        void EnviarEmailAssync(List<EmailDTO> listaDeEmailDTO);
    }
}
