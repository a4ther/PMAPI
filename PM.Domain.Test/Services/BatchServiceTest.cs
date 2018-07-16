using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using PM.Data.Models;
using PM.Domain.Models;
using PM.Domain.Repositories;
using PM.Domain.Services;
using PM.Domain.Test.Attributes;
using Xunit;

namespace PM.Domain.Test.Services
{
    public class BatchServiceTest
    {
        
        private readonly Mock<IBatchRepository> _repositoryMock;
        private readonly Mock<ITransactionService> _transactionMock;
        private readonly Mock<ICategoryService> _categoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<BatchService>> _loggerMock;
        private readonly BatchService _service;

        public BatchServiceTest()
        {
            _repositoryMock = new Mock<IBatchRepository>();
            _transactionMock = new Mock<ITransactionService>();
            _categoryMock = new Mock<ICategoryService>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<BatchService>>();
            _service = new BatchService(_repositoryMock.Object, _transactionMock.Object, _categoryMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Theory]
        [JsonFileData("../../../TestData/Batches.json")]
        public async Task AddAsyncTest_Excluded(BatchDTO batchDto, Batch batchEntity)
        {
            _mapperMock.Setup(m => m.Map<BatchDTO, Batch>(It.IsAny<BatchDTO>()))
                       .Returns(batchEntity);
            _mapperMock.Setup(m => m.Map<Batch, BatchDTO>(It.IsAny<Batch>()))
                       .Returns(batchDto);

            _repositoryMock.Setup(r => r.InsertAsync(It.IsAny<Batch>()))
                           .Returns(Task.FromResult(batchEntity));

            _transactionMock.Setup(t => t.GetByDateWithCategoryAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                            .Returns(Task.FromResult(new List<TransactionDTO> { }));

            _categoryMock.Setup(c => c.GetByNameAsync(It.IsAny<string>()))
                         .Returns(Task.FromResult((CategoryDTO)null));

            _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                           .Returns(Task.FromResult(batchEntity));

            var result = await _service.AddAsync(batchDto);

            Assert.Equal(2, result.Excluded);
        }

        [Theory]
        [JsonFileData("../../../TestData/Batches.json")]
        public async Task AddAsyncTest_Duplicated(BatchDTO batchDto, Batch batchEntity)
        {
            _mapperMock.Setup(m => m.Map<BatchDTO, Batch>(It.IsAny<BatchDTO>()))
                       .Returns(batchEntity);
            _mapperMock.Setup(m => m.Map<Batch, BatchDTO>(It.IsAny<Batch>()))
                       .Returns(batchDto);

            _repositoryMock.Setup(r => r.InsertAsync(It.IsAny<Batch>()))
                           .Returns(Task.FromResult(batchEntity));

            _transactionMock.Setup(t => t.GetByDateWithCategoryAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                            .Returns(Task.FromResult(batchDto.Transactions.Take(4).ToList()));

            _categoryMock.Setup(c => c.GetByNameAsync(It.IsAny<string>()))
                         .Returns(Task.FromResult((CategoryDTO)null));

            _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                           .Returns(Task.FromResult(batchEntity));

            var result = await _service.AddAsync(batchDto);

            Assert.Equal(4, result.Duplicated);
        }

        [Theory]
        [JsonFileData("../../../TestData/Batches.json")]
        public async Task AddAsyncTest_Added(BatchDTO batchDto, Batch batchEntity)
        {
            _mapperMock.Setup(m => m.Map<BatchDTO, Batch>(It.IsAny<BatchDTO>()))
                       .Returns(batchEntity);
            _mapperMock.Setup(m => m.Map<Batch, BatchDTO>(It.IsAny<Batch>()))
                       .Returns(batchDto);

            _repositoryMock.Setup(r => r.InsertAsync(It.IsAny<Batch>()))
                           .Returns(Task.FromResult(batchEntity));

            _transactionMock.Setup(t => t.GetByDateWithCategoryAsync(It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                            .Returns(Task.FromResult(batchDto.Transactions.Take(2).ToList()));

            _categoryMock.Setup(c => c.GetByNameAsync(It.IsAny<string>()))
                         .Returns(Task.FromResult(new CategoryDTO { }));

            _repositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                           .Returns(Task.FromResult(batchEntity));
            
            _transactionMock.Setup(t => t.AddAsync(It.IsAny<TransactionDTO>()))
                            .Returns(Task.FromResult(new TransactionDTO { }));

            var result = await _service.AddAsync(batchDto);

            Assert.Equal(24, result.Added);
        }
    }
}
