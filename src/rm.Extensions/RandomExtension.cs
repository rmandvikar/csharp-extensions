using System;

namespace rm.Extensions
{
	/// <summary>
	/// Random extensions.
	/// </summary>
	public static class RandomExtension
	{
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
	}
}
