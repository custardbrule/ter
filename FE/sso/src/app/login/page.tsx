import { Metadata } from "next";
import LoginForm from "./login-form";

export const metadata: Metadata = {
  title: "Login",
  description: "Login",
  openGraph: {
    title: "Login",
    description: "Login",
  },
};

export default function Page() {
  return (
    <div className="flex justify-center items-center h-screen">
      <LoginForm />
    </div>
  );
}
