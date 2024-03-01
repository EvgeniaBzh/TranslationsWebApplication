using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TranslationsWebApplication.Models;

public partial class Topic
{
    public int TopicId { get; set; }
    [Display(Name = "Topic")]
    public string? TopicName { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
