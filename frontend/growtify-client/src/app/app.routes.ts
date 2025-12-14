import { Routes } from '@angular/router';
import { Home } from '../features/home/home';
import { MyGrowth } from '../features/my-growth/my-growth';
import { Community } from '../features/community/community';
import { UserDetailed } from '../features/account/user-detailed/user-detailed';
import { TestErrors } from '../features/test-errors/test-errors';
import { authGuard } from '../core/guards/auth-guard';
import { NotFound } from '../shared/errors/not-found/not-found';
import { ServerError } from '../shared/errors/server-error/server-error';
import { MemberDetailed } from '../features/members/member-detailed/member-detailed';
import { MemberProfile } from '../features/members/member-profile/member-profile';
import { MemberPhotos } from '../features/members/member-photos/member-photos';
import { MemberMessages } from '../features/members/member-messages/member-messages';

export const routes: Routes = [
    { path: '', component: Home },
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [authGuard],
        children: [
            { path: 'my-growth', component: MyGrowth },
            { path: 'account/:id', component: UserDetailed },
            { path: 'community', component: Community },
            { path: 'members/:id',
                component: MemberDetailed,
                children: [
                    { path: '', redirectTo: 'profile', pathMatch: 'full' },
                    { path: 'profile', component: MemberProfile, title: 'Profile'},
                    { path: 'photos', component: MemberPhotos, title: 'Photos'},
                    { path: 'messages', component: MemberMessages, title: 'Messages'}
                ]
            }
        ]
    },
    { path: 'errors', component: TestErrors },
    { path: 'server-error', component: ServerError },
    { path: '**', component: NotFound }
];
