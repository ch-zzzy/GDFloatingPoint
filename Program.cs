// Made for Geometry Dash 2.2

using System.Globalization;

internal class Program
{
    private static void Main(string[] args)
    {
        double MAX_DECIMAL_VALUE = (double)Decimal.MaxValue;
        double MIN_DECIMAL_VALUE = MinDecimalValue();
        float freezeDistance;
        float velocity;
        double ulpValue;

        // main program
        Console.WriteLine("Input the distance you want to freeze at:");
        string? input;
        while (true)
        {
            input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Empty input. Please enter a value.");
                continue;
            }

            if (float.TryParse(input.Trim(), NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out freezeDistance))
            {
                if (float.IsNaN(freezeDistance) || float.IsInfinity(freezeDistance))
                {
                    Console.WriteLine("Invalid input. Please try again.");
                    continue;
                }
                break; // valid value entered; continue program
            }

            Console.WriteLine("Invalid input. Please try again.");
        }

        ulpValue = Ulp(freezeDistance);

        Console.WriteLine("Input the velocity you want to use:");
        while (true)
        {
            input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Empty input. Please enter a value.");
                continue;
            }

            if (float.TryParse(input.Trim(), NumberStyles.Float | NumberStyles.AllowThousands, CultureInfo.InvariantCulture, out velocity))
            {
                break; // valid value entered; continue program
            }

            Console.WriteLine("Invalid input. Please enter a valid floating-point number.");
        }

        TPS(ulpValue, velocity);
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();


        // functions

        void TPS(double ulpValue, float velocity)
        {
            double distanceToFreeze = FloorPow2(freezeDistance);
            double min = (2 + double.Epsilon) / ulpValue * velocity;
            double max = 4 / ulpValue * velocity;
            Console.WriteLine(
                $"Minimum tps to freeze at {
                    (CanRepresentAsDecimal(distanceToFreeze) ? (decimal)distanceToFreeze : distanceToFreeze)
                    } units (floored to the nearest power of 2)"
                + $" with velocity {velocity} units/s: "
                + (CanRepresentAsDecimal(min) ? (decimal)min : min)
                );
            Console.WriteLine(
                $"Maximum tps to freeze at {
                    (CanRepresentAsDecimal(distanceToFreeze) ? (decimal)distanceToFreeze : distanceToFreeze)
                    } with velocity {velocity} units/s: "
                + (CanRepresentAsDecimal(max) ? (decimal)max : max)
                );
        }

        bool CanRepresentAsDecimal(double value)
        {
            return value >= MIN_DECIMAL_VALUE && value <= MAX_DECIMAL_VALUE;
        }

        double FloorPow2(float x)
        {
            if (x <= 0.0) return 0.0;
            int exp = (int)Math.Floor(Math.Log2(x));
            return Math.Pow(2, exp);
        }

        double MinDecimalValue()
        {
            //smallest positive decimal value
            double d = (double)new decimal(1, 0, 0, false, 28);
            //get the next greater double value to ensure it's representable as decimal after casting
            double nextGreater = BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(d) + 1);
            return nextGreater;
        }

        double Ulp(float distance)
        {
            if (distance <= 0)
            {
                freezeDistance = 0;
                return float.Epsilon;
            }
            int bits = BitConverter.SingleToInt32Bits(distance);
            int nextBits = bits + 1;
            float nextValue = BitConverter.Int32BitsToSingle(nextBits);
            return nextValue - distance;
        }
    }
}