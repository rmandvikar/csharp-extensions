using System;

namespace rm.Extensions;

/// <summary>
/// Random extensions.
/// </summary>
public static class RandomExtension
{
	/// <summary>
	/// Returns a random item from <paramref name="source"/>.
	/// </summary>
	public static T NextItem<T>(this Random random, T[] source)
	{
		source.ThrowIfArgumentNull(nameof(source));
		if (source.IsNullOrEmpty())
		{
			throw new ArgumentOutOfRangeException(nameof(source.Length), source.Length, null);
		}

		return source[random.Next(source.Length)];
	}

	/// <summary>
	/// <see href="https://stackoverflow.com/questions/218060/random-gaussian-variables">source</see>
	/// </summary>
	public static double NextGaussian(this Random random, double mu = 0, double sigma = 1)
	{
		// uniform(0,1] random doubles
		double u1 = 1.0 - random.NextDouble();
		double u2 = 1.0 - random.NextDouble();
		// random normal(0,1)
		double randStdNormal = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Sin(2.0 * Math.PI * u2);
		// random normal(mean,stdDev^2)
		double randNormal = mu + sigma * randStdNormal;
		return randNormal;
	}

	/// <summary>
	/// Generates random string of <paramref name="length"/> for <paramref name="charset"/>.
	/// </summary>
	/// <remarks>
	/// <see href="https://stackoverflow.com/questions/1344221/how-can-i-generate-random-alphanumeric-strings">source</see>
	/// </remarks>
	public static string NextString(this Random random, int length, string charset)
	{
		length.ThrowIfArgumentOutOfRange(nameof(length));
		if (charset.IsNullOrEmpty())
		{
			throw new ArgumentException(nameof(charset));
		}
		var randomString = new char[length];
		for (int i = 0; i < length; i++)
		{
			randomString[i] = charset[random.Next(charset.Length)];
		}
		return new string(randomString);
	}

	/// <summary>
	/// Generates random double between <paramref name="minValue"/> and <paramref name="maxValue"/>.
	/// <para></para>
	/// Note: This is a scaled implementation: (random.nextDouble() * (max - min)) + min
	/// </summary>
	/// <remarks>
	/// <see href="https://stackoverflow.com/questions/1064901/random-number-between-2-double-numbers">source</see>
	/// </remarks>
	public static double NextDouble(this Random random, double minValue = 0d, double maxValue = double.MaxValue)
	{
		if (minValue > maxValue)
		{
			throw new ArgumentOutOfRangeException(nameof(minValue), "'minValue' cannot be greater than 'maxValue'");
		}
		// scaled
		return (random.NextDouble() * (maxValue - minValue)) + minValue;
	}

	/// <summary>
	/// Generates random decimal between <paramref name="minValue"/> and <paramref name="maxValue"/>.
	/// <para></para>
	/// Note: This is a scaled implementation: (random.nextDouble() * (max - min)) + min
	/// </summary>
	/// <remarks>
	/// <see href="https://stackoverflow.com/questions/609501/generating-a-random-decimal-in-c-sharp">source</see>
	/// </remarks>
	public static decimal NextDecimal(this Random random, decimal minValue = decimal.Zero, decimal maxValue = decimal.MaxValue)
	{
		if (minValue > maxValue)
		{
			throw new ArgumentOutOfRangeException(nameof(minValue), "'minValue' cannot be greater than 'maxValue'");
		}
		// scaled
		return ((decimal)random.NextDouble() * (maxValue - minValue)) + minValue;
	}
}
