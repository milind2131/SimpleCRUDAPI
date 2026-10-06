import { useEffect, useState } from "react";
import toast from "react-hot-toast";
import Swal from "sweetalert2";
import { roleUpgradeService } from "../services/roleUpgradeService";
import { getApiErrorMessage } from "../utils/errorHelper";
import "../styles/roleAccess.css";

export default function RoleRequestsPage() {
  const [requests, setRequests] = useState([]);

  const [loading, setLoading] = useState(true);

  const loadRequests = async () => {
    try {
      setLoading(true);

      const response = await roleUpgradeService.getPending();

      setRequests(response.data);
    } catch (error) {
      toast.error(getApiErrorMessage(error));
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    let cancelled = false;

    roleUpgradeService
      .getPending()
      .then((response) => {
        if (!cancelled) {
          setRequests(response.data);
        }
      })
      .catch((error) => {
        if (!cancelled) {
          toast.error(getApiErrorMessage(error));
        }
      })
      .finally(() => {
        if (!cancelled) {
          setLoading(false);
        }
      });

    return () => {
      cancelled = true;
    };
  }, []);

  const approveRequest = async (request) => {
    const result = await Swal.fire({
      title: "Approve Access?",
      html: `
          <div style="text-align:left">
            <p><strong>User:</strong> ${request.email}</p>
            <p><strong>Requested Role:</strong> ${request.requestedRole}</p>
          </div>
        `,
      input: "textarea",
      inputLabel: "Approval remarks (optional)",
      inputPlaceholder: "Add a short approval note...",
      icon: "question",
      showCancelButton: true,
      confirmButtonText: "Approve",
      cancelButtonText: "Cancel",
      confirmButtonColor: "#16a34a",
      reverseButtons: true,
    });

    if (!result.isConfirmed) {
      return;
    }

    try {
      const response = await roleUpgradeService.approve(
        request.requestId,
        result.value ?? "",
      );

      toast.success(response.data.message);

      await loadRequests();
    } catch (error) {
      toast.error(getApiErrorMessage(error));
    }
  };

  const rejectRequest = async (request) => {
    const result = await Swal.fire({
      title: "Reject Access?",
      html: `
          <div style="text-align:left">
            <p><strong>User:</strong> ${request.email}</p>
            <p><strong>Requested Role:</strong> ${request.requestedRole}</p>
          </div>
        `,
      input: "textarea",
      inputLabel: "Reason for rejection",
      inputPlaceholder: "Enter rejection reason...",
      inputValidator: (value) => {
        if (!value?.trim()) {
          return "Please provide a rejection reason.";
        }

        return undefined;
      },
      icon: "warning",
      showCancelButton: true,
      confirmButtonText: "Reject",
      cancelButtonText: "Cancel",
      confirmButtonColor: "#dc2626",
      reverseButtons: true,
    });

    if (!result.isConfirmed) {
      return;
    }

    try {
      const response = await roleUpgradeService.reject(
        request.requestId,
        result.value.trim(),
      );

      toast.success(response.data.message);

      await loadRequests();
    } catch (error) {
      toast.error(getApiErrorMessage(error));
    }
  };

  return (
    <div className="role-admin-page">
      <div className="role-admin-header">
        <div>
          <span>SUPERADMIN CONTROL</span>

          <h2>Access Requests</h2>

          <p>Review customers requesting Seller or Admin privileges.</p>
        </div>

        <div className="pending-count-card">
          <strong>{requests.length}</strong>

          <span>Pending</span>
        </div>
      </div>

      <div className="role-request-table-card">
        {loading ? (
          <div className="role-page-loading">
            <div className="spinner-border"></div>

            <span>Loading requests...</span>
          </div>
        ) : requests.length === 0 ? (
          <div className="role-empty-state">
            <div>
              <i className="bi bi-check2-circle"></i>
            </div>

            <h4>You're all caught up</h4>

            <p>There are no pending role requests right now.</p>
          </div>
        ) : (
          <div className="table-responsive">
            <table className="role-request-table">
              <thead>
                <tr>
                  <th>Request</th>
                  <th>User</th>
                  <th>Requested Role</th>
                  <th>Date</th>
                  <th>Status</th>
                  <th className="text-end">Actions</th>
                </tr>
              </thead>

              <tbody>
                {requests.map((request) => (
                  <tr key={request.requestId}>
                    <td>#{request.requestId}</td>

                    <td>
                      <div className="request-user">
                        <div>{request.email.charAt(0).toUpperCase()}</div>

                        <span>{request.email}</span>
                      </div>
                    </td>

                    <td>
                      <span
                        className={`requested-role-badge ${request.requestedRole.toLowerCase()}`}
                      >
                        {request.requestedRole}
                      </span>
                    </td>

                    <td>
                      {new Date(request.requestedOn).toLocaleDateString(
                        "en-IN",
                      )}
                    </td>

                    <td>
                      <span className="request-pending-badge">
                        <i className="bi bi-clock-fill"></i>
                        Pending
                      </span>
                    </td>

                    <td>
                      <div className="request-actions">
                        <button
                          type="button"
                          className="approve-request"
                          onClick={() => approveRequest(request)}
                        >
                          <i className="bi bi-check-lg"></i>
                          Approve
                        </button>

                        <button
                          type="button"
                          className="reject-request"
                          onClick={() => rejectRequest(request)}
                        >
                          <i className="bi bi-x-lg"></i>
                          Reject
                        </button>
                      </div>
                    </td>
                  </tr>
                ))}
              </tbody>
            </table>
          </div>
        )}
      </div>
    </div>
  );
}
