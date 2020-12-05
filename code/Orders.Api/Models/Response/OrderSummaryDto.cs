using System.Collections.Generic;

namespace Orders.Api.Models.Response
{ 

    public class OrderSummaryDto    {
        public int OrderNumber { get; set; } 
        public string OrderDate { get; set; } 
        public string DeliveryAddress { get; set; } 
        public IEnumerable<OrderItemSummaryDto> OrderItems { get; set; } 
        public string DeliveryExpected { get; set; } 
    }

}