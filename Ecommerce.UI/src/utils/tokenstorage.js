const ACCESS_TOKEN = "accessToken";
const REFRESH_TOKEN = "refreshToken";
const USER = "user";

export const tokenStorage = {
  getAccessToken() {
    return localStorage.getItem(ACCESS_TOKEN);
  },

  getRefreshToken() {
    return localStorage.getItem(REFRESH_TOKEN);
  },

  getUser() {
    const user = localStorage.getItem(USER);

    return user ? JSON.parse(user) : null;
  },

  setAuth(loginResponse) {
    localStorage.setItem(
      ACCESS_TOKEN,
      loginResponse.token
    );

    localStorage.setItem(
      REFRESH_TOKEN,
      loginResponse.refreshToken
    );

    localStorage.setItem(
      USER,
      JSON.stringify({
       userId: loginResponse.userId,
    firstName: loginResponse.firstName,
    lastName: loginResponse.lastName,
    email: loginResponse.email,
    roleName: loginResponse.roleName,
      })
    );
  },

  updateTokens(accessToken, refreshToken) {
    localStorage.setItem(
      ACCESS_TOKEN,
      accessToken
    );

    localStorage.setItem(
      REFRESH_TOKEN,
      refreshToken
    );
  },

  clear() {
    localStorage.removeItem(ACCESS_TOKEN);
    localStorage.removeItem(REFRESH_TOKEN);
    localStorage.removeItem(USER);
  },
};