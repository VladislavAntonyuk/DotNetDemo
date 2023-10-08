public class CollectionExpressions
{
    public CollectionExpressions()
    {
        int[] nums1 = { 1, 2, 3 };
        Console.WriteLine(string.Join(',', nums1));
        int[] nums2 = [ 1, 2, 3 ];
        Console.WriteLine(string.Join(',', nums2));

        List<int> nums3 = [ 1, 2, 3 ];
        Console.WriteLine(string.Join(',', nums3));
        nums3.Add(4);
        Console.WriteLine(string.Join(',', nums3));
        Span<int> span = [ 1, 2, 3 ];

        int[] numsA = [ 1, 2, 3 ]; 
        int[] numsB = [ 11, 12, 13 ];
        int[] numsCombined = [ ..numsA, 7, 8, ..numsB];
        Console.WriteLine(string.Join(',', numsCombined));
    }
}
