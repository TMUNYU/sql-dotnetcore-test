using System.Collections.Generic;

namespace Orders.Api.Services.Models.DomainModels
{ 

    public class OrderSummary    {
        public int OrderNumber { get; set; } 
        public string OrderDate { get; set; } 
        public string DeliveryAddress { get; set; } 
        public IEnumerable<OrderItemSummary> OrderItems { get; set; } 
        public string DeliveryExpected { get; set; } 
    }

}