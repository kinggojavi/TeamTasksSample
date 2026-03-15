import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment'; // importa el environment

export interface Task {
  projectId: string;
  title: string;
  description: string;
  assigneeId: string;
  status: string;
  priority: string;
  estimatedComplexity: number;
  dueDate: string;
  completionDate?: string;
}

export interface Developer {
  developerId: number;
  fullName: string;
  email: string;
}

@Injectable({
  providedIn: 'root'
})
export class NewTaskService {
  // Base URL del backend
  private apiUrl = `${environment.apiUrl}`;

  constructor(private http: HttpClient) {}

  // Crear nueva tarea
  createTask(task: Task): Observable<any> {
    return this.http.post(`${this.apiUrl}/tasks`, task);
  }

  // Obtener desarrolladores activos
  getActiveDevelopers(): Observable<Developer[]> {
    return this.http.get<Developer[]>(`${this.apiUrl}/developers`);
  }
}
