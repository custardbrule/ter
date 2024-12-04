import { createSlice } from "@reduxjs/toolkit";
import Cookies from "js-cookie";

import API_ENDPOINTS from "@/shared/constants/api";

const TOKEN_KEY = "accessToken";
const REFRESH_TOKEN_KEY = "accessToken";
const TOKEN_EXPIRED = 1 / 24; // day

const resfresh = async () => {
  const refreshToken = Cookies.get(REFRESH_TOKEN_KEY);
  const accessToken = Cookies.get(TOKEN_KEY);

  const response = await fetch(API_ENDPOINTS.REFRESH, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify({ refreshToken, accessToken }),
  }).then((res) => res.json());

  return response;
};

export const authSlice = createSlice({
  name: "auth",
  initialState: {
    isLogin: false,
    user: { accessToken: null, refreshToken: null },
  },
  reducers: {
    login: (state, action) => {
      Cookies.set(TOKEN_KEY, action.payload.accessToken, {
        expires: TOKEN_EXPIRED, // day
        path: "",
      });

      Cookies.set(REFRESH_TOKEN_KEY, action.payload.refreshToken, {
        expires: TOKEN_EXPIRED, // day
        path: "",
      });

      state.isLogin = true;
      state.user = action.payload;
    },
  },
});

// Action creators are generated for each case reducer function
const { login } = authSlice.actions;
export { login, resfresh };

export default authSlice.reducer;
