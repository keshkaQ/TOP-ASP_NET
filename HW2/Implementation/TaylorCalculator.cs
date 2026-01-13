public class TaylorCalculator : IMathCalculator
{
    private const int DefaultIterations = 20;

    public double CalculateSin(double x)
    {
        x = NormalizeAngle(x);
        double result = 0;
        double term = x;
        int n = 1;

        for (int i = 0; i < DefaultIterations; i++)
        {
            result += term;
            term *= -x * x / ((2 * n) * (2 * n + 1));
            n++;
        }
        return result;
    }

    public double CalculateCos(double x)
    {
        x = NormalizeAngle(x);
        double result = 1;
        double term = 1;
        int n = 1;

        for (int i = 0; i < DefaultIterations; i++)
        {
            term *= -x * x / ((2 * n - 1) * (2 * n));
            result += term;
            n++;
        }
        return result;
    }

    public double CalculateTan(double x)
    {
        x = NormalizeAngle(x);
        var cos = CalculateCos(x);
        if (Math.Abs(cos) < 1e-10)
            throw new ArgumentException($"Тангенс не определен для x = {x}");
        return CalculateSin(x) / cos;
    }

    public double CalculateLn(double x)
    {
        if (x <= 0)
            throw new ArgumentException($"ln(x) определен только для x > 0");
        if (x == 1) return 0;

        double z = (x - 1) / (x + 1);
        double result = 0;
        double term = z;

        for (int i = 0; i < DefaultIterations; i++)
        {
            result += term / (2 * i + 1);
            term *= z * z;
        }
        return 2 * result;
    }

    public double CalculateExp(double x)
    {
        if (Math.Abs(x) > 1)
        {
            double halfExp = CalculateExp(x / 2);
            return halfExp * halfExp;
        }

        double result = 1;
        double term = 1;

        for (int i = 1; i < DefaultIterations; i++)
        {
            term *= x / i;
            result += term;
        }
        return result;
    }

    private double NormalizeAngle(double x)
    {
        const double twoPi = 2 * Math.PI;
        x %= twoPi;
        if (x > Math.PI) x -= twoPi;
        if (x < -Math.PI) x += twoPi;
        return x;
    }
}
