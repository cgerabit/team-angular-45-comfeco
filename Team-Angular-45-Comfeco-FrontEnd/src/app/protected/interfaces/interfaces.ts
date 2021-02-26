export interface Event {
  id: number;
  name: string;
  date: Date;
  isActive: boolean
}

export interface Technologies {
  id: number;
  name: string;
  technologyIcon?: string;
  areaId: number
}

