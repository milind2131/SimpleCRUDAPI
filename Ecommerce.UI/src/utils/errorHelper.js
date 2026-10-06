export function getApiErrorMessage(error) {
  const data = error.response?.data;

  if (!data) {
    return "Unable to connect to server.";
  }

  if (
    Array.isArray(data.errors) &&
    data.errors.length > 0
  ) {
    return data.errors
      .map((x) => x.error)
      .join(", ");
  }

  if (
    data.errors &&
    typeof data.errors === "object"
  ) {
    return Object.values(data.errors)
      .flat()
      .join(", ");
  }

  if (data.message) {
    return data.message;
  }

  return "Something went wrong.";
}