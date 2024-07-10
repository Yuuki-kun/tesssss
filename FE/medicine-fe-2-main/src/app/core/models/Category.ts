import { Medicine } from "./Medicine";

export interface Category{
    id: number,
    name: string,
    medicines: Medicine[]
}
