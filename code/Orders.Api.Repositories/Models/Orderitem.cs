using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Orders.Api.Repositories.Models
{
    [Table("ORDERITEMS")]
    public partial class Orderitem
    {
        [Key]
        [Column("ORDERITEMID")]
        public int Orderitemid { get; set; }
        [Column("ORDERID")]
        public int? Orderid { get; set; }
        [Column("PRODUCTID")]
        public int? Productid { get; set; }
        [Column("QUANTITY")]
        public int? Quantity { get; set; }
        [Column("PRICE", TypeName = "decimal(9, 2)")]
        public decimal? Price { get; set; }
        [Column("RETURNABLE")]
        public bool? Returnable { get; set; }

        [ForeignKey(nameof(Orderid))]
        [InverseProperty("Orderitems")]
        public virtual Order Order { get; set; }
        [ForeignKey(nameof(Productid))]
        [InverseProperty("Orderitems")]
        public virtual Product Product { get; set; }
    }
}
