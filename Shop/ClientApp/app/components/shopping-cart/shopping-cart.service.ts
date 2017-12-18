import { Injectable, Inject, InjectionToken } from '@angular/core';
import { Http, Headers } from '@angular/http';
import 'rxjs/Rx';

import { OrderModel } from "../model/OrderModel";
import { AuthHttp } from "angular2-jwt/angular2-jwt";


@Injectable()
export class ShoppingCartService {
	constructor(private http: Http, private authHttp: AuthHttp) { }

	getShoppingCart(): Promise<any> {
		return this.authHttp.get('/api/ProductAPI/GetShoppingCart')
			.map(response => response.json() as any)
			.toPromise();
	}

	addProduct(productID: string): Promise<any> {
		return this.authHttp.post('/api/ProductAPI/AddProduct/' + productID, null)
			.map(response => response.json() as any)
			.toPromise()
	}

	deleteItem(productID: string): Promise<any> {
		return this.authHttp.delete('/api/ProductAPI/DeleteItem/' + productID)
			.map(response => response.json() as any)
			.toPromise()
	}

	checkout(order: OrderModel): Promise<any> {
		return this.authHttp.post('/api/ProductAPI/Checkout', JSON.stringify(order.user))
			.map(response => response.json() as any)
			.toPromise()
	}

	getUserDataForCheckout(): Promise<any> {
		return this.authHttp.get('/api/Account/GetUserDataForCheckout')
			.map(response => response.json() as any)
			.toPromise()
	}

	private handleError(error: any): Promise<any> {
		console.error('An error occurred', error);
		return Promise.reject(error.message || error.json() as any);
	}
}