import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DashboardService {

  constructor(private http: HttpClient) {}

  // Obtener resumen de carga por desarrollador
  getDeveloperWorkload(): Observable<any[]> {
    return this.http.get<any[]>(`${environment.apiUrl}/developer-load-summary`);
  }

  // Obtener estado de proyectos
  getProjectHealth(): Observable<any[]> {
    return this.http.get<any[]>(`${environment.apiUrl}/project-status-summary`);
  }

  // Obtener riesgo de retraso por desarrollador
  getDeveloperRisk(): Observable<any[]> {
    return this.http.get<any[]>(`${environment.apiUrl}/developer-risk-summary`);
  }
}
