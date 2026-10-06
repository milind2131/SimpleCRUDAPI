import { useNavigate } from "react-router-dom";

const apiOrigin = import.meta.env.VITE_API_ORIGIN;

export default function ProductCard({ product }) {
  const navigate = useNavigate();

  const getStockInfo = () => {
    if (product.stockQuantity <= 0) {
      return {
        text: "Out of Stock",
        className: "shop-stock-badge stock-out",
        icon: "bi-x-circle-fill",
      };
    }

    if (product.stockQuantity <= 10) {
      return {
        text: `Only ${product.stockQuantity} left`,
        className: "shop-stock-badge stock-low",
        icon: "bi-exclamation-circle-fill",
      };
    }

    return {
      text: "In Stock",
      className: "shop-stock-badge stock-available",
      icon: "bi-check-circle-fill",
    };
  };

  const stockInfo = getStockInfo();

  const openProductDetails = () => {
    navigate(`/products/${product.id}`);
  };

  return (
    <article className="shop-product-card">
      <div className="shop-product-image-section" onClick={openProductDetails}>
        {product.imageUrl ? (
          <img
            src={`${apiOrigin}${product.imageUrl}`}
            alt={product.name}
            className="shop-product-image"
          />
        ) : (
          <div className="shop-product-placeholder">
            <div className="shop-product-placeholder-icon">
              <i className="bi bi-image"></i>
            </div>

            <span>Image unavailable</span>
          </div>
        )}

        <span className={stockInfo.className}>
          <i className={`bi ${stockInfo.icon}`}></i>
          {stockInfo.text}
        </span>

        <button
          type="button"
          className="shop-product-quick-view"
          onClick={(e) => {
            e.stopPropagation();
            openProductDetails();
          }}
          title="View Product"
        >
          <i className="bi bi-eye"></i>
        </button>
      </div>

      <div className="shop-product-content">
        <div className="shop-product-category-row">
          <span className="shop-product-category">{product.category}</span>

          <span className="shop-product-id">#{product.id}</span>
        </div>

        <h3 className="shop-product-title" onClick={openProductDetails}>
          {product.name}
        </h3>

        <p className="shop-product-description">
          {product.description ||
            "Explore this product and view complete details."}
        </p>

        <div className="shop-product-divider"></div>

        <div className="shop-product-bottom">
          <div className="shop-product-price-section">
            <span className="shop-price-label">Price</span>

            <strong className="shop-product-price">
              ₹
              {Number(product.price).toLocaleString("en-IN", {
                minimumFractionDigits: 0,
                maximumFractionDigits: 2,
              })}
            </strong>
          </div>

          <button
            type="button"
            className="shop-details-button"
            onClick={openProductDetails}
          >
            Details
            <i className="bi bi-arrow-up-right"></i>
          </button>
        </div>
      </div>
    </article>
  );
}
