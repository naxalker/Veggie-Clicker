using System.Globalization;
using UnityEditor;
using UnityEngine;

public enum IdleAbbreviation
{
    k,
    M,
    B,
    T,
    q,
    Q,
    s,
    S,
    o,
    N,
    d,
    U,
    D,
    Td
}

public static class DoubleUtilities
{
    public static string ToIdleNotation(double value)
    {
        if (value < 1000)
            return value.ToString("F2");

        double tmpValue = value;
        int abbreviationIndex = -1;

        while (tmpValue > 1000)
        {
            tmpValue /= 1000;
            abbreviationIndex++;
        }

        if (abbreviationIndex >= System.Enum.GetValues(typeof(IdleAbbreviation)).Length)
            return ToScientificNotation(value);

        string idleAbbreviation 
            = System.Enum.GetValues(typeof(IdleAbbreviation)).GetValue(abbreviationIndex).ToString();

        return tmpValue.ToString("F2") + idleAbbreviation;
    }

    public static string ToScientificNotation(double value)
    {
        int exponent = 0;
        double tmpValue = value;

        if (tmpValue < 10)
            return value.ToString("F2");

        while (tmpValue > 10)
        {
            tmpValue /= 10;
            exponent++;
        }

        return tmpValue.ToString("F2") + "e" + exponent;
    }

    public static string ToCustomScientificNotation(double value)
    {
        if (value < Mathf.Pow(10, 12))
            return ToSeparatedThousands(value);
        else
            return ToScientificNotation(value);
    }

    public static string ToSeparatedThousands(double value)
    {
        NumberFormatInfo nfi = new NumberFormatInfo();

        nfi.NumberGroupSeparator = " ";
        nfi.NumberDecimalSeparator = ".";

        return value.ToString("N", nfi);
    }
}
