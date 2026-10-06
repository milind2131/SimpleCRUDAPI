import { useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import toast from "react-hot-toast";

import { authService } from "../services/authService";
import { getApiErrorMessage } from "../utils/errorHelper";

export default function VerifyOtpPage() {
  const location = useLocation();
  const navigate = useNavigate();

  const email = location.state?.email;

  const purpose = location.state?.purpose;

  const [otp, setOtp] = useState("");

  if (!email || !purpose) {
    return <div className="auth-page">Invalid OTP request.</div>;
  }

  const verifyOtp = async (e) => {
    e.preventDefault();

    try {
      const response = await authService.verifyOtp({
        email,
        otp,
        purpose,
      });

      toast.success(response.data.message);

      if (purpose === 1) {
        navigate("/login");
      } else {
        navigate("/reset-password", {
          state: { email },
        });
      }
    } catch (error) {
      toast.error(getApiErrorMessage(error));
    }
  };

  const resendOtp = async () => {
    try {
      const response = await authService.resendOtp({
        email,
        purpose,
      });

      toast.success(response.data.message);
    } catch (error) {
      toast.error(getApiErrorMessage(error));
    }
  };

  return (
    <div className="auth-page">
      <div className="auth-card text-center">
        <div className="brand-icon">
          <i className="bi bi-shield-check"></i>
        </div>

        <h3 className="fw-bold mt-3">Verify OTP</h3>

        <p className="text-muted">
          We sent a 6-digit OTP to
          <br />
          <strong>{email}</strong>
        </p>

        <form onSubmit={verifyOtp}>
          <input
            className="form-control form-control-lg text-center otp-input mb-3"
            maxLength="6"
            value={otp}
            onChange={(e) => setOtp(e.target.value)}
            placeholder="000000"
            required
          />

          <button className="btn btn-primary w-100">Verify OTP</button>
        </form>

        <button type="button" className="btn btn-link mt-3" onClick={resendOtp}>
          Resend OTP
        </button>
      </div>
    </div>
  );
}
