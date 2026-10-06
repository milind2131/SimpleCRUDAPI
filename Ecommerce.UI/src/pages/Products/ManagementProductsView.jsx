import { useEffect, useMemo, useState } from "react";
import toast from "react-hot-toast";
import Swal from "sweetalert2";
import { productService } from "../../services/productService";
import { getApiErrorMessage } from "../../utils/errorHelper";
import useAuth from "../../hooks/useAuth";
import { ROLES } from "../../Constants/roles";
import "../../styles/productManagement.css";

const apiOrigin = import.meta.env.VITE_API_ORIGIN;

const categories = [
  { id: 1, name: "Electronics" },
  { id: 2, name: "Fashion" },
  { id: 3, name: "Books" },
  { id: 5, name: "Furniture" },
  { id: 6, name: "Pet Supplies" },
  { id: 7, name: "Health & Wellness" },
  { id: 8, name: "Automotive" },
  { id: 9, name: "Garden" },
  {
    id: 10,
    name: "Musical Instruments",
  },
  {
    id: 11,
    name: "Office Supplies",
  },
  { id: 12, name: "Baby Care" },
  { id: 13, name: "Jewellery" },
  { id: 14, name: "Luggage" },
];

const emptyProduct = {
  name: "",
  price: "",
  categoryId: "",
  description: "",
  stockQuantity: "",
  image: null,
  imageUrl: "",
};

