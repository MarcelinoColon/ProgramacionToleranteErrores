Person person = new Person("Juan", new Address("Santo Domingo"));
//Person person = null;

//Console.WriteLine($"Hola {person.Name}");

//Null conditional

Console.WriteLine($"Hola {person?.Name}");

//null - coalescing
int length = person?.Name?.Length ?? 0;

Console.WriteLine($"El noombre tiene {length} letras");

Console.WriteLine($"Hola {person?.Name} que es de la ciudad {person?.Address?.city}");

record Address(string city);
record Person(string Name, Address Address);