
using Application.Services;
using Domain.Dto.ProductDtos;
using Domain.Result;
using Microsoft.AspNetCore.Mvc;

namespace PCDetailsShop.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

    }
}