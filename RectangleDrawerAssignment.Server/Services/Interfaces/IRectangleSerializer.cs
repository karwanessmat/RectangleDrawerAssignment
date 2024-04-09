using RectangleDrawerAssignment.Server.Models;

namespace RectangleDrawerAssignment.Server.Services.Interfaces
{
    public interface IRectangleSerializer
    {
        Task<RectangleModel?> GetFromString(string jsonFilePath);
        Task SaveAsJson(RectangleModel rectangle, string jsonFilePath);

    }
}
