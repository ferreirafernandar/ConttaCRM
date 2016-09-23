using System;
namespace Arquitetura.Web.Models
{
    public class UploadFilesResult
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public int Length { get; set; }
        
        public string Type { get; set; }

        public string Date { get; set; }
    }
}