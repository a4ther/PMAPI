using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PM.Domain.Models;
using PM.Domain.Services;
using PM.API.Models;

namespace PM.API.Controllers
{
    [Route("api/[controller]")]
    public class TransactionsController : ControllerBase
    {
        private readonly ITransactionService _service;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public TransactionsController(ITransactionService service, IMapper mapper, ILogger<TransactionsController> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<TransactionResponse>))]
        public async Task<IActionResult> GetAllAsync()
        {
            var prefix = "[GetAllAsync]";
            _logger.LogInformation($"{prefix} Executing action");

            var transactions = await _service.GetAsync();

            _logger.LogInformation($"{prefix} Returning {transactions.Count} transactions");
            return Ok(transactions);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(TransactionResponse))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByIdAsync([Required]int id)
        {
            var prefix = "[GetByIdAsync]";
            _logger.LogInformation($"{prefix} Executing action");

            var transaction = await _service.GetByIdAsync(id);
            if (transaction == null)
            {
                _logger.LogWarning($"{prefix} Transaction with id {id} not found");
                return NotFound();
            }

            _logger.LogInformation($"{prefix} Returning transaction with id {id}");
            return Ok(transaction);
        }

        [HttpGet("{fromDate}/{toDate}")]
        [ProducesResponseType(200, Type = typeof(List<TransactionResponse>))]
        public async Task<IActionResult> GetByDateAsync([Required]DateTime fromDate, [Required]DateTime toDate)
        {
            var prefix = "[GetByDateAsync]";
            _logger.LogInformation($"{prefix} Executing action");

            var transactions = await _service.GetByDateAsync(fromDate, toDate);

            _logger.LogInformation($"{prefix} Returning {transactions.Count} transactions");
            return Ok(transactions);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(TransactionResponse))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PostAsync([FromBody]PostTransaction request)
        {
            var prefix = "[PostAsync]";
            _logger.LogInformation($"{prefix} Executing action");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"{prefix} Invalid model");
                return BadRequest(ModelState);
            }

            var entity = _mapper.Map<PostTransaction, TransactionResponse>(request);
            var response = await _service.AddAsync(entity);

            _logger.LogInformation($"{prefix} Added transaction with id {response.ID}");
            return Created($"api/transactions/{response.ID}", response);
        }

        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PutAsync([FromBody]PutTransaction request)
        {
            var prefix = "[PutAsync]";
            _logger.LogInformation($"{prefix} Executing action");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"{prefix} Invalid model");
                return BadRequest(ModelState);
            }

            var entity = _mapper.Map<PutTransaction, TransactionResponse>(request);
            var transaction = await _service.UpdateAsync(entity);

            if(transaction == null)
            {
                _logger.LogWarning($"{prefix} Transaction with id {request.ID} not found");
                return NotFound();
            }

            _logger.LogInformation($"{prefix} Updated transaction with id {request.ID}");
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteAsync([Required]int id)
        {
            var prefix = "[DeleteAsync]";
            _logger.LogInformation($"{prefix} Executing action");

            var transaction = await _service.RemoveAsync(id);

            if (transaction == null)
            {
                _logger.LogWarning($"{prefix} Transaction with id {id} not found");
                return NotFound();
            }

            _logger.LogInformation($"{prefix} Deleted transaction with id {id}");
            return Ok();
        }
    }
}
