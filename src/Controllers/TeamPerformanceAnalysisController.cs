using microservices_team_performance_analysis.Models;
using microservices_team_performance_analysis.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class TeamPerformanceAnalysisController : Controller
{
    private readonly TeamPerformanceAnalysisServices _service;

    public TeamPerformanceAnalysisController(TeamPerformanceAnalysisServices service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOrders()
    {
        var result = await _service.GetAllAsync();
        return Ok(new ApiResponse<List<TeamPerformanceAnalysis>>("success", result, "List of team performance"));
    }

    [HttpGet("{codice}")]
    public async Task<IActionResult> GetOrderByCode(string codice)
    {

        var result = await _service.GetByCodeAsync(codice);

        if (result == null)
        {
            return Ok(new ApiResponse<TeamPerformanceAnalysis>("empty", result, "Team performance not found"));
        }

        return Ok(new ApiResponse<TeamPerformanceAnalysis>("success", result, "Team performance found"));
    }

    [HttpPost("add")]
    public async Task<IActionResult> Create([FromBody] TeamPerformanceAnalysis model)
    {
        if (model == null)
        {
            return BadRequest(new ApiResponse<string>("Error", null, "Invalid team performance data"));
        }

        var existWorkTeam = await _service.CheckIfWorkTeamExistsAsync(model.TeamCode);

        if (existWorkTeam == false)
        {
            return BadRequest(new ApiResponse<string>("Error", null, "The work team code is incorrect"));
        }


        model.TeamPerformanceCode = await _service.GenerateNextOrderCodeAsync();

        var created = await _service.CreateAsync(model);

        if (created == null)
        {
            return StatusCode(500, new ApiResponse<string>("Error", null, "Team performance was created but could not be retrieved"));
        }

        return CreatedAtAction(nameof(Create),
            new { codice = created.TeamPerformanceCode },
            new ApiResponse<TeamPerformanceAnalysis>("success", created, "Team performance created successfully"));
    }

    [HttpPut("update/{codice}")]
    public async Task<IActionResult> CreateOrder(string codice, [FromBody] TeamPerformanceAnalysis model)
    {
        if (model == null)
        {
            return BadRequest(new ApiResponse<string>("Error", null, "Invalid team performance data"));
        }

        var exits = await _service.CheckIfExistsAsync(codice);

        if (exits == false)
        {
            return BadRequest(new ApiResponse<string>("Error", null, "The team performance code does not exist"));
        }

        var existWorkTeam = await _service.CheckIfWorkTeamExistsAsync(model.TeamCode);

        if (existWorkTeam == false)
        {
            return BadRequest(new ApiResponse<string>("Error", null, "The work team code is incorrect"));
        }

        var updated = await _service.UpdateAsync(codice, model);

        if (updated == null)
        {
            return StatusCode(400, new ApiResponse<string>("Error", null, "Team performance was updated but could not be retrieved"));
        }

        return Ok(new ApiResponse<TeamPerformanceAnalysis>("success", updated, "Team performance updated successfully"));
    }

    [HttpDelete("delete/{codice}")]
    public async Task<IActionResult> DeñeteOrder(string codice)
    {
        var exitsOrder = await _service.CheckIfExistsAsync(codice);

        if (exitsOrder == false)
        {
            return BadRequest(new ApiResponse<string>("Error", null, "The team performance code does not exist"));
        }

        var delete = await _service.DeleteAsync(codice);

        if (delete == null)
        {
            return StatusCode(400, new ApiResponse<string>("Error", null, "Error ..."));
        }

        return Ok(new ApiResponse<TeamPerformanceAnalysis>("success", delete, "Team performance deleted successfully"));
    }
}

