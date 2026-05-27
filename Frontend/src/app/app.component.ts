import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterOutlet } from '@angular/router';
import { IT04FormComponent } from './modules/it04-1/components/it04-form.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, RouterOutlet, IT04FormComponent],
  template: `
    <div class="app-container">
      <app-it04-form></app-it04-form>
    </div>
  `,
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'IT04 Employee Management';
}
