import { createSlice } from "@reduxjs/toolkit";
import Cookies from "js-cookie";

const TOKEN_KEY = "accessToken";
const REFRESH_TOKEN_KEY = "accessToken";
const TOKEN_EXPIRED = 1 / 24; // day

export const authSlice = createSlice({
  name: "auth",
  initialState: {
    isLogin: false,
    user: { accessToken: null },
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
export const { login } = authSlice.actions;

export default authSlice.reducer;
