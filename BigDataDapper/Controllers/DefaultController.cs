using BigDataDapper.DAL.DTOs;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace BigDataDapper.Controllers
{
    public class DefaultController : Controller
    {

        private readonly string _connectionString = "Server =DESKTOP-FIKBBE9; initial catalog = CARPLATES; integrated security = true";

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Search(string keyword)
        {

            string query = @"
            SELECT TOP 10000 BRAND, SUBSTRING(PLATE, 1, 2) AS PlatePrefix, SHIFTTYPE, FUEL
            FROM PLATES
            WHERE BRAND LIKE '%' + @Keyword + '%'
               OR PLATE LIKE '%' + @Keyword + '%'
               OR SHIFTTYPE LIKE '%' + @Keyword + '%'
               OR FUEL LIKE '%' + @Keyword + '%'
        ";

            await using var connection = new SqlConnection(_connectionString);
            connection.Open();

            // Sorguyu çalıştırın ve sonuçları alın
            var searchResults = await connection.QueryAsync<SearchResult>(query, new { Keyword = keyword });

            // Sonuçları JSON formatında döndürün
            return Json(searchResults);

        }
    }
}
