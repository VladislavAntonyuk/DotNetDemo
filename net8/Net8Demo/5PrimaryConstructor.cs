public class PrimaryCtor
{
    public string DataLabel { get; }
    public float DataValue { get; }
    public PrimaryCtor(string dataLabel, float dataValue)
    {
        DataLabel = dataLabel;
        DataValue = dataValue;
    }
}


// private fields
public class PrimaryCtor1(string dataLabel, float dataValue)
{
}


// access public properties
public class PrimaryCtor2(string dataLabel, float dataValue)
{
    public string DataLabel => dataLabel;

    public float DataValue => dataValue;
}

public class PrimaryCtor3(string dataLabel, float dataValue)
{
    public string DataLabel => dataLabel;

    public float DataValue => dataValue;

    public PrimaryCtor3(string dataLabel) : this(dataLabel, 10)
    {

    }
}