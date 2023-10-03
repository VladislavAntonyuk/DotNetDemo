
int[] nums = { 1, 2, 3 };
It’s easy to shift over to collection literal syntax:
int[] nums = [ 1, 2, 3 ];

List<int> nums = [ 1, 2, 3 ];
// Couldn't do this with an array...
nums.Add(4);
Or try something more exotic, like a Span<T> , no stackalloc required:
Span<int> span = [ 1, 2, 3 ];


int[] numsA = [ 1, 2, 3 ]; 
int[] numsB = [ 11, 12, 13 ];
int[] numsCombined = [ ..numsA, 7, 8, ..nums // Now numsCombined = { 1, 2, 3, 7, 8, 11, 1
