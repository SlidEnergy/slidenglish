import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { filter, mergeMap, withLatestFrom, switchMap, catchError, tap, map } from 'rxjs/operators';

import { Store } from '@ngrx/store';
import { Effect, Actions, ofType } from '@ngrx/effects';
import { ROUTER_NAVIGATION, RouterNavigationAction } from '@ngrx/router-store';

import { AuthService } from './auth.service';
import * as coreActions from './core.store';
import { RouterStateSnapshot } from '@angular/router/src/router_state';
import { CoreState } from './core.store';
import { CategoriesService, Category } from '../api';
import { AppState } from '../shared/app-state';

// EFFECTS

@Injectable()
export class CoreEffects {
	constructor(
		private actions: Actions,
		private categoriesService: CategoriesService,
		private store: Store<AppState>,
		private authService: AuthService,
	) { }

	@Effect()
	loadNodes = this.actions
		.pipe(
			ofType(coreActions.LOAD_CATEGORIES),
			switchMap(() =>
				this.categoriesService.getList().pipe(
					map(x => new Map(x.map(i => [i.id, i] as [number, Category]))),
					map((data) => {
						return new coreActions.LoadCategoriesCompleted(0, '', data);
					}),
					catchError(err => {
						console.error(`Произошла ошибка. errorCode: ${err.code}, errorMessage: ${err.message}`);

						return of(new coreActions.LoadCategoriesCompleted(err.code, err.message));
					})
				)
			));

	// ROUTER NAVIGATION

	@Effect({ dispatch: false })
	navigateWithLoadNodes = this.actions.pipe(
		ofType(ROUTER_NAVIGATION),

		// Проверяем нужный путь
		map(action => {
			const state: RouterStateSnapshot = (<RouterNavigationAction<any>>action).payload.routerState;

			return state.root && state.root.firstChild;
		}),
		// filter(routeSnapshot => {
		// 	return !!routeSnapshot && !!routeSnapshot.data && routeSnapshot.data.name === 'authtorized';
		// }),

		// Проверяем, что мы успешно авторизовались.
		mergeMap(() => {
			return this.authService.isLoggedIn;
		}),
		filter((isLoggedIn: boolean) => isLoggedIn),

		// Выполняем действие
		withLatestFrom(this.store),
		map(([isLoggedIn, state]: [boolean, AppState]) => state.core),
		tap((state: CoreState) => {

			if (!state.categories) {
				this.store.dispatch(new coreActions.LoadCategories());
			}
		}));
}
