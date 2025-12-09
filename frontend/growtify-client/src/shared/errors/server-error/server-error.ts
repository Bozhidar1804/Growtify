import { Component, inject } from '@angular/core';
import { ApiError } from '../../../types/error';
import { Router } from '@angular/router';

@Component({
  selector: 'app-server-error',
  imports: [],
  templateUrl: './server-error.html',
  styleUrl: './server-error.css',
})
export class ServerError {
  protected error?: ApiError;
  private router = inject(Router);
  protected showDetails = false;

  constructor() {
    const navigation = history.state;
    this.error = navigation?.error;
  }

  toggleDetails() {
    this.showDetails = !this.showDetails;
  }
}
