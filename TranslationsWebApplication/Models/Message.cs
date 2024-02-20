using System;
using System.Collections.Generic;

namespace TranslationsWebApplication.Models;

public partial class Message
{
    public int MessageId { get; set; }

    public int OrderId { get; set; }

    public int CustomerId { get; set; }

    public string TextMessage { get; set; } = null!;

    public int TranslatorId { get; set; }

    public virtual Customer Customer { get; set; } = null!;

    //public virtual Order Order { get; set; } = null!;

    public virtual Translator Translator { get; set; } = null!;
}
