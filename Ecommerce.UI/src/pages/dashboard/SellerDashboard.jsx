import { Link } from "react-router-dom";

export default function SellerDashboard() {
  return (
    <>
      <section className="seller-hero">
        <div className="hero-content">
          <span className="hero-label">SELLER CENTER</span>

          <h1>Build and manage your catalogue.</h1>

          <p>
            Add new products, maintain accurate prices and keep your catalogue
            ready for customers.
          </p>

          <div className="hero-actions">
            <Link to="/products" className="btn hero-primary-button">
              Manage Products
              <i className="bi bi-arrow-right"></i>
            </Link>
          </div>
        </div>

        <div className="seller-hero-visual">
          <div className="dashboard-hero-icon">
            <i className="bi bi-shop-window"></i>
          </div>
        </div>
      </section>

      <section className="dashboard-section">
        <div className="section-heading">
          <div>
            <span className="section-eyebrow">CATALOGUE</span>
            <h3>Seller workspace</h3>
          </div>
        </div>

        <div className="management-grid">
          <Link to="/products" className="management-card">
            <div className="management-card-header">
              <div className="management-icon">
                <i className="bi bi-plus-circle-fill"></i>
              </div>

              <i className="bi bi-arrow-up-right management-arrow"></i>
            </div>

            <h4>Add Products</h4>

            <p>
              Add products to your catalogue with category and pricing details.
            </p>
          </Link>

          <Link to="/products" className="management-card">
            <div className="management-card-header">
              <div className="management-icon">
                <i className="bi bi-pencil-square"></i>
              </div>

              <i className="bi bi-arrow-up-right management-arrow"></i>
            </div>

            <h4>Update Catalogue</h4>

            <p>
              Keep product names, categories and prices accurate and current.
            </p>
          </Link>

          <div className="management-card muted-management-card">
            <div className="management-card-header">
              <div className="management-icon">
                <i className="bi bi-graph-up-arrow"></i>
              </div>

              <span className="feature-tag">Coming Soon</span>
            </div>

            <h4>Sales Analytics</h4>

            <p>View future sales performance, orders and product activity.</p>
          </div>
        </div>
      </section>
    </>
  );
}
