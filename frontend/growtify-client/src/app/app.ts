import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet, Router } from '@angular/router';
import { Nav } from '../layout/nav/nav';
import { AccountService } from '../core/services/account-service';
import { User } from '../types/user';

@Component({
  selector: 'app-root',
  imports: [Nav, RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  private accountService = inject(AccountService);
  protected router = inject(Router);

  ngOnInit(): void {
    this.setCurrentUser();
  }

  setCurrentUser() {
  this.accountService.refreshToken().subscribe({
    next: user => {
      if (user) {
        this.accountService.setCurrentUser(user);
      }
    },
    error: () => {
      // user not logged in / token invalid → do nothing
    }
  });
}
}