using System.Numerics;
using CustomLang.Interpreter.Visitors.LangObj;
using Double = CustomLang.Interpreter.Visitors.LangObj.Double;

namespace CustomLang.Interpreter.Extentions;

public static class ConvertExtentions
{
    public static Numeric ToNumeric(this LangObject value)
    {
        
        if (value is Numeric numeric)
        {
            return numeric;
        }
        
        throw new InvalidCastException("Cannot convert value to Numeric");
        
    }
    
    public static LangObject ParseInt(this string numberStr)
    {
        // if (byte.TryParse(numberStr, out byte byteValue))
        //     return byteValue;
        //
        // if (sbyte.TryParse(numberStr, out sbyte sbyteValue))
        //     return sbyteValue;
        //
        // if (short.TryParse(numberStr, out short shortValue))
        //     return shortValue;
        //
        // if (ushort.TryParse(numberStr, out ushort ushortValue))
        //     return ushortValue;
        //
        
        // if (int.TryParse(numberStr, out int intValue))
        //     return new Integer(intValue);
        //
        // if (uint.TryParse(numberStr, out uint uintValue))
        //     return new Long(uintValue);
        
        if (long.TryParse(numberStr, out long longValue))
            return new Long(longValue);
        
        // if (ulong.TryParse(numberStr, out ulong ulongValue))
        //     return ulongValue;

        // if (double.TryParse(numberStr, out double doubleValue))
        //     return new Double(doubleValue);
        //
        // if (long.TryParse(numberStr, out dec longValue))
        //     return longValue;
        
        
        // if (BigInteger.TryParse(numberStr, out BigInteger bigIntValue))
        //     return bigIntValue;
        
        throw new FormatException("Строка не является числом.");
    }
}