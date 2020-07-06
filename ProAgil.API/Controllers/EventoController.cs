using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ProAgil.API.Data;
using ProAgil.API.Model;

namespace ProAgil.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EventoController : ControllerBase
    {
        public DataContext Context { get; }
        public EventoController(DataContext context)
        {
            this.Context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var results = await Context.Eventos.ToListAsync();
                return Ok(results);
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao buscar Eventos.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var returns = await Context.Eventos.FirstOrDefaultAsync(x => x.EventoId == id);

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
                var evt = new Evento
                {
                    Local = evento.Local,
                    DataEvento = evento.DataEvento,
                    QtdPessoas = evento.QtdPessoas,
                    Tema = evento.Tema,
                    Lote = evento.Lote
                };

                Context.Add(evt);
                await Context.SaveChangesAsync();
                
                return CreatedAtAction("Get", new { id = evt.EventoId }, evt);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }

        [HttpPut]
        public async Task<IActionResult> Put(int id, Evento evento)
        {
            try
            {
                if (id != evento.EventoId)
                {
                    return BadRequest();
                }

                Context.Entry(evento).State = EntityState.Modified;

                await Context.SaveChangesAsync();
            
                return NoContent();    
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }             
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(Evento evento)
        {
            try
            {
                var evt = await Context.Eventos.FindAsync(evento.EventoId);
                if (evt == null)
                {
                    return NotFound();
                }

                Context.Eventos.Remove(evento);
                await Context.SaveChangesAsync();
                return Ok(evt);
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }             
        }
    }
}