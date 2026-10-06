import { useMemo, useState } from "react";
import { NavLink, Outlet, useLocation, useNavigate } from "react-router-dom";
import useAuth from "../hooks/useAuth";
import { ROLES } from "../Constants/roles";
import "../styles/appLayout.css";

export default function AppLayout() {
  const { user, logout } = useAuth();

  const navigate = useNavigate();

  const location = useLocation();

  const [sidebarOpen, setSidebarOpen] = useState(false);

  const roleName = user?.roleName ?? ROLES.CUSTOMER;

  const userName =
    user?.name || user?.fullName || user?.email?.split("@")[0] || "User";

  const userEmail = user?.email ?? "";

  const initials = userName
    .split(" ")
    .filter(Boolean)
    .slice(0, 2)
    .map((part) => part.charAt(0).toUpperCase())
    .join("");

  const menuItems = useMemo(() => {
    const common = [
      {
        to: "/dashboard",
        icon: "bi-grid-1x2-fill",
        label: "Dashboard",
      },
      {
        to: "/shop",
        icon: "bi-shop",
        label: "Shop Products",
      },
    ];

    if (roleName === ROLES.CUSTOMER) {
      return [
        ...common,
        {
          to: "/request-access",
          icon: "bi-person-up",
          label: "Request Access",
        },
        {
          to: "/change-password",
          icon: "bi-shield-lock",
          label: "Security",
        },
      ];
    }

    if (roleName === ROLES.SELLER || roleName === ROLES.ADMIN) {
      return [
        ...common,
        {
          to: "/products",
          icon: "bi-box-seam-fill",
          label: "My Products",
        },
        {
          to: "/change-password",
          icon: "bi-shield-lock",
          label: "Security",
        },
      ];
    }

    if (roleName === ROLES.SUPER_ADMIN) {
      return [
        ...common,
        {
          to: "/products",
          icon: "bi-boxes",
          label: "All Products",
        },
        {
          to: "/role-requests",
          icon: "bi-person-check-fill",
          label: "Access Requests",
        },
        {
          to: "/change-password",
          icon: "bi-shield-lock",
          label: "Security",
        },
      ];
    }

    return common;
  }, [roleName]);

  const pageTitle = useMemo(() => {
    if (location.pathname === "/dashboard") {
      return "Dashboard";
    }

    if (location.pathname === "/shop") {
      return "Shop";
    }

    if (location.pathname === "/products") {
      return roleName === ROLES.SUPER_ADMIN ? "All Products" : "My Products";
    }

    if (location.pathname === "/request-access") {
      return "Request Access";
    }

    if (location.pathname === "/role-requests") {
      return "Access Requests";
    }

    if (location.pathname === "/change-password") {
      return "Security";
    }

    if (location.pathname.startsWith("/products/")) {
      return "Product Details";
    }

    return "Cartora";
  }, [location.pathname, roleName]);

  const getRoleLabel = () => {
    if (roleName === ROLES.SUPER_ADMIN) {
      return "SuperAdmin";
    }

    if (roleName === ROLES.ADMIN) {
      return "Administrator";
    }

    if (roleName === ROLES.SELLER) {
      return "Seller";
    }

    return "Customer";
  };

  const getThemeClass = () => {
    if (roleName === ROLES.SUPER_ADMIN) {
      return "theme-superadmin";
    }

    if (roleName === ROLES.ADMIN) {
      return "theme-admin";
    }

    if (roleName === ROLES.SELLER) {
      return "theme-seller";
    }

    return "theme-customer";
  };

  const handleLogout = () => {
    logout();

    navigate("/login", {
      replace: true,
    });
  };

  const closeSidebar = () => {
    setSidebarOpen(false);
  };

  return (
    <div className={`cartora-layout ${getThemeClass()}`}>
      <aside
        className={sidebarOpen ? "cartora-sidebar open" : "cartora-sidebar"}
      >
        <div className="sidebar-brand">
          <div className="brand-logo">
            <i className="bi bi-bag-heart-fill"></i>
          </div>

          <div>
            <strong>Cartora</strong>

            <span>{getRoleLabel()}</span>
          </div>

          <button
            type="button"
            className="sidebar-mobile-close"
            onClick={closeSidebar}
          >
            <i className="bi bi-x-lg"></i>
          </button>
        </div>

        <div className="sidebar-user-card">
          <div className="sidebar-avatar">{initials || "U"}</div>

          <div className="sidebar-user-info">
            <strong>{userName}</strong>

            <span>{getRoleLabel()}</span>
          </div>
        </div>

        <nav className="sidebar-navigation">
          <span className="sidebar-section-label">MENU</span>

          {menuItems.map((item) => (
            <NavLink
              key={item.to}
              to={item.to}
              onClick={closeSidebar}
              className={({ isActive }) =>
                isActive ? "sidebar-nav-link active" : "sidebar-nav-link"
              }
            >
              <span className="sidebar-nav-icon">
                <i className={`bi ${item.icon}`}></i>
              </span>

              <span>{item.label}</span>

              <i className="bi bi-chevron-right sidebar-nav-chevron"></i>
            </NavLink>
          ))}
        </nav>

        <div className="sidebar-footer">
          <div className="sidebar-role-info">
            <i className="bi bi-shield-check"></i>

            <div>
              <span>Signed in as</span>

              <strong>{getRoleLabel()}</strong>
            </div>
          </div>

          <button
            type="button"
            className="sidebar-logout"
            onClick={handleLogout}
          >
            <i className="bi bi-box-arrow-left"></i>
            Logout
          </button>
        </div>
      </aside>

      {sidebarOpen && (
        <div className="sidebar-overlay" onClick={closeSidebar}></div>
      )}

      <div className="cartora-main">
        <header className="cartora-topbar">
          <div className="topbar-left">
            <button
              type="button"
              className="mobile-menu-button"
              onClick={() => setSidebarOpen(true)}
            >
              <i className="bi bi-list"></i>
            </button>

            <div>
              <span>CARTORA</span>

              <h1>{pageTitle}</h1>
            </div>
          </div>

          <div className="topbar-user">
            <div className="topbar-user-avatar">{initials || "U"}</div>

            <div className="topbar-user-details">
              <strong>{userName}</strong>

              <span>{userEmail}</span>
            </div>

            <div className="topbar-role-badge">{getRoleLabel()}</div>
          </div>
        </header>

        <main className="cartora-content">
          <Outlet />
        </main>
      </div>
    </div>
  );
}
