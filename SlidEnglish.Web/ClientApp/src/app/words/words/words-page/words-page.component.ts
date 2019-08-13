import { Component, OnInit } from '@angular/core';
import { BanksService, Bank } from 'src/app/api';
import { Observable, of } from 'rxjs';
import { MatSnackBar } from '@angular/material';
import { map, catchError } from 'rxjs/operators';

@Component({
  selector: 'app-words-page',
  templateUrl: './words-page.component.html',
  styleUrls: ['./words-page.component.scss']
})
export class WordsPageComponent implements OnInit {
  // banks: Observable<Bank[]>;

  // constructor(
  //   private banksService: BanksService,
  //   private snackBar: MatSnackBar
  // ) { }

  ngOnInit() { }
  //   this.banks = this.banksService.getList();
  // }

  // addItem = (item: Bank) => {
  //   return this.banksService.add(item).pipe(
  //     map((result) => {
  //       this.snackBar.open('Банк привязан', undefined, { duration: 5000, panelClass: ['background-green'] });
  //       return result;
  //     }),
  //     catchError(() => {
  //       this.snackBar.open('Не удалось привязать банк', undefined, { duration: 5000, panelClass: ['background-red'] });
  //       return of(false);
  //     }));
  // }

  // deleteItem = (item: Bank) => {
  //   return this.banksService.delete(item.id).pipe(
  //     map((result) => {
  //       this.snackBar.open('Банк отвязан', undefined, { duration: 5000, panelClass: ['background-green'] });
  //       return true;
  //     }),
  //     catchError(() => {
  //       this.snackBar.open('Не удалось отвязать банк', undefined, { duration: 5000, panelClass: ['background-red'] });
  //       return of(false);
  //     }));
  // }

  // editItem = (item: Bank) => {
  //   return this.banksService.update(item.id, item).pipe(
  //     map((result) => {
  //       this.snackBar.open('Банк переименован', undefined, { duration: 5000, panelClass: ['background-green'] });
  //       return result;
  //     }),
  //     catchError(() => {
  //       this.snackBar.open('Не удалось переименовать банк', undefined, { duration: 5000, panelClass: ['background-red'] });
  //       return of(false);
  //     }));
  // }
}
