namespace Orders.Api.Models.Response
{ 

    public class OrderItemSummaryDto    {
        public string Product { get; set; } 
        public int Quantity { get; set; } 
        public decimal PriceEach { get; set; } 
    }

}