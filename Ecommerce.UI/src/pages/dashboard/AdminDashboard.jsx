import { Link } from "react-router-dom";

export default function AdminDashboard() {
  return (
    <>
      <section className="admin-hero">
        <div className="hero-content">
          <span className="hero-label">ADMINISTRATION</span>

          <h1>Control your commerce platform.</h1>

          <p>
            Manage products, application access and future platform operations
            from one central workspace.
          </p>

          <div className="hero-actions">
            <Link to="/products" className="btn hero-primary-button">
              Manage Catalogue
              <i className="bi bi-arrow-right"></i>
            </Link>
          </div>
        </div>

        <div className="admin-hero-visual">
          <div className="dashboard-hero-icon">
            <i className="bi bi-shield-lock-fill"></i>
          </div>
        </div>
      </section>

      <section className="dashboard-section">
        <div className="section-heading">
          <div>
            <span className="section-eyebrow">ADMIN TOOLS</span>
            <h3>Platform management</h3>
          </div>
        </div>

        <div className="management-grid">
          <Link to="/products" className="management-card">
            <div className="management-card-header">
              <div className="management-icon">
                <i className="bi bi-box-seam-fill"></i>
              </div>

              <i className="bi bi-arrow-up-right management-arrow"></i>
            </div>

            <h4>Product Management</h4>

            <p>Add, modify and remove products across the entire catalogue.</p>
          </Link>

          <div className="management-card muted-management-card">
            <div className="management-card-header">
              <div className="management-icon">
                <i className="bi bi-people-fill"></i>
              </div>

              <span className="feature-tag">Coming Soon</span>
            </div>

            <h4>User Management</h4>

            <p>
              Manage customers, sellers, account status and platform access.
            </p>
          </div>

          <div className="management-card muted-management-card">
            <div className="management-card-header">
              <div className="management-icon">
                <i className="bi bi-shield-check"></i>
              </div>

              <span className="feature-tag">Coming Soon</span>
            </div>

            <h4>Role Management</h4>

            <p>Control roles and permissions for administrators and sellers.</p>
          </div>

          <div className="management-card muted-management-card">
            <div className="management-card-header">
              <div className="management-icon">
                <i className="bi bi-journal-code"></i>
              </div>

              <span className="feature-tag">Coming Soon</span>
            </div>

            <h4>System Logs</h4>

            <p>
              Review application errors and diagnostic activity from your
              logging system.
            </p>
          </div>
        </div>
      </section>
    </>
  );
}
