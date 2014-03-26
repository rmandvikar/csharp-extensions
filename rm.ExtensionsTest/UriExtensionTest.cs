using System;
using NUnit.Framework;
using rm.Extensions;

namespace rm.ExtensionsTest
{
    [TestFixture]
    public class UriExtensionTest
    {
        [Test]
        [TestCase("https://scontent-a-sjc.xx.fbcdn.net/hphotos-ash2/t1.0-9/206120_10150185038165390_6567409_n.jpg", Hasher.sha1, "9ca6c1c83f07a42dc5549bc2ba0b2079686d0e12")]
        [TestCase("https://scontent-a-sjc.xx.fbcdn.net/hphotos-ash2/t1.0-9/206120_10150185038165390_6567409_n.jpg", Hasher.md5, "131b39ae30d4d55db34f5a03cd6ce1f8")]
        [TestCase(@"./images/d s .jpg", Hasher.sha1, "9ca6c1c83f07a42dc5549bc2ba0b2079686d0e12")]
        [TestCase(@"./images/d s .jpg", Hasher.md5, "131b39ae30d4d55db34f5a03cd6ce1f8")]
        public void Checksum01(string url_or_path, Hasher type, string hash)
        {
            Uri uri;
            if (url_or_path.StartsWith("."))
            {
                var path = url_or_path;
                uri = new Uri(new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase), path);
            }
            else
            {
                var url = url_or_path;
                uri = new Uri(url);
            }
            var h = uri.Checksum(type);
            Console.WriteLine(h);
            Assert.AreEqual(hash, h);
        }
    }
}
