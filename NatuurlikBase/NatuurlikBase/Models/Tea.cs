using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace NatuurlikBase.Models
{
    public class Tea
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

    }
}
