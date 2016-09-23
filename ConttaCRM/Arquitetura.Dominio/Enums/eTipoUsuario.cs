using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arquitetura.Dominio.Enums
{
    public enum eTipoUsuario
    {
        [Description("Administrador")]
        Administrador = 0, 

        [Description("Funcionário")]
        Funcionario = 1,
        
        [Description("Gerente")]
        Gerente = 2,

        [Description("Suporte")]
        Suporte = 3,

        [Description("Usuário")]
        Usuario = 4
    }
}
