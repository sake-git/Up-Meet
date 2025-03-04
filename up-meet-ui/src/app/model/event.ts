export interface UserEvent {
  id: number;
  name: string;
  location: string;
  eventDateTime: Date;
  imgUrl: string;
  description: string;
  price: number;
  kidsAllowed: number;
  duration: number;
  createdBy: number;
  createdByUser?: string;
  isFavourite?: boolean;
}
