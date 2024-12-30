const API = process.env.NEXT_PUBLIC_API_HOST;

const API_ENDPOINTS = {
  LOGIN: `${API}/authenticate/login`,
  REFRESH: `${API}/authenticate/refresh`,
  REGISTER: `${API}/authenticate/REGISTER`,
  CONNECT_ACCEPT: `${API}/connect/authorize/accept`,
  CONNECT_AUTHORIZE: `${API}/connect/authorize`,
  T1: `${API}/Authenticate/Test1`,
  T2: `${API}/Authenticate/Test2`,
};

export default API_ENDPOINTS;
