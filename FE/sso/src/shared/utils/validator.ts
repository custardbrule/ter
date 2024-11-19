class AppValidator {
  hasValue(value: any): boolean {
    return value !== undefined && value !== null;
  }
  required(value: any): boolean {
    const isNullOrUndefined = this.hasValue(value);

    switch (typeof value) {
      case "string":
        return isNullOrUndefined && value.length > 0;
      default:
        return isNullOrUndefined;
    }
  }

  minLength(value: string, min: number): boolean {
    return this.hasValue(value) && value.length >= min;
  }

  maxLength(value: string, max: number): boolean {
    return this.hasValue(value) && value.length <= max;
  }

  min(value: number, min: number): boolean {
    return this.hasValue(value) && value >= min;
  }

  max(value: number, max: number): boolean {
    return this.hasValue(value) && value <= max;
  }
}

const validator = new AppValidator();

export default validator;
