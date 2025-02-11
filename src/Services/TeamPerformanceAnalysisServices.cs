using microservices_team_performance_analysis.Database;
using microservices_team_performance_analysis.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using NewtonsoftJson = Newtonsoft.Json.JsonConvert;

namespace microservices_team_performance_analysis.Services
{
    public class TeamPerformanceAnalysisServices
    {
        private readonly DBContext _context;
        private readonly HttpClient _httpClient;

        public TeamPerformanceAnalysisServices(DBContext context, HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        public async Task<List<TeamPerformanceAnalysis>> GetAllAsync()
        {
            return await _context.TeamPerformanceAnalysis.Where(x => x.Status == true).ToListAsync();
        }

        public async Task<TeamPerformanceAnalysis> GetByCodeAsync(string codice)
        {
            return await _context.TeamPerformanceAnalysis.FirstOrDefaultAsync(order => order.TeamPerformanceCode == codice && order.Status == true);
        }

        public async Task<TeamPerformanceAnalysis> CreateAsync(TeamPerformanceAnalysis model)
        {
            model.Status = true;
            model.CreatedAt = DateTime.UtcNow;
            model.UpdatedAt = DateTime.UtcNow;

            var createOrder = _context.TeamPerformanceAnalysis.Add(model);
            await _context.SaveChangesAsync();
            var id = createOrder.Entity.TeamPerformanceId;

            var result = await _context.TeamPerformanceAnalysis.FirstOrDefaultAsync(x => x.TeamPerformanceId == id);

            return result;
        }

        public async Task<TeamPerformanceAnalysis> UpdateAsync(string codice, TeamPerformanceAnalysis model)
        {
            var result = await _context.TeamPerformanceAnalysis.FirstOrDefaultAsync(x => x.TeamPerformanceCode == codice);

            if (result == null)
            {
                return null;
            }

            result.UpdatedAt = DateTime.UtcNow;
            result.TeamCode = model.TeamCode;
            result.TeamName = model.TeamName;
            result.CompletedOrders = model.CompletedOrders;
            result.PendingOrders = model.PendingOrders;
            result.AvgCompletionTime = model.AvgCompletionTime;
            result.AvgQuality = model.AvgQuality;
            result.Efficiency = model.Efficiency;

            await _context.SaveChangesAsync();

            return result;
        }

        public async Task<TeamPerformanceAnalysis> DeleteAsync(string codice)
        {
            var result = await _context.TeamPerformanceAnalysis.FirstOrDefaultAsync(x => x.TeamPerformanceCode == codice);

            if (result == null)
            {
                return null;
            }

            result.Status = false;
            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<bool> CheckIfWorkTeamExistsAsync(string codice)
        {
            //var url = $"http://localhost:8180/work-team/{codice}"; // Local
            var url = $"http://app_work_team_search:8080/work-team/{codice}"; 

            try
            {
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var apiResponse = NewtonsoftJson.DeserializeObject<ApiResponse<JObject>>(responseContent);
                    if (apiResponse != null && apiResponse.Status == "success")
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> CheckIfExistsAsync(string codice)
        {
            var exist = await _context.TeamPerformanceAnalysis.FirstOrDefaultAsync(x => x.TeamPerformanceCode == codice);
            if (exist == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<string> GenerateNextOrderCodeAsync()
        {
            var quantity = await _context.TeamPerformanceAnalysis.CountAsync();
            var nextCode = $"TFA_{quantity + 1}";
            return nextCode;
        }
    }


}
