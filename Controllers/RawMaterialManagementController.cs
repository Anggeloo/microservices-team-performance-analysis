using microservices_raw_material_management.Models;
using microservices_raw_material_management.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class RawMaterialManagementController : Controller
{
    private readonly RawMaterialManagementService _service;

    public RawMaterialManagementController(RawMaterialManagementService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
        var result = await _service.GetAllAsync();
        return Ok(new ApiResponse<List<RawMaterialManagement>>("success", result, "List of material management"));
    }

    [HttpGet("{codice}")]
    public async Task<IActionResult> GetOrderByCode(string codice)
    {

        var result = await _service.GetByCodeAsync(codice);

        if (result == null)
        {
            return Ok(new ApiResponse<RawMaterialManagement>("empty", result, "Material management not found"));
        }

        return Ok(new ApiResponse<RawMaterialManagement>("success", result, "Material management found"));
    }

    [HttpPost("add")]
    public async Task<IActionResult> Create([FromBody] RawMaterialManagement model)
    {
        if (model == null)
        {
            return BadRequest(new ApiResponse<string>("Error", null, "Invalid material management data"));
        }

        var existInventory = await _service.CheckIfInventoryExistsAsync(model.InventoryCode);

        if (existInventory == false)
        {
            return BadRequest(new ApiResponse<string>("Error", null, "The inventory code is incorrect"));
        }


        model.MaterialCode = await _service.GenerateNextOrderCodeAsync();

        var created = await _service.CreateAsync(model);

        if (created == null)
        {
            return StatusCode(500, new ApiResponse<string>("Error", null, "Material management was created but could not be retrieved"));
        }

        return CreatedAtAction(nameof(Create),
            new { codice = created.MaterialCode },
            new ApiResponse<RawMaterialManagement>("success", created, "Material management created successfully"));
    }

    [HttpPut("update/{codice}")]
    public async Task<IActionResult> CreateOrder(string codice, [FromBody] RawMaterialManagement model)
    {
        if (model == null)
        {
            return BadRequest(new ApiResponse<string>("Error", null, "Invalid material management data"));
        }

        var exits = await _service.CheckIfExistsAsync(codice);

        if (exits == false)
        {
            return BadRequest(new ApiResponse<string>("Error", null, "The material management code does not exist"));
        }

        var existInventory = await _service.CheckIfInventoryExistsAsync(model.InventoryCode);

        if (existInventory == false)
        {
            return BadRequest(new ApiResponse<string>("Error", null, "The inventory code is incorrect"));
        }

        var updated = await _service.UpdateAsync(codice, model);

        if (updated == null)
        {
            return StatusCode(400, new ApiResponse<string>("Error", null, "Material management was updated but could not be retrieved"));
        }

        return Ok(new ApiResponse<RawMaterialManagement>("success", updated, "Material management updated successfully"));
    }

    [HttpDelete("delete/{codice}")]
    public async Task<IActionResult> DeñeteOrder(string codice)
    {
        var exitsOrder = await _service.CheckIfExistsAsync(codice);

        if (exitsOrder == false)
        {
            return BadRequest(new ApiResponse<string>("Error", null, "The material management code does not exist"));
        }

        var delete = await _service.DeleteAsync(codice);

        if (delete == null)
        {
            return StatusCode(400, new ApiResponse<string>("Error", null, "Error ..."));
        }

        return Ok(new ApiResponse<RawMaterialManagement>("success", delete, "Material management deleted successfully"));
    }
}

