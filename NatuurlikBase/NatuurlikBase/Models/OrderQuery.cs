using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace NatuurlikBase.Models
{
    public class OrderQuery
    {
        public int Id { get; set; }
        [Required]
        public int? OrderId { get; set; }
        [ValidateNever]
        public Order Order { get; set; }

        [Required]
        public int? QueryReasonId { get; set; }
        [ValidateNever]
        public QueryReason QueryReason { get; set; }
        [Required]
        public string OrderQueryDescription { get; set; }
        public string? QueryStatus { get; set; }
        //public string? UploadEvidenceUrl { get; set; }

        public string? QueryFeedback { get; set; }
        [Required]
        public DateTime LoggedDate { get; set; } = DateTime.Now;
    }
}
