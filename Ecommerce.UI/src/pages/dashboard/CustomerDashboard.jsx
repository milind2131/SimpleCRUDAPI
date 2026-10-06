import { Link } from "react-router-dom";
import "../../styles/customerDashboard.css";

export default function CustomerDashboard() {
  return (
    <div className="customer-dashboard">
      <section className="customer-dashboard-hero">
        <div className="customer-dashboard-hero-content">
          <span className="customer-dashboard-eyebrow">
            <i className="bi bi-stars"></i>
            CARTORA SHOPPING
          </span>

          <h1>
            Discover products
            <span> you'll love.</span>
          </h1>

          <p>
            Explore carefully selected products across multiple categories,
            compare details and discover something useful for your lifestyle.
          </p>

          <div className="customer-hero-actions">
            <Link to="/shop" className="customer-shop-now-button">
              <span>
                <i className="bi bi-bag-check"></i>
                Shop Now
              </span>

              <span className="customer-shop-button-icon">
                <i className="bi bi-arrow-right"></i>
              </span>
            </Link>

            <Link to="/request-access" className="customer-seller-button">
              <i className="bi bi-shop"></i>
              Become a Seller
            </Link>
          </div>

          <div className="customer-hero-benefits">
            <div>
              <span className="customer-benefit-icon">
                <i className="bi bi-grid"></i>
              </span>

              <span>
                <strong>Multiple</strong>
                Categories
              </span>
            </div>

            <div>
              <span className="customer-benefit-icon">
                <i className="bi bi-box-seam"></i>
              </span>

              <span>
                <strong>Live</strong>
                Inventory
              </span>
            </div>

            <div>
              <span className="customer-benefit-icon">
                <i className="bi bi-shield-check"></i>
              </span>

              <span>
                <strong>Secure</strong>
                Experience
              </span>
            </div>
          </div>
        </div>

        <div className="customer-dashboard-hero-visual">
          <div className="customer-hero-main-icon">
            <i className="bi bi-bag-heart-fill"></i>
          </div>

          <div className="customer-floating-card customer-floating-top">
            <span className="customer-floating-icon">
              <i className="bi bi-bag-check-fill"></i>
            </span>

            <div>
              <small>Explore</small>
              <strong>New Products</strong>
            </div>
          </div>

          <div className="customer-floating-card customer-floating-bottom">
            <span className="customer-floating-icon">
              <i className="bi bi-tags-fill"></i>
            </span>

            <div>
              <small>Browse</small>
              <strong>Categories</strong>
            </div>
          </div>

          <div className="customer-hero-circle customer-circle-one"></div>
          <div className="customer-hero-circle customer-circle-two"></div>
        </div>
      </section>

      <section className="customer-dashboard-section">
        <div className="customer-dashboard-section-header">
          <div>
            <span className="customer-section-eyebrow">YOUR CARTORA</span>

            <h2>Everything you need in one place</h2>

            <p>
              Shop products, save favourites and manage your Cartora account.
            </p>
          </div>

          <Link to="/shop" className="customer-view-store-link">
            View Store
            <i className="bi bi-arrow-right"></i>
          </Link>
        </div>

        <div className="customer-dashboard-grid">
          <Link
            to="/shop"
            className="customer-dashboard-card customer-card-active"
          >
            <div className="customer-card-top">
              <div className="customer-card-icon customer-icon-products">
                <i className="bi bi-grid-fill"></i>
              </div>

              <span className="customer-card-arrow">
                <i className="bi bi-arrow-up-right"></i>
              </span>
            </div>

            <div className="customer-card-content">
              <span className="customer-card-label">SHOPPING</span>

              <h3>Browse Products</h3>

              <p>
                Explore the complete Cartora catalogue across all available
                categories.
              </p>
            </div>

            <div className="customer-card-footer">
              <span>Start Shopping</span>
              <i className="bi bi-arrow-right"></i>
            </div>
          </Link>

          <div className="customer-dashboard-card customer-card-disabled">
            <div className="customer-card-top">
              <div className="customer-card-icon customer-icon-wishlist">
                <i className="bi bi-heart-fill"></i>
              </div>

              <span className="customer-coming-badge">Coming Soon</span>
            </div>

            <div className="customer-card-content">
              <span className="customer-card-label">SAVED ITEMS</span>

              <h3>Wishlist</h3>

              <p>
                Keep products you love in one place and access them whenever
                you're ready to purchase.
              </p>
            </div>

            <div className="customer-disabled-footer">
              <i className="bi bi-clock"></i>
              Planned Feature
            </div>
          </div>

          <div className="customer-dashboard-card customer-card-disabled">
            <div className="customer-card-top">
              <div className="customer-card-icon customer-icon-orders">
                <i className="bi bi-bag-check-fill"></i>
              </div>

              <span className="customer-coming-badge">Coming Soon</span>
            </div>

            <div className="customer-card-content">
              <span className="customer-card-label">PURCHASES</span>

              <h3>My Orders</h3>

              <p>
                Track purchases, review order history and follow delivery status
                from one place.
              </p>
            </div>

            <div className="customer-disabled-footer">
              <i className="bi bi-clock"></i>
              Planned Feature
            </div>
          </div>

          <Link
            to="/request-access"
            className="customer-dashboard-card customer-card-active customer-seller-card"
          >
            <div className="customer-card-top">
              <div className="customer-card-icon customer-icon-seller">
                <i className="bi bi-shop"></i>
              </div>

              <span className="customer-card-arrow">
                <i className="bi bi-arrow-up-right"></i>
              </span>
            </div>

            <div className="customer-card-content">
              <span className="customer-card-label">GROW WITH CARTORA</span>

              <h3>Become a Seller</h3>

              <p>
                Request Seller or Admin access and get approval from the Cartora
                SuperAdmin.
              </p>
            </div>

            <div className="customer-card-footer">
              <span>Request Access</span>
              <i className="bi bi-arrow-right"></i>
            </div>
          </Link>
        </div>
      </section>

      <section className="customer-dashboard-promo">
        <div className="customer-promo-icon">
          <i className="bi bi-lightning-charge-fill"></i>
        </div>

        <div className="customer-promo-content">
          <span>CARTORA TIP</span>

          <h4>Explore products before they run out of stock.</h4>

          <p>
            Product availability is displayed directly in the storefront so you
            can quickly identify low-stock items.
          </p>
        </div>

        <Link to="/shop" className="customer-promo-button">
          Browse Store
          <i className="bi bi-arrow-right"></i>
        </Link>
      </section>
    </div>
  );
}
