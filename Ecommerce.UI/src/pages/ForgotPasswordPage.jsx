import { useState } from "react";
import { useNavigate } from "react-router-dom";
import toast from "react-hot-toast";

import { authService } from "../services/authService";
import { getApiErrorMessage } from "../utils/errorHelper";

export default function ForgotPasswordPage() {
  const navigate = useNavigate();

  const [email, setEmail] = useState("");

  const submit = async (e) => {
    e.preventDefault();

    try {
      const response = await authService.forgotPassword({
        email,
      });

      toast.success(response.data.message);

      navigate("/verify-otp", {
        state: {
          email,
          purpose: 2,
        },
      });
    } catch (error) {
      toast.error(getApiErrorMessage(error));
    }
  };

  return (
    <div className="auth-page">
      <div className="auth-card">
        <h3 className="fw-bold">Forgot Password</h3>

        <p className="text-muted">Enter your registered email address.</p>

        <form onSubmit={submit}>
          <input
            type="email"
            className="form-control mb-3"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />

          <button className="btn btn-primary w-100">Send OTP</button>
        </form>
      </div>
    </div>
  );
}
