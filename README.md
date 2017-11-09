csharp-extensions
=================


#### string extensions:

```c#
var s = "";
if (s.IsNullOrEmpty()) { /**/ }
if (s.IsNullOrWhiteSpace()) { /**/ }
```

```c#
// some string that could be null/empty/whitespace
string s = null; // or "value"
string text = "default";
if (!s.IsNullOrWhiteSpace()) text = s.Trim();
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
"{0} is a {1}".format("this", "test");
// parameter index is optional
"{} is a {}".format("this", "test");
"{} is a {1}".format("this", "test"); // mixing is ok
// parameter meta is allowed
"The name is {0}. {first} {last}.".format(lastName, firstName, lastName); // adding arg meta is ok
"The name is {last}. {first} {last}.".format(lastName, firstName); // bit intelligent about repeating arg meta
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

```c#
// parse a string in UTC format as DateTime
DateTime date = "2013-04-01T03:42:14-04:00".ParseAsUtc();
// date: 4/1/2013 7:42:14 AM, Kind: Utc
```

```c#
// convert a string to title case
string result = "war and peace".ToTitleCase();
// result: "War And Peace"
```

```c#
// split a csv string
string[] result = "a,b;c|d".SplitCsv().ToArray();
// result: [ "a", "b", "c", "d" ]
```

```c#
// substring till end
string result = "this is a test".SubstringTillEnd(4);
// result: "test"
```

```c#
// substring by specifying start index and end index
string result = "this".SubstringByIndex(1, 3);
// result: "hi"
```

#### ThrowIf extensions:

```c#
public void SomeMethod(object obj1, object obj2) 
{
	// throws ArgumentNullException if object is null
	obj1.ThrowIfArgumentNull("obj1");
	obj2.ThrowIfArgumentNull("obj2");
	// OR 
	new[] { obj1, obj2 }.ThrowIfAnyArgumentNull();
	
	// ...
	
	object obj = DoSomething();
	// throws NullReferenceException if object is null
	obj.ThrowIfNull("obj");
	// OR 
	new[] { obj1, obj2 }.ThrowIfAnyNull();
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

#### DateTime extensions:

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

#### IEnumerable extensions:

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
// shuffle collection in O(n) time (Fisher-Yates shuffle, revised by Knuth)
var shuffled = new[] { 0, 1, 2, 3 }.Shuffle();
// shuffled: { 2, 3, 1, 0 }
```

```c#
// slice a collection as Python (http://docs.python.org/2/tutorial/introduction.html#strings)
var a = new[] { 0, 1, 2, 3, 4 }
var slice = a.Slice(step: 2);
// slice: { 0, 2, 4 }

a.Slice(start, end);		// items start through end-1
a.Slice(start);				// items start through the rest of the array
a.Slice(0, end);			// items from the beginning through end-1
a.Slice();					// a copy of the whole array
a.Slice(start, end, step);	// start through not past end, by step
a.Slice(-1);				// last item in the array
a.Slice(-2);				// last two items in the array
a.Slice(-3, -2);			// third last item in the array
a.Slice(0, -2);				// everything except the last two items
a.Slice(step: -1);			// copy with array reversed

// help
a.Slice(end: 2)			// 1st 2
a.Slice(2)				// except 1st 2
a.Slice(-2)				// last 2
a.Slice(0, -2)			// except last 2
a.Slice(1, 1 + 1)		// 2nd char
a.Slice(-2, -2 + 1)		// 2nd last char
```

```c#
// scrabble a list of words (like the game)
var words = { "this", "test" };
var scrabbled = words.Scrabble();
// scrabbled: { "this", "thistest", "test", "testthis" }
```

```c#
// convert a collection to HashSet
HashSet<string> hashset = new[] { "this", "test" }.ToHashSet();
```

```c#
// check an enumerable's count efficiently
if (enumerable.Count() == 2) { ... } // inefficient for large enumerable
if (enumerable.HasCount(2)) { ... }
if (enumerable.HasCountOfAtLeast(2)) { ... } // count >= 2
```

```c#
// get permutations or combinations for particular r
var result = new[] { 1, 2 }.Permutation(2);
// result: { { 1, 2 }, { 2, 1 } }
var result = new[] { 1, 2 }.Combination(2);
// result: { { 1, 2 } }
```

```c#
// if a collection is empty instead of !collection.Any()
var collection = new[] { 1, 2 };
if (collection.IsEmpty()) { /**/ }
```

```c#
// get top n or bottom n efficiently (using min/max-heap)
IEnumerable<int> top_n = { 2, 3, 1, 4, 5 }.Top(3);
IEnumerable<int> bottom_n = { 2, 3, 1, 4, 5 }.Bottom(3);
// top_n: { 3, 5, 4 }
// bottom_n: { 3, 1, 2 }
// get top n or bottom n from IEnumerable
IEnumerable<Person> top_n = persons.Top(3);
IEnumerable<Person> bottom_n = persons.Bottom(3);
// get top n or bottom n by using a key selector or/and comparer
IEnumerable<Person> oldest_3 = persons.Top(3, x => x.Age);
IEnumerable<Person> youngest_3 = persons.Bottom(3, x => x.Age);
IEnumerable<Person> oldest_3 = persons.Top(3, personByAgeComparer);
IEnumerable<Person> youngest_3 = persons.Bottom(3, personByAgeComparer);
IEnumerable<Person> oldest_3 = persons.Top(3, x => x.Age, ageComparer);
IEnumerable<Person> youngest_3 = persons.Bottom(3, x => x.Age, ageComparer);
```

```c#
// source.Except(second, comparer) linqified instead of a full-blown class for comparer
source.ExceptBy(second, x => x.Member);
// same for source.Distinct(comparer)
source.DistinctBy(x => x.Member);
// same for source.OrderBy(keySelector, comparer)
source.OrderBy(x => x.Property,
	(p1, p2) => p1.CompareTo(p2) // where p1, p2 are of same type as x.Property
);
```

```c#
// source.OrEmpty() to avoid a null check
foreach (var item in source.OrEmpty()) { /**/ }
// instead of
if (source != null) { foreach (var item in source) { /**/ } }
```

```c#
// TrySingle() to get single without exception
if (source.TrySingle(out singleT)) { ... }
```

#### IList extensions:

```c#
// RemoveLast() to remove last item(s) in list
list.RemoveLast();
list.RemoveLast(2);
```

#### Enum extensions:

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

// enumValue.GetEnumName() is fastest of all
// fastest, dictionary lookup after 1st call
if (Color.Red.GetEnumName() == "Red") { /**/ }
// slightly slow, dictionary lookup after 1st call
if ("Red".GetEnumValue<Color>() == Color.Red) { /**/ }
// slow, due to reflection
if ("Red".Parse<Color>() == Color.Red) { /**/ }
// slowest, due to reflection
if (Color.Red.ToString() == "Red") { /**/ }
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
//  <select>
//    <option value="Red">Red color</option>
//    <option value="Green">Green</option>
//    <option value="Blue">Blue color</option>
//  </select>
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
bool hasValue = 0.IsDefined<Color>();
// hasValue: false
bool hasValue = 1.IsDefined<Color>();
// hasValue: true
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

#### NameValueCollection extensions:

```c#
// get query string for name-value collection
NameValueCollection nvc = new NameValueCollection { {"k1,", "v1"}, {"k2", "v2"} };
string query = nvc.ToQueryString(); // OR nvc.ToQueryString(prefixQuestionMark: false);
// query: "?k1%2C=v1&k2=v2" // OR "k1%2C=v1&k2=v2"
```

#### TimeSpan extensions:

```c#
// round timespan as ms, s, m, h, d, wk, mth, y.
string round = TimeSpan.FromDays(10).Round();
// round: "1wk"
```

```c#
// n Days, Hours, Minutes, Seconds, Milliseconds, etc.
TimeSpan ts = 10.Days();
```

#### Uri extensions:

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

#### Helper methods:

```c#
// swap two values or references
Helper.Swap(ref a, ref b);
```

#### decimal extensions:

```c#
// truncate decimal to specified digits
12.349m.TruncateTo(2); // 12.34m
```

#### int extensions:

```c#
// round int as k, m, g
1000.Round(); // "1k"
1000000.Round(); // "1m"
1500.Round(); // "1k"
1500.Round(1); // "1.5k"
```

#### Graph extensions:

```c#
// if graph is cyclic (used for deadlock detection)
bool isCyclic = graph.IsCyclic();
```

#### StringBuilder extensions:

```c#
// instead of buffer.AppendLine(string.Format(format, args))
buffer.AppendLine(format, args);
```

```c#
// reverse StringBuilder in-place
buffer.Reverse();
```

#### Dictionary extensions:

```c#
// for key in dictionary, get value if exists or default / specified value
var value = dictionary.GetValueOrDefault(key);
var value = dictionary.GetValueOrDefault(key, other);
```

```c#
// get dictionary as readonly
var dictionaryReadonly = dictionary.AsReadOnly();
```

#### Wrapped extensions:

```c#
// wrap (box) any type to avoid using pass by ref parameters
var intw = new Wrapped<int>(1); // or 1.Wrap();
// intw.Value = 1
```

#### BitSet:

```c#
BitSet bitset = new BitSet(10); // 0 to 10 inclusive
bitset.Add(5); // add 5
bitset.Add(6);
bitset.Remove(5); // remove 5
bitset.Remove(3);
bitset.Toggle(3); // toggle 3
bool has2 = bitset.Has(2); // if has 2
bitset.Clear(); // remove all
foreach(int item in bitset) { /**/ }
```

#### Circular Queue:

```c#
CircularQueue<int> cq = new CircularQueue<int>(capacity: 2);
cq.Enqueue(1);
cq.Enqueue(2);
cq.Enqueue(3);
cq.Enqueue(4);
int head;
head = cq.Dequeue(); // returns 3
head = cq.Dequeue(); // returns 4
```

#### Circular Stack:

```c#
CircularStack<int> cq = new CircularStack<int>(capacity: 2);
cq.Push(1);
cq.Push(2);
cq.Push(3);
cq.Push(4);
int top;
top = cq.Pop(); // returns 4
top = cq.Pop(); // returns 3
```

#### Deque:

```c#
Deque<int> dq = new Deque<int>();
Node<int> node = dq.Enqueue(1);
dq.Enqueue(2);
dq.Delete(node); // delete in O(1) time
int i = dq.Dequeue(); // returns 2
```
