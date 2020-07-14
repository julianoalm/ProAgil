using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProAgil.Domain;
using ProAgil.Repository;

namespace ProAgil.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventoController : ControllerBase
    {
        public readonly IProAgilRepository _repo;
        public EventoController(IProAgilRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var results = await _repo.GetAllEventoAsync(true);
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao buscar Eventos.");
            }
        }

        [HttpGet("{EventoId}")]
        public async Task<IActionResult> Get(int EventoId)
        {
            try
            {
                var returns = await _repo.GetEventoAsyncById(EventoId, true); 

                if (returns == null)
                {
                    return NotFound();
                }

                return Ok(returns);   
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao buscar Eventos.");
            }            
        }

        [HttpGet("getByTema/{tema}")]
        public async Task<IActionResult> Get(string tema)
        {
            try
            {
                var returns = await _repo.GetAllEventoAsyncByTema(tema, true); 

                if (returns == null)
                {
                    return NotFound();
                }

                return Ok(returns);   
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao buscar Eventos.");
            }            
        }

        [HttpPost]
        public async Task<IActionResult> Post(Evento evento)
        {
            try
            {
                _repo.Add(evento);

                if (await _repo.SaveChangesAsync())
                {
                    return Created($"/api/evento/{evento.Id}", evento);
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return BadRequest();            
        }

        [HttpPut]
        public async Task<IActionResult> Put(int EventoId, Evento evento)
        {
            try
            {
                var evt = await _repo.GetEventoAsyncById(EventoId, false);

                if (evt == null)
                    return NotFound();

                _repo.Update(evento);

                if (await _repo.SaveChangesAsync())
                {
                    return Created($"/api/evento/{evento.Id}", evento);
                }
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }

            return BadRequest();         
        }

        [HttpDelete]
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