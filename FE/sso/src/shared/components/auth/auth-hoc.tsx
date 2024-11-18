import { redirect } from "next/navigation";
import React from "react";

import APP_ENDPOINTS from "@/shared/constants/endpoints";
import { useAppSelector } from "@/lib/hooks/store";

type AuthProps = {
  isLoggedIn: boolean;
};

function withAuth<T extends AuthProps>(Component: React.ComponentType<T>) {
  const Auth = (props: T) => {
    const isAuth = useAppSelector((state) => state.auth.isLogin);

    if (!isAuth) redirect(APP_ENDPOINTS.LOGIN);

    // If user is logged in, return original component
    return <Component {...props} />;
  };

  return Auth;
}

export default withAuth;
