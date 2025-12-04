import { Routes } from '@angular/router';
import { Home } from '../features/home/home';
import { MyGrowth } from '../features/my-growth/my-growth';
import { Community } from '../features/community/community';
import { UserDetailed } from '../features/account/user-detailed/user-detailed';
import { TestErrors } from '../features/test-errors/test-errors';
import { authGuard } from '../core/guards/auth-guard';

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
        ]
    },
    { path: 'errors', component: TestErrors },
    { path: '**', component: Home }
];
