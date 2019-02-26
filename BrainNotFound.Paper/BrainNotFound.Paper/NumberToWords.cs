using System;

public static class NumberToWords
{
    static string[] ones  = new string[] { "", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine" };
    static string[] teens = new string[] { "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
    static string[] tens  = new string[] { "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };
    static string[] thousandsGroups = { "", " Thousand", " Million", " Billion" };

    public const string ACCORDIAN = "accordion";
    public const string COLLAPSE  = "collapse";
    public const string HEADING   = "heading";

    private static string IntToString(int n, string leftDigits, int thousands)
    {
        if (n == 0)
        {
            return leftDigits;
        }

        string numberName = leftDigits;

        if (numberName.Length > 0)
        {
            numberName += " ";
        }

        if (n < 10)
        {
            numberName += ones[n];
        }
        else if (n < 20)
        {
            numberName += teens[n - 10];
        }
        else if (n < 100)
        {
            numberName += IntToString(n % 10, tens[n / 10 - 2], 0);
        }
        else if (n < 1000)
        {
            numberName += IntToString(n % 100, (ones[n / 100] + " Hundred"), 0);
        }
        else
        {
            numberName += IntToString(n % 1000, IntToString(n / 1000, "", thousands + 1), 0);
            if (n % 1000 == 0)
            {
                return numberName;
            }
        }

        return numberName + thousandsGroups[thousands];
    }

    public static string IntegerToWritten(int n)
    {
        if (n == 0)
        {
            return "Zero";
        }
        else if (n < 0)
        {
            return "Negative " + IntegerToWritten(-n);
        }

        return IntToString(n, "", 0);
    }
}