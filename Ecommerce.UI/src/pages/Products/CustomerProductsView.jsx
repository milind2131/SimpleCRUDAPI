import { useEffect, useMemo, useState } from "react";
import toast from "react-hot-toast";
import ProductCard from "../../components/products/ProductCard";
import { productService } from "../../services/productService";
import { getApiErrorMessage } from "../../utils/errorHelper";
import "../../styles/customerStorefront.css";

export default function CustomerProductsView() {
  const [products, setProducts] = useState([]);
  const [searchText, setSearchText] = useState("");
  const [selectedCategory, setSelectedCategory] = useState("All");
  const [sortBy, setSortBy] = useState("default");
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    let cancelled = false;

    productService
      .getAll()
      .then((response) => {
        if (!cancelled) {
          setProducts(response.data);
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

  const categories = useMemo(() => {
    const names = products.map((product) => product.category).filter(Boolean);

    return ["All", ...new Set(names)];
  }, [products]);

  const filteredProducts = useMemo(() => {
    let result = [...products];

    const search = searchText.trim().toLowerCase();

    if (search) {
      result = result.filter((product) => {
        const name = product.name?.toLowerCase() ?? "";

        const category = product.category?.toLowerCase() ?? "";

        const description = product.description?.toLowerCase() ?? "";

        return (
          name.includes(search) ||
          category.includes(search) ||
          description.includes(search)
        );
      });
    }

    if (selectedCategory !== "All") {
      result = result.filter(
        (product) => product.category === selectedCategory,
      );
    }

    switch (sortBy) {
      case "price-low":
        result.sort((a, b) => Number(a.price) - Number(b.price));
        break;

      case "price-high":
        result.sort((a, b) => Number(b.price) - Number(a.price));
        break;

      case "name-az":
        result.sort((a, b) => a.name.localeCompare(b.name));
        break;

      case "name-za":
        result.sort((a, b) => b.name.localeCompare(a.name));
        break;

      case "stock-high":
        result.sort(
          (a, b) => Number(b.stockQuantity) - Number(a.stockQuantity),
        );
        break;

      default:
        break;
    }

    return result;
  }, [products, searchText, selectedCategory, sortBy]);

  const availableProducts = products.filter(
    (product) => product.stockQuantity > 0,
  ).length;

  const outOfStockProducts = products.filter(
    (product) => product.stockQuantity <= 0,
  ).length;

  const clearFilters = () => {
    setSearchText("");
    setSelectedCategory("All");
    setSortBy("default");
  };

  return (
    <div className="customer-shop-page">
      <section className="shop-hero">
        <div className="shop-hero-content">
          <span className="shop-hero-label">
            <i className="bi bi-stars"></i>
            CARTORA COLLECTION
          </span>

          <h1>
            Find something
            <span> you'll love.</span>
          </h1>

          <p>
            Explore our product collection across electronics, fashion, books,
            home, automotive and more.
          </p>

          <div className="shop-hero-search">
            <div className="shop-hero-search-icon">
              <i className="bi bi-search"></i>
            </div>

            <input
              type="text"
              value={searchText}
              onChange={(e) => setSearchText(e.target.value)}
              placeholder="What are you looking for today?"
            />

            {searchText && (
              <button
                type="button"
                className="shop-search-clear"
                onClick={() => setSearchText("")}
              >
                <i className="bi bi-x-lg"></i>
              </button>
            )}

            <button type="button" className="shop-search-button">
              Search
            </button>
          </div>

          <div className="shop-hero-highlights">
            <div>
              <i className="bi bi-box-seam"></i>

              <span>
                <strong>{products.length}</strong>
                Products
              </span>
            </div>

            <div>
              <i className="bi bi-check-circle"></i>

              <span>
                <strong>{availableProducts}</strong>
                Available
              </span>
            </div>

            <div>
              <i className="bi bi-grid"></i>

              <span>
                <strong>{Math.max(categories.length - 1, 0)}</strong>
                Categories
              </span>
            </div>
          </div>
        </div>

        <div className="shop-hero-decoration">
          <div className="hero-decoration-circle hero-circle-one"></div>
          <div className="hero-decoration-circle hero-circle-two"></div>

          <div className="shop-floating-card floating-card-one">
            <div className="floating-icon">
              <i className="bi bi-bag-check-fill"></i>
            </div>

            <div>
              <span>Available products</span>
              <strong>{availableProducts}</strong>
            </div>
          </div>

          <div className="shop-floating-card floating-card-two">
            <div className="floating-icon">
              <i className="bi bi-tags-fill"></i>
            </div>

            <div>
              <span>Shop categories</span>
              <strong>{Math.max(categories.length - 1, 0)}</strong>
            </div>
          </div>

          <div className="hero-shopping-icon">
            <i className="bi bi-bag-heart-fill"></i>
          </div>
        </div>
      </section>

      <section className="shop-category-section">
        <div className="shop-section-heading">
          <div>
            <span>SHOP BY CATEGORY</span>
            <h2>Browse Collection</h2>
          </div>
        </div>

        <div className="shop-category-list">
          {categories.map((category) => (
            <button
              key={category}
              type="button"
              className={
                selectedCategory === category
                  ? "shop-category-chip active"
                  : "shop-category-chip"
              }
              onClick={() => setSelectedCategory(category)}
            >
              {category === "All" && <i className="bi bi-grid-fill"></i>}

              {category}
            </button>
          ))}
        </div>
      </section>

      <section className="shop-products-section">
        <div className="shop-products-header">
          <div>
            <span className="shop-results-label">OUR PRODUCTS</span>

            <h2>
              {selectedCategory === "All"
                ? "Explore Products"
                : selectedCategory}
            </h2>

            <p>
              Showing {filteredProducts.length}{" "}
              {filteredProducts.length === 1 ? "product" : "products"}
            </p>
          </div>

          <div className="shop-sort-container">
            <i className="bi bi-sliders"></i>

            <select value={sortBy} onChange={(e) => setSortBy(e.target.value)}>
              <option value="default">Featured</option>

              <option value="price-low">Price: Low to High</option>

              <option value="price-high">Price: High to Low</option>

              <option value="name-az">Name: A to Z</option>

              <option value="name-za">Name: Z to A</option>

              <option value="stock-high">Highest Stock</option>
            </select>
          </div>
        </div>

        {(searchText || selectedCategory !== "All" || sortBy !== "default") && (
          <div className="shop-active-filters">
            <div>
              <i className="bi bi-funnel"></i>

              <span>Filters applied</span>

              {searchText && (
                <span className="shop-filter-pill">Search: {searchText}</span>
              )}

              {selectedCategory !== "All" && (
                <span className="shop-filter-pill">{selectedCategory}</span>
              )}
            </div>

            <button type="button" onClick={clearFilters}>
              Clear All
            </button>
          </div>
        )}

        {loading && (
          <div className="shop-loading-state">
            <div className="shop-loader">
              <span></span>
              <span></span>
              <span></span>
            </div>

            <strong>Finding great products...</strong>

            <p>Your collection is being loaded.</p>
          </div>
        )}

        {!loading && filteredProducts.length > 0 && (
          <div className="shop-products-grid">
            {filteredProducts.map((product) => (
              <ProductCard key={product.id} product={product} />
            ))}
          </div>
        )}

        {!loading && filteredProducts.length === 0 && (
          <div className="shop-empty-state">
            <div className="shop-empty-illustration">
              <i className="bi bi-search"></i>
            </div>

            <span>NO MATCHES FOUND</span>

            <h3>We couldn't find that product.</h3>

            <p>
              Try a different product name, category or remove the current
              filters.
            </p>

            <button type="button" onClick={clearFilters}>
              <i className="bi bi-arrow-counterclockwise"></i>
              Reset Filters
            </button>
          </div>
        )}

        {!loading && products.length > 0 && (
          <div className="shop-products-summary">
            <div>
              <i className="bi bi-check-circle-fill"></i>

              <span>{availableProducts} products currently available</span>
            </div>

            {outOfStockProducts > 0 && (
              <div>
                <i className="bi bi-info-circle"></i>

                <span>{outOfStockProducts} currently out of stock</span>
              </div>
            )}
          </div>
        )}
      </section>
    </div>
  );
}
