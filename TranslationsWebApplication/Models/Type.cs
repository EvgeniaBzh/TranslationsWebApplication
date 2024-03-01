using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TranslationsWebApplication.Models;

public partial class Type
{
    public int TypeId { get; set; }
    [Display(Name = "Type")]
    public string TypeName { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
