import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IEmployee, ICreateEmployeeRequest, IApiResponse } from '../models/employee.model';

@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  private readonly apiUrl = 'http://localhost:5000/api/employee';

  constructor(private http: HttpClient) { }

  createEmployee(request: ICreateEmployeeRequest): Observable<IApiResponse<IEmployee>> {
    return this.http.post<IApiResponse<IEmployee>>(`${this.apiUrl}/create`, request);
  }

  getEmployee(id: number): Observable<IApiResponse<IEmployee>> {
    return this.http.get<IApiResponse<IEmployee>>(`${this.apiUrl}/get/${id}`);
  }

  getAllEmployees(): Observable<IApiResponse<IEmployee[]>> {
    return this.http.get<IApiResponse<IEmployee[]>>(`${this.apiUrl}/getAll`);
  }

  updateEmployee(id: number, request: ICreateEmployeeRequest): Observable<IApiResponse<IEmployee>> {
    return this.http.put<IApiResponse<IEmployee>>(`${this.apiUrl}/update/${id}`, request);
  }

  deleteEmployee(id: number): Observable<IApiResponse<any>> {
    return this.http.delete<IApiResponse<any>>(`${this.apiUrl}/delete/${id}`);
  }
}
