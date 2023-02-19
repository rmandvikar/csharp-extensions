using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace rm.Extensions;

/// <summary>
/// String extensions.
/// </summary>
public static class StringExtension
{
	/// <summary>
	/// Returns true if string is null or empty.
	/// </summary>
	public static bool IsNullOrEmpty(this string s)
	{
		return string.IsNullOrEmpty(s);
	}

	/// <summary>
	/// Returns true if string is null, empty or only whitespaces.
	/// </summary>
	public static bool IsNullOrWhiteSpace(this string s)
	{
		return string.IsNullOrWhiteSpace(s);
	}

	/// <summary>
	/// Returns specified value if string is null/empty/whitespace else same string.
	/// </summary>
	public static string Or(this string s, string or)
	{
		if (!s.IsNullOrWhiteSpace())
		{
			return s;
		}
		return or;
	}

	/// <summary>
	/// Returns empty if string is null/empty/whitespace else same string.
	/// </summary>
	public static string OrEmpty(this string s)
	{
		return s.Or("");
	}

	/// <summary>
	/// Returns null if string is null/empty else same string.
	/// </summary>
	public static string NullIfEmpty(this string s)
	{
		return !s.IsNullOrEmpty() ? s : null;
	}

	/// <summary>
	/// Returns null if string is null/empty/whitespace else same string.
	/// </summary>
	public static string NullIfWhiteSpace(this string s)
	{
		return !s.IsNullOrWhiteSpace() ? s : null;
	}

	/// <summary>
	/// Returns html-encoded string.
	/// </summary>
	public static string HtmlEncode(this string s)
	{
		return WebUtility.HtmlEncode(s);
	}

	/// <summary>
	/// Returns html-decoded string.
	/// </summary>
	public static string HtmlDecode(this string s)
	{
		return WebUtility.HtmlDecode(s);
	}

	/// <summary>
	/// Returns url-encoded string.
	/// </summary>
	public static string UrlEncode(this string s)
	{
		return WebUtility.UrlEncode(s);
	}

	/// <summary>
	/// Returns url-decoded string.
	/// </summary>
	public static string UrlDecode(this string s)
	{
		return WebUtility.UrlDecode(s);
	}

	/// <summary>
	/// Formats string as string.Format() but the parameter index is optional and parameter meta is allowed.
	/// </summary>
	/// <example>"{} is a {1}".Format("this", "test")</example>
	public static string Format(this string format, params object[] args)
	{
		var buffer = new StringBuilder(format);
		var i = 0;
		var param = 0;
		var paramMetaToIndexMap = new Dictionary<string, string>();
		while (i < buffer.Length)
		{
			// close curly before open curly
			if (buffer[i] == '}')
			{
				// skip escaped curly "}}", else break
				if (i + 1 < buffer.Length && buffer[i + 1] == '}')
				{
					i += 2;
					continue;
				}
				else
				{
					break;
				}
			}
			// stop at '{'
			if (buffer[i] != '{')
			{
				i++;
				continue;
			}
			// skip escaped curly "{{"
			if (i + 1 < buffer.Length && buffer[i + 1] == '{')
			{
				i += 2;
				continue;
			}
			var start = i;
			while (i < buffer.Length && buffer[i] != '}')
			{
				i++;
			}
			// open curly is not matched, break
			if (i == buffer.Length)
			{
				break;
			}
			var end = i;
			//// at this point buffer[start..end] has format field
			//// insert parameter index only if not present
			////     so insert for:
			////     se  s  e  s            e  s    e  s      e  s                e
			////     {}  {:2}  {,10:#,##0.00}  {test}  {test:2}  {test,10:#,##0.00}
			////     and do not insert for:
			////     s e  s   e  s             e
			////     {0}  {0:2}  {0,10:#,##0.00}
			// find the meta substring
			var metastart = start + 1;
			var metaend = metastart;
			while (buffer[metaend] != '}' && buffer[metaend] != ':' && buffer[metaend] != ',')
			{
				metaend++;
			}
			var paramMeta = buffer.ToString(metastart, metaend - metastart).Trim();
			// insert param only if meta is not int
			if (!int.TryParse(paramMeta, out _))
			{
				string paramIndex;
				// do not insert "" into meta->index map
				if (paramMeta == "")
				{
					paramIndex = param.ToString();
					param++;
				}
				else
				{
					// remove meta
					buffer.Remove(metastart, paramMeta.Length);
					// insert param index into meta->index map if not exists
					if (!paramMetaToIndexMap.ContainsKey(paramMeta))
					{
						paramMetaToIndexMap[paramMeta] = param.ToString();
						param++;
					}
					// do not increment param as param index is reused
					paramIndex = paramMetaToIndexMap[paramMeta];
				}
				// insert index
				buffer.Insert(metastart, paramIndex);
				// adjust end as buffer is removed from and inserted into
				end += -paramMeta.Length + paramIndex.Length;
			}
			else
			{
				param++;
			}
			i = end + 1; // i++ does not work
		}
		var formatConverted = buffer.ToString();
		return string.Format(formatConverted, args);
	}

