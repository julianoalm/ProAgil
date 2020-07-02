using System.ComponentModel.DataAnnotations;

namespace ProAgil.API.Model
{
    public class Evento
    {
        public int EventoId { get; set; }
        
        [Required(ErrorMessage = "O Local deve ser inserido")]
        [MinLength(3, ErrorMessage = "O Local deve conter no mínimo 3 caracteres")]
        [MaxLength(80, ErrorMessage = "O Local deve conter no máximo 80 caracteres")]
        public string Local { get; set; }
        public string  DataEvento { get; set; }
        
        [Required(ErrorMessage = "O Tema deve ser inserido")]
        [MinLength(3, ErrorMessage = "O Tema deve conter no mínimo 3 caracteres")]
        [MaxLength(80, ErrorMessage = "O Tema deve conter no máximo 80 caracteres")]
        [RegularExpression(@"^[ a-zA-Z á]*$", ErrorMessage = "O Tema deve conter apenas letras.")]
        public string Tema { get; set; }
        public int QtdPessoas { get; set; }
        public string  Lote { get; set; }
    }
}