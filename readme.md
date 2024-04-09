
# Rectangle Drawer

This project allows users to draw and resize a rectangle SVG figure on a web page, 
displays the perimeter of the figure, and updates the rectangle's dimensions in a JSON file after resizing.

## 👣 Setup

### Backend (ASP.NET Core)

<br/>

Configure CORS to allow the Angular frontend to communicate with the backend:

```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowOrigin", policyBuilder =>
    {
        policyBuilder.WithOrigins("http://localhost:4200", "https://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

app.UseCors("AllowOrigin");
```
<br/>

Create a `rectangle.json` file with initial dimensions:

```json
{
  "X": 0,
  "Y": 0,
  "Width": 100,
  "Height": 50
}
```

<br/>

the `JsonRectangleSerializer` service to handle the serialization of rectangle data. 

```csharp
builder.Services.AddTransient<IRectangleSerializer, JsonRectangleSerializer>();
```

<br/>

Define the `IRectangleSerializer` interface to abstract the methods for serialization:

```csharp
    public interface IRectangleSerializer
    {
        Task<RectangleModel?> GetFromString(string jsonFilePath);
        Task SaveAsJson(RectangleModel rectangle, string jsonFilePath);
    }
```
<br/>

Implement the `JsonRectangleSerializer` class to manage reading and writing of the rectangle data:

```csharp
using System.Text.Json;

    public class JsonRectangleSerializer : IRectangleSerializer
    {
        
        // get data
        public async Task<RectangleModel?> GetFromString(string jsonFilePath)
        {
            await using var stream = File.OpenRead(jsonFilePath);
            return await JsonSerializer.DeserializeAsync<RectangleModel>(stream);
        }


        // save data
        public async Task SaveAsJson(RectangleModel rectangle, string jsonFilePath)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            await using var stream = File.Create(jsonFilePath);
            await JsonSerializer.SerializeAsync(stream, rectangle, options);
        }
    }
```
<br/>

Create the `RectangleModel`:

```csharp
    public class RectangleModel
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
```

<br/>

Add the `RectangleController` to manage HTTP requests:

```csharp
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

            var rectangle = await rectangleSerializer.GetFromString(jsonFilePath);
            return Ok(rectangle);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RectangleModel rectangle)
        {
            const string jsonFilePath = "rectangle.json";
            await rectangleSerializer.SaveAsJson(rectangle, jsonFilePath);
            return Ok();
        }
    }
```
<br/>
<br/>

###  Frontend (Angular)

Create `RectangleService` to manage API calls:

```typescript
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RectangleService {
  private apiBaseUrl = 'https://localhost:7246/Rectangle';

  constructor(private http: HttpClient) { }

  getRectangle(): Observable<{ x: number, y: number, width: number, height: number }> {
    return this.http.get<{ x: number, y: number, width: number, height: number }>(this.apiBaseUrl);
  }

  updateRectangle(rect: { x: number; y: number; width: number; height: number }) {
    return this.http.post(this.apiBaseUrl, rect).subscribe();
  }
}
```

<br/>
<br/>

Implement `RectangleComponent` to handle rectangle rendering and resizing:

```typescript
import { Component } from '@angular/core';
import { RectangleService } from './rectangle.service';

@Component({
  selector: 'app-rectangle',
  templateUrl: './rectangle.component.html',
  styleUrls: ['./rectangle.component.css']


})
export class RectangleComponent {
  rect = { x: 0, y: 0, width: 100, height: 50 };
  resizing = false;

  constructor(private rectangleService: RectangleService) {
    this.rectangleService.getRectangle().subscribe({
      next: (rect) => this.rect = rect,
      error: (err) => console.error('Error fetching rectangle:', err)
    });
  }

  startResizing(event: MouseEvent) {
    this.resizing = true;
  }

  resize(event: MouseEvent) {
    if (this.resizing) {
      this.rect.width = event.offsetX - this.rect.x;
      this.rect.height = event.offsetY - this.rect.y;
    }
  }

  stopResizing() {
    this.resizing = false;
    this.rectangleService.updateRectangle(this.rect);
  }

  calculatePerimeter() {
    return 2 * (this.rect.width + this.rect.height);
  }
}
```
<br/>

### 🪶 Implement a RectangleComponent which includes:

1. `startResizing(event: MouseEvent):` Activates resizing mode.
2. `stopResizing():` Deactivates resizing mode and updates the rectangle's dimensions.
3. `resize(event: MouseEvent):` Changes the dimensions of the rectangle during resizing.
4. `calculatePerimeter():` Calculates the perimeter of the rectangle based on its current dimensions.



<br/>
<br/>

In `rectangle.component.html`, define the SVG for drawing and resizing the rectangle:

```html
<div>
    <svg width="800" height="800" (mousedown)="startResizing($event)" (mouseup)="stopResizing()" (mousemove)="resize($event)">
        <rect [attr.x]="rect.x" [attr.y]="rect.y" [attr.width]="rect.width" [attr.height]="rect.height" stroke="black" stroke-width="2"></rect>
    </svg>
      <p>Perimeter: {{ calculatePerimeter() }} units</p>
      <p>Width: {{rect.width}}</p>
      <p>height: {{rect.height}}</p>
</div>
```
<br/>

### 🪶  The `mousedown`, `mouseup`, and `mousemove` are event bindings in Angular that respond to mouse events on the SVG element:
- `(mousedown)="startResizing($event)"`: This activates when you click down on the mouse. It starts the rectangle resizing process.
- `(mouseup)="stopResizing()"`: This triggers when you release the mouse button. It stops the resizing process.
- `(mousemove)="resize($event)"`: This occurs when you move the mouse. If you're holding the mouse button down (resizing), it changes the rectangle's size based on the mouse movement.


