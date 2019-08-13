import { Transaction } from './transaction';


export interface PatchAccountDataBindingModel {
    code?: string;
    balance?: number;
    transactions?: Array<Transaction>;
}
