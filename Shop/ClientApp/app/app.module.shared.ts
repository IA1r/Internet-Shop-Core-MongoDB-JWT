import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ModalModule, BsDropdownModule } from 'ngx-bootstrap';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { HttpModule } from '@angular/http';
import { CustomFormsModule } from 'ng2-validation';
import { NgxPaginationModule } from 'ngx-pagination';
import { AuthModule } from "./app.module.auth";
import { JwtHelper } from "angular2-jwt/angular2-jwt";

import { AppComponent } from './components/app/app.component';
import { ProductsComponent } from './components/product/products.component';
import { ProductService } from './components/product/product.service';
import { AuthorizationComponent } from './components/account/authorization/authorization.component'
import { AuthorizationService } from './components/account/authorization/authorization.service';
import { ProductComponent } from './components/product/product.component';
import { ShoppingCartService } from './components/shopping-cart/shopping-cart.service';
import { ShoppingCartComponent } from './components/shopping-cart/shopping-cart.component';
import { OrderComponent } from "./components/order/order.component";
import { OrderService } from "./components/order/order.service";
import { PCGameComponent } from "./components/product/pc-game.component";
import { MangaComponent } from "./components/product/manga.component";
import { TShirtComponent } from "./components/product/t-shirt.component";
import { SSDComponent } from "./components/product/ssd.component";
import { RAMComponent } from "./components/product/ram.component";
import { routes } from "./route";
import { AuthGuard } from "./auth-guard.service";




export const sharedConfig: NgModule = {
	bootstrap: [AppComponent],
	declarations: [
		AppComponent,
		ProductsComponent,
		AuthorizationComponent,
		ProductComponent,
		ShoppingCartComponent,
		OrderComponent,
		PCGameComponent,
		MangaComponent,
		TShirtComponent,
		SSDComponent,
		RAMComponent
	],
	imports: [
		FormsModule,
		BrowserModule,
		AuthModule,
		CustomFormsModule,
		NgxPaginationModule,
		HttpModule,
		RouterModule.forRoot(routes),
		ModalModule.forRoot(),
		BsDropdownModule.forRoot()
	],
	providers: [ProductService, AuthorizationService, ShoppingCartService, OrderService, JwtHelper, AuthGuard]
};
