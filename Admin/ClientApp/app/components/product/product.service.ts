import { Injectable, Inject } from '@angular/core';
import { Http, Headers } from '@angular/http';
import 'rxjs/Rx';


import { OrderModel } from "../models/OrderModel";
import { ProductTypeModel } from "../models/ProductTypeModel";
import { ProductModel } from "../models/ProductModel";
import { AuthHttp } from "angular2-jwt/angular2-jwt";

@Injectable()
export class ProductService {
	private headers = new Headers({ 'Content-Type': 'application/json' });

	constructor(private http: Http, private authHttp: AuthHttp, @Inject('ORIGIN_URL') private originUrl: string) { }

	getProductTypes(): Promise<any> {
		return this.authHttp.get(this.originUrl + '/api/ProductAPI/GetProductTypes')
			.map(response => response.json() as any)
			.toPromise();
	}

	getProductsByType(typeID: number): Promise<any> {
		return this.authHttp.get(this.originUrl + '/api/ProductAPI/GetProducts/' + typeID)
			.map(response => response.json() as any)
			.toPromise();
	}

	initDictionaryFields(type: string): Promise<any> {
		return this.authHttp.get(this.originUrl + '/api/ProductAPI/InitDictionaryFields/' + type)
			.map(response => response.json() as any)
			.toPromise();
	}

	addProductToDB(model: ProductModel): Promise<any> {
		return this.authHttp.post('/api/ProductAPI/AddProductToDB', JSON.stringify(model), { headers: this.headers })
			.map(response => response.json() as any)
			.toPromise()
	}

	getProducts() {
		return this.http.get(this.originUrl + '/api/ProductAPI/GetProducts')
			.map(response => response.json() as any)
			.toPromise();
	}

	getProduct(id: string) {
		return this.authHttp.get(this.originUrl + '/api/ProductAPI/GetProduct/' + id)
			.map(response => response.json() as any)
			.toPromise();
	}

	updateProduct(model: ProductModel): Promise<any> {
		return this.authHttp.post('/api/ProductAPI/UpdateProduct', JSON.stringify(model), { headers: this.headers })
			.map(response => response.json() as any)
			.toPromise()
	}

	searchProducts(id: string, keyword: string): Promise<any> {
		return this.authHttp.get(this.originUrl + '/api/ProductAPI/SearchProduct/' + id + '/' + keyword + '/')
			.map(response => response.json() as any)
			.toPromise();
	}

	findProducts(productID: number): Promise<any> {
		return this.authHttp.get(this.originUrl + '/api/ProductAPI/FindProduct/' + productID)
			.map(response => response.json() as any)
			.toPromise();
	}
}