import { Navigate, Route, Routes } from "react-router-dom";

import AppLayout from "./components/AppLayout";
import ProtectedRoute from "./components/ProtectedRoute";
import RoleRoute from "./components/RoleRoute";

import LoginPage from "./pages/LoginPage";
import RegisterPage from "./pages/RegisterPage";
import VerifyOtpPage from "./pages/VerifyOtpPage";
import ForgotPasswordPage from "./pages/ForgotPasswordPage";
import ResetPasswordPage from "./pages/ResetPasswordPage";
import DashboardPage from "./pages/DashboardPage";
import ProductsPage from "./pages/ProductsPage";
import CustomerProductsView from "./pages/products/CustomerProductsView";
import ProductDetailsPage from "./pages/products/ProductDetailsPage";
import ChangePasswordPage from "./pages/ChangePasswordPage";
import RoleAccessRequestPage from "./pages/RoleAccessRequestPage";
import RoleRequestsPage from "./pages/RoleRequestsPage";

import { ROLES } from "./Constants/roles";

export default function App() {
  return (
    <Routes>
      <Route path="/" element={<Navigate to="/login" replace />} />

      <Route path="/login" element={<LoginPage />} />

      <Route path="/register" element={<RegisterPage />} />

      <Route path="/verify-otp" element={<VerifyOtpPage />} />

      <Route path="/forgot-password" element={<ForgotPasswordPage />} />

      <Route path="/reset-password" element={<ResetPasswordPage />} />

      <Route element={<ProtectedRoute />}>
        <Route element={<AppLayout />}>
          <Route path="/dashboard" element={<DashboardPage />} />

          <Route path="/shop" element={<CustomerProductsView />} />

          <Route path="/products/:id" element={<ProductDetailsPage />} />

          <Route path="/change-password" element={<ChangePasswordPage />} />

          <Route
            element={
              <RoleRoute
                allowedRoles={[ROLES.SELLER, ROLES.ADMIN, ROLES.SUPER_ADMIN]}
              />
            }
          >
            <Route path="/products" element={<ProductsPage />} />
          </Route>

          <Route element={<RoleRoute allowedRoles={[ROLES.CUSTOMER]} />}>
            <Route path="/request-access" element={<RoleAccessRequestPage />} />
          </Route>

          <Route element={<RoleRoute allowedRoles={[ROLES.SUPER_ADMIN]} />}>
            <Route path="/role-requests" element={<RoleRequestsPage />} />
          </Route>
        </Route>
      </Route>

      <Route path="*" element={<Navigate to="/dashboard" replace />} />
    </Routes>
  );
}
