import { Injectable } from '@angular/core';
import { ICreateEmployeeRequest } from '../models/employee.model';

export interface IValidationError {
  field: string;
  message: string;
}

@Injectable({
  providedIn: 'root'
})
export class ValidationService {

  validateEmployee(employee: ICreateEmployeeRequest): IValidationError[] {
    const errors: IValidationError[] = [];

    // Required fields validation
    if (!employee.firstName?.trim()) {
      errors.push({ field: 'firstName', message: 'First Name is required' });
    }

    if (!employee.lastName?.trim()) {
      errors.push({ field: 'lastName', message: 'Last Name is required' });
    }

    if (!employee.email?.trim()) {
      errors.push({ field: 'email', message: 'Email is required' });
    } else if (!this.isValidEmail(employee.email)) {
      errors.push({ field: 'email', message: 'Email format is invalid' });
    }

    if (!employee.phone?.trim()) {
      errors.push({ field: 'phone', message: 'Phone is required' });
    } else if (!this.isValidPhone(employee.phone)) {
      errors.push({ field: 'phone', message: 'Phone format is invalid' });
    }

    if (!employee.birthDay) {
      errors.push({ field: 'birthDay', message: 'Birth Day is required' });
    } else if (new Date(employee.birthDay) > new Date()) {
      errors.push({ field: 'birthDay', message: 'Birth Day cannot be in the future' });
    }

    if (!employee.occupation?.trim()) {
      errors.push({ field: 'occupation', message: 'Occupation is required' });
    }

    if (!employee.sex || (employee.sex !== 'Male' && employee.sex !== 'Female')) {
      errors.push({ field: 'sex', message: 'Sex must be either Male or Female' });
    }

    return errors;
  }

  private isValidEmail(email: string): boolean {
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return emailRegex.test(email);
  }

  private isValidPhone(phone: string): boolean {
    // Thai phone numbers are typically 9-10 digits
    const digitsOnly = phone.replace(/[^0-9]/g, '');
    return digitsOnly.length >= 9 && digitsOnly.length <= 10;
  }
}
