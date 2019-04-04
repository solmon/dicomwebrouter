import { AUTH_ACTION_TYPES, Actions } from './../actions/user.actions';
import { User } from './../models/auth.model';

export const authInitialState = {
    authToken: window.localStorage.getItem('authToken') || false,
    authenticated: false,
    username: '',
};

export function userReducer(state: User = authInitialState, action: Actions) {

    switch (action.type) {

        case AUTH_ACTION_TYPES.GITHUB_AUTH:
            localStorage.setItem('authToken', action.payload.token);
            return Object.assign(state, { authToken: action.payload.token, authenticated: true });

        case AUTH_ACTION_TYPES.CHANGE_NAME:
            return Object.assign(state, { username: action.payload.username });

        default:
            return state;
    }
};
