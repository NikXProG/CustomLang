namespace CustomLang.Interpreter.Extentions;

public static class BaseConvertExtensions
{
    private const string StandardDigits = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    
    public static string GetDigitsForBase(this int baseInput)
    {
        if (baseInput < 2 || baseInput > 36)
        {
            throw new ArgumentOutOfRangeException(nameof(baseInput), "Base must be between 2 and 36");
        }
    
        return StandardDigits.Substring(0, baseInput);
    }
}

public static class NumberExtensions
{
    public static string ToBase(this long value, int radix, string digits)
    {
        if (string.IsNullOrEmpty(digits))
        {
            throw new ArgumentNullException(nameof(digits), 
                "Digits must contain character value representations");
        }

        radix = Math.Abs(radix);
        if (radix > digits.Length || radix < 2)
        {
            throw new ArgumentOutOfRangeException(nameof(radix), radix, 
                $"Radix has to be > 2 and < {digits.Length}");
        }

        if (value == 0)
        {
            return "0";
        }

        bool isNegative = value < 0;
        long absoluteValue = Math.Abs(value);

        string integerResult = string.Empty;
        long quotient = absoluteValue;
        
        while (quotient > 0)
        {
            var temp = quotient % radix;
            integerResult = digits[(int)temp] + integerResult;
            quotient /= radix;
        }

        return isNegative ? "-" + integerResult : integerResult;
    }

    public static string ToBase(this double value, int radix, string digits, int maxFractionalDigits = 10)
    {
        if (string.IsNullOrEmpty(digits))
        {
            throw new ArgumentNullException(nameof(digits), 
                "Digits must contain character value representations");
        }

        radix = Math.Abs(radix);
        if (radix > digits.Length || radix < 2)
        {
            throw new ArgumentOutOfRangeException(nameof(radix), radix, 
                $"Radix has to be > 2 and < {digits.Length}");
        }

        if (double.IsNaN(value) || double.IsInfinity(value))
        {
            throw new ArgumentException("Value must be a finite number", nameof(value));
        }

        if (value == 0)
        {
            return "0";
        }

        bool isNegative = value < 0;
        double absoluteValue = Math.Abs(value);

        // Обработка целой части
        long integerPart = (long)Math.Floor(absoluteValue);
        string integerResult = integerPart.ToBase(radix, digits);

        // Обработка дробной части
        double fractionalPart = absoluteValue - integerPart;
        string fractionalResult = string.Empty;
        
        if (fractionalPart > 0)
        {
            fractionalResult = ".";
            int digitsCount = 0;
            
            while (fractionalPart > 0 && digitsCount < maxFractionalDigits)
            {
                fractionalPart *= radix;
                int digit = (int)Math.Floor(fractionalPart);
                fractionalResult += digits[digit];
                fractionalPart -= digit;
                digitsCount++;
            }
        }

        string result = integerResult + fractionalResult;
        return isNegative ? "-" + result : result;
    }
}

public static class StringExtensions
{
    public static long FromBase(this string text, int radix)
    {
        return text.FromBase(radix, radix.GetDigitsForBase());
    }
    
    public static double DecimalFromBase(this string text, int radix)
    {
        if (text.Contains('.') || text.Contains(','))
        {
            bool isNegative = text.StartsWith("-");
            var parts = text.Split('.', ',');
            string integerPartStr = isNegative ? parts[0].Substring(1) : parts[0];
            
            long integerPart = integerPartStr.FromBase(radix);
            if (isNegative) integerPart = -integerPart;
            
            double fractionalPart = 0;
            string digits = radix.GetDigitsForBase();
        
            for (int i = 0; i < parts[1].Length; i++)
            {
                fractionalPart += digits.IndexOf(parts[1][i]) / Math.Pow(radix, i + 1);
            }
        
            return integerPart + (isNegative ? -fractionalPart : fractionalPart);
        }
        return text.FromBase(radix);
    }
    
    public static long FromBase(this string text, int radix, string digits, bool forgive = false)
    {
        if (string.IsNullOrEmpty(digits))
        {
            throw new ArgumentNullException(nameof(digits),
                "Digits must contain character value representations");
        }

        radix = Math.Abs(radix);
        if (radix > digits.Length || radix < 2)
        {
            throw new ArgumentOutOfRangeException(nameof(radix), radix, 
                $"Radix has to be > 2 and < {digits.Length}");
        }
        
        if (string.IsNullOrEmpty(text))
        {
            return 0;
        }

        bool isNegative = text.StartsWith("-");
        string processedText = isNegative ? text.Substring(1) : text;
        
        long value = 0;
        for (int i = 0; i < processedText.Length; i++)
        {
            int digitValue = digits.IndexOf(processedText[i]);
            if (digitValue < 0 || digitValue >= radix)
            {
                if (!forgive)
                    throw new IndexOutOfRangeException("Text contains characters not found in digits.");
                continue;
            }
            value = value * radix + digitValue;
        }
        
        return isNegative ? -value : value;
    }

    public static string ToBase(this string text, int fromRadix, string fromDigits, int toRadix, string toDigits)
    {
        return text.FromBase(fromRadix, fromDigits).ToBase(toRadix, toDigits);
    }

    public static string ToBase(this string text, int from, int to, string digits)
    {
        return text.FromBase(from, digits).ToBase(to, digits);
    }
    
    public static string ToBase(this string text, int from, int to)
    {
        return text.FromBase(from, from.GetDigitsForBase()).ToBase(to, to.GetDigitsForBase());
    }
    
    public static string DecimalToBase(this string text, int from, int to)
    {
        return text.DecimalFromBase(from).ToBase(to, to.GetDigitsForBase());
    }
}