import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { EmployeeService } from '../services/employee.service';
import { ValidationService, IValidationError } from '../services/validation.service';
import { ICreateEmployeeRequest } from '../models/employee.model';

@Component({
  selector: 'app-it04-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './it04-form.component.html',
  styleUrls: ['./it04-form.component.scss']
})
export class IT04FormComponent implements OnInit {
  @ViewChild('profileInput') profileInput!: ElementRef;

  form!: FormGroup;
  validationErrors: IValidationError[] = [];
  successMessage: string = '';
  serverErrors: string[] = [];
  isLoading = false;
  createdId: number | null = null;

  occupations = [
    { id: 1, name: 'Engineer' },
    { id: 2, name: 'Manager' },
    { id: 3, name: 'Designer' },
    { id: 4, name: 'Developer' },
    { id: 5, name: 'Sales' },
    { id: 6, name: 'HR' },
    { id: 7, name: 'Other' }
  ];

  constructor(
    private fb: FormBuilder,
    private employeeService: EmployeeService,
    private validationService: ValidationService
  ) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  initializeForm(): void {
    this.form = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      phone: ['', Validators.required],
      birthDay: ['', Validators.required],
      occupation: ['', Validators.required],
      profileImage: [''],
      sex: ['Male', Validators.required]
    });
  }

  onFileSelected(event: any): void {
    const file: File = event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = (e: any) => {
        // Remove data:image/...;base64, prefix
        const base64 = e.target.result.split(',')[1];
        this.form.patchValue({
          profileImage: base64
        });
      };
      reader.readAsDataURL(file);
    }
  }

  selectFile(): void {
    this.profileInput.nativeElement.click();
  }

  save(): void {
    // Clear previous messages
    this.validationErrors = [];
    this.serverErrors = [];
    this.successMessage = '';
    this.createdId = null;

    // Client-side validation
    const clientErrors = this.validationService.validateEmployee({
      firstName: this.form.get('firstName')?.value,
      lastName: this.form.get('lastName')?.value,
      email: this.form.get('email')?.value,
      phone: this.form.get('phone')?.value,
      birthDay: this.form.get('birthDay')?.value,
      occupation: this.form.get('occupation')?.value,
      profileImageBase64: this.form.get('profileImage')?.value,
      sex: this.form.get('sex')?.value
    });

    if (clientErrors.length > 0) {
      this.validationErrors = clientErrors;
      return;
    }

    // Prepare request
    const request: ICreateEmployeeRequest = {
      firstName: this.form.get('firstName')?.value,
      lastName: this.form.get('lastName')?.value,
      email: this.form.get('email')?.value,
      phone: this.form.get('phone')?.value,
      birthDay: new Date(this.form.get('birthDay')?.value),
      occupation: this.form.get('occupation')?.value,
      profileImageBase64: this.form.get('profileImage')?.value,
      sex: this.form.get('sex')?.value
    };

    this.isLoading = true;

    // Call API
    this.employeeService.createEmployee(request).subscribe({
      next: (response) => {
        this.isLoading = false;
        if (response.success && response.data) {
          this.successMessage = response.message || 'save data success';
          this.createdId = response.data.id;
          this.form.reset({ sex: 'Male' });
          this.validationErrors = [];
        }
      },
      error: (error) => {
        this.isLoading = false;
        if (error.error?.errors) {
          this.serverErrors = Array.isArray(error.error.errors) ? error.error.errors : [error.error.errors];
        } else {
          this.serverErrors = [error.error?.message || 'An error occurred while saving'];
        }
      }
    });
  }

  clear(): void {
    this.form.reset({ sex: 'Male' });
    this.validationErrors = [];
    this.serverErrors = [];
    this.successMessage = '';
    this.createdId = null;
  }

  getErrorMessage(field: string): string | null {
    const error = this.validationErrors.find(e => e.field === field);
    return error ? error.message : null;
  }

  hasError(field: string): boolean {
    return this.validationErrors.some(e => e.field === field);
  }
}
