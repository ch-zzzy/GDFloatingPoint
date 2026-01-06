// Made for Geometry Dash 2.2

using System.Globalization;

internal class Program
{
    private static void Main(string[] args)
    {
        float freezeDistance;
        float velocity;
        double ulpValue;

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

        void TPS(double ulpValue, float velocity)
        {
            double min = (2 / ulpValue * velocity) + float.Epsilon;
            double max = (4 / ulpValue * velocity);
            if (min <= (double)Decimal.MaxValue)
            {
                Console.WriteLine(
                $"Minimum tps to freeze at {freezeDistance} units"
                + $" with velocity {velocity} units/s: "
                + (decimal)min
                );
            }
            else
            {
                Console.WriteLine(
                    $"Minimum tps to freeze at {freezeDistance} units"
                    + $" with velocity {velocity} units/s: "
                    + min
                );
            }

            if (max <= (double)Decimal.MaxValue)
            {
                Console.WriteLine(
                    $"Maximum tps to freeze at {freezeDistance} units"
                    + $" with velocity {velocity} units/s: "
                    + (decimal)max
                );
            }
            else
            {
                Console.WriteLine(
                    $"Maximum tps to freeze at {freezeDistance} units"
                    + $" with velocity {velocity} units/s: "
                    + max
                );
            }
        }
        double Ulp(float distance)
        {
            if (distance <= 0)
            {
                freezeDistance = 0;
                return float.Epsilon;
            }

            byte[] bytes = BitConverter.GetBytes(distance);
            int bits = BitConverter.ToInt32(bytes, 0);
            int nextBits = bits + 1;
            byte[] nextBytes = BitConverter.GetBytes(nextBits);
            float nextValue = BitConverter.ToSingle(nextBytes, 0);
            return nextValue - distance;
        }
    }
}