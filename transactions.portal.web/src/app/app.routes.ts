import { Routes } from '@angular/router';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'transactions'
  },
  {
    path: 'transactions',
    loadComponent: () =>
      import('./transactions/transactions-list/transactions-list.component').then(
        m => m.TransactionsListComponent
      )
  },
  {
    path: 'transactions/:id',
    loadComponent: () =>
      import('./transactions/transaction-detail/transaction-detail.component').then(
        m => m.TransactionDetailComponent
      )
  },
  {
    path: 'transactions/:id/dispute',
    loadComponent: () =>
      import('./transactions/dispute-form/dispute-form.component').then(
        m => m.DisputeFormComponent
      )
  },
  {
    path: 'disputes',
    loadComponent: () =>
      import('./disputes/disputes-list/disputes-list.component').then(
        m => m.DisputesListComponent
      )
  },
  {
    path: 'disputes/:id',
    loadComponent: () =>
      import('./disputes/disputes-list/disputes-list.component').then(
        m => m.DisputesListComponent
      )
  },
  {
    path: '**',
    redirectTo: 'transactions'
  }
];
