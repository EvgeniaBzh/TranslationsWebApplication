using System;
using System.Collections.Generic;

namespace TranslationsWebApplication.Models;

public partial class Admin
{
    public int AdminId { get; set; }

    public string AdminName { get; set; } = null!;

    public string AdminEmail { get; set; } = null!;

    public string AdminPassword { get; set; } = null!;

    public int AccountId { get; set; }

    public virtual PersonalAccount Account { get; set; } = null!;

    public virtual ICollection<Translator> Translators { get; set; } = new List<Translator>();
}