	/// <summary>
	/// Try-parses string to bool, else default value.
	/// </summary>
	public static bool ToBool(this string value, bool defaultValue)
	{
		bool result;
		if (bool.TryParse(value, out result))
		{
			return result;
		}
		return defaultValue;
	}

	/// <summary>
	/// Munges substitutions.
	/// </summary>
	private static List<KeyValuePair<string, string>> mungeUnmungeSubstitutions =
		GetMungeUnmungeSubstitutions();
	private static List<KeyValuePair<string, string>> GetMungeUnmungeSubstitutions()
	{
		var mungeSubstitutions = new List<KeyValuePair<string, string>>();
		new[]
		{
			new[] {"a", "@"},
			new[] {"b", "8"},
			new[] {"c", "("},
			new[] {"d", "6"},
			new[] {"e", "3"},
			new[] {"f", "#"},
			new[] {"g", "9"},
			new[] {"h", "#"},
			new[] {"i", "1"},
			new[] {"i", "!"},
			new[] {"k", "<"},
			new[] {"l", "1"},
			new[] {"l", "i"},
			new[] {"o", "0"},
			new[] {"q", "9"},
			new[] {"s", "$"},
			new[] {"s", "5"},
			new[] {"t", "+"},
			new[] {"v", ">"},
			new[] {"v", "<"},
			new[] {"w", "uu"},
			new[] {"w", "2u"},
			new[] {"x", "%"},
			new[] {"y", "?"},
		}.ToList().ForEach(x => mungeSubstitutions.Add(new KeyValuePair<string, string>(x[0], x[1])));
		return mungeSubstitutions;
	}

	/// <summary>
	/// key->value[] map.
	/// </summary>
	private static IDictionary<string, string[]> mungeMap =
		mungeUnmungeSubstitutions
			.GroupBy(x => x.Key)
			.ToDictionary(g => g.Key, g => g.Select(x => x.Value)
			.ToArray());

	/// <summary>
	/// value->key[] map.
	/// </summary>
	private static IDictionary<string, string[]> unmungeMap =
		mungeUnmungeSubstitutions
			.GroupBy(x => x.Value)
			.ToDictionary(g => g.Key, g => g.Select(x => x.Key)
			.ToArray());

	/// <summary>
	/// Munges or unmunges password as per substitution map.
	/// </summary>
	private static IList<string> MungeUnmunge(this string password,
		IDictionary<string, string[]> muMap, bool isMunging)
	{
		password.ThrowIfArgumentNull(nameof(password));
		var items = new List<string>();
		MungeUnmunge
		(
			password, muMap, isMunging, items,
			0, new StringBuilder(password.Length), new Dictionary<string, string>()
		);
		return items;
	}

	/// <summary>
	/// Munges or unmunges recursively.
	/// </summary>
	/// <param name="password">Password to munge/unmunge.</param>
	/// <param name="muMap">Substitution map.</param>
	/// <param name="isMunging">True if munging, false if unmunging.</param>
	/// <param name="items">List to hold the passwords.</param>
	/// <param name="index">Index (start) of currently processed part.</param>
	/// <param name="buffer">Buffer to hold the munge/unmunged password.</param>
	/// <param name="partReplaceMap">Map to hold part replacements.</param>
	private static void MungeUnmunge(string password,
		IDictionary<string, string[]> muMap, bool isMunging, List<string> items,
		int index, StringBuilder buffer, Dictionary<string, string> partReplaceMap)
	{
		if (index == password.Length)
		{
			items.Add(buffer.ToString());
			return;
		}
		Func<string, int, int, string> substring = (pwd, idx, len) =>
		{
			if (idx + len > pwd.Length)
			{
				return null;
			}
			return pwd.Substring(idx, len);
		};
		// look ahead 1, 2 chars only
		for (int length = 1; length <= 2; length++)
		{
			var part = substring(password, index, length);
			foreach (var muSubstitute in
				GetMungeUnmungeParts(part, muMap, isMunging, partReplaceMap))
			{
				buffer.Insert(index, muSubstitute);
				partReplaceMap[part] = muSubstitute;
				MungeUnmunge(password, muMap, isMunging, items, index + length, buffer, partReplaceMap);
				buffer.Length -= muSubstitute.Length;
				partReplaceMap.Remove(part);
			}
		}
	}

	/// <summary>
	/// Returns the munged/unmunged parts for part.
	/// </summary>
	/// <example>munge("o"), returns {"o", "0"}.</example>
	/// <example>Unmunge("0"), returns {"o"}.</example>
	private static IEnumerable<string> GetMungeUnmungeParts(string part,
		IDictionary<string, string[]> muMap, bool isMunging,
		Dictionary<string, string> partReplaceMap)
	{
		if (part == null)
		{
			yield break;
		}
		// yield same replacement for part as before
		if (partReplaceMap.ContainsKey(part))
		{
			yield return partReplaceMap[part];
		}
		else if (part.Length == 1)
		{
			// yield same part only if munging and length 1
			if (isMunging)
			{
				yield return part;
			}
			// yield if in muMap
			string[] muParts;
			if (muMap.TryGetValue(part, out muParts))
			{
				foreach (var muPart in muParts)
				{
					yield return muPart;
				}
			}
			else
			{
				// yield only if not munging and not in muMap
				if (!isMunging)
				{
					yield return part;
				}
			}
		}
		else if (part.Length == 2)
		{
			// yield only if in muMap
			string[] muParts;
			if (muMap.TryGetValue(part, out muParts))
			{
				foreach (var muPart in muParts)
				{
					yield return muPart;
				}
			}
		}
	}

