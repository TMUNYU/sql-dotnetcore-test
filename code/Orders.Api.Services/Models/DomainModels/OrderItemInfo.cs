using System;

namespace Orders.Api.Services.Models.DomainModels
{
    public class OrderItemInfo
    {
        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public bool Returnable { get; set; }

        public decimal UnitPrice => CalculateUnitPrice();

        public virtual ProductInfo Product { get; set; }

        private decimal CalculateUnitPrice()
        {
            throw new NotImplementedException();
        }
    }
}
