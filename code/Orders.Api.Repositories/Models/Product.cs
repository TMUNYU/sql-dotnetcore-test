using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Orders.Api.Repositories.Models
{
    [Table("PRODUCTS")]
    public partial class Product
    {
        public Product()
        {
            Orderitems = new HashSet<Orderitem>();
        }

        [Key]
        [Column("PRODUCTID")]
        public int Productid { get; set; }
        [Column("PRODUCTNAME")]
        [StringLength(50)]
        public string Productname { get; set; }
        [Column("PACKHEIGHT", TypeName = "decimal(9, 2)")]
        public decimal? Packheight { get; set; }
        [Column("PACKWIDTH", TypeName = "decimal(9, 2)")]
        public decimal? Packwidth { get; set; }
        [Column("PACKWEIGHT", TypeName = "decimal(8, 3)")]
        public decimal? Packweight { get; set; }
        [Column("COLOUR")]
        [StringLength(20)]
        public string Colour { get; set; }
        [Column("SIZE")]
        [StringLength(20)]
        public string Size { get; set; }

        [InverseProperty(nameof(Orderitem.Product))]
        public virtual ICollection<Orderitem> Orderitems { get; set; }
    }
}
