type ApiError = {
  type: "ValidationFailure" | "BussinessException" | "InternalServerError";
  title: string;
  status: number;
  detail: string;
  errors: ValidationErrorDetail[] | never | undefined;
};

type ValidationErrorDetail = {
  propertyName: string;
  errorMessage: string;
  severity: 0;
  errorCode: null;
};
