export interface ApiError {
  status: number;
  title: string;
  message: string;
  errors?: Record<string, string[]>;
}

