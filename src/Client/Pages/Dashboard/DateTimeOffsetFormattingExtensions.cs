namespace Client.Pages.Dashboard;

/// <summary>
/// Extensions for <see cref="DateTimeOffset"/> to format it in different ways.
/// </summary>
internal static class DateTimeOffsetFormattingExtensions
{
    private static readonly SortedList<double, Func<TimeSpan, string>> _offsets = new()
    {
        // Offset in minutes, lambda that formats value as string
        // F0 converts floating point numbers like 6.789 into 6
        { 0.75, _ => "mindre än en minut" },
        { 1.5, _ => "ungefär en minut" },
        { 45, x => $"{x.TotalMinutes:F0} minuter" },
        { 90, _ => "ungefär en timme" },
        { 1440, x => $"ungefär {x.TotalHours:F0} timmar" },
        { 2880, _ => "en dag" },
        { 43200, x => $"{x.TotalDays:F0} dagar" },
        { 86400, _ => "ungefär en månad" },
        { 525600, x => $"{x.TotalDays / 30:F0} månader" },
        { 1051200, _ => "ungefär ett år" },
        { double.MaxValue, x => $"{x.TotalDays / 365:F0} år" },
    };

    /// <summary>
    /// Converts to a date string relative from now, e.g. "5 hours ago".
    /// </summary>
    /// <param name="input">The date and time to convert.</param>
    /// <returns>A formatted relative date and time.</returns>
    public static string ToRelativeDate(this DateTimeOffset input)
    {
        TimeSpan difference = DateTime.Now - input;
        const string suffix = " sedan";
        difference = new TimeSpan(Math.Abs(difference.Ticks));
        return _offsets.First(offset => offset.Key > difference.TotalMinutes).Value(difference) + suffix;
    }
}
