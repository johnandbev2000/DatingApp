import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { User } from '../_models/user';
import { UserService } from '../_services/user.service';
import { AlertifyService } from '../_services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';


@Injectable()
export class MemberDetailResolver implements Resolve<User> {

    constructor(private userService: UserService, private router: Router, private aslertify: AlertifyService) {}

    resolve(route: ActivatedRouteSnapshot): Observable<User> {
        // return this.userService.getUser(route.params['id']);
        // note above this is only a tslint warning and would work but use below....
        return this.userService.getUser(route.params.id).pipe(
            catchError(error => {
                this.aslertify.error('Problem retrieving data');
                this.router.navigate(['/members']);
                return of(null);
            })
        );
    }
}
