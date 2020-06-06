using System;
using System.Text.Json;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {


            B b = new B();

            b.Name = "Hello world";

            string json = b.ToJson();

            Console.WriteLine(json);
            Console.ReadKey();
        }
    }

    internal class B : A
    {
        public string Name { get; set; } 
    }

    internal class A
    {
        public string ToJson()
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                IgnoreNullValues = true
            };

            return JsonSerializer.Serialize(this, options);
        }
    }
}
