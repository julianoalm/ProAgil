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
    public class ValueController : ControllerBase
    {
        public ProAgilContext Context { get; }
        public ValueController(ProAgilContext context)
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
                var returns = await Context.Eventos.FirstOrDefaultAsync(x => x.Id == id);

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
                    Lotes = evento.Lotes
                };

                Context.Add(evt);
                await Context.SaveChangesAsync();
                
                return CreatedAtAction("Get", new { id = evt.Id }, evt);
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
                if (id != evento.Id)
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
                var evt = await Context.Eventos.FindAsync(evento.Id);
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