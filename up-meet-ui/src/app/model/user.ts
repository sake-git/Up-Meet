export interface User {
  id: number;
  name: string;
  loginId: string;
  password?: string;
  phone?: string;
  email: string;
  token?: string;
}
