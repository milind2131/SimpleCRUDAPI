import { Link } from "react-router-dom";

export default function SuperAdminDashboard() {
  return (
    <div className="dashboard-role-page">
      <div className="dashboard-role-header">
        <span className="section-eyebrow">SUPERADMIN CONTROL</span>

        <h2>Platform Administration</h2>

        <p>
          Manage Cartora products, access requests and platform administration.
        </p>
      </div>

      <div className="dashboard-action-grid">
        <Link to="/products" className="dashboard-action-card">
          <div className="dashboard-action-icon">
            <i className="bi bi-box-seam-fill"></i>
          </div>

          <div>
            <span>CATALOGUE</span>

            <h4>All Products</h4>

            <p>View, edit and remove products across the platform.</p>
          </div>

          <i className="bi bi-arrow-up-right dashboard-action-arrow"></i>
        </Link>

        <Link to="/role-requests" className="dashboard-action-card">
          <div className="dashboard-action-icon">
            <i className="bi bi-person-check-fill"></i>
          </div>

          <div>
            <span>ACCESS CONTROL</span>

            <h4>Role Requests</h4>

            <p>Approve or reject Seller and Admin access requests.</p>
          </div>

          <i className="bi bi-arrow-up-right dashboard-action-arrow"></i>
        </Link>

        <Link to="/shop" className="dashboard-action-card">
          <div className="dashboard-action-icon">
            <i className="bi bi-shop"></i>
          </div>

          <div>
            <span>STOREFRONT</span>

            <h4>Customer Shop</h4>

            <p>Preview the shopping experience seen by customers.</p>
          </div>

          <i className="bi bi-arrow-up-right dashboard-action-arrow"></i>
        </Link>
      </div>
    </div>
  );
}
