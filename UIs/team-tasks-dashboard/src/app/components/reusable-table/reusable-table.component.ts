import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

export interface TableColumn {
  header: string;
  field: string;
  sortable?: boolean;
  isLink?: boolean;       // <-- nueva propiedad opcional
  linkField?: string;     // <-- campo que contiene el id para el link
}

@Component({
  selector: 'app-reusable-table',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './reusable-table.component.html',
  styleUrls: ['./reusable-table.component.css']
})
export class ReusableTableComponent {
  @Input() columns: TableColumn[] = [];
  @Input() data: any[] = [];
  @Input() rowClassFn?: (row: any) => string;

  sortDirection: { [key: string]: 'asc' | 'desc' } = {};

  ngOnInit() {
    this.columns.forEach(col => {
      if (col.sortable) {
        this.sortDirection[col.field] = 'asc'; // inicializa en ascendente
      }
    });
  }

  sort(field: string) {
    this.sortDirection[field] = this.sortDirection[field] === 'asc' ? 'desc' : 'asc';
    const direction = this.sortDirection[field];

    this.data = [...this.data].sort((a, b) => {
      let result: number;
      if (typeof a[field] === 'string') {
        result = a[field].localeCompare(b[field]);
      } else {
        result = a[field] - b[field];
      }
      return direction === 'asc' ? result : -result;
    });
  }

  isDate(value: any): boolean {
    return value instanceof Date || (!isNaN(Date.parse(value)) && isNaN(Number(value)));
  }

  isNumber(value: any): boolean {
    return typeof value === 'number' || (!isNaN(Number(value)) && value !== '' && isFinite(Number(value)));
  }
}
