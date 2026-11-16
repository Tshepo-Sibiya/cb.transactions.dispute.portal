import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { MerchantStatement } from '../../../../core/models/transaction.model';

@Component({
  selector: 'app-merchant-statement',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './merchant-statement.component.html',
  styleUrl: './merchant-statement.component.scss'
})
export class MerchantStatementComponent {
  @Input() statement?: MerchantStatement;
  @Input() paymentBrand: string = '';
  @Input() cardLast4: string = '';
}
