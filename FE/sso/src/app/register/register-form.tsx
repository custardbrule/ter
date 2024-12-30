"use client";
import { TextField, Button } from "@mui/material";
import { FormEvent, useState } from "react";
import { toast } from "react-toastify";
import { useRouter } from "next/navigation";

import API_ENDPOINTS from "@/shared/constants/api";
import REGEX from "@/shared/constants/regex";
import APP_MESSAGE from "@/shared/constants/message";
import MIMETYPES from "@/shared/constants/mime-type";
import APP_ENDPOINTS from "@/shared/constants/endpoints";
import useAppFetch from "@/lib/hooks/fetch";
import validator from "@/shared/utils/validator";
import ErrorBoundary from "@/shared/components/error-boundary";

export default function RegisterForm() {
  const [registerInfo, setRegisterInfo] = useState({
    email: "",
    password: "",
    firstName: "",
    lastName: "",
    phoneNumber: "",
  });
  const [validated, setValidated] = useState({
    email: "",
    password: "",
    firstName: "",
    lastName: "",
    phoneNumber: "",
  });

  const router = useRouter();
  const fetcher = useAppFetch();

  const onInputChange = <T extends keyof typeof registerInfo>(
    key: T,
    value: string
  ) => {
    return setRegisterInfo((pre) => ({ ...pre, [key]: value }));
  };

  const isValid = <T extends keyof typeof validated>(key: T) => {
    return !validator.required(validated[key]);
  };

  const validate = () => {
    const res: typeof validated = {
      email: validator.match(registerInfo.email, REGEX.EMAIL)
        ? ""
        : APP_MESSAGE.INVALID(registerInfo.email),
      password: validator.match(registerInfo.password, REGEX.PASSWORD)
        ? ""
        : APP_MESSAGE.INVALID_PASSWORD(),
      firstName: validator.required(registerInfo.firstName)
        ? ""
        : APP_MESSAGE.REQUIRED("First Name"),
      lastName: validator.required(registerInfo.lastName)
        ? ""
        : APP_MESSAGE.REQUIRED("Last Name"),
      phoneNumber: validator.required(registerInfo.phoneNumber)
        ? ""
        : APP_MESSAGE.REQUIRED("Phone Number"),
    };

    const valid = Object.keys(res).every(isValid as any);
    if (!valid) setValidated(res);

    return valid;
  };

  const submit = (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    if (!validate()) return;

    // register
    fetcher(API_ENDPOINTS.REGISTER, {
      method: "POST",
      headers: { "Content-Type": MIMETYPES[".json"] },
      body: JSON.stringify(registerInfo),
    })
      .then(async (res) => {
        const data = await res.json();
        if (res.ok) {
          toast.success(APP_MESSAGE.REGISTER_SUCCESS());
          return router.push(APP_ENDPOINTS.LOGIN);
        }

        throw data;
      })
      .catch((err: ApiError) => {
        switch (err.type) {
          case "ValidationFailure":
            console.log(err);
            break;
          case "BussinessException":
            toast.error(`${err.status} - ${err.detail}`);
            break;
          default:
            throw err;
        }
      });
  };

  return (
    <ErrorBoundary>
      <div className="flex flex-col justify-center items-center p-16 gap-8 sm:w-[48rem]">
        <h1 className="text-3xl font-bold">Register</h1>
        <form
          className="flex flex-col gap-8 w-full"
          onSubmit={(e) => submit(e)}
        >
          {/* firstName & lastName */}
          <div className="flex gap-4 w-full">
            {/* firstName */}
            <div className="flex-2">
              {!isValid("firstName") && (
                <p className="py-2 flex gap-2 items-center">
                  <span className="text-sm text-red-600 dark:text-red-400">
                    {APP_MESSAGE.REQUIRED("First name")}
                  </span>
                </p>
              )}
              <TextField
                required
                fullWidth={true}
                label="First name"
                variant="outlined"
                size="small"
                value={registerInfo.firstName}
                onChange={(e) => onInputChange("firstName", e.target.value)}
              />
            </div>

            {/* lastName */}
            <div className="flex-1">
              {!isValid("firstName") && (
                <p className="py-2 flex gap-2 items-center">
                  <span className="text-sm text-red-600 dark:text-red-400">
                    {APP_MESSAGE.REQUIRED("Last name")}
                  </span>
                </p>
              )}
              <TextField
                required
                fullWidth={true}
                label="Last name"
                variant="outlined"
                size="small"
                value={registerInfo.lastName}
                onChange={(e) => onInputChange("lastName", e.target.value)}
              />
            </div>
          </div>

          {/* phone number */}
          <div>
            {!isValid("phoneNumber") && (
              <p className="py-2 flex gap-2 items-center">
                <span className="text-sm text-red-600 dark:text-red-400">
                  {APP_MESSAGE.INVALID(registerInfo.phoneNumber)}
                </span>
              </p>
            )}
            <TextField
              required
              fullWidth={true}
              label="Phone number"
              variant="outlined"
              size="small"
              value={registerInfo.phoneNumber}
              onChange={(e) => onInputChange("phoneNumber", e.target.value)}
            />
          </div>

          {/* email */}
          <div>
            {!isValid("email") && (
              <p className="py-2 flex gap-2 items-center">
                <span className="text-sm text-red-600 dark:text-red-400">
                  {APP_MESSAGE.INVALID(registerInfo.email)}
                </span>
              </p>
            )}
            <TextField
              required
              fullWidth={true}
              label="Email"
              variant="outlined"
              size="small"
              autoComplete="email"
              value={registerInfo.email}
              onChange={(e) => onInputChange("email", e.target.value)}
            />
          </div>

          {/* password */}
          <div>
            {!isValid("password") && (
              <p className="py-2 flex gap-2 items-center">
                <span className="text-sm text-red-600 dark:text-red-400">
                  {APP_MESSAGE.INVALID_PASSWORD()}
                </span>
              </p>
            )}
            <TextField
              required
              fullWidth={true}
              label="Password"
              variant="outlined"
              type="password"
              size="small"
              autoComplete="new-password"
              value={registerInfo.password}
              onChange={(e) => onInputChange("password", e.target.value)}
            />
          </div>

          <div className="flex flex-row-reverse gap-2 w-full">
            <Button variant="contained" type="submit">
              Register
            </Button>
          </div>
        </form>
      </div>
    </ErrorBoundary>
  );
}
