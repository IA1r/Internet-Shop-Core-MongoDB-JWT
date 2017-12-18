import { ProductModel } from "./ProductModel";

export class ShoppingCartModel {

	shoppingCartID: number;
	userName: string;
	totalPrice: number;
	products: ProductModel[];
}
