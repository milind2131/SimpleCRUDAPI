import { useState } from "react";
import toast from "react-hot-toast";

import { authService } from "../services/authService";
import { getApiErrorMessage } from "../utils/errorHelper";

export default function ChangePasswordPage() {
  const [form, setForm] = useState({
    currentPassword: "",
    newPassword: "",
    confirmPassword: "",
  });

  const submit = async (e) => {
    e.preventDefault();

    try {
      const response = await authService.changePassword(form);

      toast.success(response.data.message);

      setForm({
        currentPassword: "",
        newPassword: "",
        confirmPassword: "",
      });
    } catch (error) {
      toast.error(getApiErrorMessage(error));
    }
  };

  return (
    <>
      <h2 className="fw-bold">Change Password</h2>

      <p className="text-muted">Update your account password securely.</p>

      <div
        className="content-card mt-4"
        style={{
          maxWidth: "600px",
        }}
      >
        <form onSubmit={submit}>
          <div className="mb-3">
            <label className="form-label">Current Password</label>

            <input
              type="password"
              className="form-control"
              value={form.currentPassword}
              onChange={(e) =>
                setForm({
                  ...form,
                  currentPassword: e.target.value,
                })
              }
              required
            />
          </div>

          <div className="mb-3">
            <label className="form-label">New Password</label>

            <input
              type="password"
              className="form-control"
              value={form.newPassword}
              onChange={(e) =>
                setForm({
                  ...form,
                  newPassword: e.target.value,
                })
              }
              required
            />
          </div>

          <div className="mb-4">
            <label className="form-label">Confirm Password</label>

            <input
              type="password"
              className="form-control"
              value={form.confirmPassword}
              onChange={(e) =>
                setForm({
                  ...form,
                  confirmPassword: e.target.value,
                })
              }
              required
            />
          </div>

          <button className="btn btn-primary">Change Password</button>
        </form>
      </div>
    </>
  );
}
