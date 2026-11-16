import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { Refund } from '../../../../core/models/transaction.model';

@Component({
  selector: 'app-refund-details',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './refund-details.component.html',
  styleUrl: './refund-details.component.scss'
})
export class RefundDetailsComponent {
  @Input() refund?: Refund;
}
