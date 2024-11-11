"use client";
import { TextField, Button } from "@mui/material";
import { ChangeEvent, FormEvent, useState } from "react";
import { toast } from "react-toastify";

import API_ENDPOINTS from "@/shared/constants/api";
import REGEX from "@/shared/constants/regex";
import Tooltip from "@/shared/components/tooltip";
import APP_MESSAGE from "@/shared/constants/message";

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
      body: JSON.stringify({ email, password }),
    });
  };

  const ToolTipIcon = () => (
    <svg
      xmlns="http://www.w3.org/2000/svg"
      fill="none"
      viewBox="0 0 24 24"
      strokeWidth={1.5}
      stroke="currentColor"
      className="size-5"
    >
      <path
        strokeLinecap="round"
        strokeLinejoin="round"
        d="m11.25 11.25.041-.02a.75.75 0 0 1 1.063.852l-.708 2.836a.75.75 0 0 0 1.063.853l.041-.021M21 12a9 9 0 1 1-18 0 9 9 0 0 1 18 0Zm-9-3.75h.008v.008H12V8.25Z"
      />
    </svg>
  );

  const ToolTipInfo = () => <>{APP_MESSAGE.INVALID(email)}</>;

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
              <Tooltip
                display={<ToolTipIcon />}
                info={<ToolTipInfo />}
                position="center-right"
              ></Tooltip>
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
