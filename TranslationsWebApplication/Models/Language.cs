using System;
using System.Collections.Generic;

namespace TranslationsWebApplication.Models;

public partial class Language
{
    public int LanguageId { get; set; }

    public string LanguageName { get; set; } = null!;

    //public virtual ICollection<Order> OrderOriginalLanguages { get; set; } = new List<Order>();

    //public virtual ICollection<Order> OrderTranslationLanguages { get; set; } = new List<Order>();
}
