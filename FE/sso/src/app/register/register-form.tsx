"use client";
import { TextField, Button } from "@mui/material";
import { ChangeEvent, FormEvent, useState } from "react";
import { toast } from "react-toastify";
import { useRouter } from "next/navigation";

import API_ENDPOINTS from "@/shared/constants/api";
import REGEX from "@/shared/constants/regex";
import APP_MESSAGE from "@/shared/constants/message";
import MIMETYPES from "@/shared/constants/mime-type";
import APP_ENDPOINTS from "@/shared/constants/endpoints";
import { useAppDispatch } from "@/lib/hooks/store";
import { login } from "@/lib/features/authSlice";
import useAppFetch from "@/lib/hooks/fetch";
import validator from "@/shared/utils/validator";

export default function RegisterForm() {
  const [registerInfo, setRegisterInfo] = useState({
    email: "",
    password: "",
    firstName: "",
    lastName: "",
    phoneNumber: "",
  });
  const [validated, setValidated] = useState({
    email: { valid: true, message: "" },
    password: { valid: true, message: "" },
    firstName: { valid: true, message: "" },
    lastName: { valid: true, message: "" },
    phoneNumber: { valid: true, message: "" },
  });

  const router = useRouter();
  const dispatch = useAppDispatch();
  const fetcher = useAppFetch();

  const onInputChange = <T extends keyof typeof registerInfo>(
    key: T,
    value: string
  ) => {
    return setRegisterInfo((pre) => ({ ...pre, [key]: value }));
  };

  const isValid = <T extends keyof typeof validated>(key: T) => {
    return validated[key].valid;
  };

  const validate = () => {
    const isEmailValid =
      validator.required(registerInfo.email) &&
      REGEX.EMAIL.test(registerInfo.email);

    const isPasswordValid =
      validator.required(registerInfo.password) &&
      REGEX.PASSWORD.test(registerInfo.password);

    const res = {
      ...validated,
      ...(!REGEX.EMAIL.test(registerInfo.email) && {
        email: {
          valid: false,
          message: APP_MESSAGE.INVALID(registerInfo.email),
        },
      }),
      ...(!REGEX.PASSWORD.test(registerInfo.password) && {
        password: { valid: false, message: APP_MESSAGE.INVALID_PASSWORD() },
      }),
      ...(!registerInfo.firstName && {
        firstName: {
          valid: false,
          message: APP_MESSAGE.REQUIRED("First Name"),
        },
      }),
      ...(!registerInfo.lastName && {
        lastName: { valid: false, message: APP_MESSAGE.REQUIRED("Last Name") },
      }),
      ...(!registerInfo.phoneNumber && {
        phoneNumber: {
          valid: false,
          message: APP_MESSAGE.REQUIRED("Phone Number"),
        },
      }),
    };
  };

  const submit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    // register
    fetcher(
      API_ENDPOINTS.REGISTER,
      "POST",
      { "Content-Type": MIMETYPES[".json"] },
      JSON.stringify(registerInfo)
    )
      .then(async (res) => {
        const data = await res.json();
        if (res.ok) return dispatch(login(data));

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
    <div className="flex flex-col justify-center items-center p-16 gap-8 sm:w-[48rem]">
      <h1 className="text-3xl font-bold">Register</h1>
      <form className="flex flex-col gap-8 w-full" onSubmit={(e) => submit(e)}>
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
            value={registerInfo.email}
            onChange={(e) => onInputChange("email", e.target.value)}
          />
        </div>
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
  );
}
