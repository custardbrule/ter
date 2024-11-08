import { TextField, Button } from "@mui/material";
import type { Metadata } from "next";

export const metadata: Metadata = {
  title: "...",
  description: "...",
};

export default function LoginForm() {
  return (
    <div className="flex flex-col justify-center items-center p-16 gap-8 sm:w-[32rem]">
      <p>Login</p>
      <div className="flex flex-col gap-8 w-full">
        <TextField
          required
          id="outlined-basic"
          fullWidth={true}
          label="Account"
          variant="outlined"
          size="small"
        />
        <TextField
          required
          id="outlined-basic"
          fullWidth={true}
          label="Password"
          variant="outlined"
          type="password"
          size="small"
        />
      </div>
      <div className="flex flex-row-reverse gap-2 w-full">
        <Button variant="text">Register</Button>
        <Button variant="contained">Login</Button>
      </div>
    </div>
  );
}
