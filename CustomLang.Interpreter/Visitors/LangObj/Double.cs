using System.Globalization;

namespace CustomLang.Interpreter.Visitors.LangObj;


public class Double : Numeric
{
    private double _value;
    
    public Double(double value) => _value = value;

    public override LangType GetObjType() => LangType.Double;
    public override object Value => _value;
    
    public void SetValue(double value) => _value = value;
    
    protected override Numeric PerformAdd(Numeric other)
    {
        _value += ((Double)other)._value;
        return this;
    }
    
    protected override Numeric PerformSub(Numeric other)
    {
        _value -= ((Double)other)._value;
        return this;
    }
    
    protected override Numeric PerformMultiply(Numeric other)
    {
        _value *= ((Double)other)._value;
        return this;
    }
    
    protected override Numeric PerformDivide(Numeric other)
    {
        var divisor = ((Double)other)._value;
        if (divisor == 0) throw new DivideByZeroException();
        _value /= divisor;
        return this;
    }
    
    protected override Numeric PerformModulo(Numeric other)
    {
        var divisor = ((Double)other)._value;
        if (divisor == 0) throw new DivideByZeroException();
        _value %= divisor;
        return this;
    }
    
    protected override Numeric PerformPower(Numeric other)
    {
        _value = Math.Pow(_value, ((Double)other)._value);
        return this;
    }
    
    public override Numeric ToFloat() => this;
    public override Numeric ToInteger() => new Integer((int)_value);
    public override Numeric ToLong() => new Long((long)_value);
    
    public override string ToString() => _value.ToString(CultureInfo.InvariantCulture);
}