using Double = CustomLang.Interpreter.Visitors.LangObj.Double;

namespace CustomLang.Interpreter.Visitors.LangObj;
public class Integer : Numeric
{
    private int _value;
    
    public Integer(int value) => _value = value;

    public override LangType GetObjType() => LangType.Int32;
    public override object Value => _value;
    
    public void SetValue(int value) => _value = value;
    
    protected override Numeric PerformAdd(Numeric other)
    {
        _value = checked(_value + ((Integer)other)._value);
        return this;
    }
    
    protected override Numeric PerformSub(Numeric other)
    {
        _value = checked(_value - ((Integer)other)._value);
        return this;
    }
    
    protected override Numeric PerformMultiply(Numeric other)
    {
        _value = checked(_value * ((Integer)other)._value);
        return this;
    }
    
    protected override Numeric PerformDivide(Numeric other)
    {
        var divisor = ((Integer)other)._value;
        if (divisor == 0) throw new DivideByZeroException();
        _value /= divisor;
        return this;
    }
    
    protected override Numeric PerformModulo(Numeric other)
    {
        var divisor = ((Integer)other)._value;
        if (divisor == 0) throw new DivideByZeroException();
        _value %= divisor;
        return this;
    }
    
    protected override Numeric PerformPower(Numeric other)
    {
        _value = (int)Math.Pow(_value, ((Integer)other)._value);
        return this;
    }
    
    protected override Numeric PerformNot()
    {
        _value = ~_value;
        return this;
    }
    
    protected override Numeric UnaryNegate()
    {
        _value = -_value;
        return this;
    }
    
    public override Numeric ToFloat() => new Double(_value);
    public override Numeric ToInteger() => this;
    public override Numeric ToLong() => new Long(_value);
    
    public override string ToString() => _value.ToString();
}
