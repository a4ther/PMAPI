using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PM.API.Controllers;
using PM.API.Models;
using PM.API.Models.Request;
using PM.Domain.Models;
using PM.Domain.Services;
using Xunit;

namespace PM.API.Test.Controllers
{
    public class CategoriesControllerTest
    {
        private Mock<ICategoryService> _serviceMock;
        private Mock<IMapper> _mapperMock;
        private Mock<ILogger<CategoriesController>> _loggerMock;
        private CategoriesController controller;

        public CategoriesControllerTest()
        {
            _serviceMock = new Mock<ICategoryService>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<CategoriesController>>();

            controller = new CategoriesController(_serviceMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task GetAllAsyncTest_ReturnsOk()
        {
            _serviceMock.Setup(s => s.GetAsync())
                        .Returns(Task.FromResult(new List<CategoryResponse>()));

            var result = await controller.GetAllAsync();

            var assert = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetByIdAsyncTest_ReturnsOk()
        {
            var category = new CategoryResponse();

            _serviceMock.Setup(s => s.GetByIdAsync(1))
                        .Returns(Task.FromResult(category));

            var result = await controller.GetByIdAsync(1);

            var assert = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetByIdAsyncTest_ReturnsNotFound()
        {
            _serviceMock.Setup(s => s.GetByIdAsync(1))
                        .Returns(Task.FromResult((CategoryResponse)null));

            var result = await controller.GetByIdAsync(1);

            var assert = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task GetSubcategoriesAsyncTest_ReturnsOk()
        {
            _serviceMock.Setup(s => s.GetSubcategoriesAsync(1))
                        .Returns(Task.FromResult(new List<CategoryResponse>()));

            var result = await controller.GetSubcategoriesAsync(1);

            var assert = Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task PostAsyncTest_ReturnsCreated()
        {
            var category = new CategoryResponse();
            var postCategory = new PostCategory();

            _mapperMock.Setup(m => m.Map<PostCategory, CategoryResponse>(postCategory))
                       .Returns(category);
            _serviceMock.Setup(s => s.AddAsync(category))
                        .Returns(Task.FromResult(category));

            var result = await controller.PostAsync(postCategory);

            var assert = Assert.IsType<CreatedResult>(result);
        }

        [Fact]
        public async Task PostAsyncTest_ReturnsBadRequest()
        {
            var postCategory = new PostCategory();
            controller.ModelState.AddModelError("MockError", "Model is invalid");

            var result = await controller.PostAsync(postCategory);

            var assert = Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task PutAsyncTest_ReturnsNoContent()
        {
            var category = new CategoryResponse();
            var putCategory = new PutCategory();

            _mapperMock.Setup(m => m.Map<PutCategory, CategoryResponse>(putCategory))
                       .Returns(category);
            _serviceMock.Setup(s => s.UpdateAsync(category))
                        .Returns(Task.FromResult(category));

            var result = await controller.PutAsync(putCategory);

            var assert = Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutAsyncTest_ReturnsBadRequest()
        {
            var putCategory = new PutCategory();
            controller.ModelState.AddModelError("MockError", "Model is invalid");

            var result = await controller.PutAsync(putCategory);

            var assert = Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task PutAsyncTest_ReturnsNotFound()
        {
            var category = new CategoryResponse();
            var putCategory = new PutCategory();

            _mapperMock.Setup(m => m.Map<PutCategory, CategoryResponse>(putCategory))
                       .Returns(category);
            _serviceMock.Setup(s => s.UpdateAsync(category))
                        .Returns(Task.FromResult((CategoryResponse)null));

            var result = await controller.PutAsync(putCategory);

            var assert = Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task DeleteAsyncTest_ReturnsOk()
        {
            _serviceMock.Setup(s => s.RemoveAsync(1))
                        .Returns(Task.FromResult(new CategoryResponse()));

            var result = await controller.DeleteAsync(1);

            var assert = Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task DeleteAsyncTest_ReturnsNotFound()
        {
            _serviceMock.Setup(s => s.RemoveAsync(1))
                        .Returns(Task.FromResult((CategoryResponse)null));

            var result = await controller.DeleteAsync(1);

            var assert = Assert.IsType<NotFoundResult>(result);
        }
    }
}
