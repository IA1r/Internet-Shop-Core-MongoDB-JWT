import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Http } from '@angular/http';


import { ProductComponent } from "./product.component";
import { ProductModel } from "../model/ProductModel";


@Component({
	selector: 'manga',
	templateUrl: './manga.component.html',
	styleUrls: ['../product/product.component.css']
})

export class MangaComponent {

	@Input('product') public product: ProductModel;
	@Input('info') public info: string;
	@Output() addProduct = new EventEmitter();

	public IsEdit: boolean;

	constructor() { }

	addProductToCart() {
		this.addProduct.emit();
	}

}
