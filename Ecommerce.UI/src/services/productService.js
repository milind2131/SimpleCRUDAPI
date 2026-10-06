import axiosClient from "../api/axiosClient";

export const productService = {
  getAll() {
    return axiosClient.get("/Product");
  },

  getMine() {
    return axiosClient.get("/Product/mine");
  },

  getById(id) {
    return axiosClient.get(`/Product/${id}`);
  },

  create(product) {
    return axiosClient.post("/Product", product);
  },

  update(id, product) {
    return axiosClient.put(
      `/Product/${id}`,
      product
    );
  },

  delete(id) {
    return axiosClient.delete(
      `/Product/${id}`
    );
  },

  uploadImage(id, image) {
    const formData = new FormData();

    formData.append(
      "image",
      image
    );

    return axiosClient.post(
      `/Product/${id}/image`,
      formData
    );
  },
};