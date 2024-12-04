import { redirect } from "next/navigation";
import React from "react";

import APP_ENDPOINTS from "@/shared/constants/endpoints";
import { useAppSelector } from "@/lib/hooks/store";

function withOutAuth<T extends React.PropsWithChildren>(
  Component: React.ComponentType<T>
) {
  const NoAuth = (props: T) => {
    const isAuth = useAppSelector((state) => state.auth.isLogin);

    if (isAuth) redirect(APP_ENDPOINTS.HOME);

    // If user is logged in, return original component
    return <Component {...props} />;
  };

  return NoAuth;
}

export default withOutAuth;
