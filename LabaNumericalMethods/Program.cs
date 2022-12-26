
using LanaNumericalMethods;

Console.WriteLine("Введите путь к данным:");
var path = Console.ReadLine();
Console.WriteLine("Введите путь куда вывести ответ:");
var newPath = Console.ReadLine();
var startAlgorithm = new Algorithm(path, newPath);

