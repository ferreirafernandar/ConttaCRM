using System.IO;

namespace Arquitetura.DTO
{
    public class AnexoEmailDTO
    {
        public Stream ContentStream { get; set; }

        public string FileName { get; set; }
    }
}
