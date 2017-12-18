export class ProductModel {
	_Id: string;
	tags: string[];
	type: string;
	characteristic: { [key: string]: string };
}