export default function ManagementProductsView() {
  const { user } = useAuth();

  const [products, setProducts] = useState([]);

  const [form, setForm] = useState(emptyProduct);

  const [editingId, setEditingId] = useState(null);

  const [showForm, setShowForm] = useState(false);

  const [loading, setLoading] = useState(true);

  const [saving, setSaving] = useState(false);

  const [searchText, setSearchText] = useState("");

  const [categoryFilter, setCategoryFilter] = useState("All");

  const [stockFilter, setStockFilter] = useState("All");

  const isSuperAdmin = user?.roleName === ROLES.SUPER_ADMIN;

  const isAdmin = user?.roleName === ROLES.ADMIN;

  const isSeller = user?.roleName === ROLES.SELLER;

  const canAddProduct = isSuperAdmin || isAdmin || isSeller;

  const canEditProduct = isSuperAdmin || isAdmin || isSeller;

  const canDeleteProduct = isSuperAdmin;

  const getProductsRequest = () => {
    if (isSuperAdmin) {
      return productService.getAll();
    }

    return productService.getMine();
  };

  useEffect(() => {
    let cancelled = false;

    getProductsRequest()
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
  }, [isSuperAdmin]);

  useEffect(() => {
    if (!showForm) {
      return undefined;
    }

    const previousOverflow = document.body.style.overflow;

    document.body.style.overflow = "hidden";

    return () => {
      document.body.style.overflow = previousOverflow;
    };
  }, [showForm]);

  const loadProducts = async () => {
    try {
      setLoading(true);

      const response = await getProductsRequest();

      setProducts(response.data);
    } catch (error) {
      toast.error(getApiErrorMessage(error));
    } finally {
      setLoading(false);
    }
  };

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

    if (categoryFilter !== "All") {
      result = result.filter((product) => product.category === categoryFilter);
    }

    if (stockFilter === "InStock") {
      result = result.filter((product) => product.stockQuantity > 10);
    }

    if (stockFilter === "LowStock") {
      result = result.filter(
        (product) => product.stockQuantity > 0 && product.stockQuantity <= 10,
      );
    }

    if (stockFilter === "OutOfStock") {
      result = result.filter((product) => product.stockQuantity <= 0);
    }

    return result;
  }, [products, searchText, categoryFilter, stockFilter]);

  const totalProducts = products.length;

  const inStockCount = products.filter(
    (product) => product.stockQuantity > 10,
  ).length;

  const lowStockCount = products.filter(
    (product) => product.stockQuantity > 0 && product.stockQuantity <= 10,
  ).length;

  const outOfStockCount = products.filter(
    (product) => product.stockQuantity <= 0,
  ).length;

  const openAdd = () => {
    setEditingId(null);

    setForm(emptyProduct);

    setShowForm(true);
  };

  const openEdit = (product) => {
    setEditingId(product.id);

    setForm({
      name: product.name,
      price: product.price,
      categoryId: product.categoryId,
      description: product.description ?? "",
      stockQuantity: product.stockQuantity ?? 0,
      image: null,
      imageUrl: product.imageUrl ?? "",
    });

    setShowForm(true);
  };

  const closeForm = () => {
    if (saving) {
      return;
    }

    setShowForm(false);
    setEditingId(null);
    setForm(emptyProduct);
  };

  const saveProduct = async (e) => {
    e.preventDefault();

    const request = {
      name: form.name.trim(),

      price: Number(form.price),

      categoryId: Number(form.categoryId),

      description: form.description.trim(),

      stockQuantity: Number(form.stockQuantity),
    };

    if (!request.name) {
      toast.error("Product name is required.");
      return;
    }

    if (request.price <= 0) {
      toast.error("Price must be greater than zero.");
      return;
    }

    if (request.categoryId <= 0) {
      toast.error("Please select a category.");
      return;
    }

    if (request.stockQuantity < 0) {
      toast.error("Stock quantity cannot be negative.");
      return;
    }

    try {
      setSaving(true);

      let productId;

      if (editingId) {
        await productService.update(editingId, request);

        productId = editingId;
      } else {
        const response = await productService.create(request);

        productId = response.data.id;
      }

      if (form.image) {
        await productService.uploadImage(productId, form.image);
      }

      toast.success(
        editingId
          ? "Product updated successfully."
          : "Product added successfully.",
      );

      setShowForm(false);
      setEditingId(null);
      setForm(emptyProduct);

      await loadProducts();
    } catch (error) {
      toast.error(getApiErrorMessage(error));
    } finally {
      setSaving(false);
    }
  };

  const deleteProduct = async (product) => {
    const result = await Swal.fire({
      title: "Delete Product?",

      html: `Are you sure you want to delete <strong>${product.name}</strong>?`,

      icon: "warning",

      showCancelButton: true,

      confirmButtonText: "Yes, Delete",

      cancelButtonText: "Cancel",

      reverseButtons: true,

      confirmButtonColor: "#dc3545",

      cancelButtonColor: "#6c757d",
    });

    if (!result.isConfirmed) {
      return;
    }

    try {
      await productService.delete(product.id);

      await Swal.fire({
        title: "Deleted!",

        text: `${product.name} has been deleted successfully.`,

        icon: "success",

        confirmButtonText: "OK",
      });

      await loadProducts();
    } catch (error) {
      await Swal.fire({
        title: "Unable to Delete",

        text: getApiErrorMessage(error),

        icon: "error",

        confirmButtonText: "OK",
      });
    }
  };

  const clearFilters = () => {
    setSearchText("");

    setCategoryFilter("All");

    setStockFilter("All");
  };

  return (
    <div className="catalog-page">
      <div className="catalog-header">
        <div>
          <span className="catalog-eyebrow">
            {isSuperAdmin
              ? "SUPERADMIN CATALOGUE"
              : isAdmin
                ? "ADMIN CATALOGUE"
                : "SELLER CENTER"}
          </span>

          <h2>{isSuperAdmin ? "All Products" : "My Products"}</h2>

          <p>
            {isSuperAdmin
              ? "Manage the complete Cartora product catalogue."
              : "Manage products created by your account."}
          </p>
        </div>

        {canAddProduct && (
          <button
            type="button"
            className="catalog-add-button"
            onClick={openAdd}
          >
            <i className="bi bi-plus-lg"></i>
            Add Product
          </button>
        )}
      </div>

      <div className="catalog-stats">
        <div className="catalog-stat-card">
          <div className="catalog-stat-icon stat-total">
            <i className="bi bi-box-seam-fill"></i>
          </div>

          <div>
            <span>Total Products</span>

            <strong>{totalProducts}</strong>
          </div>
        </div>

        <div className="catalog-stat-card">
          <div className="catalog-stat-icon stat-stock">
            <i className="bi bi-check-circle-fill"></i>
          </div>

          <div>
            <span>In Stock</span>

            <strong>{inStockCount}</strong>
          </div>
        </div>

        <div className="catalog-stat-card">
          <div className="catalog-stat-icon stat-low">
            <i className="bi bi-exclamation-circle-fill"></i>
          </div>

          <div>
            <span>Low Stock</span>

            <strong>{lowStockCount}</strong>
          </div>
        </div>

        <div className="catalog-stat-card">
          <div className="catalog-stat-icon stat-out">
            <i className="bi bi-x-circle-fill"></i>
          </div>

          <div>
            <span>Out of Stock</span>

            <strong>{outOfStockCount}</strong>
          </div>
        </div>
      </div>

      <div className="catalog-toolbar">
        <div className="catalog-search">
          <i className="bi bi-search"></i>

          <input
            type="text"
            value={searchText}
            onChange={(e) => setSearchText(e.target.value)}
            placeholder="Search product, category or description..."
          />

          {searchText && (
            <button type="button" onClick={() => setSearchText("")}>
              <i className="bi bi-x-lg"></i>
            </button>
          )}
        </div>

        <select
          className="catalog-filter"
          value={categoryFilter}
          onChange={(e) => setCategoryFilter(e.target.value)}
        >
          <option value="All">All Categories</option>

          {categories.map((category) => (
            <option key={category.id} value={category.name}>
              {category.name}
            </option>
          ))}
        </select>

        <select
          className="catalog-filter"
          value={stockFilter}
          onChange={(e) => setStockFilter(e.target.value)}
        >
          <option value="All">All Stock</option>

          <option value="InStock">In Stock</option>

          <option value="LowStock">Low Stock</option>

          <option value="OutOfStock">Out of Stock</option>
        </select>

        {(searchText || categoryFilter !== "All" || stockFilter !== "All") && (
          <button
            type="button"
            className="catalog-clear-button"
            onClick={clearFilters}
          >
            Clear
          </button>
        )}
      </div>

      <div className="catalog-table-card">
        <div className="catalog-table-header">
          <div>
            <h5>{isSuperAdmin ? "Platform Products" : "Your Catalogue"}</h5>

            <span>
              Showing {filteredProducts.length} of {totalProducts}
            </span>
          </div>
        </div>

        <div className="table-responsive">
          <table className="catalog-table">
            <thead>
              <tr>
                <th>Sr. No.</th>

                <th>Product</th>

                <th>Category</th>

                <th>Inventory</th>

                <th>Price</th>

                <th className="text-end">Actions</th>
              </tr>
            </thead>

            <tbody>
              {loading && (
                <tr>
                  <td colSpan="6" className="catalog-loading-cell">
                    <div className="spinner-border spinner-border-sm"></div>
                    Loading products...
                  </td>
                </tr>
              )}

              {!loading &&
                filteredProducts.map((product, index) => (
                  <tr key={product.id}>
                    <td className="catalog-serial">{index + 1}</td>

                    <td>
                      <div className="catalog-product-cell">
                        {product.imageUrl ? (
                          <img
                            src={`${apiOrigin}${product.imageUrl}`}
                            alt={product.name}
                            className="catalog-product-image"
                          />
                        ) : (
                          <div className="catalog-product-placeholder">
                            <i className="bi bi-image"></i>
                          </div>
                        )}

                        <div className="catalog-product-info">
                          <strong>{product.name}</strong>

                          <span>
                            {product.description
                              ? product.description.length > 65
                                ? `${product.description.substring(0, 65)}...`
                                : product.description
                              : "No description available"}
                          </span>
                        </div>
                      </div>
                    </td>

                    <td>
                      <span className="catalog-category-badge">
                        {product.category}
                      </span>
                    </td>

                    <td>
                      {product.stockQuantity > 10 && (
                        <span className="catalog-stock stock-good">
                          <i className="bi bi-check-circle-fill"></i>
                          {product.stockQuantity} in stock
                        </span>
                      )}

                      {product.stockQuantity > 0 &&
                        product.stockQuantity <= 10 && (
                          <span className="catalog-stock stock-low">
                            <i className="bi bi-exclamation-circle-fill"></i>
                            Only {product.stockQuantity} left
                          </span>
                        )}

                      {product.stockQuantity <= 0 && (
                        <span className="catalog-stock stock-empty">
                          <i className="bi bi-x-circle-fill"></i>
                          Out of stock
                        </span>
                      )}
                    </td>

                    <td className="catalog-price">
                      ₹{Number(product.price).toLocaleString("en-IN")}
                    </td>

                    <td>
                      <div className="catalog-actions">
                        {canEditProduct && (
                          <button
                            type="button"
                            className="catalog-action-button edit-action"
                            onClick={() => openEdit(product)}
                            title="Edit Product"
                          >
                            <i className="bi bi-pencil-square"></i>
                          </button>
                        )}

                        {canDeleteProduct && (
                          <button
                            type="button"
                            className="catalog-action-button delete-action"
                            onClick={() => deleteProduct(product)}
                            title="Delete Product"
                          >
                            <i className="bi bi-trash3"></i>
                          </button>
                        )}
                      </div>
                    </td>
                  </tr>
                ))}

              {!loading && filteredProducts.length === 0 && (
                <tr>
                  <td colSpan="6" className="catalog-empty-cell">
                    <div className="catalog-empty-icon">
                      <i className="bi bi-box-seam"></i>
                    </div>

                    <strong>
                      {products.length === 0
                        ? "No products created yet"
                        : "No products found"}
                    </strong>

                    <span>
                      {products.length === 0
                        ? "Start by adding your first product."
                        : "Try changing the current filters."}
                    </span>

                    {products.length > 0 && (
                      <button type="button" onClick={clearFilters}>
                        Clear Filters
                      </button>
                    )}
                  </td>
                </tr>
              )}
            </tbody>
          </table>
        </div>
      </div>

      {showForm && (
        <div
          className="product-modal-overlay"
          onMouseDown={(e) => {
            if (e.target === e.currentTarget) {
              closeForm();
            }
          }}
        >
          <div className="product-modal">
            <div className="product-modal-header">
              <div>
                <span className="product-modal-eyebrow">
                  {editingId ? "UPDATE CATALOGUE" : "NEW PRODUCT"}
                </span>

                <h3>{editingId ? "Edit Product" : "Add Product"}</h3>

                <p>
                  {editingId
                    ? "Update product information, stock and image."
                    : "Create a new product in your catalogue."}
                </p>
              </div>

              <button
                type="button"
                className="product-modal-close"
                onClick={closeForm}
                disabled={saving}
              >
                <i className="bi bi-x-lg"></i>
              </button>
            </div>

            <form className="product-modal-form" onSubmit={saveProduct}>
              <div className="product-modal-body">
                <div className="product-form-section">
                  <div className="product-form-section-title">
                    <div className="product-form-section-icon">
                      <i className="bi bi-info-circle-fill"></i>
                    </div>

                    <div>
                      <h6>Basic Information</h6>

                      <span>Information customers will see.</span>
                    </div>
                  </div>

                  <div className="product-form-grid">
                    <div className="product-form-field full-width">
                      <label>Product Name</label>

                      <input
                        type="text"
                        value={form.name}
                        onChange={(e) =>
                          setForm({
                            ...form,
                            name: e.target.value,
                          })
                        }
                        placeholder="Enter product name"
                        required
                      />
                    </div>

                    <div className="product-form-field full-width">
                      <label>Description</label>

                      <textarea
                        rows="4"
                        value={form.description}
                        onChange={(e) =>
                          setForm({
                            ...form,
                            description: e.target.value,
                          })
                        }
                        placeholder="Describe product features and usage..."
                      ></textarea>

                      <span className="field-hint">
                        Write a clear customer-friendly description.
                      </span>
                    </div>
                  </div>
                </div>

                <div className="product-form-section">
                  <div className="product-form-section-title">
                    <div className="product-form-section-icon">
                      <i className="bi bi-tags-fill"></i>
                    </div>

                    <div>
                      <h6>Pricing & Category</h6>

                      <span>Configure price and category.</span>
                    </div>
                  </div>

                  <div className="product-form-grid">
                    <div className="product-form-field">
                      <label>Price</label>

                      <div className="input-with-prefix">
                        <span>₹</span>

                        <input
                          type="number"
                          step="0.01"
                          min="0"
                          value={form.price}
                          onChange={(e) =>
                            setForm({
                              ...form,
                              price: e.target.value,
                            })
                          }
                          placeholder="0.00"
                          required
                        />
                      </div>
                    </div>

                    <div className="product-form-field">
                      <label>Category</label>

                      <select
                        value={form.categoryId}
                        onChange={(e) =>
                          setForm({
                            ...form,
                            categoryId: Number(e.target.value),
                          })
                        }
                        required
                      >
                        <option value="">Select Category</option>

                        {categories.map((category) => (
                          <option key={category.id} value={category.id}>
                            {category.name}
                          </option>
                        ))}
                      </select>
                    </div>
                  </div>
                </div>

                <div className="product-form-section">
                  <div className="product-form-section-title">
                    <div className="product-form-section-icon">
                      <i className="bi bi-boxes"></i>
                    </div>

                    <div>
                      <h6>Inventory</h6>

                      <span>Track available stock.</span>
                    </div>
                  </div>

                  <div className="product-form-grid">
                    <div className="product-form-field">
                      <label>Stock Quantity</label>

                      <input
                        type="number"
                        min="0"
                        value={form.stockQuantity}
                        onChange={(e) =>
                          setForm({
                            ...form,
                            stockQuantity: e.target.value,
                          })
                        }
                        placeholder="0"
                        required
                      />
                    </div>

                    <div className="inventory-preview">
                      <span>Stock Status</span>

                      {Number(form.stockQuantity) > 10 && (
                        <strong className="preview-good">
                          <i className="bi bi-check-circle-fill"></i>
                          Healthy Stock
                        </strong>
                      )}

                      {Number(form.stockQuantity) > 0 &&
                        Number(form.stockQuantity) <= 10 && (
                          <strong className="preview-low">
                            <i className="bi bi-exclamation-circle-fill"></i>
                            Low Stock
                          </strong>
                        )}

                      {Number(form.stockQuantity) <= 0 && (
                        <strong className="preview-empty">
                          <i className="bi bi-x-circle-fill"></i>
                          Out of Stock
                        </strong>
                      )}
                    </div>
                  </div>
                </div>

                <div className="product-form-section">
                  <div className="product-form-section-title">
                    <div className="product-form-section-icon">
                      <i className="bi bi-image-fill"></i>
                    </div>

                    <div>
                      <h6>Product Image</h6>

                      <span>Add a professional product image.</span>
                    </div>
                  </div>

                  <div className="product-image-editor">
                    <div className="product-image-preview">
                      {form.image ? (
                        <img
                          src={URL.createObjectURL(form.image)}
                          alt="Product preview"
                        />
                      ) : form.imageUrl ? (
                        <img
                          src={`${apiOrigin}${form.imageUrl}`}
                          alt={form.name}
                        />
                      ) : (
                        <div className="product-image-empty">
                          <i className="bi bi-image"></i>

                          <span>No image selected</span>
                        </div>
                      )}
                    </div>

                    <div className="product-upload-area">
                      <div className="product-upload-icon">
                        <i className="bi bi-cloud-arrow-up-fill"></i>
                      </div>

                      <strong>
                        {editingId
                          ? "Replace product image"
                          : "Upload product image"}
                      </strong>

                      <span>JPG, JPEG, PNG or WEBP up to 5 MB.</span>

                      <label className="product-file-button">
                        Choose Image
                        <input
                          type="file"
                          accept="image/png,image/jpeg,image/webp"
                          onChange={(e) =>
                            setForm({
                              ...form,
                              image: e.target.files?.[0] ?? null,
                            })
                          }
                        />
                      </label>

                      {form.image && <small>Selected: {form.image.name}</small>}
                    </div>
                  </div>
                </div>
              </div>

              <div className="product-modal-footer">
                <button
                  type="button"
                  className="product-cancel-button"
                  onClick={closeForm}
                  disabled={saving}
                >
                  Cancel
                </button>

                <button
                  type="submit"
                  className="product-save-button"
                  disabled={saving}
                >
                  {saving ? (
                    <>
                      <span className="spinner-border spinner-border-sm"></span>
                      Saving...
                    </>
                  ) : editingId ? (
                    <>
                      <i className="bi bi-check-lg"></i>
                      Update Product
                    </>
                  ) : (
                    <>
                      <i className="bi bi-plus-lg"></i>
                      Add Product
                    </>
                  )}
                </button>
              </div>
            </form>
          </div>
        </div>
      )}
    </div>
  );
}
