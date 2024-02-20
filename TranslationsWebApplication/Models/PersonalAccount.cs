using System;
using System.Collections.Generic;

namespace TranslationsWebApplication.Models;

public partial class PersonalAccount
{
    public int AccountId { get; set; }

    public string AccountName { get; set; } = null!;

    public byte[]? AccountPhoto { get; set; }

    public DateTime RegistrationDate { get; set; }

    public virtual ICollection<Admin> Admins { get; set; } = new List<Admin>();

    public virtual ICollection<Customer> Customers { get; set; } = new List<Customer>();

    public virtual ICollection<Translator> Translators { get; set; } = new List<Translator>();
}
