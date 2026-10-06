import { useState } from "react";

import { Link, useNavigate } from "react-router-dom";

import toast from "react-hot-toast";

import useAuth from "../hooks/useAuth";

import { getApiErrorMessage } from "../utils/errorHelper";

export default function LoginPage() {
  const navigate = useNavigate();

  const { login } = useAuth();

  const [form, setForm] = useState({
    email: "",
    password: "",
  });

  const [loading, setLoading] = useState(false);

  const handleChange = (e) => {
    setForm({
      ...form,
      [e.target.name]: e.target.value,
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    console.log("1. Login button clicked");
    console.log("Login form:", form);

    try {
      setLoading(true);

      await login(form.email, form.password);

      toast.success("Login successful");
      navigate("/dashboard");
    } catch (error) {
      console.error("LoginPage error:", error);
      toast.error(getApiErrorMessage(error));
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="auth-page">
      <div className="auth-card">
        <div className="text-center mb-4">
          <div className="brand-icon">
            <i className="bi bi-bag-check-fill"></i>
          </div>

          <h2 className="fw-bold mt-3">Sign in to Cartora</h2>

          <p className="text-muted">
            Shop, sell and manage everything from one place.
          </p>
        </div>

        <form onSubmit={handleSubmit}>
          <div className="mb-3">
            <label className="form-label">Email Address</label>

            <input
              type="email"
              name="email"
              className="form-control form-control-lg"
              value={form.email}
              onChange={handleChange}
              placeholder="name@example.com"
              required
            />
          </div>

          <div className="mb-3">
            <label className="form-label">Password</label>

            <input
              type="password"
              name="password"
              className="form-control form-control-lg"
              value={form.password}
              onChange={handleChange}
              placeholder="Enter password"
              required
            />
          </div>

          <div className="text-end mb-3">
            <Link to="/forgot-password" className="text-decoration-none">
              Forgot password?
            </Link>
          </div>

          <button className="btn btn-primary btn-lg w-100" disabled={loading}>
            {loading ? "Signing in..." : "Sign In"}
          </button>
        </form>

        <p className="text-center mt-4 mb-0">
          Don't have an account?{" "}
          <Link to="/register" className="fw-semibold text-decoration-none">
            Register
          </Link>
        </p>
      </div>
    </div>
  );
}
