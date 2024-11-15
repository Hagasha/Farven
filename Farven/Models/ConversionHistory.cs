namespace Farven.Models
{
    public class ConversionHistory
    {
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string FromCurrency { get; set; }
        public string ToCurrency { get; set; }
        public decimal Amount { get; set; }
        public decimal ConvertedAmount { get; set; }
        public DateTime ConversionDate { get; set; }

        public Cliente Cliente { get; set; }
    }
}