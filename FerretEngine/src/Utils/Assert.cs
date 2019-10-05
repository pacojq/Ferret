using System.Diagnostics;

namespace FerretEngine.Utils
{
	public static class Assert
	{
		[Conditional("DEBUG")]
		[DebuggerHidden]
		public static void Fail()
		{
			Debug.Assert(false);
			Debugger.Break();
		}
		
		[Conditional("DEBUG")]
		[DebuggerHidden]
		public static void Fail(string message, params object[] args)
		{
			Debug.Assert(false, string.Format(message, args));
			Debugger.Break();
		}
		
		
		[Conditional("DEBUG")]
		[DebuggerHidden]
		public static void IsTrue(bool condition)
		{
			if (!condition)
				Fail();
		}
		
		[Conditional("DEBUG")]
		[DebuggerHidden]
		public static void IsTrue(bool condition, string message, params object[] args)
		{
			if (!condition)
				Fail(message, args);
		}
		
		[Conditional("DEBUG")]
		[DebuggerHidden]
		public static void IsFalse(bool condition)
		{
			IsTrue(!condition);
		}
		
		
		
		
		
		[Conditional("DEBUG")]
		[DebuggerHidden]
		public static void IsNull(object obj, string message, params object[] args)
		{
			IsTrue(obj == null, message, args);
		}
		
		[Conditional("DEBUG")]
		[DebuggerHidden]
		public static void IsNull(object obj)
		{
			IsTrue(obj == null);
		}
		
		
		[Conditional("DEBUG")]
		[DebuggerHidden]
		public static void IsNotNull(object obj, string message, params object[] args)
		{
			IsTrue(obj != null, message, args);
		}
		
		[Conditional("DEBUG")]
		[DebuggerHidden]
		public static void IsNotNull(object obj)
		{
			IsTrue(obj != null);
		}
	}
}