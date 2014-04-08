csharp-extensions
=================


####string extensions:

```c#
var s = "";
if (s.IsNullOrEmpty()) { /**/ }
if (s.IsNullOrWhiteSpace()) { /**/ }
```

```c#
// some string that could be null/empty/whitespace
string s = null; // or "value"
string text = "default";
if (!s.IsNullOrWhiteSpace())
	text = s.Trim();
// fluent code by avoiding comparison
string text = s.OrEmpty().Trim(); // "" when s is null/empty/whitespace
string text = s.Or("default").Trim(); // "default" when s is null/empty/whitespace
```

```c#
// html-en/decode, url-en/decode
string s = "";
string htmlencoded = s.HtmlEncode();
string htmldecoded = s.HtmlDecode();
string urlencoded = s.UrlEncode();
string urldecoded = s.UrlDecode();
```

```c#
// "".format() instead of string.Format()
"{0}".format("test")
```

```c#
// bool try-parse string with default value
bool b = "".ToBool(defaultValue: true);
// b: true
```

```c#
// munge a password
string[] munged = "password".Munge().ToArray();
// munged: { "p@$$w0rd" }
string[] munged = "ai".Munge().ToArray();
// munged: { "@1", "@!" }
// unmunge a password
string[] unmunged = "p@$$w0rd".Unmunge().ToArray();
// unmunged: { "password" }
string[] unmunged = "@1".Unmunge().ToArray();
// unmunged: { "ai", "al" }
```

```c#
// scrabble characters of word (like the game)
var word = "on";
var scrabbled = word.Scrabble();
// scrabbled: { "o", "on", "n", "no" }
```

####ThrowIf extensions:

```c#
public void SomeMethod(object obj1, object obj2) 
{
    // throws ArgumentNullException if object is null
    obj1.ThrowIfArgumentNull("obj1");
    obj2.ThrowIfArgumentNull("obj2");
    // OR 
    new[] { obj1, obj2 }.ThrowIfArgumentNull();
    
    // ...
    
    object obj = DoSomething();
    // throws NullReferenceException if object is null
    obj.ThrowIfNull("obj");
}
```

```c#
public void SomeMethod(string s1, string s2) 
{
    // throws ArgumentNullException or EmptyException if string is null or empty
    s1.ThrowIfNullOrEmptyArgument("s1"); // or s1.ThrowIfNullOrWhiteSpaceArgument("s1")
    s2.ThrowIfNullOrEmptyArgument("s2");
    // OR 
    new[] { s1, s2 }.ThrowIfNullOrEmptyArgument();
    
    // ...
    
    string s = DoSomething();
	// throws NullReferenceException or EmptyException if string is null or empty.
    s.ThrowIfNullOrEmpty("s"); // or s1.ThrowIfNullOrWhiteSpace("s")
}
```

####DateTime extensions:

```c#
// gives date in UTC format string
string dateUtc = date.ToUtcFormatString();
// dateUtc: "1994-11-05T13:15:30.000Z"
```

```c#
// gives min date that can be inserted in sql database without exception (SqlDateTime.MinValue)
DateTime date = new DateTime().ToSqlDateTimeMinUtc();
// date: 1/1/1753 12:00:00 AM
```

```c#
// date read from db or parsed from string has its Kind as Unspecified.
// specifying its kind as UTC is needed if date is expected to be UTC.
// ToUniversalTime() assumes that the kind is local while converting it and is undesirable.
DateTime date = DateTime.Parse("4/1/2014 12:00:00 AM").AsUtcKind();
// date: 4/1/2014 12:00:00 AM, Kind: Utc
```

####IEnumerable extensions:

```c#
// creates chunks of given collection of specified size
IEnumerable<IEnumerable<int>> chunks = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }.Chunk(3);
// chunks: { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 }, { 10 } }
```

```c#
// if a collection is null or empty
var collection = new[] { 1, 2 };
if (collection.IsNullOrEmpty()) { /**/ }
```

```c#
// split a collection into n parts
var collection = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
IEnumerable<IEnumerable<int>> splits = collection.Split(3);
// splits: { { 1, 4, 7, 10 }, { 2, 5, 8 }, { 3, 6, 9 } }
```

```c#
// if a collection is sorted
var collection1 = new[] { 1, 2, 3, 4 };
bool isSorted1 = collection1.IsSorted();
var collection2 = new[] { 7, 5, 3 };
bool isSorted2 = collection2.IsSorted();
// isSorted1, isSorted2: true
```

```c#
// Double(), DoubleOrDefault() as Single(), SingleOrDefault()
IEnumerable<int> doubleitems = new[] { 1, 2 }.Double();
// doubleitems: { 1, 2 }
IEnumerable<int> doubleitems = new[] { 1, 2, 3 }.Double(x => x > 1);
// doubleitems: { 2, 3 }
IEnumerable<int> doubleordefaultitems = new int[0].DoubleOrDefault();
// doubleordefaultitems: null
IEnumerable<int> doubleordefaultitems = new[] { 1, 2, 3 }.DoubleOrDefault(x => x > 1);
// doubleordefaultitems: { 2, 3 }

// throws InvalidOperationException
new[] { 1 }.Double();
new[] { 1 }.Double(x => x > 0);
new[] { 1 }.DoubleOrDefault();
new[] { 1 }.DoubleOrDefault(x => x > 0);
```

```c#
// shuffle collection in O(n) time (Fisher-Yates shuffle)
var shuffled = new[] { 0, 1, 2, 3 }.Shuffle();
// shuffled: { 2, 3, 1, 0 }
```

