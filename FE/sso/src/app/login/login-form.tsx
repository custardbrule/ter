"use client";
import { TextField, Button } from "@mui/material";
import type { Metadata } from "next";
import { useState } from "react";

import API_ENDPOINTS from "@/lib/constants/api";
import REGEX from "@/lib/constants/regex";

export const metadata: Metadata = {
  title: "...",
  description: "...",
};

export default function LoginForm() {
  const [password, setPassword] = useState("");
  const [email, setEmail] = useState("");

  const submit = async () => {
    // validate email and password
    if (!REGEX.EMAIL.test(email)) {
    }

    // login
    fetch(API_ENDPOINTS.LOGIN, {
      method: "POST",
      body: JSON.stringify({ email, password }),
    });
  };

  return (
    <div className="flex flex-col justify-center items-center p-16 gap-8 sm:w-[32rem]">
      <p>Login</p>
      <div className="flex flex-col gap-8 w-full">
        <TextField
          required
          fullWidth={true}
          label="Account"
          variant="outlined"
          size="small"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
        />
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
        <Button variant="contained" onClick={submit}>
          Login
        </Button>
      </div>
    </div>
  );
}
