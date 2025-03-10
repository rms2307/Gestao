﻿namespace Gestao.Domain
{
    public class Document
    {
        public int Id { get; set; }
        public string Path { get; set; } = null!;// /wwwroot/files/transactions/1/comprovante.pdf
        public int? FinancialTransactionId { get; set; }
        public FinancialTransaction? FinancialTransaction { get; set; }
    }
}