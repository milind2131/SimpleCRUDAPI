import axiosClient from "../api/axiosClient";

export const authService = {
  register(data) {
    return axiosClient.post(
      "/Auth/register",
      data
    );
  },

  login(data) {
    return axiosClient.post(
      "/Auth/login",
      data
    );
  },

  verifyOtp(data) {
    return axiosClient.post(
      "/Auth/verify-otp",
      data
    );
  },

  resendOtp(data) {
    return axiosClient.post(
      "/Auth/resend-otp",
      data
    );
  },

  forgotPassword(data) {
    return axiosClient.post(
      "/Auth/forgot-password",
      data
    );
  },

  resetPassword(data) {
    return axiosClient.post(
      "/Auth/reset-password",
      data
    );
  },

  changePassword(data) {
    return axiosClient.post(
      "/Auth/change-password",
      data
    );
  },

  logout(refreshToken) {
    return axiosClient.post(
      "/Auth/logout",
      {
        refreshToken,
      }
    );
  },

  logoutAll() {
    return axiosClient.post(
      "/Auth/logout-all"
    );
  },
};