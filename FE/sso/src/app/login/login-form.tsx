"use client";
import { TextField, Button } from "@mui/material";
import { ChangeEvent, FormEvent, useState } from "react";
import { toast } from "react-toastify";

import API_ENDPOINTS from "@/shared/constants/api";
import REGEX from "@/shared/constants/regex";
import APP_MESSAGE from "@/shared/constants/message";
import MIMETYPES from "@/shared/constants/mime-type";

export default function LoginForm() {
  const [password, setPassword] = useState("");
  const [email, setEmail] = useState("");
  const [isEmailValid, setIsEmailValid] = useState(true);

  const onEmailChange = (
    e: ChangeEvent<HTMLInputElement | HTMLTextAreaElement>
  ) => {
    if (!isEmailValid) setIsEmailValid(true);
    setEmail(e.target.value);
  };

  const submit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    // validate email and password
    if (!REGEX.EMAIL.test(email)) {
      setIsEmailValid(false);
      return toast("halo", {
        position: "top-right",
        autoClose: 5000,
        pauseOnHover: false,
      });
    }

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
        console.log(data);
        if (res.ok) return data;

        throw { message: res.statusText, status: res.status, data: data };
      })
      .catch((err) => console.log(err));
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
          <Button variant="text">Register</Button>
          <Button variant="contained" type="submit">
            Login
          </Button>
        </div>
      </form>
    </div>
  );
}
