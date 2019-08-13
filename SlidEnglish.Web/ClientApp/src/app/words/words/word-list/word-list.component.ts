import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { Bank } from 'src/app/api';
import { MatTableDataSource, MatDialog, MatSort } from '@angular/material';
import { Observable } from 'rxjs';
import { filter, flatMap, map } from 'rxjs/operators';
import { Router } from '@angular/router';
import { MessageDialogComponent } from 'src/app/shared/message-dialog/message-dialog.component';

@Component({
  selector: 'app-word-list',
  templateUrl: './word-list.component.html',
  styleUrls: ['./word-list.component.scss']
})
export class WordListComponent implements OnInit {
  // @ViewChild(MatSort) sort: MatSort;

  // @Input() itemAdding: (item: Bank) => Observable<any>;
  // @Input() itemDeleting: (item: Bank) => Observable<boolean>;
  // @Input() itemChanging: (item: Bank) => Observable<Bank>;

  // // список транзакций пользователя
  // dataSource = new MatTableDataSource<Bank>();

  // @Input('banks') set transactionsInput(value: Bank[]) {
  //   if (value) {
  //     this.loadingVisible = false;
  //     this.dataSource.data = value;
  //   }
  // }

  // // Список колонок, которые нужно показать в таблице
  // columnsToDisplay = ['title', 'ownFunds', 'actions'];
  // loadingVisible = true;

  // constructor(
  //   public dialog: MatDialog,
  //   private router: Router
  // ) { }

  ngOnInit() { }
  //   this.dataSource.sort = this.sort;
  //   this.dataSource.sortingDataAccessor = this.sortingDataAccessor.bind(this);
  // }

  // sortingDataAccessor(bank: Bank, property: string) {
  //   switch (property) {
  //     case 'title': {
  //       return bank.title.toLowerCase();
  //     }

  //     default: return bank[property];
  //   }
  // }

  // row_click(row: Bank) {
  //   this.router.navigate(['banks', row.id, 'accounts']);
  // }

  // addNew() {

  // }

  // editItem(bank: Bank) {

  // }

  // deleteItem(item: Bank) {
  //   const dialogRef = this.dialog.open(MessageDialogComponent, {
  //     data: { caption: 'Вы уверены что хотите отвязать банк?', text: item.title }
  //   });

  //   dialogRef.afterClosed().pipe(filter(x => x), flatMap(() => this.itemDeleting(item).pipe(filter(x => x))))
  //     .subscribe(() => {
  //       this.dataSource.data = this.dataSource.data.filter((value) => value.id != item.id);
  //     });
  // }

  // getTotalOwnFunds() {
  //   return this.dataSource.data.map(b => b.ownFunds).reduce((acc, value) => acc + value, 0);
  // }
}
