import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { FlexLayoutModule } from '@angular/flex-layout';
import {
    MatNativeDateModule, MatDatepickerModule, MatIconModule, MatButtonModule, MatCheckboxModule, MatToolbarModule,
    MatCardModule, MatFormFieldModule, MatInputModule, MatRadioModule, MatListModule, MatSnackBarModule, MatTableModule,
    MatAutocompleteModule, MatProgressSpinnerModule, MatSelectModule, MatSortModule, MatMenuModule, MatDialogModule, MatPaginatorModule
} from '@angular/material';

import { MatProgressButtonsModule } from 'mat-progress-buttons'
import { MessageDialogComponent } from './message-dialog/message-dialog.component';

@NgModule({
    declarations: [
        MessageDialogComponent,
    ],
    imports: [
        CommonModule,
        FormsModule,
        RouterModule,
        MatDialogModule,
        MatButtonModule,
        MatTableModule,
        MatProgressSpinnerModule,
        MatCardModule,
    ],
    exports: [
        CommonModule,
        FormsModule,
        RouterModule,
        ReactiveFormsModule,
        FlexLayoutModule,

        MatNativeDateModule,
        FormsModule,
        MatDatepickerModule,
        MatIconModule,
        MatButtonModule,
        MatCheckboxModule,
        MatToolbarModule,
        MatCardModule,
        MatFormFieldModule,
        MatInputModule,
        MatListModule,
        MatRadioModule,
        MatSnackBarModule,
        MatTableModule,
        MatAutocompleteModule,
        MatProgressSpinnerModule,
        MatProgressButtonsModule,
        MatSelectModule,
        MatSortModule,
        MatMenuModule,
        MatDialogModule,
        MatPaginatorModule,
        MessageDialogComponent,
    ],
    entryComponents: [
        MessageDialogComponent
    ],
    providers: [
    ]
})
export class SharedModule { }
