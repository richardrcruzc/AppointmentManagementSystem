export interface Service {
  id: number;
  serviceName: string;
  durationHour: number;
  durationMinute: number;
  price: number;
  photo: string;
  serviceDescription: string;
  activeStatus: boolean;
  serviceCategoryId: number;
}
