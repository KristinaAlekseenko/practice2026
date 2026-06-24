using System;

namespace task01;

public static class StringExtensions
{
    public static bool IsPalindrome(this string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return false;
        }

        string lowerInput = input.ToLower();
        string cleaned = "";

        foreach (char c in lowerInput)
        {
            if (!char.IsWhiteSpace(c) && !char.IsPunctuation(c))
            {
                cleaned += c;
            }
        }

        char[] charArray = cleaned.ToCharArray();
        Array.Reverse(charArray);
        string reversed = new string(charArray);

        return cleaned == reversed;
    }
}