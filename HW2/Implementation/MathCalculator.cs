public class MathCalculator : IMathCalculator
{
    public double CalculateSin(double x) => Math.Sin(x);
    public double CalculateCos(double x) => Math.Cos(x);
    public double CalculateTan(double x) => Math.Tan(x);
    public double CalculateLn(double x) => Math.Log(x);
    public double CalculateExp(double x) => Math.Exp(x);
}
