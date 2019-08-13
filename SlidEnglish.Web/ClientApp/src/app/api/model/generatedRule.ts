import { CategoryDistribution } from './categoryDistribution';


export interface GeneratedRule {
    accountId?: number;
    description?: string;
    mcc?: number;
    bankCategory?: string;
    categories?: Array<CategoryDistribution>;
    count?: number;
}
