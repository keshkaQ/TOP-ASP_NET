public class CalculatorService
{
    private readonly Dictionary<string, IMathCalculator> _calculators;

    public CalculatorService(IEnumerable<IMathCalculator> calculators)
    {
        _calculators = new Dictionary<string, IMathCalculator>
        {
            ["math"] = calculators.First(c => c is MathCalculator),
            ["taylor"] = calculators.First(c => c is TaylorCalculator)
        };
    }

    private IMathCalculator GetCalculator(string method)
    {
        var key = method.ToLower() ?? "math";

        if (_calculators.TryGetValue(key, out var calculator))
            return calculator;

        throw new ArgumentException($"Неизвестный метод: {method}. " +
            $"Доступные: {string.Join(", ", _calculators.Keys)}");
    }

    public double CalculateSin(double x, string method) => GetCalculator(method).CalculateSin(x);
    public double CalculateCos(double x, string method) => GetCalculator(method).CalculateCos(x);
    public double CalculateTan(double x, string method) => GetCalculator(method).CalculateTan(x);
    public double CalculateLn(double x, string method) => GetCalculator(method).CalculateLn(x);
    public double CalculateExp(double x, string method) => GetCalculator(method).CalculateExp(x);
}