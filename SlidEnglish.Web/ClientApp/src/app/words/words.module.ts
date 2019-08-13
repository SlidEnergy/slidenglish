import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { Routes, RouterModule } from '@angular/router';
import { WordsPageComponent } from './words/words-page/words-page.component';
import { WordListComponent } from './words/word-list/word-list.component';

const routes: Routes = [
  { path: '', component: WordsPageComponent },
];

@NgModule({
  declarations: [
    WordsPageComponent,
    WordListComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forChild(routes)
  ],
  entryComponents: [
  ]
})
export class WordsModule { }
