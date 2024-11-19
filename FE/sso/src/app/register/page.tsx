import { Metadata } from "next";
import RegisterForm from "./register-form";

export const metadata: Metadata = {
  title: "Register",
  description: "Register",
  openGraph: {
    title: "Register",
    description: "Register",
  },
};

export default function Page() {
  return (
    <div className="flex justify-center items-center h-screen">
      <RegisterForm />
    </div>
  );
}
