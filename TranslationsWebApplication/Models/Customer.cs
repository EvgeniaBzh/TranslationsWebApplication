using System;
using System.Collections.Generic;

namespace TranslationsWebApplication.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string CustomerName { get; set; } = null!;

    public string CustomerEmail { get; set; } = null!;

    public string CustomerPassword { get; set; } = null!;

    public int AccountId { get; set; }

    public virtual PersonalAccount Account { get; set; } = null!;

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    //public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
