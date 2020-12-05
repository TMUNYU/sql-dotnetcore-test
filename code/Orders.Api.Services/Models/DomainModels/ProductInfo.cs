namespace Orders.Api.Services.Models.DomainModels
{
    public class ProductInfo
    { 
        public string ProductName { get; set; }

        public decimal PackHeight { get; set; }

        public decimal PackWidth { get; set; }

        public decimal PackWeight { get; set; }

        public string Colour { get; set; }

        public string Size { get; set; }
    }
}
