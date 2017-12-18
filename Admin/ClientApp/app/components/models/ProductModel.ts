export class ProductModel {
	_Id: string;
	productTypeID: number;
	cartContentID: number;
	type: string = "";
	tag: string = "";
	characteristics: { [key: string]: string };
}