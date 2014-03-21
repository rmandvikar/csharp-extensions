csharp-extensions
=================


####string extensions:

```c#
var s = "";
if (s.IsNullOrEmpty()) { /**/ }
if (s.IsNullOrWhiteSpace()) { /**/ }
```

####check extensions:

```c#
public void SomeMethod(object obj1, object obj2) 
{
    // throws ArgumentNullException if object is null
    obj1.NullArgumentCheck("obj1");
	obj2.NullArgumentCheck("obj2");
    
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