export interface TransactionListItem {
  id: string;
  reference: string;
  date: string;
  amount: number;
  merchant: string;
  status: string;
  paymentBrand: string;
  cardLast4: string;
  hasActiveDispute: boolean;
  disputeId?: string;
  disputeStatus?: string;
  isEligibleForDispute: boolean;
  ineligibleReason?: string;
}

export interface DisputeSummary {
  id: string;
  status: string;
  reason: string;
  createdAt: string;
}

export interface Refund {
  id: string;
  reference: string;
  amount: number;
  percentage: number;
  processedAt: string;
}

export interface TransactionEvent {
  id: string;
  timestamp: string;
  message: string;
}

export interface MerchantStatement {
  id: string;
  reference: string;
  type: string;
  postingDate: string;
  amount: number;
}

export interface TransactionDetail {
  id: string;
  reference: string;
  date: string;
  amount: number;
  merchant: string;
  status: string;
  paymentBrand: string;
  cardLast4: string;
  customerId: string;
  isEligibleForDispute: boolean;
  ineligibleReason?: string;
  disputeSummary?: DisputeSummary;
  refund?: Refund;
  events: TransactionEvent[];
  merchantStatement?: MerchantStatement;
}

