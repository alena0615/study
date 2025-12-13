import { Component, ChangeDetectorRef } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ProductService } from '../../services/product.service';

@Component({
  selector: 'app-search',
  standalone: true,
  imports: [FormsModule, CommonModule],
  template: `
    <div class="search-container">
      <h1>Поиск товаров на складе</h1>
      
      <div class="search-form">
        <div class="form-group">
          <label>Параметр поиска:</label>
          <select [(ngModel)]="searchType" class="form-control">
            <option value="barcode">Штрихкод</option>
            <option value="storageCell">Ячейка на складе</option>
            <option value="productBarcode">Баркод товара</option>
            <option value="article">Артикул</option>
          </select>
        </div>

        <div class="form-group">
          <label>Значение для поиска:</label>
          <input 
            type="text" 
            [(ngModel)]="searchValue" 
            class="form-control"
            (keyup.enter)="onSearch()">
        </div>

        <button class="btn-search" (click)="onSearch()" [disabled]="isLoading">
          {{ isLoading ? 'Поиск...' : 'Найти' }}
        </button>
      </div>

      <div *ngIf="isLoading" class="loading">
        <div class="spinner"></div>
        <p>Ищем товар...</p>
      </div>

      <div *ngIf="notFound" class="not-found">
        <p>❌ Товар не найден</p>
      </div>

      <div *ngIf="searchResult" class="result-card">
        <h2>Информация о товаре</h2>
        
        <div class="result-grid">
          <div class="result-item">
            <span class="label">Наименование:</span>
            <span class="value">{{ searchResult.productName }}</span>
          </div>

          <div class="result-item">
            <span class="label">Ячейка:</span>
            <span class="value highlight">{{ searchResult.cellLocation }}</span>
          </div>

          <div class="result-item">
            <span class="label">Принял:</span>
            <span class="value">{{ searchResult.receivedByEmployee }}</span>
          </div>

          <div class="result-item">
            <span class="label">Дата приёмки:</span>
            <span class="value">{{ searchResult.receivedDate | date:'dd.MM.yyyy HH:mm' }}</span>
          </div>

          <div class="result-item">
            <span class="label">Выдал:</span>
            <span class="value">{{ searchResult.issuedByEmployee || 'Не выдан' }}</span>
          </div>

          <div class="result-item">
            <span class="label">Дата выдачи:</span>
            <span class="value">
              {{ searchResult.issuedDate ? (searchResult.issuedDate | date:'dd.MM.yyyy HH:mm') : '-' }}
            </span>
          </div>
          
           <div class="result-item">
            <span class="label">Вес (приемка):</span>
            <span class="value">{{ searchResult.weightReceived }} кг</span>
          </div>
        </div>
      </div>
    </div>
  `,
  styles: [`
    .search-container { max-width: 800px; margin: 0 auto; padding: 20px; font-family: sans-serif; }
    .search-form { background: white; padding: 30px; border-radius: 8px; box-shadow: 0 2px 10px rgba(0,0,0,0.1); margin-bottom: 20px; }
    .form-group { margin-bottom: 20px; }
    .form-control { width: 100%; padding: 10px; margin-top: 5px; box-sizing: border-box; font-size: 16px; }
    .btn-search { width: 100%; padding: 12px; background: #3498db; color: white; border: none; border-radius: 4px; cursor: pointer; font-size: 16px; }
    .btn-search:disabled { background: #ccc; }
    .result-card { background: white; padding: 20px; border-radius: 8px; box-shadow: 0 2px 10px rgba(0,0,0,0.1); }
    .result-grid { display: grid; gap: 10px; }
    .result-item { display: flex; justify-content: space-between; padding: 10px 0; border-bottom: 1px solid #eee; }
    .label { font-weight: bold; color: #555; }
    .value { font-weight: 500; text-align: right; }
    .value.highlight { color: #3498db; font-size: 1.2em; font-weight: bold; }
    .loading { text-align: center; padding: 20px; background: white; border-radius: 8px; margin-bottom: 20px; }
    .spinner { width: 40px; height: 40px; border: 4px solid #f3f3f3; border-top: 4px solid #3498db; border-radius: 50%; animation: spin 1s linear infinite; margin: 0 auto 10px; }
    .not-found { padding: 20px; background: #fff5f5; color: #c0392b; text-align: center; border-radius: 8px; border: 1px solid #e74c3c; margin-bottom: 20px; font-weight: bold; }
    @keyframes spin { 0% { transform: rotate(0deg); } 100% { transform: rotate(360deg); } }
  `]
})
export class SearchComponent {
  searchType: string = 'barcode';
  searchValue: string = '';
  searchResult: any = null;
  isLoading: boolean = false;
  notFound: boolean = false;

  constructor(
    private productService: ProductService,
    private cdr: ChangeDetectorRef 
  ) {}

  onSearch(): void {
    if (!this.searchValue.trim()) return;

    this.isLoading = true;
    this.notFound = false;
    this.searchResult = null;

    this.productService.searchProduct({
      searchType: this.searchType,
      searchValue: this.searchValue
    }).subscribe({
      next: (data) => {
        console.log('Данные получены:', data);
        this.isLoading = false;

        if (data) {
          this.searchResult = data;
          this.notFound = false;
        } else {
          this.searchResult = null;
          this.notFound = true;
        }

        this.cdr.detectChanges();
      },
      error: (error) => {
        console.error('Ошибка:', error);
        this.isLoading = false;
        this.notFound = true;

        this.cdr.detectChanges();
      }
    });
  }
}