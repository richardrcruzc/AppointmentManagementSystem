export interface Location {
  id: number;
  description: string;
  contactName: string;
  address: string;
  address1: string;
  city: string;
  state: string;
  zipCode: string;
  country: string;
  phone: string;
  active: boolean;
  confirmation: boolean;
  reminder: boolean;
  rescheduling: boolean;
  thankYou: boolean;
  cancelation: boolean;
  noShowUp: boolean;
  totalMsgs: number;
  contactEmail: string;
}
export interface LocationList {
  id: number;
  description: string;
  contactName: string; 
  city: string; 
  phone: string;
  totalMsgs: number;
  totalAppt: number;
  totalClosed: number;
}
