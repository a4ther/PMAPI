using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PM.API.Controllers;
using PM.API.Models.Request;
using PM.Domain.Models;
using PM.Domain.Services;
using Xunit;

namespace PM.API.Test.Controllers
{
    public class TransactionsControllerTest
    {
        private Mock<ITransactionService> _serviceMock;
        private Mock<IMapper> _mapperMock;
        private Mock<ILogger<TransactionsController>> _loggerMock;
        private TransactionsController controller;

        public TransactionsControllerTest()
        {
            _serviceMock = new Mock<ITransactionService>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<TransactionsController>>();

            controller = new TransactionsController(_serviceMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetAllAsyncTest_ReturnsOk()
        {
            _serviceMock.Setup(s => s.GetAsync())
                        .Returns(Task.FromResult(new List<TransactionDTO>()));

            var result = await controller.GetAllAsync();

            var assert = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetByIdAsyncTest_ReturnsOk()
        {
            var transaction = new TransactionDTO();

            _serviceMock.Setup(s => s.GetByIdAsync(1))
                        .Returns(Task.FromResult(transaction));

            var result = await controller.GetByIdAsync(1);

            var assert = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetByIdAsyncTest_ReturnsNotFound()
        {
            _serviceMock.Setup(s => s.GetByIdAsync(1))
                        .Returns(Task.FromResult((TransactionDTO)null));

            var result = await controller.GetByIdAsync(1);

            var assert = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetByDateAsyncTest_ReturnsOk()
        {
            _serviceMock.Setup(s => s.GetByDateAsync(DateTime.Today, DateTime.Today))
                        .Returns(Task.FromResult(new List<TransactionDTO>()));

            var result = await controller.GetByDateAsync(DateTime.Today, DateTime.Today);

            var assert = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task PostAsyncTest_ReturnsCreated()
        {
            var transaction = new TransactionDTO();
            var postTransaction = new PostTransaction();

            _mapperMock.Setup(m => m.Map<PostTransaction, TransactionDTO>(postTransaction))
                       .Returns(transaction);
            _serviceMock.Setup(s => s.AddAsync(transaction))
                        .Returns(Task.FromResult(transaction));

            var result = await controller.PostAsync(postTransaction);

            var assert = Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public async Task PostAsyncTest_ReturnsBadRequest()
        {
            var postTransaction = new PostTransaction();
            controller.ModelState.AddModelError("MockError", "Model is invalid");

            var result = await controller.PostAsync(postTransaction);

            var assert = Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task PutAsyncTest_ReturnsNoContent()
        {
            var transaction = new TransactionDTO();
            var putTransaction = new PutTransaction();

            _mapperMock.Setup(m => m.Map<PutTransaction, TransactionDTO>(putTransaction))
                       .Returns(transaction);
            _serviceMock.Setup(s => s.UpdateAsync(transaction))
                        .Returns(Task.FromResult(transaction));

            var result = await controller.PutAsync(putTransaction);

            var assert = Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutAsyncTest_ReturnsBadRequest()
        {
            var putTransaction = new PutTransaction();
            controller.ModelState.AddModelError("MockError", "Model is invalid");

            var result = await controller.PutAsync(putTransaction);

            var assert = Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task PutAsyncTest_ReturnsNotFound()
        {
            var transaction = new TransactionDTO();
            var putTransaction = new PutTransaction();

            _mapperMock.Setup(m => m.Map<PutTransaction, TransactionDTO>(putTransaction))
                       .Returns(transaction);
            _serviceMock.Setup(s => s.UpdateAsync(transaction))
                        .Returns(Task.FromResult((TransactionDTO)null));

            var result = await controller.PutAsync(putTransaction);

            var assert = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteAsyncTest_ReturnsOk()
        {
            _serviceMock.Setup(s => s.RemoveAsync(1))
                        .Returns(Task.FromResult(new TransactionDTO()));

            var result = await controller.DeleteAsync(1);

            var assert = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteAsyncTest_ReturnsNotFound()
        {
            _serviceMock.Setup(s => s.RemoveAsync(1))
                        .Returns(Task.FromResult((TransactionDTO)null));

            var result = await controller.DeleteAsync(1);

            var assert = Assert.IsType<NotFoundResult>(result);
        }
    }
}
