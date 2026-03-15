import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProjectTasksService {
  constructor(private http: HttpClient) {}

  getTasksByProject(
    projectId: string,
    page: number,
    pageSize: number,
    state?: string,
    developer?: string
  ): Observable<any[]> {
    let url = `${environment.apiUrl}/projects/${projectId}/tasks?page=${page}&pageSize=${pageSize}`;
    if (state) url += `&state=${state}`;
    if (developer) url += `&developer=${developer}`;
    return this.http.get<any[]>(url);
  }
}
