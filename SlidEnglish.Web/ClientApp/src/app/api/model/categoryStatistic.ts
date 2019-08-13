import { MonthStatistic } from './monthStatistic';


export interface CategoryStatistic {
    categoryId?: number;
    averageAmount?: number;
    months?: Array<MonthStatistic>;
}
