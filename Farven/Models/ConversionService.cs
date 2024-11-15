using Farven.Data;
using Farven.Models;

public class ConversionService
{
    private readonly ApplicationDbContext _context;

    public ConversionService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task SaveConversionAsync(int clienteId, string fromCurrency, string toCurrency, decimal amount, decimal convertedAmount)
    {
        var conversion = new ConversionHistory
        {
            ClienteId = clienteId,
            FromCurrency = fromCurrency,
            ToCurrency = toCurrency,
            Amount = amount,
            ConvertedAmount = convertedAmount,
            ConversionDate = DateTime.UtcNow
        };

        _context.ConversionHistories.Add(conversion);
        await _context.SaveChangesAsync();
    }
}