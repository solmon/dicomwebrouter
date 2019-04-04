export interface NoParamConstructor<T> {
    new (): T;
}

export interface IdResolve {
    GetId(): string
}

export class AETitles implements IdResolve {
    GetId(): string {
       return this.aeId;
    }
    public aeId: string;
    public aeTitle: string;
    public remoteHost: string;
    public isActive:boolean;    
}

export class DicomSend  implements IdResolve{
    GetId(): string {
        return this.dicomSendId;
    }
    public dicomSendId: string;
    public registeredTime: Date;
    public isActive: boolean;
    public aeTitle: string;
    public port: string;
    public remoteHost: string;
    public callingAETitle: string;
}

export class LinkClient  implements IdResolve{
    GetId(): string {
        return this.linkClientId;
    }
    public linkClientId: string;
    public registeredTime: Date;
    public isActive: boolean;
    public urlEndPoint: string;
    public errorMessage: boolean;
}

export class RoutingTable  implements IdResolve{
    GetId(): string {
        return this.routingId;
    }
    public routingId: string;
    public registeredTime: Date;
    public isActive: boolean;
    public inComing: string;
    public outGoing: string;
    public isLinkRoute: boolean;
    public isDSendRoute: boolean;
}

export class Settings  implements IdResolve{
    GetId(): string {
        return this.id.toString();
    }
    public id: number;
    public name: string;
    public value: string;
}