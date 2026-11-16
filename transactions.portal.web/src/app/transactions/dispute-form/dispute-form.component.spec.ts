import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';
import { of } from 'rxjs';
import { TransactionDetail } from '../../core/models/transaction.model';
import { TransactionsService } from '../../core/services/transactions.service';
import { DisputeFormComponent } from './dispute-form.component';

describe('DisputeFormComponent', () => {
  let component: DisputeFormComponent;
  let fixture: ComponentFixture<DisputeFormComponent>;

  const mockTransaction: TransactionDetail = {
    id: 'txn-100',
    reference: 'TXN-1002003004',
    date: '2026-11-01T10:00:00Z',
    amount: 850.00,
    merchant: 'Test Merchant SA',
    status: 'Completed',
    paymentBrand: 'Visa',
    cardLast4: '4821',
    customerId: 'CUST-839201',
    isEligibleForDispute: true,
    events: []
  };

  beforeEach(async () => {
    const mockService = {
      getTransactionById: () => of(mockTransaction),
      createDispute: () => of({})
    };

    await TestBed.configureTestingModule({
      imports: [DisputeFormComponent],
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
        provideRouter([]),
        { provide: TransactionsService, useValue: mockService }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(DisputeFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should initialize with an invalid form', () => {
    expect(component.form.valid).toBe(false);
    expect(component.form.get('reason')?.valid).toBe(false);
    expect(component.form.get('description')?.valid).toBe(false);
  });

  it('should validate reason as required', () => {
    const reasonControl = component.form.get('reason');
    expect(reasonControl?.hasError('required')).toBe(true);

    reasonControl?.setValue('Unauthorized charge');
    expect(reasonControl?.hasError('required')).toBe(false);
  });

  it('should validate description minimum and maximum length', () => {
    const descControl = component.form.get('description');

    descControl?.setValue('Too short');
    expect(descControl?.hasError('minlength')).toBe(true);

    descControl?.setValue('Valid description that exceeds ten characters');
    expect(descControl?.hasError('minlength')).toBe(false);
    expect(descControl?.valid).toBe(true);

    const over500 = 'a'.repeat(501);
    descControl?.setValue(over500);
    expect(descControl?.hasError('maxlength')).toBe(true);
  });

  it('should be valid when valid reason and description are provided', () => {
    component.form.setValue({
      reason: 'Duplicate charge',
      description: 'The transaction was charged twice on my card.'
    });

    expect(component.form.valid).toBe(true);
  });
});
