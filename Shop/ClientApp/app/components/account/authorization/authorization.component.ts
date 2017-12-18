import { Component, ViewChild, TemplateRef } from '@angular/core';
import { NgIf } from '@angular/common';
import { ModalDirective } from 'ngx-bootstrap';

import { AuthorizationService } from '../authorization/authorization.service';
import { JwtHelper } from "angular2-jwt/angular2-jwt";
import { RegistrationModel, SignInModel } from "../../model/AuthorizationModels";


@Component({
	selector: 'authorization',
	templateUrl: './authorization.component.html',
	styleUrls: ['./authorization.component.css'],
	providers: [AuthorizationService, JwtHelper]
})
export class AuthorizationComponent {
	public registrationModel: RegistrationModel;
	public signInModel: SignInModel;
	public authType: string;
	public signInError: any;
	public currentUser: any;
	public IsLoggedIn: boolean;

	constructor(private authorizationService: AuthorizationService, private jwtHelper: JwtHelper) {
		this.registrationModel = new RegistrationModel();
		this.signInModel = new SignInModel();
	}
	ngOnInit() {
		this.authType = "Registration";

		var token = sessionStorage.getItem('token');
		if (token) {
			this.currentUser = this.jwtHelper.decodeToken(token);
			this.IsLoggedIn = this.jwtHelper.isTokenExpired(token) ? false : true;
		}
	}

	selectAuthType(type: string) {
		this.authType = type;
	}

	registration(model: RegistrationModel): void {
		this.authorizationService.registration(model)
			.then(response => { location.reload(); })
			.catch(error => {
				document.getElementById('regError').hidden = false;
				document.getElementById('regError').textContent = (error.json() as any).responseStatus.message;
				setTimeout(() => {
					document.getElementById('regError').hidden = true;
				}, 3000);
			})
	}

	signIn(model: SignInModel): void {
		this.authorizationService.signIn(model)
			.then(response => {
				sessionStorage.setItem('token', response.token);
				location.reload();
			})
			.catch(error => {
				document.getElementById('signInError').hidden = false;
				document.getElementById('signInError').textContent = (error.json() as any).responseStatus.message;
				setTimeout(() => {
					document.getElementById('signInError').hidden = true;
				}, 3000);
			});
	}

	signOut(): void {
		sessionStorage.removeItem('token');
		window.location.reload();
	}
}

