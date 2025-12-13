import { Component } from '@angular/core';
import { SearchComponent } from './components/search/search.component';

@Component({
  selector: 'my-app',
  standalone: true,
  imports: [SearchComponent],
  template: `<app-search></app-search>`
})
export class AppComponent {}