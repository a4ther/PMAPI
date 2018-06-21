using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PMAPI.Domain.Infrastructure;
using PMAPI.Domain.Models;
using PMAPI.Domain.Services;
using PMAPI.Models;

namespace PMAPI.Controllers
{
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _service;
        private readonly IErrorHandler _errorHandler;
        private readonly IMapper _mapper;

        public TransactionsController(ITransactionService service, IErrorHandler errorHandler, IMapper mapper)
        {
            _service = service;
            _errorHandler = errorHandler;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<TransactionResponse>))]
        public async Task<IActionResult> GetAllAsync()
        {
            var transactions = await _service.GetAsync();
            return Ok(transactions);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(TransactionResponse))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByIdAsync([Required]int id)
        {
            var transaction = await _service.GetByIdAsync(id);
            if (transaction == null)
            {
                return NotFound();
            }
            return Ok(transaction);
        }

        [HttpGet("{fromDate}/{toDate}")]
        [ProducesResponseType(200, Type = typeof(List<TransactionResponse>))]
        public async Task<IActionResult> GetByDateAsync([Required]DateTime fromDate,[Required]DateTime toDate)
        {
            var transactions = await _service.WhereAsync(e => e.Date >= fromDate && e.Date <= toDate);
            return Ok(transactions);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(TransactionResponse))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PostAsync([FromBody]PostTransaction request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = _mapper.Map<PostTransaction, TransactionResponse>(request);
            var response = await _service.AddAsync(entity);
            return Created($"api/transactions/{response.ID}", response);
        }

        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PutAsync([FromBody]PutTransaction request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = _mapper.Map<PutTransaction, TransactionResponse>(request);
            var transaction = await _service.UpdateAsync(entity);

            if(transaction == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteAsync([Required]int id)
        {
            var transaction = await _service.RemoveAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }
            return Ok();

        }
    }
}
