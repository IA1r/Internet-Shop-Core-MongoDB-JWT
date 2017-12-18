import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Http } from '@angular/http';


import { ProductComponent } from "./product.component";
import { ProductModel } from "../model/ProductModel";

@Component({
	selector: 'pc-game',
	templateUrl: './pc-game.component.html',
	styleUrls: ['../product/product.component.css']
})

export class PCGameComponent {

	@Input('product') public product: ProductModel;
	@Input('info') public info: string;
	@Output() addProduct = new EventEmitter();

	public IsEdit: boolean;

	constructor() { }

	addProductToCart() {
		this.addProduct.emit();
	}

}
