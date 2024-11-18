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

export default function LoginForm() {
  const [password, setPassword] = useState("");
  const [email, setEmail] = useState("");
  const [isEmailValid, setIsEmailValid] = useState(true);

  const router = useRouter();

  const onEmailChange = (
    e: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) => {
    if (!isEmailValid) setIsEmailValid(true);
    setEmail(e.target.value);
  };

  const onRegister = () => router.push(APP_ENDPOINTS.REGISTER);

  const submit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    // validate email and password
    if (!REGEX.EMAIL.test(email)) return setIsEmailValid(false);

    // login
    fetch(API_ENDPOINTS.LOGIN, {
      method: "POST",
      headers: {
        "Content-Type": MIMETYPES[".json"],
      },
      body: JSON.stringify({ email, password }),
    })
      .then(async (res) => {
        const data = await res.json();
        if (res.ok) return data;

        throw data;
      })
      .catch((err: ApiError) => {
        switch (err.type) {
          case "ValidationFailure":
            if (
              typeof err.errors?.find(
                (e) => e.propertyName.toLowerCase() === "email"
              ) === "undefined"
            )
              return;
            setIsEmailValid(false);
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
    <div className="flex flex-col justify-center items-center p-16 gap-8 sm:w-[32rem]">
      <p>Login</p>
      <form className="flex flex-col gap-8 w-full" onSubmit={(e) => submit(e)}>
        <div>
          {!isEmailValid && (
            <p className="py-2 flex gap-2 items-center">
              <span className="text-sm text-red-600 dark:text-red-400">
                {APP_MESSAGE.INVALID(email)}
              </span>
            </p>
          )}
          <TextField
            required
            fullWidth={true}
            label="Account"
            variant="outlined"
            size="small"
            value={email}
            onChange={(e) => onEmailChange(e)}
          />
        </div>
        <div>
          <TextField
            required
            fullWidth={true}
            label="Password"
            variant="outlined"
            type="password"
            size="small"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
          />
        </div>
        <div className="flex flex-row-reverse gap-2 w-full">
          <Button variant="text" type="button" onClick={() => onRegister()}>
            Register
          </Button>
          <Button variant="contained" type="submit">
            Login
          </Button>
        </div>
      </form>
    </div>
  );
}
