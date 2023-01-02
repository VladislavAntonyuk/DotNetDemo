// Generic attributes
// Before C# 11:
public class TypeAttribute : Attribute
{
    public TypeAttribute(Type t) => ParamType = t;

    public Type ParamType { get; }
}



// C# 11 feature:
public class GenericAttribute<T> : Attribute { }