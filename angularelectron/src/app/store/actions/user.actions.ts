import { Action } from '@ngrx/store';

export const AUTH_ACTION_TYPES = {
    GITHUB_AUTH: 'GITHUB_AUTH',
    CHANGE_NAME: 'CHANGE_NAME',
};

// actions
export class GithubAuth implements Action {
    type: string = AUTH_ACTION_TYPES.GITHUB_AUTH;
    constructor(public payload: any) { }
};

export class ChangeName implements Action {
    type: string = AUTH_ACTION_TYPES.CHANGE_NAME;
    constructor(public payload: any) { }
};

export type Actions = GithubAuth | ChangeName;
