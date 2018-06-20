using System;
using Microsoft.AspNetCore.Mvc;
using PMAPI.Domain.Models;
using AutoMapper;
using PMAPI.Domain.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using PMAPI.Domain.Infrastructure;
using System.Linq;
using PMAPI.Models.Transactions;
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
        public async Task<IEnumerable<TransactionResponse>> GetAllAsync()
        {
            return await _service.GetAsync();
        }

        [HttpGet("{id}")]
        public async Task<TransactionResponse> GetByIdAsync([Required]int id)
        {
            return await _service.GetByIdAsync(id);
        }

        [HttpGet("{fromDate}/{toDate}")]
        public IEnumerable<TransactionResponse> GetByDate([Required]DateTime fromDate,[Required]DateTime toDate)
        {
            return _service.Where(e => e.Date >= fromDate && e.Date <= toDate);
        }

        [HttpPost]
        //[ProducesResponseType(201, Type = typeof(TransactionResponse))]
        //[ProducesResponseType(400)]
        public void Post([FromBody]TransactionRequest request)
        {
            if (!ModelState.IsValid)
            {
                //return BadRequest(ModelState);
            }

            var entity = _mapper.Map<TransactionRequest, TransactionResponse>(request);
            _service.AddOrUpdate(entity);
            //return Ok();
        }

        [HttpDelete("{id}")]
        public void Delete([Required]int id)
        {
            _service.Remove(id);

        }
    }
}
