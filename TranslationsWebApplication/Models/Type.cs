using System;
using System.Collections.Generic;

namespace TranslationsWebApplication.Models;

public partial class Type
{
    public int TypeId { get; set; }

    public string TypeName { get; set; } = null!;

    //public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
