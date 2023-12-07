
using System.Linq;
internal class Program
{
    private static List<int> numbersSet = new List<int> { 10, 50, 100 };
    private static HashSet<List<int>> seen = new HashSet<List<int>>();
    private static void Main(string[] args)
    {

        while (true)
        {
            Console.WriteLine("Say the money amout to make the combinations");
            if (int.TryParse(Console.ReadLine(), out int target))
            {

                var result = GetAllCombinations(target);
                foreach (var r in result)
                {
                    Console.WriteLine(String.Join(',', r));
                }
            }
            Console.WriteLine("Above are the combinations\n##############");
            seen = new HashSet<List<int>>();
        }
    }

    private static List<List<int>> GetAllCombinations(int target)
    {

        List<List<int>> result = new List<List<int>>();
        FindCombinations(target, 0, 0, new List<int>(), result);
        return result;

    }

    private static void FindCombinations(int target, int currentSum, int startIndex,
        List<int> currentCombination, List<List<int>> result)
    {
        if (currentSum == target)
        {
            result.Add(currentCombination);
            return;
        }
        else if (currentSum > target)
        {
            return;
        }

        for (int i = startIndex; i < numbersSet.Count(); i++)
        {
            //foreach(var n in numbersSet){
            var n = numbersSet.ElementAt(i);
            int newSum = currentSum + n;

            List<int> newCombination = new List<int>(currentCombination);
            newCombination.Add(n);

            seen.Add(newCombination);
            FindCombinations(target, newSum, i, newCombination, result);


        }
    }
}