using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TranslationsWebApplication.Models;

public partial class Topic
{
    public int TopicId { get; set; }
    [Required(ErrorMessage = "Поле не повинне бути порожнім")]
    [Display(Name = "Тема")]
    public string? TopicName { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
