using Microsoft.AspNetCore.Mvc;
using RectangleDrawerAssignment.Server.Models;
using RectangleDrawerAssignment.Server.Services.Interfaces;

namespace RectangleDrawerAssignment.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RectangleController(IRectangleSerializer rectangleSerializer) : ControllerBase
    {


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            const string jsonFilePath = "rectangle.json";
            if (!System.IO.File.Exists(jsonFilePath))
            {
                return NotFound("not found.");
            }

            var rectangle =await rectangleSerializer.GetFromString(jsonFilePath);
            return Ok(rectangle);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RectangleModel rectangle)
        {
            const string jsonFilePath = "rectangle.json";
          await rectangleSerializer.SaveAsJson(rectangle,jsonFilePath);
            return Ok();
        }
    }
}