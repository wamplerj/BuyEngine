namespace BuyEngine.Checkout.Shipping
{
    public class PackageDimension
    {
        public decimal Length { get; }
        public decimal Width { get; }
        public decimal Height { get; }

        public UnitOfMeasure Unit { get; }

        public PackageDimension(decimal length, decimal width, decimal height, UnitOfMeasure unit)
        {
            Length = length;
            Width = width;
            Height = height;
            Unit = unit;
        }
    }

    public enum UnitOfMeasure
    {
        Inches = 0,
        Millimeters = 1
    }
}
