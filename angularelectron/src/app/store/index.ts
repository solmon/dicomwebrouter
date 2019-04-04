import { User } from './models/auth.model';
import { authInitialState, userReducer } from './reducers/user.reducer';
import { ActionReducerMap } from '@ngrx/store';

export interface State {
    user: User;
};

export const initialState: State = {
    user: authInitialState,
};

export const reducers: ActionReducerMap<State> = {
    user: userReducer,
};
