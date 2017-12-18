import { Routes, RouterModule, CanActivate } from '@angular/router';

import { ProductsComponent } from "./components/product/products.component";
import { AuthorizationComponent } from "./components/account/authorization/authorization.component";
import { ProductComponent } from "./components/product/product.component";
import { ShoppingCartComponent } from "./components/shopping-cart/shopping-cart.component";
import { OrderComponent } from "./components/order/order.component";
import { AuthGuard } from "./auth-guard.service";

export const routes: Routes = [
	{ path: 'products', component: ProductsComponent},
	{ path: '', redirectTo: 'products', pathMatch: 'full' },
	{ path: 'registration', component: AuthorizationComponent },
	{ path: 'product/:id', component: ProductComponent},
	{ path: 'shopping-cart', component: ShoppingCartComponent, canActivate: [AuthGuard] },
	{ path: 'orders', component: OrderComponent, canActivate: [AuthGuard] }
]