import axiosClient from "../api/axiosClient";

export const roleUpgradeService = {
  create(requestedRole) {
    return axiosClient.post(
      "/RoleUpgradeRequest",
      {
        requestedRole,
      }
    );
  },

  getMine() {
    return axiosClient.get(
      "/RoleUpgradeRequest/mine"
    );
  },

  getPending() {
    return axiosClient.get(
      "/RoleUpgradeRequest/pending"
    );
  },

  approve(requestId, remarks = "") {
    return axiosClient.put(
      `/RoleUpgradeRequest/${requestId}/approve`,
      {
        remarks,
      }
    );
  },

  reject(requestId, remarks = "") {
    return axiosClient.put(
      `/RoleUpgradeRequest/${requestId}/reject`,
      {
        remarks,
      }
    );
  },
};