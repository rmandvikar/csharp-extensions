using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;

namespace rm.Extensions
{
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
		/// Format string as string.Format() but the parameter index is optional and parameter meta is allowed.
		/// </summary>
		/// <example>"{} is a {1}".format("this", "test")</example>
		public static string format(this string format, params object[] args)
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
				int ignore;
				if (!int.TryParse(paramMeta, out ignore))
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
		/// Try-parse string to bool, else default value.
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
		/// Munge substitutions.
		/// </summary>
		private static List<KeyValuePair<char, char>> mungeSubstitutions = GetMungeSubstitutions();
		private static List<KeyValuePair<char, char>> GetMungeSubstitutions()
		{
			var mungeSubstitutions = new List<KeyValuePair<char, char>>();
			new[]
			{
				new[] {'a', '@'},
				new[] {'b', '8'},
				new[] {'c', '('},
				new[] {'e', '3'},
				new[] {'g', '9'},
				new[] {'i', '1'},
				new[] {'i', '!'},
				new[] {'l', '1'},
				new[] {'o', '0'},
				new[] {'s', '$'},
				new[] {'t', '+'},
				// add more here
			}.ToList().ForEach(x => mungeSubstitutions.Add(new KeyValuePair<char, char>(x[0], x[1])));
			return mungeSubstitutions;
		}
		/// <summary>
		/// key->value[] map.
		/// </summary>
		private static IDictionary<char, char[]> mungeMap = mungeSubstitutions.GroupBy(x => x.Key)
			.ToDictionary(g => g.Key, g => g.Select(x => x.Value).ToArray());
		/// <summary>
		/// value->key[] map.
		/// </summary>
		private static IDictionary<char, char[]> unmungeMap = mungeSubstitutions.GroupBy(x => x.Value)
			.ToDictionary(g => g.Key, g => g.Select(x => x.Key).ToArray());
		/// <summary>
		/// Munges or unmunges password as per substitution map.
		/// </summary>
		private static IEnumerable<string> MungeUnmunge(this string password,
			IDictionary<char, char[]> map)
		{
			password.ThrowIfArgumentNull(nameof(password));
			var list = new List<string>();
			MungeUnmunge(password, map, 0, new StringBuilder(), list);
			return list.AsEnumerable();
		}
		/// <summary>
		/// Recursive method to munge/unmunge.
		/// </summary>
		/// <param name="password">Password to munge/unmunge.</param>
		/// <param name="map">Substitution map.</param>
		/// <param name="index">Index of currently processed character.</param>
		/// <param name="buffer">Buffer to hold the munge/unmunged password.</param>
		/// <param name="list">List to hold the passwords.</param>
		private static void MungeUnmunge(string password,
			IDictionary<char, char[]> map, int index, StringBuilder buffer,
			List<string> list)
		{
			if (index == password.Length)
			{
				list.Add(buffer.ToString());
				return;
			}
			char[] chars;
			if (!map.TryGetValue(password[index], out chars))
			{
				chars = new[] { password[index] };
			}
			foreach (var c in chars)
			{
				buffer.Append(c);
				MungeUnmunge(password, map, index + 1, buffer, list);
				buffer.Length--;
			}
		}
		/// <summary>
		/// Munge a password.
		/// </summary>
		/// <remarks>http://en.wikipedia.org/wiki/Munged_password</remarks>
		public static IEnumerable<string> Munge(this string password)
		{
			return MungeUnmunge(password, mungeMap);
		}
		/// <summary>
		/// Unmunge a (munged) password.
		/// </summary>
		public static IEnumerable<string> Unmunge(this string password)
		{
			return MungeUnmunge(password, unmungeMap);
		}

		/// <summary>
		/// Returns a new collection with characters scrabbled like the game.
		/// </summary>
		public static IEnumerable<string> Scrabble(this string word)
		{
			word.ThrowIfArgumentNull(nameof(word));
			return word.Select(x => x.ToString()).Scrabble();
		}
		/// <summary>
		/// Parse a string in UTC format as DateTime.
		/// </summary>
		public static DateTime ParseAsUtc(this string s)
		{
			return DateTimeOffset.Parse(s).UtcDateTime;
		}
		/// <summary>
		/// Convert a string to title case.
		/// </summary>
		/// <example>"war and peace" -> "War And Peace"</example>
		public static string ToTitleCase(this string s)
		{
			return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(s);
		}

		/// <summary>
		/// Get permutations of string for particular r.
		/// </summary>
		/// <remarks>nPr permutations</remarks>
		public static IEnumerable<string> Permutation(this string s, int r)
		{
			return s.ToCharArray().Permutation(r)
				.Select(x => new string(x.ToArray()));
		}
		/// <summary>
		/// Get permutations of string.
		/// </summary>
		/// <remarks>nPn permutations</remarks>
		public static IEnumerable<string> Permutation(this string s)
		{
			return s.Permutation(s.Length);
		}
		/// <summary>
		/// Get combinations of string for particular r.
		/// </summary>
		/// <remarks>nCr combinations</remarks>
		public static IEnumerable<string> Combination(this string s, int r)
		{
			return s.ToCharArray().Combination(r)
				.Select(x => new string(x.ToArray()));
		}
		/// <summary>
		/// Get combinations of string.
		/// </summary>
		/// <remarks>nCn combinations</remarks>
		public static IEnumerable<string> Combination(this string s)
		{
			return s.Combination(s.Length);
		}

		/// <summary>
		/// Split csv string using common delimiters.
		/// </summary>
		public static IEnumerable<string> SplitCsv(this string s)
		{
			return s.Split(new[] { ",", ";", "|" }, StringSplitOptions.RemoveEmptyEntries);
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
}
