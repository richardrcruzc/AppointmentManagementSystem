export interface ClientI {
  id: number;
  lastName: string;
  firstName: string;
  mobile: string;
  email: string;
  active: boolean;
  sendNotificationBy: string;
  acceptsMarketingNotifications: boolean;
  photo: string;
  fullName: string;
}
export interface SendBy {

  sendByName: string;
}