```c#
// slice a collection as Python (http://docs.python.org/2/tutorial/introduction.html#strings)
var a = new[] { 0, 1, 2, 3, 4 }
var slice = a.Slice(step: 2);
// slice: { 0, 2 }

a.Slice(start, end) // items start through end-1
a.Slice(start) // items start through the rest of the array
a.Slice(0, end) // items from the beginning through end-1
a.Slice() // a copy of the whole array
a.Slice(start, end, step) // start through not past end, by step
a.Slice(-1) // last item in the array
a.Slice(-2) // last two items in the array
a.Slice(-3, -2) // third last item in the array
a.Slice(0, -2) // everything except the last two items
a.Slice(step: -1) // copy with array reversed
```

```c#
// scrabble a list of words (like the game)
var words = { "this", "test" };
var scrabbled = words.Scrabble();
// scrabbled: { "this", "thistest", "test", "testthis" }
```

####Enum extensions:

```c#
enum Color { Red = 1, Green, Blue };
Color color = "Red".Parse<Color>();
// OR
Color color;
"Red".TryParse<Color>(out color);
// color: Color.Red
```

```c#
enum Color
{
    [Description("Red color")] Red = 1, 
    Green, 
    [Description("Blue color")] Blue
}
string redDesc = Color.Red.GetDescription();
string greenDesc = Color.Green.GetDescription();
// redDesc: "Red color"
// greenDesc: "Green"
```

```c#
enum Color { Red = 1, Green, Blue };
Color color = "Red".GetEnumValue<Color>();
// color: Color.Red

// string.GetEnumValue<TEnum> is faster than enumValue.ToString()
// faster
if ("Red".GetEnumValue<Color>() == Color.Red) { /**/ }
// slower
if ("Red" == Color.Red.ToString()) { /**/ }
```

```c#
enum Color
{
    [Description("Red color")] Red = 1, 
    Green, 
    [Description("Blue color")] Blue
}
IDictionary<string, string> colorsMap = EnumExtension.GetEnumNameToDescriptionMap<Color>();
// build a select list
IEnumerable<ListItem> selectOptions = colorsMap
	.Select(x => new ListItem() { text: x.Value, value: x.Key });
//	<select>
//	  <option value="Red">Red color</option>
//	  <option value="Green">Green</option>
//	  <option value="Blue">Blue color</option>
//	</select>
```

```c#
enum Color
{
    [Description("Red color")] Red = 1, 
    Green, 
    [Description("Blue color")] Blue
}
string redName = "Red color".GetEnumNameFromDescription<Color>()
// redName: "Red"
```

```c#
enum Color
{
    [Description("Red color")] Red = 1, 
    Green, 
    [Description("Blue color")] Blue
}
string colorjson = EnumExtension.GetJson<Color>();
// colorjson: @"{
//   Red: "Red color",
//   Green: "Green",
//   Blue: "Blue color"
// }"
```

```c#
// compile error: cannot have int as enum values or hyphen sign in enum values
enum Grade { Toddler, Pre-K, Kindergarten, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, College }
// work-around: use Description attribute
enum Grade 
{
	Toddler = 1, 
    [Description("Pre-K")] PreK, 
	Kindergarten, 
	[Description("1")] One, 
	[Description("2")] Two, 
	[Description("3")] Three, 
	[Description("4")] Four, 
	[Description("5")] Five, 
	[Description("6")] Six, 
	[Description("7")] Seven, 
	[Description("8")] Eight, 
	[Description("9")] Nine, 
	[Description("10")] Ten, 
	[Description("11")] Eleven, 
	[Description("12")] Twelve, 
	College
}

// to sort gradesUnsorted, use GetEnumValueFromDescription<T>() and GetDescription<T>() methods
string[] gradesUnsorted = new[] { "Pre-K", "1", "College", "2", "Toddler" };
Grade[] grades = gradesUnsorted
    .Select(x => x.GetEnumValueFromDescription<Grade>()).ToArray();
Array.Sort(grades);
string[] gradesSorted = grades.Select(x => x.GetDescription());
// gradesSorted: { "Toddler", "Pre-K", "1", "2", "College" } 
```

####NameValueCollection extensions:

```c#
// get query string for name-value collection
NameValueCollection nvc = new NameValueCollection { {"k1,", "v1"}, {"k2", "v2"} };
string query = nvc.ToQueryString(); // OR nvc.ToQueryString(prefixQuestionMark: false);
// query: "?k1%2C=v1&k2=v2" // OR "k1%2C=v1&k2=v2"
```

####TimeSpan extensions:

```c#
// round timespan as ms, s, h, d, wk, mth, y.
string round = TimeSpan.FromDays(10).Round();
// round: "1wk"
```

####Uri extensions:

```c#
// calculate uri's checksum (sha1, md5)
var uri = new Uri(@"https://www.google.com/images/srpr/logo11w.png") // url
	// OR new Uri(@"D:\temp\images\logo.png"); // local file
	// OR new Uri(new Uri(@"D:\temp\"), @".\images\logo.png"); // dir and relative path
string sha1 = uri.Checksum(Hasher.sha1);
string md5 = uri.Checksum(Hasher.md5);
// sha1: 349841408d1aa1f5a8892686fbdf54777afc0b2c
// md5: 57e396baedfe1a034590339082b9abce
```

####Helper methods:

```c#
// swap two values or references
a.Swap(ref a, ref b); // OR Helper.Swap(ref a, ref b);
```
