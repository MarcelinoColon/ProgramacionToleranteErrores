using System.Diagnostics;

int n = 1000;
string[] data = { "123", "XYZ" };

var sw = new Stopwatch();

sw.Start();

int successNumber = 0;

try
{
    for (int i = 0; i < n; i++)
    {
        var val = data[i % 2];

        if (int.TryParse(val, out int number))
        {
            successNumber++;
        }

        //try
        //{
        //    int.Parse(val);
        //    successNumber++;
        //}
        //catch (FormatException)
        //{
        //}
    }
}
catch (Exception ex)
{
    Console.WriteLine(ex.Message);
}

sw.Stop();

Console.WriteLine($"Tiempo: {sw.ElapsedMilliseconds} ms, existos {successNumber}");