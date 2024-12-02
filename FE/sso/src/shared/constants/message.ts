const APP_MESSAGE = {
  INVALID: (email: string) => `'${email}' is invalid.`,
  INVALID_PASSWORD: () =>
    `Password is invalid. Password must contain one uppercase letter, one lowercase letter, one number and one special character.`,
  REQUIRED: (fieldName: string) => `'${fieldName}' is required.`,

  REGISTER_SUCCESS: () => "Account registered successfully.",
};

export default APP_MESSAGE;
