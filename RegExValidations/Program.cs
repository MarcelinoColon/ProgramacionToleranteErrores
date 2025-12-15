
// ^ inicia la regular expresion y termina con $
//[a-zA-Z] permite letras de la a a la z minusculas y mayusculas
//\s permite espacios y {1, 50} es el tamaño en caracteres
//Á-ÿ permite caracteres unicode de ese rango, donde entran los acentos


using System.Text.RegularExpressions;

var patterName = @"^[a-zA-ZÁ-ÿ\s]{1, 50}$";

var regexName = new Regex(patterName);

var name = "Francisco Ibarra";

if (regexName.IsMatch(name))
{
    Console.WriteLine($"{name} es un nombre valido");
}
else
{
    Console.WriteLine($"{name} no es un nombre valido");
}