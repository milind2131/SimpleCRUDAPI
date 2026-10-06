import axios from "axios";
import { tokenStorage } from "../utils/tokenStorage";

const baseURL =
  import.meta.env.VITE_API_BASE_URL;


console.log("API Base URL:", baseURL);

const axiosClient = axios.create({
  baseURL,
});

axiosClient.interceptors.request.use(
  (config) => {
    const token =
      tokenStorage.getAccessToken();

    if (token) {
      config.headers.Authorization =
        `Bearer ${token}`;
    }

    return config;
  },
  (error) => Promise.reject(error)
);

axiosClient.interceptors.response.use(
  (response) => response,

  async (error) => {
    const originalRequest = error.config;

    if (
      error.response?.status === 401 &&
      !originalRequest._retry &&
      !originalRequest.url?.includes(
        "/Auth/refresh-token"
      )
    ) {
      originalRequest._retry = true;

      const refreshToken =
        tokenStorage.getRefreshToken();

      if (!refreshToken) {
        tokenStorage.clear();

        window.location.href = "/login";

        return Promise.reject(error);
      }

      try {
        const response = await axios.post(
          `${baseURL}/Auth/refresh-token`,
          {
            refreshToken,
          }
        );

        const newAccessToken =
          response.data.accessToken;

        const newRefreshToken =
          response.data.refreshToken;

        tokenStorage.updateTokens(
          newAccessToken,
          newRefreshToken
        );

        originalRequest.headers.Authorization =
          `Bearer ${newAccessToken}`;

        return axiosClient(originalRequest);
      } catch (refreshError) {
        tokenStorage.clear();

        window.location.href = "/login";

        return Promise.reject(
          refreshError
        );
      }
    }

    return Promise.reject(error);
  }
);

export default axiosClient;