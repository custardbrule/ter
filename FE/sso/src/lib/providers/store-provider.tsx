"use client";

import { useEffect, useRef, useState } from "react";
import { Provider } from "react-redux";
import Cookies from "js-cookie";
import { LinearProgress } from "@mui/material";

import { AppStore, makeStore } from "@/lib/store";
import { useAppDispatch } from "@/lib/hooks/store";
import { login } from "@/lib/features/authSlice";
import COOKIES_CONSTANT from "@/shared/constants/cookies";
import API_ENDPOINTS from "@/shared/constants/api";

export default function StoreProvider({
  children,
}: {
  children: React.ReactNode;
}) {
  const storeRef = useRef<AppStore>();

  if (!storeRef.current) {
    // Create the store instance the first time this renders
    storeRef.current = makeStore();
  }

  function RefreshComp({ children }: { children: React.ReactNode }) {
    const [ready, setReady] = useState(false);
    const dispatch = useAppDispatch();

    const resfresh = async () => {
      const refreshToken = Cookies.get(COOKIES_CONSTANT.REFRESH_TOKEN_KEY);
      const accessToken = Cookies.get(COOKIES_CONSTANT.TOKEN_KEY);

      if (!refreshToken || !accessToken) return null;

      const response = await fetch(API_ENDPOINTS.REFRESH, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({ refresh: refreshToken, access: accessToken }),
      }).then((res) => res.json());

      return response;
    };

    useEffect(() => {
      resfresh()
        .then((res) => {
          if (res) dispatch(login(res));
        })
        .finally(() => setReady(true));
    }, []);

    return <>{ready ? children : <LinearProgress />}</>;
  }

  return (
    <Provider store={storeRef.current}>
      <RefreshComp>{children}</RefreshComp>
    </Provider>
  );
}
