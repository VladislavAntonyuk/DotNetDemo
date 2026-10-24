namespace Net11Demo;

// Only this assembly can derive from GateState, so the compiler knows every subtype
public closed record class GateState;
public record class GateClosed : GateState;
public record class GateOpen(float Percent) : GateState;
public record class GateMaintenance(string Technician) : GateState;

public class ClosedHierarchies
{
    public ClosedHierarchies()
    {
        GateState[] states = [new GateClosed(), new GateOpen(75), new GateMaintenance("Olena")];
        foreach (var state in states)
        {
            Console.WriteLine(Format(state));
        }
    }

    // No CS8509 warning and no '_' branch
    static string Format(GateState state) => state switch
    {
        GateClosed => "Gate is closed",
        GateOpen(var percent) => $"Gate is {percent}% open",
        GateMaintenance(var technician) => $"Under maintenance ({technician})"
    };
}
