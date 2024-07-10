import { Category } from "./Category"
import { Image } from "./Image"

export interface Medicine{
    id: number
    name: string,
    description: string,
    price: number,
    images: Image[],
    popularityMedicine: boolean,
    primaryImageId: number,
    medicineCategory: Category
  }
  