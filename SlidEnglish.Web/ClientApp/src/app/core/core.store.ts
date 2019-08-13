import { Action } from '@ngrx/store';
import { Category } from '../api';
import { ActionWithPayload } from '../shared/action-with-payload';

// STATE

export interface CoreState {
	categories: Map<number, Category> | null;

	loadCategoriesError: { code: number, message: string } | null;
}

const initialState: CoreState = {
	categories: null,

	loadCategoriesError: null
};

// ACTIONS

export const LOAD_CATEGORIES = '[core] LOAD_CATEGORIES';

export class LoadCategories implements Action {
	readonly type = LOAD_CATEGORIES;
}

export const LOAD_CATEGORIES_COMPLETED = '[core] LOAD_CATEGORIES_COMPLETED';

export class LoadCategoriesCompleted implements ActionWithPayload {
	readonly type = LOAD_CATEGORIES_COMPLETED;
	public payload: any;

	constructor(errorCode: number, errorMessage: string, categories: Map<number, Category> | null = null) {
		this.payload = { errorCode, errorMessage, categories };
	}
}

// REDUCER

export function coreReducer(state = initialState, action: ActionWithPayload): CoreState {
	switch (action.type) {
		case LOAD_CATEGORIES:
			return state;

		case LOAD_CATEGORIES_COMPLETED:
			return Object.assign({}, state, {
				loadNodesError: action.payload.errorCode ? { code: action.payload.errorCode, message: action.payload.errorMessage } : null,
				categories: action.payload.categories
			});

		default:
			return state;
	}
}
