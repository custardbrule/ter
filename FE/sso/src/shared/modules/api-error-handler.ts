function handlerValidationError(error: ApiError) {
  return {
    ...error,
    errors: error.errors?.reduce(
      (acc: { [x: string]: any[] }, cur: ValidationErrorDetail) => {
        if (typeof acc[cur.propertyName] === "undefined")
          acc[cur.propertyName] = [cur.errorMessage];

        acc[cur.propertyName].push(cur.errorMessage);
        return acc;
      },
      {} as { [k: string]: string[] }
    ),
  };
}

function handlerResponseError(error: ApiError) {
  switch (error.type) {
    case "ValidationFailure":
      return handlerValidationError(error);
    default:
      throw new Error("Invalid error type");
  }
}

export default handlerResponseError;
