export class Study{
    studyId:string;
    calledAE:string;
    remoteHostIP:string;
    recievedTime:Date;
    patientName:string;
    priority:string;
    acquisitionDateTime:Date;
    studyDescription:string;
    studyInstanceUID:string;
    seriesCount:number;
    series:Series[];
    totalImages:number;
    totalImagesRecieved:number;
    isComplete:boolean;
}
export class Series{
    seriesId:string;
    studyId:string;
    seriesInstanceUID:string;
    instanceCount:number;
    modality:string;
    instance:Instance[];
}
export class Instance{
    instanceId:string;
    seriesId:string;
    sopInstanceUID:string;
}