"use client";

import withAuth from "@/shared/components/auth/auth-hoc";

function Home() {
  return (
    <div className="flex justify-center items-center h-screen">
      Welcome, you have logged in.
    </div>
  );
}

export default withAuth(Home);
