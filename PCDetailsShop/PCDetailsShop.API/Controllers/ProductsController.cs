using System;
using System.Text;
using Domain.Dto;
using Domain.Dto.CategoryDto;
using Domain.Dto.ProductDtos;
using Domain.Interfaces.Mapping;
using Domain.Interfaces.Services;
using Domain.Models;
using Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.OpenApi.Models;

namespace PCDetailsShop.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        private readonly IDtoMapper<Product, ProductDto> _productDtoMapper;

        public ProductsController(IProductService productService, IDtoMapper<Product, ProductDto> productDtoMapper)
        {
            _productService = productService;
            _productDtoMapper = productDtoMapper;
        }

        [HttpGet]
        public async Task<ActionResult<CollectionResult<ProductDto>>> GetAllAsync()
        {
           CollectionResult<Product> response = await _productService.GetAllAsync();

           if(response.IsSuccess)
           {
                CollectionResult<ProductDto> mappedProducts = await _productDtoMapper.FromModelsToDtosAsync(response.Data);
                return Ok(mappedProducts);
           }

           return BadRequest(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<BaseResult<ProductDto>>> GetByIdAsync(Guid id)
        {
            BaseResult<Product> response = await _productService.GetByIdAsync(id);

            if(response.IsSuccess)
            {
                BaseResult<ProductDto> mappedProduct = _productDtoMapper.FromModelToDtoResult(response.Data);
                return Ok(mappedProduct);
            }

            return BadRequest(response);
        }

        [HttpGet("{namePart}")]
        public async Task<ActionResult<CollectionResult<ProductDto>>> GetByNamePartAsync(string namePart)
        {
            CollectionResult<Product> response = await _productService.GetByNamePartAsync(namePart);

            if(response.IsSuccess)
            {
                CollectionResult<ProductDto> mappedProducts = await _productDtoMapper.FromModelsToDtosAsync(response.Data);
                return Ok(mappedProducts);
            }

            return BadRequest(response);
        }

        [HttpGet("categories/")]
        public async Task<ActionResult<CollectionResult<Product>>> GetByCategoriesNamesAsync([FromQuery] string[] categories, int countPerPage, int page)
        {
            CollectionResult<Product> response = await _productService.GetByCategoriesNamesAsync(categories, countPerPage, page);

            if(response.IsSuccess)
            {
                CollectionResult<ProductDto> mappedProducts = await _productDtoMapper.FromModelsToDtosAsync(response.Data);
                return Ok(mappedProducts);
            }

            return BadRequest(response);
        }

        [HttpPost]
        public async Task<ActionResult<BaseResult<ProductDto>>> CreateAsync(CreateProductDto createDto)
        {
            BaseResult<Product> response = await _productService.CreateAsync(createDto);

            if(response.IsSuccess)
            {
                BaseResult<ProductDto> mappedProduct = _productDtoMapper.FromModelToDtoResult(response.Data);
                return Ok(mappedProduct);
            }

            return BadRequest(response);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<BaseResult<Guid>>> DeleteByIdAsync(Guid id)
        {
            BaseResult<Guid> response = await _productService.DeleteByIdAsync(id);

            if(response.IsSuccess)
               return Ok(response);

            return BadRequest(response);
        }

        [HttpPut("{productId:guid}/change-name")]
        public async Task<ActionResult<BaseResult<string>>> ChangeNameByIdAsync(Guid productId, string newName)
        {
            BaseResult<string> response = await _productService.ChangeNameByIdAsync(productId, newName);

            if(response.IsSuccess)
                return Ok(response);

            return BadRequest(response); 
        }

        [HttpPut("{productId:guid}/change-description")]
        public async Task<ActionResult<BaseResult<string>>> ChangeDescriptionByIdAsync(Guid productId, string newDescription)
        {
            BaseResult<string> response = await _productService.ChangeDescriptionByIdAsync(productId, newDescription);

            if(response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpPut("{productId:guid}/change-price")]
        public async Task<ActionResult<BaseResult<string>>> ChangePriceByIdAsync(Guid productId, decimal newPrice)
        {
            BaseResult<decimal> response = await _productService.ChangePriceByIdAsync(productId, newPrice);

            if(response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpPut("{productId:guid}/change-weight")]
        public async Task<ActionResult<BaseResult<string>>> ChangeWeightByIdAsync(Guid productId, float newWeight)
        {
            BaseResult<float> response = await _productService.ChangeWeightByIdAsync(productId, newWeight);

            if(response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpPut("{productId:guid}/add-category-to-product")]
        public async Task<ActionResult<BaseResult<string>>> AddCategoryToProductByNameAsync(Guid productId, string categoryName, List<CharacteristicRealizationCreateDto> categoryCharacteristicRealizations)
        {
            BaseResult<string> response = await _productService.AddCategoryToProductByNameAsync(productId, categoryName, categoryCharacteristicRealizations);

            if(response.IsSuccess)
                return Ok(response);
            
            return BadRequest(response);
        }

        [HttpPut("{categoryName}/delete-category-from-product")]
        public async Task<ActionResult<BaseResult<string>>> DeleteCategoryFromProductByNameAsync(Guid productId, string categoryName)
        {
            BaseResult<string> response = await _productService.DeleteCategoryFromProductByNameAsync(productId, categoryName);

            if(response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpPut("{productId:guid}/add-product-to-cart")]
        public async Task<ActionResult<BaseResult<string>>> AddProductToCartAsync(Guid productId, Guid cartId)
        {
            BaseResult<string> response = await _productService.AddProductToCartAsync(productId, cartId);

            if(response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }

        [HttpPut("{productId:guid}/change-characteristicValue")]
        public async Task<ActionResult<BaseResult<string>>> ChangeCharacteristicValueAsync(Guid productId, string characteristicName, string newCharacteristicValue)
        {
            BaseResult<string> response = await _productService.ChangeCharacteristicValueAsync(productId, characteristicName, newCharacteristicValue);

            if(response.IsSuccess)
                return Ok(response);

            return BadRequest(response);
        }
    }
}