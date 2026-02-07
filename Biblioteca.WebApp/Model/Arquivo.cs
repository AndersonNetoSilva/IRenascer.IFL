using System.ComponentModel.DataAnnotations;

namespace IFL.WebApp.Model
{
    public class Arquivo: EntityBase
    {
        public string NomeOriginal { get; set; } = null!;
        public string Descricao { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public byte[] Conteudo { get; set; } = null!;
        public int Tamanho { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime? DataUltimaAlteracao { get; set; } = null;
    }

    public class ArquivoVM 
    {
        public int Id { get; set; }
        public string? Descricao { get; set; } = null;

        [MaxFileSize(2 * 1024 * 1024)]
        public IFormFile? FormFile { get; set; }
    }

    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly long _maxFileSize;

        public MaxFileSizeAttribute(long maxFileSize)
        {
            _maxFileSize = maxFileSize;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var file = value as IFormFile;

            if (file == null)
                return ValidationResult.Success;

            if (file.Length > _maxFileSize)
                return new ValidationResult($"O arquivo deve ter no máximo {_maxFileSize / 1024 / 1024} MB.");

            return ValidationResult.Success;
        }
    }
}
