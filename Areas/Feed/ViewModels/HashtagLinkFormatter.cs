using System.Text.RegularExpressions;

namespace Lab5.Areas.Feed.ViewModels;

public static partial class HashtagLinkFormatter
{
    [GeneratedRegex("#\\w+")]
    private static partial Regex ExtractRegex();

    [GeneratedRegex("#(\\w+)")]
    private static partial Regex ReplaceRegex();

    public static List<string> ExtractHashtags(string postContent)
    {
        return ExtractRegex().Matches(postContent)
            .Select(m => m.Value.TrimStart('#'))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();
    }

    public static string WithLinks(string postContent)
    {
        return ReplaceRegex().Replace(
            postContent,
            "<a class='text-swamp-600 hover:text-swamp-800 font-medium' href='/ponds/$1'>#$1</a>");
    }
}
