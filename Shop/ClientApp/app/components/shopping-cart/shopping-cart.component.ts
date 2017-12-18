import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { Http } from '@angular/http';
import { ActivatedRoute, ParamMap } from '@angular/router';

import { ShoppingCartService } from '../shopping-cart/shopping-cart.service';
import { ModalDirective } from "ngx-bootstrap";
import { ResponseStatusModel } from "../model/ResponseStatusModel";
import { ShoppingCartModel } from "../model/ShoppingCartModel";
import { OrderModel } from "../model/OrderModel";
import { UserModel } from "../model/UserModel";

@Component({
	selector: 'shopping-cart',
	templateUrl: './shopping-cart.component.html',
	providers: [ShoppingCartService],
	styleUrls: ['./shopping-cart.component.css']
})

export class ShoppingCartComponent implements OnInit {
	public shoppingCart: ShoppingCartModel;
	public orderModel: OrderModel;
	public responseStatus: ResponseStatusModel;
	@ViewChild('staticModal') public staticModal: ModalDirective;
	public p: number = 1;


	constructor(private shoppingCartService: ShoppingCartService) {
		this.orderModel = new OrderModel();
		this.orderModel.user = new UserModel()
	}

	ngOnInit() {
		this.getShoppingCart();
	}

	getShoppingCart() {
		this.shoppingCartService.getShoppingCart()
			.then(response => {
				if (response != null) {
					this.responseStatus = response.responseStatus;
					this.shoppingCart = response.cart
				}
				else {
					this.shoppingCart = null;
				}
			});
	}

	deleteItem(productID: string) {
		this.shoppingCartService.deleteItem(productID)
			.then(() => {
				this.getShoppingCart();
			});
	}
	checkout() {
		this.shoppingCartService.checkout(this.orderModel)
			.then(response => {
				if (response.responseStatus.success)
					window.location.href = '/orders';
			});
	}

	getUserDataForCheckout() {
		this.shoppingCartService.getUserDataForCheckout()
			.then(response => {
				this.orderModel.user = response.user
			});
	}

	//checkout() {
	//	this.orderModel.totalPrice = this.shoppingCart.totalPrice;
	//	this.staticModal.hide();

	//	this.shoppingCartService.checkout(this.orderModel)
	//		.then(response => {
	//			if (response.responseStatus.success)
	//				window.location.href = '/orders';
	//		});
	//}
}
