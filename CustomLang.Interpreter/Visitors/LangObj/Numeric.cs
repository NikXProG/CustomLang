using Double = CustomLang.Interpreter.Visitors.LangObj.Double;

namespace CustomLang.Interpreter.Visitors.LangObj;

public abstract class Numeric : LangObject
{
    public abstract object Value { get; }
    
    // Операторы
    public static Numeric operator +(Numeric a, Numeric b) => a.Add(b);
    public static Numeric operator -(Numeric a, Numeric b) => a.Sub(b);
    public static Numeric operator *(Numeric a, Numeric b) => a.Multiply(b);
    public static Numeric operator /(Numeric a, Numeric b) => a.Divide(b);
    public static Numeric operator %(Numeric a, Numeric b) => a.Mod(b);
    public static Numeric operator -(Numeric a) => a.Negate();
    public static Numeric operator ~(Numeric a) => a.Not();

    // Базовые операции
    public Numeric Add(Numeric other) => PerformOperation(other, (a, b) => a.PerformAdd(b));
    public Numeric Sub(Numeric other) => PerformOperation(other, (a, b) => a.PerformSub(b));
    public Numeric Multiply(Numeric other) => PerformOperation(other, (a, b) => a.PerformMultiply(b));
    public Numeric Divide(Numeric other) => PerformOperation(other, (a, b) => a.PerformDivide(b));
    public Numeric Mod(Numeric other) => PerformOperation(other, (a, b) => a.PerformModulo(b));
    public Numeric Pow(Numeric other) => PerformOperation(other, (a, b) => a.PerformPower(b));
    public virtual Numeric Not() => PerformNot();
    public virtual Numeric Negate() => UnaryNegate();

    private Numeric PerformOperation(Numeric other, Func<Numeric, Numeric, Numeric> operation)
    {
        if (GetType() == other.GetType())
            return operation(this, other);

        var promoted = PromoteTypes(this, other);
        return operation(promoted.a, promoted.b);
    }

    protected static (Numeric a, Numeric b) PromoteTypes(Numeric a, Numeric b)
    {
        if (a is Double || b is Double)
            return (a.ToFloat(), b.ToFloat());
        
        if (a is Long || b is Long)
            return (a.ToLong(), b.ToLong());
            
        return (a.ToInteger(), b.ToInteger());
    }

    // Абстрактные методы
    public abstract Numeric ToFloat();
    public abstract Numeric ToInteger();
    public abstract Numeric ToLong();
    
    protected abstract Numeric PerformAdd(Numeric other);
    protected abstract Numeric PerformSub(Numeric other);
    protected abstract Numeric PerformMultiply(Numeric other);
    protected abstract Numeric PerformDivide(Numeric other);
    protected abstract Numeric PerformModulo(Numeric other);
    protected abstract Numeric PerformPower(Numeric other);
    protected virtual Numeric PerformNot() => this;
    protected virtual Numeric UnaryNegate() => this;
}