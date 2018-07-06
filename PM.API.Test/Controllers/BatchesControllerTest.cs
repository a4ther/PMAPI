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
    public class BatchesControllerTest
    {
        private Mock<IBatchService> _serviceMock;
        private Mock<IMapper> _mapperMock;
        private Mock<ILogger<BatchesController>> _loggerMock;
        private BatchesController controller;

        public BatchesControllerTest()
        {
            _serviceMock = new Mock<IBatchService>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<BatchesController>>();
            controller = new BatchesController(_serviceMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetByIdAsyncTest_ReturnsOk()
        {
            var batch = new BatchDTO();

            _serviceMock.Setup(s => s.GetByIdAsync(1))
                        .Returns(Task.FromResult(batch));

            var result = await controller.GetByIdAsync(1);

            var assert = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetByIdAsyncTest_ReturnsNotFound()
        {
            _serviceMock.Setup(s => s.GetByIdAsync(1))
                        .Returns(Task.FromResult((BatchDTO)null));

            var result = await controller.GetByIdAsync(1);

            var assert = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetByDateAsyncTest_ReturnsOk()
        {
            _serviceMock.Setup(s => s.GetByDateAsync(DateTime.Today, DateTime.Today))
                        .Returns(Task.FromResult(new List<BatchDTO>()));

            var result = await controller.GetByDateAsync(DateTime.Today, DateTime.Today);

            var assert = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task PostAsyncTest_ReturnsCreated()
        {
            var batch = new BatchDTO();
            var postBatch = new PostBatch();

            _mapperMock.Setup(m => m.Map<PostBatch, BatchDTO>(postBatch))
                       .Returns(batch);
            _serviceMock.Setup(s => s.AddAsync(batch))
                        .Returns(Task.FromResult(batch));

            var result = await controller.PostAsync(postBatch);

            var assert = Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public async Task PostAsyncTest_ReturnsBadRequest()
        {
            var postBatch = new PostBatch();
            controller.ModelState.AddModelError("MockError", "Model is invalid");

            var result = await controller.PostAsync(postBatch);

            var assert = Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task DeleteAsyncTest_ReturnsOk()
        {
            _serviceMock.Setup(s => s.RemoveAsync(1))
                        .Returns(Task.FromResult(new BatchDTO()));

            var result = await controller.DeleteAsync(1);

            var assert = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteAsyncTest_ReturnsNotFound()
        {
            _serviceMock.Setup(s => s.RemoveAsync(1))
                        .Returns(Task.FromResult((BatchDTO)null));

            var result = await controller.DeleteAsync(1);

            var assert = Assert.IsType<NotFoundResult>(result);
        }
    }
}
