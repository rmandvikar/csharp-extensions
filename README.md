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