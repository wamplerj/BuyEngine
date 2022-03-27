using BuyEngine.Common;

namespace BuyEngine.Catalog.Brands;

public class BrandService : IBrandService
{
    private readonly IBrandRepository _brandRepository;
    private readonly IModelValidator<Brand> _validator;

    public BrandService(IBrandRepository brandRepository, IModelValidator<Brand> validator)
    {
        _brandRepository = brandRepository;
        _validator = validator;
    }

    public async Task<Brand> GetAsync(Guid brandId) => await _brandRepository.GetAsync(brandId);

    public async Task<IPagedList<Brand>> GetAllAsync(int pageSize = CatalogConfiguration.DefaultRecordsPerPage, int page = 0) =>
        await _brandRepository.GetAllAsync(pageSize, page);

    public async Task<Guid> AddAsync(Brand brand)
    {
        Guard.Null(brand, nameof(brand));

        var result = await _validator.ValidateAsync(brand);
        if (!result.IsValid)
            throw new ValidationException(result, nameof(brand));

        var id = await _brandRepository.AddAsync(brand);
        return id;
    }

    public async Task<bool> UpdateAsync(Brand brand)
    {
        Guard.Null(brand, nameof(brand));

        var result = await _validator.ValidateAsync(brand);
        if (!result.IsValid)
            throw new ValidationException(result, nameof(brand));

        var success = await _brandRepository.UpdateAsync(brand);
        return success;
    }

    public async Task RemoveAsync(Brand brand)
    {
        Guard.Null(brand, nameof(brand));
        Guard.Default(brand.Id, nameof(brand.Id));

        await _brandRepository.RemoveAsync(brand);
    }

    public async Task RemoveAsync(Guid brandId)
    {
        Guard.Default(brandId, nameof(brandId));
        var brand = await GetAsync(brandId);

        await RemoveAsync(brand);
    }

    public async Task<bool> IsValidAsync(Brand brand)
    {
        var result = await ValidateAsync(brand);
        return result.IsValid;
    }

    public async Task<ValidationResult> ValidateAsync(Brand brand) => await _validator.ValidateAsync(brand);
}

public interface IBrandService
{
    Task<Brand> GetAsync(Guid brandId);
    Task<IPagedList<Brand>> GetAllAsync(int pageSize = 25, int page = 0);
    Task<Guid> AddAsync(Brand brand);
    Task<bool> UpdateAsync(Brand brand);
    Task RemoveAsync(Brand brand);
    Task RemoveAsync(Guid brandId);

    Task<bool> IsValidAsync(Brand brand);
    Task<ValidationResult> ValidateAsync(Brand brand);
}