using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PM.API.Models.Request;
using PM.Domain.Models;
using PM.Domain.Services;

namespace PM.API.Controllers
{
    [Route("api/[controller]")]
    public class BatchesController : ControllerBase
    {
        private readonly IBatchService _service;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public BatchesController(IBatchService service, IMapper mapper, ILogger<BatchesController> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(BatchDTO))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByIdAsync([Required]int id)
        {
            var prefix = "[GetByIdAsync]";
            _logger.LogInformation($"{prefix} Executing action");

            var batch = await _service.GetByIdAsync(id);
            if (batch == null)
            {
                _logger.LogWarning($"{prefix} Batch with id {id} not found");
                return NotFound();
            }

            _logger.LogInformation($"{prefix} Returning batch with id {id}");
            return Ok(batch);
        }

        [HttpGet("{fromDate}/{toDate}")]
        [ProducesResponseType(200, Type = typeof(List<BatchDTO>))]
        public async Task<IActionResult> GetByDateAsync([Required]DateTime fromDate, [Required]DateTime toDate)
        {
            var prefix = "[GetByDateAsync]";
            _logger.LogInformation($"{prefix} Executing action");

            var batches = await _service.GetByDateAsync(fromDate, toDate);

            _logger.LogInformation($"{prefix} Returning {batches.Count} batches");
            return Ok(batches);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(BatchDTO))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PostAsync([FromBody]PostBatch request)
        {
            var prefix = "[PostAsync]";
            _logger.LogInformation($"{prefix} Executing action");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"{prefix} Invalid model");
                return BadRequest(ModelState);
            }

            var entity = _mapper.Map<PostBatch, BatchDTO>(request);
            var response = await _service.AddAsync(entity);

            _logger.LogInformation($"{prefix} Added batch with id {response.ID}");
            return Created($"api/batches/{response.ID}", response);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteAsync([Required]int id)
        {
            var prefix = "[DeleteAsync]";
            _logger.LogInformation($"{prefix} Executing action");

            var batch = await _service.RemoveAsync(id);

            if (batch == null)
            {
                _logger.LogWarning($"{prefix} Batch with id {id} not found");
                return NotFound();
            }

            _logger.LogInformation($"{prefix} Deleted batch with id {id}");
            return Ok();
        }
    }
}
