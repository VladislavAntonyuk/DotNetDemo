// DateTime
public readonly struct AutoDefaultStruct
{
    public int IntegerValue { get; }
    public string StringValue { get; }
    public double DoubleValue { get; }
    public AutoDefaultStruct()
    {
        //IntegerValue = default;
        //StringValue = default;
        //DoubleValue = default;
        // doesn't have to set values for all fields, default initializations will be added at the beginning of constructor by compiler
    }
}
