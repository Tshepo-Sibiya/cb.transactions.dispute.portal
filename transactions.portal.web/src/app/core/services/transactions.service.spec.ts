import { provideHttpClient } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { PagedResult } from '../models/paged-result.model';
import { TransactionDetail, TransactionListItem } from '../models/transaction.model';
import { TransactionsService } from './transactions.service';

describe('TransactionsService', () => {
  let service: TransactionsService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        TransactionsService,
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    });

    service = TestBed.inject(TransactionsService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should fetch transactions with query parameters', () => {
    const dummyResult: PagedResult<TransactionListItem> = {
      items: [
        {
          id: 'txn-1',
          reference: 'TXN-1000000001',
          date: '2026-11-05T12:00:00Z',
          amount: 540.00,
          merchant: 'Woolworths',
          status: 'Completed',
          paymentBrand: 'Visa',
          cardLast4: '4821',
          hasActiveDispute: false,
          isEligibleForDispute: true
        }
      ],
      totalCount: 1,
      page: 1,
      pageSize: 10,
      totalPages: 1
    };

    service.getTransactions('Woolworths', 'Completed', '2026-11-01', '2026-11-10', 1, 10).subscribe(result => {
      expect(result.items.length).toBe(1);
      expect(result.items[0].merchant).toBe('Woolworths');
    });

    const req = httpMock.expectOne(request =>
      request.url === '/api/transactions' &&
      request.params.get('search') === 'Woolworths' &&
      request.params.get('status') === 'Completed' &&
      request.params.get('fromDate') === '2026-11-01' &&
      request.params.get('toDate') === '2026-11-10' &&
      request.params.get('page') === '1' &&
      request.params.get('pageSize') === '10'
    );
    expect(req.request.method).toBe('GET');
    req.flush(dummyResult);
  });

  it('should fetch transaction details by id', () => {
    const dummyDetail: TransactionDetail = {
      id: 'txn-1',
      reference: 'TXN-1000000001',
      date: '2026-11-05T12:00:00Z',
      amount: 540.00,
      merchant: 'Woolworths',
      status: 'Completed',
      paymentBrand: 'Visa',
      cardLast4: '4821',
      customerId: 'CUST-839201',
      isEligibleForDispute: true,
      events: []
    };

    service.getTransactionById('txn-1').subscribe(detail => {
      expect(detail.id).toBe('txn-1');
      expect(detail.reference).toBe('TXN-1000000001');
    });

    const req = httpMock.expectOne('/api/transactions/txn-1');
    expect(req.request.method).toBe('GET');
    req.flush(dummyDetail);
  });

  it('should send post request to create dispute', () => {
    const disputeRequest = {
      reason: 'Unauthorized charge',
      description: 'Fraudulent transaction not made by cardholder'
    };

    const mockResponse = {
      id: 'disp-1',
      transactionId: 'txn-1',
      transactionReference: 'TXN-1000000001',
      merchant: 'Woolworths',
      amount: 540.00,
      reason: 'Unauthorized charge',
      description: 'Fraudulent transaction not made by cardholder',
      status: 'Open',
      createdAt: '2026-11-10T12:00:00Z',
      statusHistory: []
    };

    service.createDispute('txn-1', disputeRequest).subscribe(response => {
      expect(response.id).toBe('disp-1');
      expect(response.status).toBe('Open');
    });

    const req = httpMock.expectOne('/api/transactions/txn-1/disputes');
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(disputeRequest);
    req.flush(mockResponse);
  });
});

