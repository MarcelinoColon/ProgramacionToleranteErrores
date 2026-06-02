using Polly.Fallback;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace FormFallback
{
    public class DollarRate
    {
        private readonly AsyncFallbackPolicy<decimal> _fallback;

        public DollarRate(AsyncFallbackPolicy<decimal> fallback)
        {
            _fallback = fallback;
        }

        public async Task<decimal> GetRateAsync()
            => await _fallback.ExecuteAsync(async () =>
            {
                var httpclient = new HttpClient();

                var response = await httpclient.GetAsync("https://api.exchangerate-api.com/v4/latest/USD");

                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var data = JsonDocument.Parse(content);

                return data.RootElement.GetProperty("rates").GetProperty("DOP").GetDecimal();
            });
    }
}
