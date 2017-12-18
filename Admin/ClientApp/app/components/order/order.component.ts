﻿
import { Component, Inject, OnInit, PipeTransform, Pipe } from '@angular/core';
import { SlicePipe } from '@angular/common';

import { OrderService } from "./order.service";
import { ProductModel } from "../models/ProductModel";
import { OrderModel } from "../models/OrderModel";



@Component({
	selector: 'order',
	templateUrl: './order.component.html',
	providers: [OrderService],
	styleUrls: ['./order.component.css']
})

export class OrderComponent implements OnInit {

	public order: OrderModel;
	public orderList: OrderModel[];
	public isNewOrder: boolean;
	public userRole: string = "user";

	constructor(private orderService: OrderService) { }


	ngOnInit() {
		this.getOrderList();
	}

	checkNewOrders(orderList: OrderModel[]) {
		let flagUser = false;
		for (let i = 0; i < orderList.length; i++) {
			if (!orderList[i].isApprove)
				flagUser = true;
			if (flagUser)
				break
		}
		this.isNewOrder = flagUser;
	}

	getOrder(orderID: string) {
		this.orderService.getOrder(orderID)
			.then(response => {
				this.order = response.order;
			});
	}

	getOrderList() {
		this.orderService.getOrderList()
			.then(response => {
				this.orderList = response.orderList
				this.checkNewOrders(this.orderList);
			});
	}
	confirmOrder(orderID: string) {
		this.orderService.confirmOrder(orderID)
			.then(response => {
				this.order.isApprove = true;
				this.orderList.find(item => item._Id == orderID).isApprove = true;
				this.checkNewOrders(this.orderList);
			})
	}

	deleteOrder(orderID: string) {
		this.orderService.deleteOrder(orderID)
			.then(response => {
				var index = this.orderList.indexOf(this.orderList.find(item => item._Id == orderID), 0);
				if (index > -1) {
					this.orderList = this.orderList.splice(index + 1, 1);
				}
				this.checkNewOrders(this.orderList);
				this.order = null;
			})
	}

}

@Pipe({ name: 'filterOrders' })
export class FilterOrdersPipe implements PipeTransform {
	transform(arr: OrderModel[], args: string): any {
		if (arr !== undefined) {
			if (args == "user")
				return arr.filter(order => order.userID != null);
			if (args == "guest")
				return arr.filter(order => order.guestID != null);
		}
	}
}

@Pipe({ name: 'sortOrders' })
export class SortOrdersPipe implements PipeTransform {

	transform(arr: OrderModel[], args: any): OrderModel[] {
		if (arr !== undefined) {
			return arr.sort((a, b) => {
				if (a.isApprove > b.isApprove) {
					return 1;
				}
				if (a.isApprove < b.isApprove) {
					return -1;
				}
				return 0;
			});
		}
	}
}

