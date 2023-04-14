
some2();


void some2()
{
    int k = 0;
    Parallel.For(0, 5000, i =>
    {
        Console.WriteLine(i);
        k += k++;
    });
    Console.WriteLine("asdasda"+ k);
}