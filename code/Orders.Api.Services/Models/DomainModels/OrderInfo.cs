using System;
using System.Collections.Generic;

namespace Orders.Api.Services.Models.DomainModels
{
    public class OrderInfo
    {
        public int OrderId { get; set; }

        public string CustomerId { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime? DeliveryExpected { get; set; }

        public bool ContainsGift { get; set; }

        public string ShippingMode { get; set; }

        public string OrderSource { get; set; }

        public virtual ICollection<OrderItemInfo> OrderItems { get; set; }
    }
}
