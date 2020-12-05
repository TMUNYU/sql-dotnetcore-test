using System.ComponentModel.DataAnnotations;

namespace Orders.Api.Models.Dtos.Request
{
    public class CustomerIdentity
    {
        [EmailAddress]
        [Required]
        public string User { get; set; }

        [Required]
        public string CustomerId { get; set; }
    }
}
