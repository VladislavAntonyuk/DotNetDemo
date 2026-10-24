static class Helpers
{
    public static string Format(string greeting, Customer customer) => $"{greeting}, {customer.Name} from {customer.City}!";
}
