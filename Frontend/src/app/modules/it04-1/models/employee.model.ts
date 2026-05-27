export interface IEmployee {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  birthDay: Date;
  occupation: string;
  profileImageBase64?: string;
  sex: string;
  createdAt: Date;
  updatedAt?: Date;
}

export interface ICreateEmployeeRequest {
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  birthDay: Date;
  occupation: string;
  profileImageBase64?: string;
  sex: string;
}

export interface IApiResponse<T> {
  success: boolean;
  message?: string;
  errors?: string[];
  data?: T;
}
