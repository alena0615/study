import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SearchComponent } from './components/search/search.component';
import { LoginComponent } from './components/login/login.component';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'my-app',
  standalone: true,
  imports: [SearchComponent, LoginComponent, CommonModule],
  template: `
    <app-login *ngIf="!authService.currentUser" (loginSuccess)="onLogin()"></app-login>

    <div *ngIf="authService.currentUser">
      <div class="header">
        <span>Вы вошли как: <b>{{ authService.currentUser.username }}</b> ({{ authService.currentUser.role }})</span>
        <button (click)="logout()">Выйти</button>
      </div>
      <app-search></app-search>
    </div>
  `,
  styles: [`
    .header { background: #333; color: white; padding: 10px 20px; display: flex; justify-content: space-between; align-items: center; }
    button { background: #e74c3c; color: white; border: none; padding: 5px 10px; cursor: pointer; }
  `]
})
export class AppComponent {
  constructor(public authService: AuthService) {}

  onLogin() {
  }

  logout() {
    this.authService.logout();
  }
}