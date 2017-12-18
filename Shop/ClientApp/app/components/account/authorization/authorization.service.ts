
import { Injectable, Inject } from '@angular/core';
import { Http, Headers } from '@angular/http';
import { AuthHttp } from "angular2-jwt/angular2-jwt";
import 'rxjs/Rx';
import { RegistrationModel, SignInModel } from "../../model/AuthorizationModels";


@Injectable()
export class AuthorizationService {

	private headers = new Headers({ 'Content-Type': 'application/json' });

	constructor(private http: Http, private authHttp: AuthHttp, @Inject('ORIGIN_URL') private originUrl: string) { }

	registration(model: RegistrationModel): Promise<any> {
		return this.http.post('/api/Account/Registration', JSON.stringify(model), { headers: this.headers })
			.toPromise()
			.then(res => res as any)
	}

	signIn(model: SignInModel): Promise<any> {
		return this.http.post('/api/Account/SignIn', JSON.stringify(model), { headers: this.headers })
			.map(response => response.json() as any)
			.toPromise()
	}

	signOut(): Promise<any> {
		return this.authHttp.get('/api/Account/SignOut', { headers: this.headers })
			.toPromise()
			.then(res => res as any)
	}

	private handleError(error: any): Promise<any> {
		console.error('An error occurred', error);
		return Promise.reject(error.message || error.json() as any);
	}
}