import { useAppSelector } from "@/lib/hooks/store";
import { redirect } from "next/navigation";

export default function Home() {
  const isAuth = useAppSelector((state) => state.auth.isLogin);

  if (!isAuth) redirect("/login");

  return (
    <div className="flex justify-center items-center h-screen">
      Welcome, you have logged in.
    </div>
  );
}
