public class DataSlice {
public string DataLabel { get; } public float DataValue { get; }
public DataSlice(string dataLabel, float {
DataLabel = dataLabel;
DataValue = dataValue; }
}


// private fields
public class DataSlice(string dataLabel, flo {
... }


// access public properties
public class DataSlice(string dataLabel, flo {
public string DataLabel { get => dataLab
public float DataValue { get => dataValu }



You can also add additional constructors, provided they call the primary constructor:
public class DataSlice(string dataLabel, flo {
public string DataLabel { get => dataLab public float DataValue { get => dataValu
public DataSlice(string dataLabel) : thi }