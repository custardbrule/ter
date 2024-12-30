import { useAppSelector } from "./store";

const useAppFetch = () => {
  const { isLogin, user } = useAppSelector((state) => state.auth) as any;

  const handleFetch = (url: string | URL | Request, init: RequestInit) => {
    return fetch(url, {
      ...init,
      headers: {
        ...(isLogin && { Authorization: `Bearer ${user.accessToken}` }),
        ...init.headers,
      },
    });
  };

  return handleFetch;
};

export default useAppFetch;
