namespace Orders.Api.Services.Models.DomainModels
{ 

    public class LatestOrderInfo {
        public CustomerNames Customer { get; set; } 
        public OrderSummary Order { get; set; } 
    }

}