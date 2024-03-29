﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TranslationsWebApplication.Models;

public partial class Language
{
    public int LanguageId { get; set; }

    public string LanguageName { get; set; } = null!;

    public virtual ICollection<Order> OriginalLanguage { get; set; } = new List<Order>();

    public virtual ICollection<Order> TranslationLanguage { get; set; } = new List<Order>();
}
