using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProAgil.API.Dtos;
using ProAgil.Domain;
using ProAgil.Repository;

namespace ProAgil.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        public readonly IProAgilRepository _repo;
        public IMapper _mapper { get; set; }
        public EventoController(IProAgilRepository repo, IMapper mapper)
        {
            this._mapper = mapper;
            this._repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var eventos = await _repo.GetAllEventoAsync(true);
                var results = _mapper.Map<EventoDto[]>(eventos); //Como eventos é uma lista, é preciso converter para Array.

                return Ok(results);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao buscar Eventos: { ex.Message }");
            }
        }

        [HttpPost("upload")]
        public IActionResult upload()
        {
            try
            {
                //Pega o arquivo enviado
                var file = Request.Form.Files[0];
                //Pega o diretório onde o arquivo será salvo
                var folderName = Path.Combine("Resources","Images");
                //Concatena o diretório da aplicação (API) com a pasta onde o arquivo será salvo
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    //Recuperar o FileName (nome do arquivo) que está sendo feito o upload
                    var filename = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName;
                    //Remove \ e espaços do nome completo do arquivo
                    var fullPath = Path.Combine(pathToSave, filename.Replace("\"", " ").Trim());

                    using(var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        //Salva o arquivo na pasta
                        file.CopyTo(stream);
                    }
                }

                return Ok();
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao buscar Eventos: { ex.Message }");
            }

            // return BadRequest("Erro ao tentar realizar upload");
        }

        [HttpGet("{EventoId}")]
        public async Task<IActionResult> Get(int EventoId)
        {
            try
            {
                var evento = await _repo.GetEventoAsyncById(EventoId, true);
                var results = _mapper.Map<EventoDto>(evento); 

                if (results == null)
                {
                    return NotFound();
                }

                return Ok(results);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao buscar Eventos: { ex.Message }");
            }
        }

        [HttpGet("getByTema/{tema}")]
        public async Task<IActionResult> Get(string tema)
        {
            try
            {
                var eventos = await _repo.GetAllEventoAsyncByTema(tema, true);
                var results = _mapper.Map<EventoDto[]>(eventos);

                if (results == null)
                {
                    return NotFound();
                }

                return Ok(results);
            }
            catch (System.Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, $"Erro ao buscar Eventos: { ex.Message }");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(EventoDto eventoDto)
        {
            try
            {
                var evento = _mapper.Map<Evento>(eventoDto);
                _repo.Add(evento);

                if (await _repo.SaveChangesAsync())
                {
                    return Created($"/api/evento/{eventoDto.Id}", _mapper.Map<EventoDto>(evento));
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return BadRequest();
        }

        [HttpPut("{EventoId}")]
        public async Task<IActionResult> Put(int EventoId, EventoDto eventoDto)
        {
            try
            {
                var evento = await _repo.GetEventoAsyncById(EventoId, false);
                if (evento == null) return NotFound();

                var idLotes = new List<int>();
                var idRedesSociais = new List<int>();

                eventoDto.Lotes.ForEach(item => idLotes.Add(item.Id));
                eventoDto.RedesSociais.ForEach(item => idRedesSociais.Add(item.Id));

                var lotes = evento.Lotes.Where(
                    lote => !idLotes.Contains(lote.Id)
                ).ToArray();

                var redesSociais = evento.RedesSociais.Where(
                    rede => !idLotes.Contains(rede.Id)
                ).ToArray();

                if (lotes.Length > 0) _repo.DeleteRange(lotes);
                if (redesSociais.Length > 0) _repo.DeleteRange(redesSociais);

                _mapper.Map(eventoDto, evento); //Atualiza o evento com o Mapper para atualização no banco. 

                _repo.Update(evento);

                if (await _repo.SaveChangesAsync())
                {
                    return Created($"/api/evento/{eventoDto.Id}", _mapper.Map<EventoDto>(evento));
                }            
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return BadRequest();
        }

        [HttpDelete("{EventoId}")]
        public async Task<IActionResult> Delete(int EventoId)
        {
            try
            {
                var evt = await _repo.GetEventoAsyncById(EventoId, false);

                if (evt == null)
                    return NotFound();

                _repo.Delete(evt);

                if (await _repo.SaveChangesAsync())
                {
                    return Ok();
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return BadRequest();
        }
    }
}