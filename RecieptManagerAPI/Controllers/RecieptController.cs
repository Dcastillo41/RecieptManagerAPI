using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using RecieptManagerAPI.Services;
using RecieptManagerAPI.Models;

namespace RecieptManagerAPI.Controllers
{
    [ApiController]
    [Route("reciepts")]
    public class RecieptController : ControllerBase
    {
        private readonly ILogger<RecieptController> _logger;
        private readonly RecieptService _recieptService;

        public RecieptController(ILogger<RecieptController> logger, RecieptService recieptService){
            _logger = logger;
            _recieptService = recieptService;
        }

        [HttpPut("new")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Reciept>> CreateReciept(Reciept reciept){
            if(!ModelState.IsValid) return BadRequest(ModelState);
            
            var createdReciept = await _recieptService.CreateReciept(reciept);
            return Created("User created successfully", createdReciept);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Reciept>> GetReciept(int id){
            var reciept = await _recieptService.GetRecieptById(id);

            if(reciept == null) return NotFound($"Reciept not found for id: {id}");

            return Ok(reciept);
        }

        [HttpGet("{id}/edit")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Reciept>> EditReciept(int id, Reciept recieptToUpdate){
            var reciept = await _recieptService.GetRecieptById(id);
            if(reciept == null) return NotFound($"Reciept not found for id: {id}");

            if(!ModelState.IsValid) return BadRequest(ModelState);

            var updatedReciept = await _recieptService.UpdateReciept(recieptToUpdate);
            return Ok(updatedReciept);
        }

    }
}
