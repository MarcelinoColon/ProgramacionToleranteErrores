Console.WriteLine("Escribe tu edad: ");
string sAge = Console.ReadLine();

//Validaciones sintactica
if (!int.TryParse(sAge, out int age))
{
    Console.WriteLine($"[{sAge}] no tiene un formato de edad valido");
    return;
}

//Valicones Semanticas
if (age < 0 || age > 125)
{
    Console.WriteLine($"[{sAge}] la edad esta fuera de un rango real");
    return;
}
if (age < 18)
{
    Console.WriteLine("Debes ser mayor de edad");
    return;
}

Console.WriteLine($"Tu edad {sAge} es correcta, continuar...");