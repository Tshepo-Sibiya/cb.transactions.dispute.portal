import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-original-transaction',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './original-transaction.component.html',
  styleUrl: './original-transaction.component.scss'
})
export class OriginalTransactionComponent {
  @Input() reference: string = '';
  @Input() merchant: string = '';
  @Input() date: string = '';
  @Input() amount: number = 0;
  @Input() paymentBrand: string = '';
  @Input() cardLast4: string = '';
}
