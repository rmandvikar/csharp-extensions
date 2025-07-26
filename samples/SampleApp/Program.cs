using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using rm.Extensions;

// GetValueOrDefault(key, defaultValue)
var map = new Dictionary<string, string>()
{
	["test"] = "test",
};
var value = map.GetValueOrDefault("test");

// AsReadOnly()
var romap = map.AsReadOnly();

// Chunk(size)
var items = Enumerable.Range(0, 10);
var chunks = items.Chunk(3).ToArray();

if (Debugger.IsAttached)
{
	Debugger.Break();
}
