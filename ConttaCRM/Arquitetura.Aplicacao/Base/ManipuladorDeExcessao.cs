using Arquitetura.Infraestrutura.Logging;
using System;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;

namespace Arquitetura.Aplicacao.Base
{
    public static class ManipuladorDeExcecao
    {
        public static Exception TrateExcecao(Exception ex)
        {
            var appException = ex as AppException;
            if (appException != null)
            {
                throw appException;
            }

            var dbUpdateException = ex as DbUpdateException;
            if (dbUpdateException != null)
            {
                UpdateException updateException = (UpdateException)dbUpdateException.InnerException;
                string mensagemErro = updateException.InnerException.Message;

                if (mensagemErro.Contains("Cannot delete or update a parent row: a foreign key constraint fails"))
                {
                    throw new AppException("Não é possível excluir o registro, pois existem dados associados a ele.");
                }
                else
                {
                    LoggerFactory.CreateLog().LogError(ex);
                    throw new AppException(dbUpdateException.Message);
                }
            }

            LoggerFactory.CreateLog().LogError(ex);
            throw new Exception("Falha de comunicação com o servidor.");
        }
    }
}
