using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace RiaSimulator
{

    class Program
    {
        private static object StartCountingIdAt = 6;
        static async Task Main()
        {
            const string baseUrl = "https://localhost:7060/customer";
            const int numberOfRequests = 5;

            var tasks = new List<Task>();

            for (int i = 0; i < numberOfRequests; i++)
            {
                tasks.Add(SendPostRequest(baseUrl));
                tasks.Add(SendGetRequest(baseUrl));
            }

            await Task.WhenAll(tasks);

            Console.WriteLine("Simulated requests completed.");
        }

        static async Task SendPostRequest(string baseUrl)
        {
            var customers = GenerateRandomCustomers(2);
            using (var httpClient = new HttpClient())
            {
                try
                {

                    var response = await httpClient.PostAsJsonAsync($"{baseUrl}", customers);

                    if (response.IsSuccessStatusCode)
                    {

                        Console.WriteLine($"POST request completed successfully. {response.StatusCode}");
                    }
                    else
                    {
                        string responseMessage = await response.Content.ReadAsStringAsync();
                        Console.WriteLine($"POST request failed. {responseMessage}");

                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine(e.Message);

                }
            }
        }

        static async Task SendGetRequest(string baseUrl)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    var response = await httpClient.GetAsync($"{baseUrl}");
                    response.EnsureSuccessStatusCode();
                    var customersStream = await response.Content.ReadAsStreamAsync();
                    var customers = JsonSerializer.Deserialize<List<Customer>>(customersStream);
                    Console.WriteLine($"GET request completed successfully. {response.StatusCode}");
                    Console.WriteLine($"Retrieved {customers.Count} customers.");
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine(e.Message);

                }
            }
        }

        static List<Customer> GenerateRandomCustomers(int count)
        {
            var customers = new List<Customer>();
            var random = new Random();
            for (int i = 0; i < count; i++)
            {
                lock (StartCountingIdAt)
                {
                    var cid = (int)StartCountingIdAt;
                    cid++;
                    

                    customers.Add(new Customer
                    {
                        Id = cid,
                        FirstName = GetRandomFirstName(),
                        LastName = GetRandomLastName(),
                        Age = random.Next(10, 91)
                    });
                    StartCountingIdAt = (object)cid;
                }

            }

            return customers;
        }

        static string GetRandomFirstName()
        {

            var firstNames = new[] { "Leia",
                                "Sadie",
                                "Jose",
                                "Sara",
                                "Frank",
                                "Dewey",
                                "Tomas",
                                "Joel",
                                "Lukas",
                                "Carlos"
                                };
            var random = new Random();
            return firstNames[random.Next(firstNames.Length)];
        }

        static string GetRandomLastName()
        {
            var lastNames = new[] { "Liberty",
                                "Ray",
                                "Harrison",
                                "Ronan",
                                "Drew",
                                "Powell",
                                "Larsen",
                                "Chan",
                                "Anderson",
                                "Lane"
                                };
            var random = new Random();
            return lastNames[random.Next(lastNames.Length)];
        }
    }

}