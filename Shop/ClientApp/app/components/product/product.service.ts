
import { Injectable, Inject, InjectionToken } from '@angular/core';
import { Http, Headers } from '@angular/http';
import 'rxjs/Rx';

import { ShoppingCartService } from "../shopping-cart/shopping-cart.service";


@Injectable()
export class ProductService {

	private headers = new Headers({ 'Content-Type': 'application/json' });

	constructor(private http: Http, @Inject('ORIGIN_URL') private originUrl: string, private shoppingCartService: ShoppingCartService) { }

	getProducts(): Promise<any> {
		return this.http.get(this.originUrl + '/api/ProductAPI/GetProducts', { headers: this.headers })
			.map(response => response.json() as any)
			.toPromise();
	}

	getProductsByType(type: string): Promise<any> {
		return this.http.get(this.originUrl + '/api/ProductAPI/GetProducts/' + type)
			.map(response => response.json() as any)
			.toPromise();
	}

	getProduct(id: string): Promise<any> {
		return this.http.get(this.originUrl + '/api/ProductAPI/GetProduct/' + id, { headers: this.headers })
			.map(response => response.json() as any)
			.toPromise();
	}

	searchProducts(keyword: string): Promise<any> {
		return this.http.get(this.originUrl + '/api/ProductAPI/SearchProduct/' + keyword)
			.map(response => response.json() as any)
			.toPromise();
	}

	addProduct(productID: string): Promise<any> {
		return this.shoppingCartService.addProduct(productID);
	}


}