import { Injectable } from "@angular/core";

import * as moment from 'moment';
declare let require: any;

// Локализация пайпов
import { registerLocaleData } from '@angular/common';
import localeRu from '@angular/common/locales/ru';

@Injectable()
export class InitializationService {
    constructor() { }

    init() {

        this.initMoment();

        this.initPipes();
    }

    // Инициализируем библиотеку moment

    private initMoment() {
        moment.locale('ru');
    }

    // Инициализация pipes требующих локализацию

    private initPipes() {
        // Загружаем локализацию для pipes
        registerLocaleData(localeRu);
    }
}