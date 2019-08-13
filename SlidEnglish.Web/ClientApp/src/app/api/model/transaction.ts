

export interface Transaction {
    id?: number;
    accountId?: number;
    dateTime?: Date;
    amount?: number;
    categoryId?: number;
    description?: string;
    mcc?: number;
    bankCategory?: string;
    approved?: boolean;
}
