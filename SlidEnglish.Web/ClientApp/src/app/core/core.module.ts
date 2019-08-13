import { Optional, SkipSelf, NgModule } from '@angular/core';
import { ApiModule } from '../api';

import { apiConfigFactory } from './api-config-factory';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { SharedModule } from '../shared/shared.module';
import { DateInterceptor } from './date-interceptor';
import { AuthService } from './auth.service';
import { AuthGuard } from './auth-guard.service';
import { HeaderComponent } from './header/header.component';
import { InitializationService } from './initialization.service';
import { StoreRouterConnectingModule } from '@ngrx/router-store';
import { StoreModule } from '@ngrx/store';
import { coreReducer } from './core.store';
import { CoreEffects } from './core.effects';
import { EffectsModule } from '@ngrx/effects';

@NgModule({
    imports: [
        HttpClientModule,
        SharedModule,

        SharedModule,
        ApiModule.forRoot(apiConfigFactory),

        // Сторонние компоненты
        StoreRouterConnectingModule,
		StoreModule.forRoot({ core: coreReducer }),
		EffectsModule.forRoot([CoreEffects]),
    ],
    declarations: [
        HeaderComponent
    ],
    providers: [
        AuthService,
        AuthGuard,
        InitializationService,
        // Регистрируем interceptor, который разбирает строки в виде даты в ответе от сервера и преобразует их в даты.
        {
            provide: HTTP_INTERCEPTORS,
            useClass: DateInterceptor,
            multi: true
        }
    ],
    exports: [
        HeaderComponent
    ]
})
export class CoreModule {
    constructor(@Optional() @SkipSelf() parentModule: CoreModule,
        initialization: InitializationService) {
        if (parentModule) {
            throw new Error(
                'CoreModule is already loaded. Import it in the AppModule only');
        }

        initialization.init();
    }
}
