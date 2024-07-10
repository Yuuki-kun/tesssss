
const MEDICINE_BASE = "Medicine";
const CATEGORY_BASE = "Category";
 
export const Constant = {
  API_ENDPOINT: {
    MEDICINE: {
      BASE: MEDICINE_BASE,
      GET_LIST: `${MEDICINE_BASE}/medicines`,
      SEARCH: `${MEDICINE_BASE}/get-by-name`,
      GET_BY_CATEGORY: `${MEDICINE_BASE}/get-by-category`,
    },
    CATEGORY: {
      BASE: CATEGORY_BASE,
      GET_LIST: `${CATEGORY_BASE}/categories`,
      SEARCH: `${CATEGORY_BASE}/get-by-name?name=`
    },
    IMAGE: {
      BASE: "Image",
    },
  },
};
