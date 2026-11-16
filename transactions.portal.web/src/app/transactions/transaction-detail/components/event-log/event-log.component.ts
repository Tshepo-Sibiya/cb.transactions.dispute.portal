import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { TransactionEvent } from '../../../../core/models/transaction.model';

@Component({
  selector: 'app-event-log',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './event-log.component.html',
  styleUrl: './event-log.component.scss'
})
export class EventLogComponent {
  @Input() events: TransactionEvent[] = [];
}
