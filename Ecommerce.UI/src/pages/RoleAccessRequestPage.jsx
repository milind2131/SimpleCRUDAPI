import { useEffect, useState } from "react";
import toast from "react-hot-toast";
import Swal from "sweetalert2";
import { roleUpgradeService } from "../services/roleUpgradeService";
import { getApiErrorMessage } from "../utils/errorHelper";
import "../styles/roleAccess.css";

export default function RoleAccessRequestPage() {
  const [currentRequest, setCurrentRequest] = useState(null);

  const [loading, setLoading] = useState(true);

  const [submittingRole, setSubmittingRole] = useState("");

  useEffect(() => {
    let cancelled = false;

    roleUpgradeService
      .getMine()
      .then((response) => {
        if (!cancelled) {
          setCurrentRequest(response.data);
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

  const requestRole = async (requestedRole) => {
    if (currentRequest?.status === "Pending") {
      toast.error("You already have a pending role request.");
      return;
    }

    const result = await Swal.fire({
      title: `Request ${requestedRole} access?`,
      text:
        requestedRole === "Seller"
          ? "Seller access allows you to create and manage your own products."
          : "Admin access provides additional application management capabilities.",
      icon: "question",
      showCancelButton: true,
      confirmButtonText: "Submit Request",
      cancelButtonText: "Cancel",
      reverseButtons: true,
    });

    if (!result.isConfirmed) {
      return;
    }

    try {
      setSubmittingRole(requestedRole);

      const response = await roleUpgradeService.create(requestedRole);

      toast.success(response.data.message);

      const latest = await roleUpgradeService.getMine();

      setCurrentRequest(latest.data);
    } catch (error) {
      toast.error(getApiErrorMessage(error));
    } finally {
      setSubmittingRole("");
    }
  };

  if (loading) {
    return (
      <div className="role-page-loading">
        <div className="spinner-border"></div>
        <span>Loading access information...</span>
      </div>
    );
  }

  return (
    <div className="role-access-page">
      <section className="role-access-hero">
        <span>ACCOUNT ACCESS</span>

        <h1>Grow your Cartora account</h1>

        <p>
          Your account currently has Customer access. Request Seller or Admin
          privileges and a SuperAdmin will review your application.
        </p>
      </section>

      {currentRequest && (
        <section
          className={`current-role-request status-${currentRequest.status.toLowerCase()}`}
        >
          <div className="current-request-icon">
            {currentRequest.status === "Pending" && (
              <i className="bi bi-hourglass-split"></i>
            )}

            {currentRequest.status === "Approved" && (
              <i className="bi bi-check-circle-fill"></i>
            )}

            {currentRequest.status === "Rejected" && (
              <i className="bi bi-x-circle-fill"></i>
            )}
          </div>

          <div className="current-request-content">
            <span>LATEST REQUEST</span>

            <h4>{currentRequest.requestedRole} Access</h4>

            <p>
              Status: <strong>{currentRequest.status}</strong>
            </p>

            {currentRequest.reviewRemarks && (
              <small>Review note: {currentRequest.reviewRemarks}</small>
            )}
          </div>

          <span className="current-request-status">
            {currentRequest.status}
          </span>
        </section>
      )}

      <div className="role-option-grid">
        <article className="role-option-card seller-option">
          <div className="role-card-icon">
            <i className="bi bi-shop"></i>
          </div>

          <span className="role-card-label">SELLER ACCOUNT</span>

          <h3>Start selling products</h3>

          <p>
            Create products and manage the catalogue items that belong to your
            account.
          </p>

          <div className="role-feature-list">
            <div>
              <i className="bi bi-check-circle-fill"></i>
              Add your own products
            </div>

            <div>
              <i className="bi bi-check-circle-fill"></i>
              Edit your own products
            </div>

            <div>
              <i className="bi bi-check-circle-fill"></i>
              Manage your inventory
            </div>

            <div>
              <i className="bi bi-check-circle-fill"></i>
              Upload product images
            </div>
          </div>

          <button
            type="button"
            onClick={() => requestRole("Seller")}
            disabled={
              currentRequest?.status === "Pending" || submittingRole !== ""
            }
          >
            {submittingRole === "Seller" ? (
              <>
                <span className="spinner-border spinner-border-sm"></span>
                Submitting...
              </>
            ) : (
              <>
                <i className="bi bi-send"></i>
                Request Seller Access
              </>
            )}
          </button>
        </article>

        <article className="role-option-card admin-option">
          <div className="role-card-icon">
            <i className="bi bi-shield-check"></i>
          </div>

          <span className="role-card-label">ADMIN ACCOUNT</span>

          <h3>Request administrative access</h3>

          <p>
            Apply for additional application management privileges controlled by
            the SuperAdmin.
          </p>

          <div className="role-feature-list">
            <div>
              <i className="bi bi-check-circle-fill"></i>
              Manage your own products
            </div>

            <div>
              <i className="bi bi-check-circle-fill"></i>
              Administrative dashboard
            </div>

            <div>
              <i className="bi bi-check-circle-fill"></i>
              Future platform tools
            </div>

            <div>
              <i className="bi bi-lock-fill"></i>
              Requires SuperAdmin approval
            </div>
          </div>

          <button
            type="button"
            onClick={() => requestRole("Admin")}
            disabled={
              currentRequest?.status === "Pending" || submittingRole !== ""
            }
          >
            {submittingRole === "Admin" ? (
              <>
                <span className="spinner-border spinner-border-sm"></span>
                Submitting...
              </>
            ) : (
              <>
                <i className="bi bi-send"></i>
                Request Admin Access
              </>
            )}
          </button>
        </article>
      </div>

      <div className="role-access-note">
        <i className="bi bi-info-circle-fill"></i>

        <div>
          <strong>What happens after approval?</strong>

          <span>
            Log out and sign in again after your request is approved. Your new
            JWT will contain the updated role.
          </span>
        </div>
      </div>
    </div>
  );
}
