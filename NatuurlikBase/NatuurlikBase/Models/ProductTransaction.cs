using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NatuurlikBase.Models
{
    public class ProductTransaction
    {
        [Key]
        public int ProductTransactionId { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        public int QuantityBefore { get; set; }

        [Required]
        public ProductTransactionType ActivityType { get; set; }

        [Required]
        public int QuantityAfter { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; }

        [Required]
        public string Actor { get; set; }

     
        public Product Product { get; set; }
    }
}
