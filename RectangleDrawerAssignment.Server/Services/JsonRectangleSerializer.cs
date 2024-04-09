using RectangleDrawerAssignment.Server.Models;
using RectangleDrawerAssignment.Server.Services.Interfaces;
using System.Text.Json;

namespace RectangleDrawerAssignment.Server.Services
{
    public class JsonRectangleSerializer:IRectangleSerializer
    {
        public async Task<RectangleModel?> GetFromString(string jsonFilePath)
        {
            await using var stream = File.OpenRead(jsonFilePath);
            var rectangle = await JsonSerializer.DeserializeAsync<RectangleModel>(stream);
            return rectangle;
        }

        public async Task SaveAsJson(RectangleModel rectangle, string jsonFilePath)
        {

            var options = new JsonSerializerOptions { WriteIndented = true };
            await using var stream = File.Create(jsonFilePath);
            await JsonSerializer.SerializeAsync(stream, rectangle, options);
        }
    }
}
