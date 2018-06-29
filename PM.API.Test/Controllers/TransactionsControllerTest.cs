using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PM.API.Controllers;
using PM.API.Models;
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
                        .Returns(Task.FromResult(new List<TransactionResponse>()));

            var result = await controller.GetAllAsync();

            var listResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetByIdAsyncTest_ReturnsOk()
        {
            var transaction = new TransactionResponse();

            _serviceMock.Setup(s => s.GetByIdAsync(1))
                        .Returns(Task.FromResult(transaction));

            var result = await controller.GetByIdAsync(1);

            var listResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetByIdAsyncTest_ReturnsNotFound()
        {
            _serviceMock.Setup(s => s.GetByIdAsync(1))
                        .Returns(Task.FromResult((TransactionResponse)null));

            var result = await controller.GetByIdAsync(1);

            var listResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetByDateAsyncTest_ReturnsOk()
        {
            _serviceMock.Setup(s => s.GetByDateAsync(DateTime.Today, DateTime.Today))
                        .Returns(Task.FromResult(new List<TransactionResponse>()));

            var result = await controller.GetByDateAsync(DateTime.Today, DateTime.Today);

            var listResult = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task PostAsyncTest_ReturnsCreated()
        {
            var transaction = new TransactionResponse();
            var postTransaction = new PostTransaction();

            _mapperMock.Setup(m => m.Map<PostTransaction, TransactionResponse>(postTransaction))
                       .Returns(transaction);
            _serviceMock.Setup(s => s.AddAsync(transaction))
                        .Returns(Task.FromResult(transaction));

            var result = await controller.PostAsync(postTransaction);

            var listResult = Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public async Task PostAsyncTest_ReturnsBadRequest()
        {
            var postTransaction = new PostTransaction();
            controller.ModelState.AddModelError("MockError", "Model is invalid");

            var result = await controller.PostAsync(postTransaction);

            var listResult = Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task PutAsyncTest_ReturnsNoContent()
        {
            var transaction = new TransactionResponse();
            var putTransaction = new PutTransaction();

            _mapperMock.Setup(m => m.Map<PutTransaction, TransactionResponse>(putTransaction))
                       .Returns(transaction);
            _serviceMock.Setup(s => s.UpdateAsync(transaction))
                        .Returns(Task.FromResult(transaction));

            var result = await controller.PutAsync(putTransaction);

            var listResult = Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutAsyncTest_ReturnsBadRequest()
        {
            var putTransaction = new PutTransaction();
            controller.ModelState.AddModelError("MockError", "Model is invalid");

            var result = await controller.PutAsync(putTransaction);

            var listResult = Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task PutAsyncTest_ReturnsNotFound()
        {
            var transaction = new TransactionResponse();
            var putTransaction = new PutTransaction();

            _mapperMock.Setup(m => m.Map<PutTransaction, TransactionResponse>(putTransaction))
                       .Returns(transaction);
            _serviceMock.Setup(s => s.UpdateAsync(transaction))
                        .Returns(Task.FromResult((TransactionResponse)null));

            var result = await controller.PutAsync(putTransaction);

            var listResult = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteAsyncTest_ReturnsOk()
        {
            _serviceMock.Setup(s => s.RemoveAsync(1))
                        .Returns(Task.FromResult(new TransactionResponse()));

            var result = await controller.DeleteAsync(1);

            var listResult = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteAsyncTest_ReturnsNotFound()
        {
            _serviceMock.Setup(s => s.RemoveAsync(1))
                        .Returns(Task.FromResult((TransactionResponse)null));

            var result = await controller.DeleteAsync(1);

            var listResult = Assert.IsType<NotFoundResult>(result);
        }
    }
}
