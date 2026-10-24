namespace Net11Demo;

public record class Cat(string Name, int LivesLeft);
public record class Dog(string Name, string Breed);
public record class Bird(string Name, bool CanFly);

public union Pet(Cat, Dog, Bird);

public class Unions
{
    public Unions()
    {
        Pet[] pets = [new Dog("Rex", "Shepherd"), new Cat("Tom", 9), new Bird("Kesha", true)];

        foreach (var pet in pets)
        {
            Console.WriteLine(Describe(pet));
        }
    }

    // Exhaustive: no '_' branch needed. Add a case to Pet and this method stops compiling cleanly.
    static string Describe(Pet pet) => pet switch
    {
        Dog d => $"Dog {d.Name} ({d.Breed})",
        Cat c => $"Cat {c.Name}, lives left: {c.LivesLeft}",
        Bird b => $"Bird {b.Name}, can fly: {b.CanFly}"
    };
}
