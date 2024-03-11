using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TranslationsWebApplication.Models;

public enum OrderStatus
{
    Offer,
    InProgress,
    Done
}

public partial class Order
{
    public int OrderId { get; set; }
    [Display(Name = "Name")]
    public string OrderName { get; set; } = null!;
    [Display(Name = "Original Language")]
    public int? OriginalLanguageId { get; set; }
    [Display(Name = "Translation Language")]
    public int? TranslationLanguageId { get; set; }
    [Display(Name = "Type")]
    public int? TypeId { get; set; }
    [Display(Name = "Topic")]
    public int? TopicId { get; set; }
    [Display(Name = "Scope (in symbols)")]
    public int OrderScope { get; set; }
    [Display(Name = "Price (in Hryvnias)")]
    public double OrderPrice { get; set; }

    public string? FileName { get; set; }
    public byte[]? FileData { get; set; }

    public byte[]? SubmittedFileData { get; set; }
    public string? SubmittedFileName { get; set; }
    [Display(Name = "Submission Date")]
    public DateTime OrderSubmissionDate { get; set; }
    [Display(Name = "Status")]
    public OrderStatus OrderStatus { get; set; }

    public virtual Type? Type { get; set; } = null!;

    public virtual Topic? Topic { get; set; }

    public virtual Language? OriginalLanguage { get; set; } = null!;

    public virtual Language? TranslationLanguage { get; set; } = null!;
}
