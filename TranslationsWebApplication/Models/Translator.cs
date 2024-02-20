using System;
using System.Collections.Generic;

namespace TranslationsWebApplication.Models;

public partial class Translator
{
    public int TranslatorId { get; set; }

    public string TranslatorName { get; set; } = null!;

    public string TranslatorEmail { get; set; } = null!;

    public string TranslatorPassword { get; set; } = null!;

    public int TranslatorExperience { get; set; }

    public int AddedAdminId { get; set; }

    public int AccountId { get; set; }

    public virtual Admin AddedAdmin { get; set; } = null!;

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    //public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual PersonalAccount Account { get; set; } = null!;
}
