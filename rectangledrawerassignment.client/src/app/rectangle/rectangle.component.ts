import { Component, OnInit } from '@angular/core';
import { RectangleService } from './rectangle.service';

@Component({
  selector: 'app-rectangle',
  templateUrl: './rectangle.component.html',
  styleUrl: './rectangle.component.css'
})
export class RectangleComponent 
{
  // Default dimensions
  rect = { x: 0, y: 0, width: 100, height: 50 }; 
  resizing = false;
  constructor(private rectangleService: RectangleService) {
    this.rectangleService.getRectangle().subscribe({
      next: (rect: { x: number; y: number; width: number; height: number }) => this.rect = rect,
      error: (err: any) => console.error('Error fetching rectangle:', err)
    });
  }

  startResizing(event: MouseEvent) {
    this.resizing = true;
  }

  stopResizing() {
    
    this.resizing = false;
    this.rectangleService.updateRectangle(this.rect);

  }

  resize(event: MouseEvent) {

    if (this.resizing) {
      this.rect.width = event.offsetX - this.rect.x;
      this.rect.height = event.offsetY - this.rect.y;
    }


  }

  calculatePerimeter() {
    return 2 * (this.rect.width + this.rect.height);
  }  
}
