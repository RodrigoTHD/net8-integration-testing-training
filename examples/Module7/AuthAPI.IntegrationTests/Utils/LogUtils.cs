namespace AuthAPI.IntegrationTests.Utils;

public static class LogUtils
{
    public static void WriteLine<T>(T message) => TestContext.Out.WriteLine($"{message}");

    public static void ActualResult<T>(T message) => TestContext.Out.WriteLine($"Actual result: {message}");
}
