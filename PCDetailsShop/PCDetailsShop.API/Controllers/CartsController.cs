using System;
using Domain.Dto.ProductDtos;
using Domain.Interfaces.Mapping;
using Domain.Interfaces.Services;
using Domain.Models;
using Domain.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PCDetailsShop.API.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class CartsController : ControllerBase
{
    private readonly ICartService _cartService;
    private readonly IDtoMapper<Product, ProductDto> _productDtoMapper;

    public CartsController(ICartService cartService, IDtoMapper<Product, ProductDto> productDtoMapper)
    {
        _cartService = cartService;
        _productDtoMapper = productDtoMapper;
    }

    [HttpGet("cart-products/{id:guid}")]
    public async Task<ActionResult<CollectionResult<ProductDto>>> GetAllProductsAsync(Guid id)
    {
        CollectionResult<Product> response = await _cartService.GetAllProductsInCartAsync(id);

        if(response.IsSuccess)
        {
            CollectionResult<ProductDto> mappedProducts = await _productDtoMapper.FromModelsToDtosAsync(response.Data); 
            return Ok(mappedProducts);
        }

        return BadRequest(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BaseResult<ProductDto>>> GetProductFromCartByNameAsync(Guid id, string productName)
    {
        BaseResult<Product> response = await _cartService.GetProductFromCartByNameAsync(id, productName);

        if(response.IsSuccess)
        {
            BaseResult<ProductDto> mappedProduct = _productDtoMapper.FromModelToDtoResult(response.Data);
            return Ok(mappedProduct);
        }

        return BadRequest(response);
    }

    [HttpPut("remove-product-from-cart/{id:guid}")]
    public async Task<ActionResult<BaseResult<ProductDto>>> RemoveProductFromCartByIdAsync(Guid id, Guid productId)
    {
        BaseResult<Guid> response = await _cartService.RemoveProductFromCartByIdAsync(id, productId);

        if(response.IsSuccess)
            return Ok(response);

        return BadRequest(response);
    }

    [HttpPut("{id:guid}/buy-products")]
    public async Task<ActionResult<BaseResult<int>>> BuyProductsAsync(Guid id)
    {
        BaseResult<int> response = await _cartService.BuyProductsAsync(id);

        if(response.IsSuccess)
        {
            return Ok(response);
        }

        return BadRequest(response);
    }

    [HttpGet("{id:guid}/get-total-price")]
    public async Task<ActionResult<BaseResult<decimal>>> GetProductsTotalPriceByIdAsync(Guid id)
    {
        BaseResult<decimal> response = await _cartService.GetProductsTotalPriceByIdAsync(id);

        if(response.IsSuccess)
            return Ok(response);

        return BadRequest(response);
    }

    [HttpGet("{id:guid}/get-total-weight")]
    public async Task<ActionResult<BaseResult<float>>> GetProductsTotalWeightAsync(Guid id)
    {
        BaseResult<float> response = await _cartService.GetProductsTotalWeightAsync(id);

        if(response.IsSuccess)
            return Ok(response);

        return BadRequest(response);
    }
}