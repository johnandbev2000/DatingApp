import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { ListsComponent } from './lists/lists.component';
import { AuthGuard } from './_guards/auth.guard';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberDetailResolver } from './_resolvers/member-detail.resolver';
import { MemberListResolver } from './_resolvers/member-list.resolver';

// first match wins....
export const appRoutes: Routes = [
    { path: '', component: HomeComponent },

    /*
     { path: 'members', component: MemberListComponent, canActivate: [AuthGuard] },
     { path: 'messages', component: MessagesComponent },
     { path: 'lists', component: ListsComponent },*/
     {
         path: '',
         runGuardsAndResolvers: 'always',
         canActivate: [AuthGuard],
         children: [
            { path: 'members', component: MemberListComponent, resolve: {currentUsers: MemberListResolver}},
            { path: 'members/:id', component: MemberDetailComponent, resolve: {currentUser: MemberDetailResolver}},
            { path: 'messages', component: MessagesComponent },
            { path: 'lists', component: ListsComponent }
         ]
     },
     { path: '**',  redirectTo : '', pathMatch: 'full'}
];
