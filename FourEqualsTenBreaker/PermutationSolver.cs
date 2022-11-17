namespace FourEqualsTenBreaker;

internal class PermutationSolver
{
    public static T[][] Permute<T>(T[] elems)
    {
        var list = new List<List<T>>();
        return DoPermute(elems, 0, elems.Length - 1, list);
    }

    static T[][] DoPermute<T>(T[] elems, int start, int end, List<List<T>> list)
    {
        if (start == end)
        {
            // We have one of our possible n! solutions,
            // add it to the list.
            list.Add(new List<T>(elems));
        }
        else
        {
            for (var i = start; i <= end; i++)
            {
                Swap(ref elems[start], ref elems[i]);
                DoPermute(elems, start + 1, end, list);
                Swap(ref elems[start], ref elems[i]);
            }
        }

        return list.ConvertAll(x => x.ToArray()).ToArray();
    }

    static void Swap<T>(ref T a, ref T b)
    {
        (a, b) = (b, a);
    }
}