	/// <summary>
	/// Munges a password (up to two chars).
	/// </summary>
	/// <remarks>http://en.wikipedia.org/wiki/Munged_password</remarks>
	public static IEnumerable<string> Munge(this string password)
	{
		var items = MungeUnmunge(password, mungeMap, true);
		// 1st item is same as password
		if (items.Any() && items[0] == password)
		{
			items.RemoveAt(0);
		}
		return items.AsEnumerable();
	}

	/// <summary>
	/// Unmunges a (munged) password (up to two chars).
	/// </summary>
	/// <remarks>http://en.wikipedia.org/wiki/Munged_password</remarks>
	public static IEnumerable<string> Unmunge(this string password)
	{
		var items = MungeUnmunge(password, unmungeMap, false);
		// remove 1st item if same as password
		if (items.Any() && items[0] == password)
		{
			items.RemoveAt(0);
		}
		return items.AsEnumerable();
	}

	/// <summary>
	/// Returns a new collection with characters scrabbled like the game.
	/// </summary>
	public static IEnumerable<string> Scrabble(this string word)
	{
		word.ThrowIfArgumentNull(nameof(word));
		return word.Scrabble<char>().Select(x => new string(x));
	}

	/// <summary>
	/// Returns a new collection with characters scrabbled like the game for particular r.
	/// </summary>
	public static IEnumerable<string> Scrabble(this string word, int limit)
	{
		word.ThrowIfArgumentNull(nameof(word));
		limit.ThrowIfArgumentOutOfRange(nameof(limit), maxRange: word.Length);
		return word.Scrabble<char>(limit).Select(x => new string(x));
	}

	/// <summary>
	/// Parses a string in UTC format as DateTime.
	/// </summary>
	public static DateTime ParseAsUtc(this string s)
	{
		return DateTimeOffset.Parse(s).UtcDateTime;
	}

	/// <summary>
	/// Converts a string to title case.
	/// </summary>
	/// <example>"war and peace" -> "War And Peace"</example>
	public static string ToTitleCase(this string s)
	{
		return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(s);
	}

	/// <summary>
	/// Gets permutations of string for particular r.
	/// </summary>
	/// <remarks>nPr permutations</remarks>
	public static IEnumerable<string> Permutation(this string s, int r)
	{
		return s.Permutation<char>(r).Select(x => new string(x));
	}

	/// <summary>
	/// Gets permutations of string.
	/// </summary>
	/// <remarks>nPn permutations</remarks>
	public static IEnumerable<string> Permutation(this string s)
	{
		return s.Permutation(s.Length);
	}

	/// <summary>
	/// Gets combinations of string for particular r.
	/// </summary>
	/// <remarks>nCr combinations</remarks>
	public static IEnumerable<string> Combination(this string s, int r)
	{
		return s.Combination<char>(r).Select(x => new string(x));
	}

	/// <summary>
	/// Gets combinations of string.
	/// </summary>
	/// <remarks>nCn combinations</remarks>
	public static IEnumerable<string> Combination(this string s)
	{
		return s.Combination(s.Length);
	}

	/// <summary>
	/// Splits csv string using common delimiters.
	/// </summary>
	public static IEnumerable<string> SplitCsv(this string s)
	{
		return s.Split(new[] { ",", ";", "|" }, StringSplitOptions.RemoveEmptyEntries);
	}

	/// <summary>
	/// Returns substring from start of length <paramref name="n"/>.
	/// </summary>
	public static string SubstringFromStart(this string source, int n)
	{
		source.ThrowIfArgumentNull(nameof(source));
		n.ThrowIfArgumentOutOfRange(nameof(n));
		if (n >= source.Length)
		{
			return source;
		}
		return source.Substring(0, n);
	}

	/// <summary>
	/// Returns substring till end of length <paramref name="n"/>.
	/// </summary>
	public static string SubstringTillEnd(this string source, int n)
	{
		source.ThrowIfArgumentNull(nameof(source));
		n.ThrowIfArgumentOutOfRange(nameof(n));
		if (n >= source.Length)
		{
			return source;
		}
		return source.Substring(source.Length - n);
	}

	/// <summary>
	/// Returns substring by specifying start index and end index.
	/// </summary>
	public static string SubstringByIndex(this string source, int startIndex, int endIndex)
	{
		source.ThrowIfArgumentNull(nameof(source));
		startIndex.ThrowIfArgumentOutOfRange(nameof(startIndex));
		endIndex.ThrowIfArgumentOutOfRange(nameof(endIndex), maxRange: source.Length);
		return source.Substring(startIndex, endIndex - startIndex);
	}
}
