import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7142/api/auth';
  
  currentUser: { username: string, role: string, token: string } | null = null;

  constructor(private http: HttpClient) {}

  login(username: string, password: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/login`, { username, password }).pipe(
      tap(response => {
        const token = btoa(username + ':' + password);
        this.currentUser = {
          username: username,
          role: response.role,
          token: token
        };
      })
    );
  }

  register(username: string, password: string): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/register`, { username, password });
  }

  logout() {
    this.currentUser = null;
  }
}