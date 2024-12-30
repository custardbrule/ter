"use client";

import useAppFetch from "@/lib/hooks/fetch";
import withAuth from "@/shared/components/auth/auth-hoc";
import API_ENDPOINTS from "@/shared/constants/api";

function Home() {
  const fetcher = useAppFetch();

  const consent = () => {
    const urlencoded = new URLSearchParams({
      redirect_uri: "http://localhost:3000/callback/login/local",
      client_id: "sso",
      response_type: "code",
      code_challenge: "DLMAuJP-MflqBAP7YGvskLj78v_QiQ6Qy2Yxga8dMOo",
    });

    return fetcher(API_ENDPOINTS.CONNECT_ACCEPT, {
      method: "POST",
      headers: {
        "Content-Type": "application/x-www-form-urlencoded",
      },
      body: urlencoded,
    }).catch((e) => console.log(e));
  };

  const getToken = () => {
    const urlencoded = new URLSearchParams({
      redirect_uri: "http://localhost:3000/callback/login/local",
      client_id: "sso",
      client_secret: "901564A5-E7FE-42CB-B10D-61EF6A8F3654",
      response_type: "code",
      code_challenge: "DLMAuJP-MflqBAP7YGvskLj78v_QiQ6Qy2Yxga8dMOo",
      code_verifier: "FIVygcLaYUoj-yiebQbC0lD4TtwnNxvkXsfzFC-KX-g",
    });

    return fetcher(API_ENDPOINTS.CONNECT_AUTHORIZE, {
      method: "POST",
      headers: {
        "Content-Type": "application/x-www-form-urlencoded",
      },
      body: urlencoded,
      redirect: "manual",
    })
      .then((response) => {
        if (response.type === "opaqueredirect") {
          console.log("Redirect status:", response.status);
          console.log("Redirect location:", response.headers.get("location"));
        } else {
          console.log("Response status:", response.status);
          console.log("Response did not redirect:", response);
        }
      })
      .then(console.log)
      .catch((e) => console.log(e));
  };

  const t1 = () => {
    return fetcher(API_ENDPOINTS.T1, {
      method: "GET",
    })
      .then((res) => res.text())
      .then(console.log)
      .catch((e) => console.log(e));
  };

  const t2 = () => {
    return fetcher(API_ENDPOINTS.T2, {
      method: "GET",
    })
      .then((res) => res.text())
      .then(console.log)
      .catch((e) => console.log(e));
  };

  return (
    <div className="flex justify-center items-center h-screen">
      Welcome, you have logged in.
      <div className="flex gap-4">
        <button onClick={consent}>consent</button>
        <button onClick={getToken}>getToken</button>
        <button onClick={t1}>t1</button>
        <button onClick={t2}>t2</button>
      </div>
    </div>
  );
}

export default withAuth(Home);
