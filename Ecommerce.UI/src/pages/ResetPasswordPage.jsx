import { useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";

import toast from "react-hot-toast";

import { authService } from "../services/authService";
import { getApiErrorMessage } from "../utils/errorHelper";

export default function ResetPasswordPage() {
  const location = useLocation();
  const navigate = useNavigate();

  const email = location.state?.email;

  const [form, setForm] = useState({
    newPassword: "",
    confirmPassword: "",
  });

  const submit = async (e) => {
    e.preventDefault();

    try {
      const response = await authService.resetPassword({
        email,
        ...form,
      });

      toast.success(response.data.message);

      navigate("/login");
    } catch (error) {
      toast.error(getApiErrorMessage(error));
    }
  };

  return (
    <div className="auth-page">
      <div className="auth-card">
        <h3 className="fw-bold">Reset Password</h3>

        <form onSubmit={submit}>
          <input
            type="password"
            className="form-control mb-3"
            placeholder="New password"
            value={form.newPassword}
            onChange={(e) =>
              setForm({
                ...form,
                newPassword: e.target.value,
              })
            }
          />

          <input
            type="password"
            className="form-control mb-3"
            placeholder="Confirm password"
            value={form.confirmPassword}
            onChange={(e) =>
              setForm({
                ...form,
                confirmPassword: e.target.value,
              })
            }
          />

          <button className="btn btn-primary w-100">Reset Password</button>
        </form>
      </div>
    </div>
  );
}
