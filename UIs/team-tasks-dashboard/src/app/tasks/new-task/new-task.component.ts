import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router'; 
import { NewTaskService, Task } from '../../services/new-task.service';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-new-task',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './new-task.component.html',
  styleUrls: ['./new-task.component.css']
})
export class NewTaskComponent implements OnInit {
  projectId: string = '';
  projectName: string = '';

  task: Task = {
    projectId: '',
    title: '',
    description: '',
    assigneeId: '',
    status: 'ToDo',
    priority: 'Medium',
    estimatedComplexity: 1,
    dueDate: ''
  };

  developers: any[] = []; // lista de desarrolladores activos

  successMessage = '';
  errorMessage = '';

  constructor(
    private newTaskService: NewTaskService,
    private http: HttpClient,
    private route: ActivatedRoute,
    private router: Router  
  ) {}

 ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
      this.projectId = params['projectId'] ?? '';
      this.projectName = params['projectName'] ?? '';
      this.task.projectId = this.projectId;
    });

    // Cargar lista de desarrolladores activos usando el servicio
    this.newTaskService.getActiveDevelopers().subscribe({
      next: (data) => {
        this.developers = data;
      },
      error: () => {
        this.errorMessage = 'Error al cargar la lista de desarrolladores.';
      }
    });
  }

  onSubmit(form: NgForm): void {
    this.successMessage = '';
    this.errorMessage = '';

    if (form.invalid) {
      this.errorMessage = 'Por favor completa los campos requeridos.';
      return;
    }

    this.newTaskService.createTask(this.task).subscribe({
      next: () => {
        this.successMessage = 'Tarea creada exitosamente.';
        form.resetForm();
      },
      error: (err) => {
        this.errorMessage = 'Error al crear la tarea: ' + err.message;
      }
    });
  }

  goBack(): void {
    this.router.navigate(
      ['/projects', this.projectId],
      { queryParams: { projectName: this.projectName } }
    );
  }
}
