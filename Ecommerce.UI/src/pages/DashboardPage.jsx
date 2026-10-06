import useAuth from "../hooks/useAuth";
import { ROLES } from "../Constants/roles";
import CustomerDashboard from "./dashboard/CustomerDashboard";
import SellerDashboard from "./dashboard/SellerDashboard";
import AdminDashboard from "./dashboard/AdminDashboard";
import SuperAdminDashboard from "./dashboard/SuperAdminDashboard";

export default function DashboardPage() {
  const { user } = useAuth();

  if (user?.roleName === ROLES.SUPER_ADMIN) {
    return <SuperAdminDashboard />;
  }

  if (user?.roleName === ROLES.ADMIN) {
    return <AdminDashboard />;
  }

  if (user?.roleName === ROLES.SELLER) {
    return <SellerDashboard />;
  }

  return <CustomerDashboard />;
}
