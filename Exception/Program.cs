try
{
    Console.WriteLine("Escribe un numero: ");
    int a = Convert.ToInt32(Console.ReadLine());

    Console.WriteLine("Escribe otro numero: ");
    int b = Convert.ToInt32(Console.ReadLine());

    int result = a / b;

    Console.WriteLine("El resultado de la division es: " + result);
}
catch(FormatException ex)
{
    Console.WriteLine("Debes escribir numeros");
}
catch(DivideByZeroException ex)
{
    Console.WriteLine("No puedes dividir entre cero" + ex.Message);
}
catch (Exception ex)
{
    Console.WriteLine("Ocurrio un error: " + ex.Message);
}
finally
{
    Console.WriteLine("Programa finalizado");
}

/*
Las excepciones son para situaciones inesperadas y poder tomar una decision
cuando pasen estas situaciiones y que el programa no se cierre.

Las excepciones tienen tipos y se pueden poner tantas excepciones quieras
clasificar.

sirven para manejar las excepciones individualmente dependiendo su tipo y no solo
mandar un mensaje generico de error.

*/