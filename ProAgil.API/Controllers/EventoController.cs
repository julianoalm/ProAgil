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
                return Ok(returns);   
            }
            catch (System.Exception)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError, "Erro ao buscar Eventos.");
            }            
        }

        [HttpPost]
        public string Post(Evento evento)
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
                Context.SaveChanges();
                
                return "Evento cadastrado com sucesso!";
            }
            catch (System.Exception)
            {
                return "Erro ao cadastrar Eventos.";
            }            
        }

        [HttpPut]
        public string Put(Evento evento)
        {
            try
            {
                Context.Update(evento);
                Context.SaveChanges();
            
                return "Evento alterado com sucesso!";    
            }
            catch (System.Exception)
            {
                return "Erro ao alterar Eventos.";
            }             
        }

        [HttpDelete]
        public string Delete(Evento evento)
        {
            try
            {
                Context.Eventos.Remove(evento);
                Context.SaveChanges();
            
                return "Evento removido com sucesso!";    
            }
            catch (System.Exception)
            {
                return "Erro ao remover Eventos.";
            }             
        }
    }
}