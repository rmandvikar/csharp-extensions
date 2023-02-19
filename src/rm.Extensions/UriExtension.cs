using System;
using System.Net;
using System.Security.Cryptography;

namespace rm.Extensions;

/// <summary>
/// Hash algorithm type.
/// </summary>
public enum Hasher
{
	sha1 = 1,
	md5
}

/// <summary>
/// Uri extensions.
/// </summary>
public static class UriExtension
{
	/// <summary>
	/// Calculates checksum / hash (sha1 or md5) for uri (url or local file).
	/// <para>
	/// hash: sha1 20 bytes, md5 hash 16 bytes
	/// </para>
	/// </summary>
	/// <param name="uri">uri (url or local file).</param>
	/// <param name="type">Hasher.sha1 or Hasher.md5.</param>
	/// <returns>hash</returns>
	/// <remarks>http://hash.online-convert.com/</remarks>
	public static string Checksum(this Uri uri, Hasher type = Hasher.sha1)
	{
		uri.ThrowIfArgumentNull(nameof(uri));
		switch (type)
		{
			case Hasher.sha1:
				using (var hasher = new SHA1CryptoServiceProvider())
				{
					return Checksum(hasher, uri);
				}
			case Hasher.md5:
				using (var hasher = MD5.Create())
				{
					return Checksum(hasher, uri);
				}
			default:
				throw new ArgumentOutOfRangeException("Unknown hash algorithm type.");
		}
	}

	private static string Checksum(HashAlgorithm hasher, Uri uri)
	{
		// webclient works for uris and local files
		using (var webclient = new WebClient())
		{
			using (var stream = webclient.OpenRead(uri))
			{
				stream.ThrowIfNull(nameof(stream));
				var hash = BitConverter.ToString(hasher.ComputeHash(stream)).Replace("-", "").ToLower();
				return hash;
			}
		}
	}
}
