using System;
using System.Collections.Generic;

namespace TranslationsWebApplication.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public string OrderName { get; set; } = null!;

    //public int? TypeId { get; set; }

    public int? TopicId { get; set; }

    public int OrderScope { get; set; }

    public double OrderPrice { get; set; }

    public DateTime OrderSubmissionDate { get; set; }

    public string OrderStatus { get; set; } = null!;

    //public virtual Type Type { get; set; } = null!;

    public virtual Topic? Topic { get; set; }

}
