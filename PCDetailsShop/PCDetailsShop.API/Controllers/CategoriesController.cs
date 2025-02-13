using System;
using System.Threading.Tasks;
using Domain.Dto.CategoryDtos;
using Domain.Dto.CharacteristicPatternDto;
using Domain.Interfaces.Mapping;
using Domain.Interfaces.Services;
using Domain.Models;
using Domain.Result;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace PCDetailsShop.API.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoriesController : ControllerBase
{
	private readonly ICategoryService _categoryService;
	private readonly IValidator<CreateCategoryDto> _createCategoryValidator;

	private readonly ICategoryDtoMapper _categoryDtoMapper;
	private readonly IDtoMapper<CharacteristicPattern, CharacteristicPatternDto> _characteristicPatternDtoMapper;

	public CategoriesController(ICategoryService categoryService, IValidator<CreateCategoryDto> createCategoryDto,
	ICategoryDtoMapper categoryDtoMapper, IDtoMapper<CharacteristicPattern, CharacteristicPatternDto> characteristicPatternDtoMapper)
	{
		_categoryService = categoryService;
		_createCategoryValidator = createCategoryDto;
		_categoryDtoMapper = categoryDtoMapper;
		_characteristicPatternDtoMapper = characteristicPatternDtoMapper;
	}

	[HttpGet]
	public async Task<ActionResult<CollectionResult<CategoryDto>>> GetAllAsync()
	{
		CollectionResult<Category> response = await _categoryService.GetAllAsync();

		if(response.IsSuccess)
		{
			CollectionResult<CategoryDto> mappedCategories = await _categoryDtoMapper.FromModelsToDtosAsync(response.Data.ToList());
			return Ok(mappedCategories);
		}

			return BadRequest(response);
	}

	[HttpGet("{id:guid}")]
	public async Task<ActionResult<CategoryDto>> GetByIdAsync(Guid id)
	{
		BaseResult<Category> response = await _categoryService.GetByIdAsync(id);

		if(response.IsSuccess)
		{
			BaseResult<CategoryDto> mappedCategory = await _categoryDtoMapper.FromModelToDtoResultAsync(response.Data);
			return Ok(mappedCategory);
		}

		return BadRequest(response);
	}

	[HttpGet("[controller]/{name}")]
	public async Task<ActionResult<CollectionResult<CategoryDto>>> GetByNameAsync(string name)
	{
		CollectionResult<Category> response = await _categoryService.GetByNameAsync(name);

		if(response.IsSuccess)
		{
			CollectionResult<CategoryDto> mappedCategories = await _categoryDtoMapper.FromModelsToDtosAsync(response.Data.ToList());
			return Ok(mappedCategories);
		}

		return BadRequest(response);
	}

	[HttpPost]
	public async Task<ActionResult<BaseResult<CategoryDto>>> CreateAsync(CreateCategoryDto createCategoryDto)
	{
		ValidationResult createValidationResult = await _createCategoryValidator.ValidateAsync(createCategoryDto);
		
		if(!createValidationResult.IsValid)
		    return BadRequest(createValidationResult);
		
		BaseResult<Category> response = await _categoryService.CreateAsync(createCategoryDto);

		if(response.IsSuccess)
		{
			BaseResult<CategoryDto> mappedCategory = await _categoryDtoMapper.FromModelToDtoResultAsync(response.Data);
			return Ok(mappedCategory);
		}
		
		return BadRequest(response);
	}

	[HttpDelete("{id:guid}")]
	public async Task<ActionResult<BaseResult<Guid>>> DeleteAsync(Guid id)
	{
		BaseResult<Guid> response = await _categoryService.DeleteByIdAsync(id);

		if(response.IsSuccess)
			return Ok(response.Data);

		return BadRequest(response);
	}

	[HttpPut("{id:guid}/changeName")]
	public async Task<ActionResult<BaseResult<string>>> ChangeNameAsync(Guid id, string newName)
	{
		BaseResult<string> response = await _categoryService.ChangeNameByIdAsync(id, newName);

		if(response.IsSuccess)
			return Ok(response.Data);

		return BadRequest(response);
	}

	[HttpGet("characteristicPatterns/{id:guid}")]
	public async Task<ActionResult<CollectionResult<CharacteristicPatternDto>>> GetCategoryCharacteristicsByIdAsync(Guid id)
	{
		BaseResult<Category> response = await _categoryService.GetByIdAsync(id);

		if(response.IsSuccess)
		{
			CollectionResult<CharacteristicPatternDto> mappedCharacteristics = await _characteristicPatternDtoMapper
			.FromModelsToDtosAsync(response.Data.CharacteristicPatterns.ToList());

			return Ok(mappedCharacteristics);
		}

		return BadRequest(response);
	}

	[HttpPut("{id:guid}/change-characteristicPatternName")]
	public async Task<ActionResult<BaseResult<CharacteristicPatternDto>>> ChangeCharacteristicPatternName(Guid id, string oldName, string newName)
	{
		BaseResult<string> response = await _categoryService.ChangeCharacteristicNameByNameAsync(id, oldName, newName);

		if(response.IsSuccess)
			return Ok(response.Data);

		return BadRequest(response);
	}
}