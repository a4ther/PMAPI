using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using PM.Domain.Services;
using PM.Domain.Models;
using System.ComponentModel.DataAnnotations;
using PM.API.Models.Request;

namespace PM.API.Controllers
{
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _service;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public CategoriesController(ICategoryService service, IMapper mapper, ILogger<CategoriesController> logger)
        {
            _service = service;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<CategoryDTO>))]
        public async Task<IActionResult> GetAllAsync()
        {
            var prefix = "[GetAllAsync]";
            _logger.LogInformation($"{prefix} Executing action");

            var categories = await _service.GetAsync();

            _logger.LogInformation($"{prefix} Returning {categories.Count} categories");
            return Ok(categories);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(200, Type = typeof(CategoryDTO))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetByIdAsync([Required]int id)
        {
            var prefix = "[GetByIdAsync]";
            _logger.LogInformation($"{prefix} Executing action");

            var category = await _service.GetByIdAsync(id);
            if (category == null)
            {
                _logger.LogWarning($"{prefix} Category with id {id} not found");
                return NotFound();
            }

            _logger.LogInformation($"{prefix} Returning category with id {id}");
            return Ok(category);
        }

        [HttpGet("{id}/subcategories")]
        [ProducesResponseType(200, Type = typeof(List<CategoryDTO>))]
        public async Task<IActionResult> GetSubcategoriesAsync([Required]int id)
        {
            var prefix = "[GetSubcategoriesAsync]";
            _logger.LogInformation($"{prefix} Executing action");

            var categories = await _service.GetSubcategoriesAsync(id);

            _logger.LogInformation($"{prefix} Returning {categories.Count} subcategories for parent with id {id}");
            return Ok(categories);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(CategoryDTO))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PostAsync([FromBody]PostCategory request)
        {
            var prefix = "[PostAsync]";
            _logger.LogInformation($"{prefix} Executing action");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"{prefix} Invalid model");
                return BadRequest(ModelState);
            }

            var entity = _mapper.Map<PostCategory, CategoryDTO>(request);
            var response = await _service.AddAsync(entity);

            _logger.LogInformation($"{prefix} Added category with id {response.ID}");
            return Created($"api/categories/{response.ID}", response);
        }

        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> PutAsync([FromBody]PutCategory request)
        {
            var prefix = "[PutAsync]";
            _logger.LogInformation($"{prefix} Executing action");

            if (!ModelState.IsValid)
            {
                _logger.LogWarning($"{prefix} Invalid model");
                return BadRequest(ModelState);
            }

            var entity = _mapper.Map<PutCategory, CategoryDTO>(request);
            var response = await _service.UpdateAsync(entity);

            if (response == null)
            {
                _logger.LogWarning($"{prefix} Category with id {request.ID} not found");
                return NotFound();
            }

            _logger.LogInformation($"{prefix} Updated category with id {request.ID}");
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteAsync([Required]int id)
        {
            var prefix = "[DeleteAsync]";
            _logger.LogInformation($"{prefix} Executing action");

            var category = await _service.RemoveAsync(id);

            if (category == null)
            {
                _logger.LogWarning($"{prefix} Category with id {id} not found");
                return NotFound();
            }

            _logger.LogInformation($"{prefix} Deleted category with id {id}");
            return Ok();
        }
    }
}
