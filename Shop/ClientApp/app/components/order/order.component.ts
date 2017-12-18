
import { Component, Inject, OnInit } from '@angular/core';

import { OrderService } from "./order.service";
import { OrderModel } from "../model/OrderModel";



@Component({
	selector: 'order',
	templateUrl: './order.component.html',
	providers: [OrderService],
	styleUrls: ['./order.component.css']
})

export class OrderComponent implements OnInit {

	public order: OrderModel;
	public orderList: OrderModel[];
	constructor(private orderService: OrderService) { }


	ngOnInit() {
		this.getOrderList();
	}

	getOrder(orderID: string) {

		//for (var item of this.orderList) {
		//	if (item._Id == orderID)
		//		this.order = item;
		//}

		this.orderService.getOrder(orderID)
			.then(response => {
				this.order = response.order;
			});
	}

	getOrderList() {
		this.orderService.getOrderList()
			.then(response => {
				if (response != null)
					this.orderList = response.orderList;
			});
	}
}
