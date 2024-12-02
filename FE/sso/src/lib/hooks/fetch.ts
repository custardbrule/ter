import { useAppSelector } from "./store";

const useAppFetch = () => {
  const { isLogin, user } = useAppSelector((state) => state.auth) as any;

  const handleFetch = (
    url: string,
    method: "GET" | "POST" | "PUT" | "DELETE" | "PATCH",
    headers: HeadersInit,
    body: BodyInit
  ) => {
    return fetch(url, {
      method,
      headers: {
        ...(isLogin && { Authorization: `Bearer ${user.accessToken}` }),
        ...headers,
      },
      body,
    });
  };

  return handleFetch;
};

export default useAppFetch;
