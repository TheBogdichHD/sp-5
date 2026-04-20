using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Net;
using System.Text;

namespace Lab5.Areas.Feed.Services;

public partial class HashtagFormatter : IHashtagFormatter
{
    [GeneratedRegex("(?<![\\w/])#([A-Za-z0-9_]{2,64})", RegexOptions.Compiled)]
    private static partial Regex HashtagRegex();

    public string Format(string? content)
    {
        var normalized = WebUtility.HtmlDecode(content ?? string.Empty);

        var matches = HashtagRegex().Matches(normalized);
        if (matches.Count == 0)
        {
            return HtmlEncoder.Default.Encode(normalized);
        }

        var sb = new StringBuilder(normalized.Length + 64);
        var cursor = 0;

        foreach (Match match in matches)
        {
            if (match.Index > cursor)
            {
                sb.Append(HtmlEncoder.Default.Encode(normalized[cursor..match.Index]));
            }

            var tag = match.Groups[1].Value;
            sb.Append($"<a class=\"text-swamp-600 hover:text-swamp-800\" href=\"/ponds/{tag}\">#{tag}</a>");

            cursor = match.Index + match.Length;
        }

        if (cursor < normalized.Length)
        {
            sb.Append(HtmlEncoder.Default.Encode(normalized[cursor..]));
        }

        return sb.ToString();
    }
}
