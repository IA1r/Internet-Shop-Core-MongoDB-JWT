import { Component, Inject, OnInit } from '@angular/core';
import { Http } from '@angular/http';
import { ActivatedRoute, ParamMap } from '@angular/router';


import { ProductService } from '../product/product.service'
import { ResponseStatusModel } from "../model/ResponseStatusModel";
import { ShoppingCartService } from "../shopping-cart/shopping-cart.service";
import { ProductModel } from "../model/ProductModel";

@Component({
	selector: 'product',
	templateUrl: './product.component.html',
	providers: [ProductService, ShoppingCartService],
	styleUrls: ['./product.component.css']
})

export class ProductComponent implements OnInit {

	constructor(private productService: ProductService, private route: ActivatedRoute) { }

	public product: ProductModel;
	public responseStatus: ResponseStatusModel;
	public info: string;

	ngOnInit(): void {
		this.route.paramMap
			.switchMap((params: ParamMap) => this.productService.getProduct(params.get('id')))
			.subscribe(
			response => {
				this.responseStatus = response.responseStatus;
				this.product = response.product;
			},
			error => {
				this.responseStatus = (error.json() as any).responseStatus
			});
	}

	handleError(error: any): Promise<any> {
		return error.json as any;
	}

	addProduct(): void {
		this.productService.addProduct(this.product._Id)
			.then(response => {
				this.info = response.responseStatus.message
				setTimeout(() => {
					this.info = null;
				}, 5000);
			},
			error => {
				this.info = "Sign In please";
				setTimeout(() => {
					this.info = null;
				}, 5000);
			});
	}
}
