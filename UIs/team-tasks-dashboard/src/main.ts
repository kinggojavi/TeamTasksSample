import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { provideHttpClient } from '@angular/common/http';
import { provideRouter, Routes } from '@angular/router';

// Importa los componentes standalone que usarás en las rutas
import { DashboardComponent } from './app/dashboard/dashboard.component';
import { ProjectTasksComponent } from './app/projects/project-tasks/project-tasks.component';
import { NewTaskComponent } from './app/tasks/new-task/new-task.component'; // <-- importar el nuevo componente

const routes: Routes = [
  { path: '', component: DashboardComponent },        // Dashboard como vista principal
  { path: 'projects/:id', component: ProjectTasksComponent }, // Vista de tareas por proyecto
  { path: 'tasks/new-task', component: NewTaskComponent }     // <-- nueva ruta para crear tarea
];

bootstrapApplication(AppComponent, {
  providers: [
    provideHttpClient(),   // habilita HttpClient en toda la app
    provideRouter(routes)  // habilita routing con las rutas definidas
  ]
});
