using System.Reflection;

public static class SingletonHelper
{
    public static object ReplaceSingletonInstance<T>(object newInstance)
    {
        var field = typeof(T).GetField("_instance", BindingFlags.Static | BindingFlags.NonPublic);
        if (field == null)
            throw new InvalidOperationException($"Field '_instance' not found in {typeof(T)}.");

        var originalInstance = field.GetValue(null);
        field.SetValue(null, newInstance);
        return originalInstance;
    }

    public static void RestoreSingletonInstance<T>(object originalInstance)
    {
        var field = typeof(T).GetField("_instance", BindingFlags.Static | BindingFlags.NonPublic);
        if (field == null)
            throw new InvalidOperationException($"Field '_instance' not found in {typeof(T)}.");

        field.SetValue(null, originalInstance);
    }
}