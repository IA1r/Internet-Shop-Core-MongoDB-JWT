
import { Component, Inject, OnInit } from '@angular/core';
import { FileUploader } from "ng2-file-upload";

import { ProductService } from "../product.service";
import { ProductModel } from "../../models/ProductModel";
import { ProductTypeModel } from "../../models/ProductTypeModel";


@Component({
	selector: 'addproduct',
	templateUrl: './addproduct.component.html',
	providers: [ProductService],
	styleUrls: ['./addproduct.component.css']
})

export class AddProductComponent implements OnInit {
	public productTypes: string[];
	public product: ProductModel;
	public selectedType: string;
	public uploader: FileUploader;
	public success: boolean;
	public successUpload: boolean;

	constructor(private productServie: ProductService) {
		this.product = new ProductModel();
	}

	ngOnInit() {
		this.productServie.getProductTypes()
			.then(response => {
				this.productTypes = response.types;
			})
	}

	initDictionaryFields() {
		this.productServie.initDictionaryFields(this.product.type)
			.then(response => {
				this.product.characteristics = response.product.characteristics;
			})
	}

	addProductToDB() {
		this.productServie.addProductToDB(this.product)
			.then(response => {
				this.product._Id = response.producID;
				this.success = response.responseStatus.success;
				this.uploader = new FileUploader({ url: '/api/ProductAPI/ImageUpload/' + this.product._Id });
			})
	}

	updateImage() {
		this.uploader.uploadAll();
		this.uploader.onCompleteItem = (response: any) => {
			this.product.characteristics.Image = JSON.parse(response._xhr.response).imageID;
			this.successUpload = JSON.parse(response._xhr.response).responseStatus.success;
		}
	}
}
