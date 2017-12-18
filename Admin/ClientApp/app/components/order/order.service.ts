﻿import { Injectable, Inject } from '@angular/core';
import { Http, Headers } from '@angular/http';
import 'rxjs/Rx';

import { OrderModel } from "../models/OrderModel";
import { AuthHttp } from "angular2-jwt/angular2-jwt";

@Injectable()
export class OrderService {

	private headers = new Headers({ 'Content-Type': 'application/json' });

	constructor(private http: Http, private authHttp: AuthHttp, @Inject('ORIGIN_URL') private originUrl: string) { }

	getOrder(orderID: string): Promise<any> {
		return this.authHttp.get(this.originUrl + '/api/OrderAPI/GetOrder/' + orderID)
			.map(response => response.json() as any)
			.toPromise()
	}

	getOrderList(): Promise<any> {
		return this.authHttp.get(this.originUrl + '/api/OrderAPI/GetOrderList')
			.map(response => response.json() as any)
			.toPromise()
	}

	confirmOrder(orderID: string): Promise<any> {
		return this.authHttp.post('/api/OrderAPI/ConfirmOrder/' + orderID, null, { headers: this.headers })
			.map(response => response.json() as any)
			.toPromise()
	}

	deleteOrder(orderID: string): Promise<any> {
		return this.authHttp.post('/api/OrderAPI/DeleteOrder/' + orderID, null, { headers: this.headers })
			.map(response => response.json() as any)
			.toPromise()
	}

	private handleError(error: any): Promise<any> {
		console.error('An error occurred', error);
		return Promise.reject(error.message || error as any);
	}

}