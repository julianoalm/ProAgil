using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProAgil.API.Dtos
{
    public class EventoDto
    {
        public int Id { get; set; }

        [Required (ErrorMessage="O capmpo {0} é obrigatório.")]
        [StringLength (100, MinimumLength=3, ErrorMessage="O campo Local deve ter entre 3 e 100 carateres")]
        public string Local { get; set; }
        public string  DataEvento { get; set; }
        
        [Required (ErrorMessage="O capmpo {0} é obrigatório.")]
        public string Tema { get; set; }
        
        [Range(2, 120000, ErrorMessage="A quantodade de Pessoas é entre 2 e 120.000")]
        public int qtdPessoas { get; set; }
        public string  ImagemURL { get; set; }
        
        [Phone]
        public string  Telefone { get; set; }

        [EmailAddress]
        public string  Email { get; set; }
        public List<LoteDto> Lotes { get; set; }
        public List<RedeSocialDto> RedesSociais { get; set; }
        public List<PalestranteDto> Palestrantes { get; set; }
    }
}