import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReusableTableComponent } from '../components/reusable-table/reusable-table.component';
import { DashboardService } from '../services/dashboard.service';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, ReusableTableComponent],
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  developerWorkload: any[] = [];
  projectHealth: any[] = [];
  developerRisk: any[] = [];

  constructor(private dashboardService: DashboardService) {}

  ngOnInit(): void {
    this.loadDeveloperWorkload();
    this.loadProjectHealth();
    this.loadDeveloperRisk();
  }

  private loadDeveloperWorkload(): void {
    this.dashboardService.getDeveloperWorkload().subscribe({
      next: (data) => {
        console.log('Developer workload:', data);
        this.developerWorkload = data;
      },
      error: (err) => console.error('Error loading developer workload', err)
    });
  }

  private loadProjectHealth(): void {
    this.dashboardService.getProjectHealth().subscribe({
      next: (data) => {
        console.log('Project health:', data);
        this.projectHealth = data;
      },
      error: (err) => console.error('Error loading project health', err)
    });
  }

  private loadDeveloperRisk(): void {
    this.dashboardService.getDeveloperRisk().subscribe({
      next: (data) => {
        console.log('Developer risk:', data);
        this.developerRisk = data;
      },
      error: (err) => console.error('Error loading developer risk', err)
    });
  }

  // Método para verificar si un proyecto debe resaltarse
  isProjectHighlighted(project: any): string {
    return project.openTasks > project.completedTasks ? 'highlight' : '';
  }

  // Método para verificar si un desarrollador está en alto riesgo
  isHighRisk(dev: any): string {
    return dev.highRiskFlag === 1 ? 'high-risk' : '';
  }
}
