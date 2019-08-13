import { NgModule, ModuleWithProviders, SkipSelf, Optional } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { Configuration } from './configuration';

import { AccountsService } from './api/accounts.service';
import { BanksService } from './api/banks.service';
import { CategoriesService } from './api/categories.service';
import { ImportService } from './api/import.service';
import { RulesService } from './api/rules.service';
import { StatisticsService } from './api/statistics.service';
import { TokenService } from './api/token.service';
import { TransactionsService } from './api/transactions.service';
import { UsersService } from './api/users.service';

@NgModule({
    imports: [CommonModule, HttpClientModule],
    declarations: [],
    exports: [],
    providers: [
        AccountsService,
        BanksService,
        CategoriesService,
        ImportService,
        RulesService,
        StatisticsService,
        TokenService,
        TransactionsService,
        UsersService]
})
export class ApiModule {
    public static forRoot(configurationFactory: () => Configuration): ModuleWithProviders {
        return {
            ngModule: ApiModule,
            providers: [{ provide: Configuration, useFactory: configurationFactory }]
        }
    }

    constructor(@Optional() @SkipSelf() parentModule: ApiModule) {
        if (parentModule) {
            throw new Error('ApiModule is already loaded. Import your base AppModule only.');
        }
    }
}
