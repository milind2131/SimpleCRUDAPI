import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import toast from "react-hot-toast";
import { productService } from "../../services/productService";
import { getApiErrorMessage } from "../../utils/errorHelper";
import "../../styles/productDetails.css";

const apiOrigin = import.meta.env.VITE_API_ORIGIN;

export default function ProductDetailsPage() {
  const { id } = useParams();
  const navigate = useNavigate();

  const [product, setProduct] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    let cancelled = false;

    productService
      .getById(id)
      .then((response) => {
        if (!cancelled) {
          setProduct(response.data);
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
  }, [id]);

  if (loading) {
    return (
      <div className="pd-loading">
        <div className="pd-loading-spinner">
          <span></span>
          <span></span>
          <span></span>
        </div>

        <strong>Loading product details...</strong>
        <p>Please wait while we prepare the product information.</p>
      </div>
    );
  }

  if (!product) {
    return (
      <div className="pd-not-found">
        <div className="pd-not-found-icon">
          <i className="bi bi-box-seam"></i>
        </div>

        <span>PRODUCT NOT FOUND</span>

        <h2>We couldn't find this product.</h2>

        <p>The product may have been removed or is no longer available.</p>

        <button type="button" onClick={() => navigate("/shop")}>
          <i className="bi bi-arrow-left"></i>
          Back to Products
        </button>
      </div>
    );
  }

  const isOutOfStock = product.stockQuantity <= 0;
  const isLowStock = product.stockQuantity > 0 && product.stockQuantity <= 10;

  return (
    <div className="pd-page">
      <button
        type="button"
        className="pd-back-button"
        onClick={() => navigate("/shop")}
      >
        <span className="pd-back-icon">
          <i className="bi bi-arrow-left"></i>
        </span>

        <span className="pd-back-content">
          <small>Continue Shopping</small>
          <strong>Back to Products</strong>
        </span>

        <i className="bi bi-grid pd-back-grid-icon"></i>
      </button>

      <div className="pd-card">
        <div className="pd-image-panel">
          <div className="pd-image-wrapper">
            {product.imageUrl ? (
              <img
                src={`${apiOrigin}${product.imageUrl}`}
                alt={product.name}
                className="pd-image"
              />
            ) : (
              <div className="pd-image-placeholder">
                <i className="bi bi-image"></i>
                <span>Product image unavailable</span>
              </div>
            )}

            <span className="pd-image-category">{product.category}</span>
          </div>

          <div className="pd-image-info">
            <div>
              <i className="bi bi-shield-check"></i>

              <span>
                <strong>Secure Shopping</strong>
                <small>Safe and reliable experience</small>
              </span>
            </div>

            <div>
              <i className="bi bi-box-seam"></i>

              <span>
                <strong>Stock Updated</strong>
                <small>Live inventory information</small>
              </span>
            </div>
          </div>
        </div>

        <div className="pd-content">
          <div className="pd-top-section">
            <span className="pd-category-label">{product.category}</span>

            <span className="pd-product-code">Product #{product.id}</span>
          </div>

          <h1 className="pd-title">{product.name}</h1>

          <p className="pd-subtitle">
            Discover complete product information, availability and pricing.
          </p>

          <div className="pd-price-row">
            <div className="pd-price-block">
              <span>Price</span>

              <strong>
                ₹
                {Number(product.price).toLocaleString("en-IN", {
                  minimumFractionDigits: 0,
                  maximumFractionDigits: 2,
                })}
              </strong>
            </div>

            <div className="pd-stock-wrapper">
              {isOutOfStock ? (
                <span className="pd-stock pd-stock-out">
                  <i className="bi bi-x-circle-fill"></i>
                  Out of Stock
                </span>
              ) : isLowStock ? (
                <span className="pd-stock pd-stock-low">
                  <i className="bi bi-exclamation-circle-fill"></i>
                  Only {product.stockQuantity} left
                </span>
              ) : (
                <span className="pd-stock pd-stock-good">
                  <i className="bi bi-check-circle-fill"></i>
                  In Stock
                </span>
              )}
            </div>
          </div>

          <div className="pd-divider"></div>

          <section className="pd-section">
            <div className="pd-section-heading">
              <div className="pd-section-icon">
                <i className="bi bi-info-circle-fill"></i>
              </div>

              <div>
                <span>PRODUCT INFORMATION</span>
                <h3>About this product</h3>
              </div>
            </div>

            <p className="pd-description">
              {product.description ||
                "Detailed product information will be available soon."}
            </p>
          </section>

          <section className="pd-meta-grid">
            <div className="pd-meta-card">
              <div className="pd-meta-icon">
                <i className="bi bi-grid-fill"></i>
              </div>

              <div className="pd-meta-content">
                <span>Category</span>
                <strong>{product.category}</strong>
              </div>
            </div>

            <div className="pd-meta-card">
              <div className="pd-meta-icon">
                <i className="bi bi-boxes"></i>
              </div>

              <div className="pd-meta-content">
                <span>Availability</span>

                <strong>
                  {isOutOfStock
                    ? "Currently unavailable"
                    : `${product.stockQuantity} units available`}
                </strong>
              </div>
            </div>
          </section>

          <div className="pd-divider"></div>

          <div className="pd-actions">
            <button type="button" className="pd-cart-button" disabled>
              <span className="pd-cart-icon">
                <i className="bi bi-cart-plus"></i>
              </span>

              <span className="pd-cart-content">
                <strong>Add to Cart</strong>
                <small>Coming Soon</small>
              </span>
            </button>

            <button
              type="button"
              className="pd-wishlist-button"
              disabled
              title="Wishlist coming soon"
            >
              <i className="bi bi-heart"></i>
            </button>
          </div>

          <div className="pd-coming-soon">
            <i className="bi bi-info-circle"></i>

            <span>
              Cart and Wishlist will be available in the next shopping update.
            </span>
          </div>
        </div>
      </div>
    </div>
  );
}
