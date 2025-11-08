
import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';

@Component({
  selector: 'app-root',
  imports: [],
  templateUrl: './app.html',
  styleUrl: './app.scss'
})

export class App implements OnInit{
  private http = inject(HttpClient);
  protected readonly title = 'Growtify Client';

  ngOnInit(): void {
    this.http.get('https://localhost:5001/api/AppUsers').subscribe({
      next: response => console.log(response),
      error: error => console.log(error),
      complete: () => console.log('Http Request completed')
  });

  }
}
