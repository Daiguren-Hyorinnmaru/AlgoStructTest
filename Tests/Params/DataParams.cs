namespace Tests.Params
{
    public class DataParams
    {
        public int IntMin { get; set; } = int.MinValue;
        public int IntMax { get; set; } = int.MaxValue;

        public double DoubleMin { get; set; } = double.MinValue;
        public double DoubleMax { get; set; } = double.MaxValue;

        public int StringMinLength { get; set; } = 5;
        public int StringMaxLength { get; set; } = 15;
    }
}
