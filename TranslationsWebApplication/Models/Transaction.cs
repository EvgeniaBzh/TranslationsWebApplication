using System;
using System.Collections.Generic;

namespace TranslationsWebApplication.Models;

public partial class Transaction
{
    public int TransactionId { get; set; }

    public int OrderId { get; set; }

    public double AmountOfMoney { get; set; }

    public DateTime TransactionDate { get; set; }

    public virtual Order Order { get; set; } = null!;
}
