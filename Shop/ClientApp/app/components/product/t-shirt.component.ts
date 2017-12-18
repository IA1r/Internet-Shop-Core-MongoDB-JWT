import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Http } from '@angular/http';


import { ProductComponent } from "./product.component";
import { ProductModel } from "../model/ProductModel";

@Component({
	selector: 't-shirt',
	templateUrl: './t-shirt.component.html',
	styleUrls: ['../product/product.component.css']
})

export class TShirtComponent {

	@Input('product') public product: ProductModel;
	@Input('info') public info: string;
	@Output() addProduct = new EventEmitter();

	public IsEdit: boolean;

	constructor() { }

	addProductToCart() {
		this.addProduct.emit();
	}

}
