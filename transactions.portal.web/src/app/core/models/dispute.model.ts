export interface DisputeStatusHistory {
  id: string;
  disputeId: string;
  status: string;
  changedAt: string;
  note?: string;
}

export interface DisputeListItem {
  id: string;
  transactionId: string;
  transactionReference: string;
  merchant: string;
  amount: number;
  reason: string;
  description: string;
  status: string;
  createdAt: string;
  resolvedAt?: string;
}

export interface DisputeDetail {
  id: string;
  transactionId: string;
  transactionReference: string;
  merchant: string;
  amount: number;
  reason: string;
  description: string;
  status: string;
  createdAt: string;
  resolvedAt?: string;
  statusHistory: DisputeStatusHistory[];
}

export interface CreateDisputeRequest {
  reason: string;
  description: string;
}

