import { useState } from "react";

import AuthContext from "./AuthContext";

import { authService } from "../services/authService";
import { tokenStorage } from "../utils/tokenStorage";

export default function AuthProvider({ children }) {
  const [user, setUser] = useState(tokenStorage.getUser());

  const login = async (email, password) => {
    const response = await authService.login({
      email,
      password,
    });

    tokenStorage.setAuth(response.data);

    setUser(tokenStorage.getUser());

    return response.data;
  };

  const logout = async () => {
    const refreshToken = tokenStorage.getRefreshToken();

    try {
      if (refreshToken) {
        await authService.logout(refreshToken);
      }
    } finally {
      tokenStorage.clear();
      setUser(null);
    }
  };

  const logoutAll = async () => {
    try {
      await authService.logoutAll();
    } finally {
      tokenStorage.clear();
      setUser(null);
    }
  };

  return (
    <AuthContext.Provider
      value={{
        user,
        login,
        logout,
        logoutAll,
        isAuthenticated: !!user,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
}
