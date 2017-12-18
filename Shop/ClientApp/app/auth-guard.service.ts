import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { tokenNotExpired } from 'angular2-jwt';
import { Observable } from "rxjs/Observable";

@Injectable()
export class AuthGuard implements CanActivate {

	constructor(private router: Router) { }

	canActivate() {

		if (tokenNotExpired(null, sessionStorage.getItem('token'))) {
			return true;
		} else {
			this.router.navigate(['products']);
			return false;
		}
	}
}