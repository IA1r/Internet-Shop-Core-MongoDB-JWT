import { ProductModel } from "./ProductModel";
import { UserModel } from "./UserModel";

export class OrderModel {
	public _Id: string;
	public user: UserModel;
	public totalPrice: number;
	public date: Date;
	public isApprove: boolean;
	public products: ProductModel[];
}
