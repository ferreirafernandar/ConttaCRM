using System.ComponentModel.DataAnnotations;

namespace Arquitetura.Web.Models
{
    public class ContatoModel
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Email é obrigatório")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mensagem é obrigatória")]
        public string Mensagem { get; set; }

        [Required(ErrorMessage = "Telefone é obrigatório")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "Assunto é obrigatório")]
        public string Assunto { get; set; }
    }
}