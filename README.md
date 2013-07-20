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

####check extensions:

```c#
public void SomeMethod(object obj1, object obj2) 
{
    // throws ArgumentNullException if object is null
    obj1.NullArgumentCheck("obj1");
	obj2.NullArgumentCheck("obj2");
    // OR 
    new[] { obj1, obj2 }.NullArgumentCheck();
    
    // ...
    
    object obj = DoSomething();
    // throws NullReferenceException if object is null
    obj.NullCheck("obj");
}
```

```c#
public void SomeMethod(string s1, string s2) 
{
    // throws ArgumentNullException or EmptyException if string is null or empty
    s1.NullOrEmptyArgumentCheck("s1");
    s2.NullOrEmptyArgumentCheck("s2");
    // OR 
    new[] { s1, s2 }.NullOrEmptyArgumentCheck();
    
    // ...
    
    string s = DoSomething();
	// throws NullReferenceException or EmptyException if string is null or empty.
    s.NullOrEmptyCheck("s");
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
