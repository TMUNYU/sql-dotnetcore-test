namespace Orders.Api.Services.Models.DomainModels
{ 

    public class OrderItemSummary    {
        public string Product { get; set; } 
        public int Quantity { get; set; } 
        public decimal PriceEach { get; set; } 
    }

}