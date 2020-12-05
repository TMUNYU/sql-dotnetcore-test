using System;
using System.Collections.Generic;

namespace Orders.Api.Services.Models.DomainModels
{
    public class OrderInfo
    {
        public partial class Order
        {
            public string CustomerDd { get; set; }

            public DateTime? OrderDate { get; set; }

            public DateTime? DeliveryExpected { get; set; }

            public bool ContainsGift { get; set; }

            public string ShippingMode { get; set; }

            public string OrderSource { get; set; }

            public virtual ICollection<OrderItemInfo> OrderItems { get; set; }
        }
    }
}
