import { Navigate, Outlet } from "react-router-dom";
import useAuth from "../hooks/useAuth";

export default function RoleRoute({ allowedRoles }) {
  const { user } = useAuth();

  if (!user || !allowedRoles.includes(user.roleName)) {
    return <Navigate to="/dashboard" replace />;
  }

  return <Outlet />;
}
