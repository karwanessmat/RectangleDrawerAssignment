import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RectangleService {
  private apiBaseUrl = 'https://localhost:7246/Rectangle'; // Adjust the port if your API runs on a different one

  rect = { x: 0, y: 0, width: 100, height: 50 }; 

  constructor(private http: HttpClient) { }

  getRectangle(): Observable<{ x: number, y: number, width: number, height: number }> {
    return this.http.get<{ x: number, y: number, width: number, height: number }>(this.apiBaseUrl);

    
    
  }

  updateRectangle(rect: { x: number; y: number; width: number; height: number }) {


    return this.http.post(this.apiBaseUrl, rect).subscribe();

  }
}
