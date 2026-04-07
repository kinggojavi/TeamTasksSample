import { Component, OnInit, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ProjectTasksService } from '../../services/project-tasks.service';
import { ActivatedRoute, Router } from '@angular/router';1
import { Chart, registerables } from 'chart.js';
import ChartDataLabels from 'chartjs-plugin-datalabels';

// Registrar controladores, escalas y elementos de Chart.js
Chart.register(...registerables);
Chart.register(ChartDataLabels);

type TaskStatus = 'ToDo' | 'InProgress' | 'Blocked' | 'Completed';

@Component({
  selector: 'app-project-tasks',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './project-tasks.component.html',
  styleUrls: ['./project-tasks.component.css']
})
export class ProjectTasksComponent implements OnInit, AfterViewInit {
  tasks: any[] = [];
  projectId: string = '';
  projectName: string = '';
  page = 1;
  pageSize = 10;
  selectedState = '';
  selectedDeveloper = '';
  selectedTask: any = null;
  developers: { id: number, name: string }[] = [];
  
  private chart: Chart | null = null;
  private chartComplexity: Chart | null = null;

  constructor(
    private route: ActivatedRoute,
    private tasksService: ProjectTasksService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.projectId = this.route.snapshot.paramMap.get('id') ?? '';
    this.projectName = this.route.snapshot.queryParamMap.get('projectName') ?? '';
    this.loadTasks();
    this.updateChart();
  }

  ngAfterViewInit(): void {
    this.initChart();
  }

  private loadTasks(): void {
    this.tasksService.getTasksByProject(
      this.projectId,
      this.page,
      this.pageSize,
      this.selectedState,
      this.selectedDeveloper
    ).subscribe({
      next: (data) => {
        this.tasks = data;

        // Calcular lista de desarrolladores únicos
        this.developers = [
          ...new Map(
            this.tasks.map(t => [t.assigneeId, { id: t.assigneeId, name: t.assignedTo }])
          ).values()
        ];

        this.updateChart();
      },
      error: (err) => console.error('Error loading tasks', err)
    });
  }

  private initChart(): void {
    const ctx = document.getElementById('tasksChart') as HTMLCanvasElement;
    if (!ctx) return;

    this.chart = new Chart(ctx, {
      type: 'bar',
      data: {
        labels: ['ToDo', 'InProgress', 'Blocked', 'Completed'],
        datasets: [{
          label: 'Número de tareas',
          data: [0, 0, 0, 0],
          backgroundColor: ['#42A5F5', '#66BB6A', '#FFA726', '#AB47BC']
        }]
      },
      options: {
        responsive: true,
        plugins: {
          legend: {
            display: true,
            labels: {
              font: {
                size: 16 // 👉 tamaño más grande para la leyenda
              }
            }
          },
          datalabels: {
            color: '#000',
            font: {
              size: 18,   // 👉 labels internos más grandes
              weight: 'bold'
            },
            anchor: 'end',
            align: 'top',
            formatter: (value) => value
          }
        },
        scales: {
          x: {
            ticks: {
              font: {
                size: 18, // 👉 tamaño más grande para los labels del eje X
                weight: 'bold'
              }
            }
          },
          y: {
            ticks: {
              font: {
                size: 14 // 👉 opcional: ajustar tamaño de los números del eje Y
              }
            }
          }
        }
      },
      plugins: [ChartDataLabels] // 👉 registrar el plugin en este gráfico
    });
  }

  private updateChart(): void {
    if (!this.chart) return;

    const counts: Record<TaskStatus, number> = {
      ToDo: 0,
      InProgress: 0,
      Blocked: 0,
      Completed: 0
    };

    this.tasks.forEach(task => {
      const status = task.status as TaskStatus;
      if (status && counts[status] !== undefined) {
        counts[status] = counts[status] + 1;
      }
    });

    this.chart.data.datasets[0].data = [
      counts['ToDo'],
      counts['InProgress'],
      counts['Blocked'],
      counts['Completed']
    ];
    this.chart.update();
  }

  goToNewTask(): void {
    this.router.navigate(
      ['/tasks/new-task'],
      { queryParams: { projectId: this.projectId, projectName: this.projectName } }
    );
  }

  onFilterChange(): void {
    this.page = 1;
    this.loadTasks();
  }

  onPageChange(newPage: number): void {
    this.page = newPage;
    this.loadTasks();
  }

  showTaskDetail(task: any): void {
    this.selectedTask = task;
  }

  closeDetail(): void {
    this.selectedTask = null;
  }
}
