export interface LoginModel {
  email: string;
  password: string;
  rememberMe: boolean;
}


export interface LogedInModel {
  accessToken: string;
  userName: string;
  userRole: string;
}
