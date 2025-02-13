using microservices_raw_material_management.Database;
using microservices_raw_material_management.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using NewtonsoftJson = Newtonsoft.Json.JsonConvert;
using Microsoft.Extensions.Configuration;
using System;

namespace microservices_raw_material_management.Services
{
    public class RawMaterialManagementService
    {
        private readonly DBContext _context;
        private readonly HttpClient _httpClient;

        private readonly string _inventorySearchUrl;

        public RawMaterialManagementService(DBContext context, HttpClient httpClient, IConfiguration configuration)
        {
            _context = context;
            _httpClient = httpClient;
            _inventorySearchUrl = Environment.GetEnvironmentVariable("SERVICE_INVENTORY_SEARCH_URL") 
                            ?? configuration["SERVICE_INVENTORY_SEARCH_URL"];
        }

        public async Task<List<RawMaterialManagement>> GetAllAsync()
        {
            return await _context.RawMaterialManagement.Where(x => x.MaterialStatus == true).ToListAsync();
        }

        public async Task<RawMaterialManagement> GetByCodeAsync(string codice)
        {
            return await _context.RawMaterialManagement.FirstOrDefaultAsync(order => order.MaterialCode == codice && order.MaterialStatus == true);
        }

        public async Task<RawMaterialManagement> CreateAsync(RawMaterialManagement model)
        {
            model.MaterialStatus = true;
            model.CreatedAt = DateTime.UtcNow;
            model.UpdatedAt = DateTime.UtcNow;

            var createOrder = _context.RawMaterialManagement.Add(model);
            await _context.SaveChangesAsync();
            var id = createOrder.Entity.MaterialId;

            var result = await _context.RawMaterialManagement.FirstOrDefaultAsync(x => x.MaterialId == id);

            return result;
        }

        public async Task<RawMaterialManagement> UpdateAsync(string codice, RawMaterialManagement model)
        {
            var result = await _context.RawMaterialManagement.FirstOrDefaultAsync(x => x.MaterialCode == codice);

            if (result == null)
            {
                return null;
            }

            result.UpdatedAt = DateTime.UtcNow;
            result.InventoryCode = model.InventoryCode;
            result.MaterialName = model.MaterialName;
            result.AvailableQuantity = model.AvailableQuantity;
            result.TotalUsedCost = model.TotalUsedCost;
            result.UsedQuantity = model.UsedQuantity;

            await _context.SaveChangesAsync();

            return result;
        }

        public async Task<RawMaterialManagement> DeleteAsync(string codice)
        {
            var result = await _context.RawMaterialManagement.FirstOrDefaultAsync(x => x.MaterialCode == codice);

            if (result == null)
            {
                return null;
            }

            result.MaterialStatus = false;
            await _context.SaveChangesAsync();
            return result;
        }

        public async Task<bool> CheckIfInventoryExistsAsync(string codice)
        {
            //var url = $"http://localhost:8080/inventory/searchInventoryByCodice/search?codice={codice}"; // Local
            //var url = $"http://app-inventario-list:8080/inventory/searchInventoryByCodice/search?codice={codice}"; 
            var url = $"{_inventorySearchUrl}/inventory/searchInventoryByCodice/search?codice={codice}"; 

            try
            {
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var apiResponse = NewtonsoftJson.DeserializeObject<ApiResponse<JArray>>(responseContent);

                    var dataJson = apiResponse.Data.ToString();
                    var data = NewtonsoftJson.DeserializeObject<List<Inventory>>(dataJson);

                    if (apiResponse != null && apiResponse.Status == "success" && data.Count() > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public async Task<bool> CheckIfExistsAsync(string codice)
        {
            var exist = await _context.RawMaterialManagement.FirstOrDefaultAsync(x => x.MaterialCode == codice);
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
            var quantity = await _context.RawMaterialManagement.CountAsync();
            var nextCode = $"RMM_{quantity + 1}";
            return nextCode;
        }
    }
}
