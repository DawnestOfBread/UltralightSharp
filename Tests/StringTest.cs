using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using Ultralight;
using UltralightSharp;


namespace Tests;

[TestClass]
public sealed class StringTest
{
	[TestMethod]
	public void StringData()
	{
		const string testString = "Hello World!";
		var str = CAPI_String.UlCreateString(testString);
		
		unsafe
		{
			sbyte* data = CAPI_String.UlStringGetData(str);
			string managedString = Marshal.PtrToStringAnsi((IntPtr)data)!;
		
			Assert.AreEqual(testString, managedString);
		}
	}
}