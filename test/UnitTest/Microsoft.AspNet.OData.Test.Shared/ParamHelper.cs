namespace System
{
    /// <summary>
    /// Helper for getting the correct text part of the <see cref="ArgumentException"/>.
    /// <remarks>The text part differs in .NET Core 3.</remarks>
	/// </summary>
    public static class ParamHelper
    {
#if NETCORE3x
        public static string Text(string parameterName)
		{
			return $" (Parameter '{parameterName}')";
		}
#else
		public static string Text(string parameterName)
		{
			return $"\r\nParameter name: {parameterName}";
		}
#endif
    }
}