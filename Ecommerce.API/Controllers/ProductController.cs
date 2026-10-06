using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SimpleCRUDAPI.DTO_s;
using SimpleCRUDAPI.Ecommerce.Application.Interfaces;
using System.Security.Claims;

namespace SimpleCRUDAPI.Ecommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProductController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IWebHostEnvironment _environment;

    public ProductController(
        IProductService productService,
        IWebHostEnvironment environment)
    {
        _productService = productService;
        _environment = environment;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products =
            await _productService.GetAllProductsAsync();

        return Ok(products);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(
        int id)
    {
        var product =
            await _productService.GetProductByIdAsync(
                id);

        if (product == null)
        {
            return NotFound(new
            {
                message = "Product not found."
            });
        }

        return Ok(product);
    }

    [Authorize(
        Roles = "Seller,Admin,SuperAdmin")]
    [HttpGet("mine")]
    public async Task<IActionResult> GetMyProducts()
    {
        var userId =
            GetCurrentUserId();

        var products =
            await _productService.GetMyProductsAsync(
                userId);

        return Ok(products);
    }

    [Authorize(
        Roles = "Seller,Admin,SuperAdmin")]
    [HttpPost]
    public async Task<IActionResult> Add(
        ProductRequestDto request)
    {
        var userId =
            GetCurrentUserId();

        var product =
            await _productService.AddProductAsync(
                request,
                userId);

        if (product == null)
        {
            return BadRequest(new
            {
                message =
                    "Unable to create product."
            });
        }

        return CreatedAtAction(
            nameof(GetById),
            new
            {
                id = product.Id
            },
            product);
    }

    [Authorize(
        Roles = "Seller,Admin,SuperAdmin")]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        ProductRequestDto request)
    {
        var currentUserId =
            GetCurrentUserId();

        var isSuperAdmin =
            User.IsInRole("SuperAdmin");

        var canModify =
            await _productService
                .CanModifyProductAsync(
                    id,
                    currentUserId,
                    isSuperAdmin);

        if (!canModify)
        {
            return StatusCode(
                StatusCodes.Status403Forbidden,
                new
                {
                    message =
                        "You can update only products created by you."
                });
        }

        var updated =
            await _productService.UpdateProductAsync(
                id,
                request,
                currentUserId,
                isSuperAdmin);

        if (!updated)
        {
            return BadRequest(new
            {
                message =
                    "Unable to update product."
            });
        }

        return Ok(new
        {
            message =
                "Product updated successfully."
        });
    }

    [Authorize(Roles = "SuperAdmin")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(
        int id)
    {
        var deleted =
            await _productService.DeleteProductAsync(
                id);

        if (!deleted)
        {
            return NotFound(new
            {
                message =
                    "Product not found."
            });
        }

        return Ok(new
        {
            message =
                "Product deleted successfully."
        });
    }

    [Authorize(
        Roles = "Seller,Admin,SuperAdmin")]
    [HttpPost("{id:int}/image")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadImage(
        int id,
        IFormFile image)
    {
        var currentUserId =
            GetCurrentUserId();

        var isSuperAdmin =
            User.IsInRole("SuperAdmin");

        var canModify =
            await _productService
                .CanModifyProductAsync(
                    id,
                    currentUserId,
                    isSuperAdmin);

        if (!canModify)
        {
            return StatusCode(
                StatusCodes.Status403Forbidden,
                new
                {
                    message =
                        "You can update only images of products created by you."
                });
        }

        if (image == null ||
            image.Length == 0)
        {
            return BadRequest(new
            {
                message =
                    "Please select an image."
            });
        }

        var allowedExtensions =
            new[]
            {
                ".jpg",
                ".jpeg",
                ".png",
                ".webp"
            };

        var extension =
            Path.GetExtension(
                image.FileName)
            .ToLowerInvariant();

        if (!allowedExtensions.Contains(
                extension))
        {
            return BadRequest(new
            {
                message =
                    "Only JPG, JPEG, PNG and WEBP images are allowed."
            });
        }

        const long maxFileSize =
            5 * 1024 * 1024;

        if (image.Length > maxFileSize)
        {
            return BadRequest(new
            {
                message =
                    "Image size cannot exceed 5 MB."
            });
        }

        var webRootPath =
            _environment.WebRootPath;

        if (string.IsNullOrWhiteSpace(
                webRootPath))
        {
            webRootPath =
                Path.Combine(
                    _environment.ContentRootPath,
                    "wwwroot");
        }

        var folderPath =
            Path.Combine(
                webRootPath,
                "uploads",
                "products");

        Directory.CreateDirectory(
            folderPath);

        var fileName =
            $"{Guid.NewGuid()}{extension}";

        var filePath =
            Path.Combine(
                folderPath,
                fileName);

        await using (
            var stream =
                new FileStream(
                    filePath,
                    FileMode.Create))
        {
            await image.CopyToAsync(
                stream);
        }

        var imageUrl =
            $"/uploads/products/{fileName}";

        var updated =
            await _productService
                .UpdateProductImageAsync(
                    id,
                    imageUrl,
                    currentUserId,
                    isSuperAdmin);

        if (!updated)
        {
            if (System.IO.File.Exists(
                    filePath))
            {
                System.IO.File.Delete(
                    filePath);
            }

            return BadRequest(new
            {
                message =
                    "Unable to update product image."
            });
        }

        return Ok(new
        {
            imageUrl
        });
    }

    private int GetCurrentUserId()
    {
        var userIdValue =
            User.FindFirst(
                ClaimTypes.NameIdentifier)
            ?.Value;

        if (!int.TryParse(
                userIdValue,
                out var userId))
        {
            throw new UnauthorizedAccessException(
                "User identifier is missing from token.");
        }

        return userId;
    }
}