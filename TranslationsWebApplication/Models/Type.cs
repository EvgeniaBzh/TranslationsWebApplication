using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TranslationsWebApplication.Models;

public partial class Type
{
    public int TypeId { get; set; }
    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Тип")]
    public string TypeName { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
