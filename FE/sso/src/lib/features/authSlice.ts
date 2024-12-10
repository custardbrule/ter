import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import Cookies from "js-cookie";

import API_ENDPOINTS from "@/shared/constants/api";
import COOKIES_CONSTANT from "@/shared/constants/cookies";

export const authSlice = createSlice({
  name: "auth",
  initialState: {
    isLogin: false,
    user: { accessToken: null, refreshToken: null },
  },
  reducers: {
    login: (state, action) => {
      Cookies.set(COOKIES_CONSTANT.TOKEN_KEY, action.payload.accessToken, {
        expires: COOKIES_CONSTANT.TOKEN_EXPIRED, // day
        path: "",
      });

      Cookies.set(
        COOKIES_CONSTANT.REFRESH_TOKEN_KEY,
        action.payload.refreshToken,
        {
          expires: COOKIES_CONSTANT.TOKEN_EXPIRED, // day
          path: "",
        }
      );

      state.isLogin = true;
      state.user = action.payload;
    },
  },
});

// Action creators are generated for each case reducer function
const { login } = authSlice.actions;
export { login };

export default authSlice.reducer;
