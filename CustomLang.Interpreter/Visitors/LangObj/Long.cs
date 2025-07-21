using CustomLang.Interpreter.Exceptions;

namespace CustomLang.Interpreter.Visitors.LangObj;
public class Long : Numeric
{
    private long _value;
    
    public Long(long value) => _value = value;

    public override LangType GetObjType() => LangType.Int64;
    public override object Value => _value;
    
    public void SetValue(long value) => _value = value;
    
    protected override Numeric PerformAdd(Numeric other)
    {
        _value = checked(_value + ((Long)other)._value);
        return this;
    }
    
    protected override Numeric PerformSub(Numeric other)
    {
        _value = checked(_value - ((Long)other)._value);
        return this;
    }
    
    protected override Numeric PerformMultiply(Numeric other)
    {
        _value = checked(_value * ((Long)other)._value);
        return this;
    }
    
    protected override Numeric PerformDivide(Numeric other)
    {
        var divisor = ((Long)other)._value;
      
        switch (divisor)
        {
            case 0 when _value == 0:
                throw new UncertaintyException();
            case 0:
                throw new DivideByZeroException();
            default:
                _value /= divisor;
                return this;
        }
     
    }
    
    protected override Numeric PerformModulo(Numeric other)
    {
        var divisor = ((Long)other)._value;
        
        switch (divisor)
        {
            case 0 when _value == 0:
                throw new UncertaintyException();
            case 0:
                throw new DivideByZeroException();
            default:
                _value %= divisor;
                return this;
        }
    }
    
    protected override Numeric PerformPower(Numeric other)
    {
        var exp = ((Long)other)._value;
        if (exp == 0 &&  _value == 0) throw new UncertaintyException();
        _value = (long)Math.Pow(_value, exp);
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
    public override Numeric ToInteger() => new Integer((int)_value);
    public override Numeric ToLong() => this;
    
    public override string ToString() => _value.ToString();
    
}