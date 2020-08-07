namespace ConciliaPraGente.Infra
{
    public static class DecimalExtension
    {
        public static string Format(this decimal value)
        {
            return $"R$ {value:N}";
        }

        public static string FormatMonetaryOrZeroInReal(this decimal value)
        {
            return value != 0 ? value.Format() : "R$ 0,00";
        }
    }
}