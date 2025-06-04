using System.Net.Http.Json;

class TestApi
{
    public async static void Test()
    {
        using var client = new HttpClient();
        var loginData = new
        {
            login = "paw",
            password = "123456"
        };
        var response = await client.PostAsJsonAsync("http://localhost:5091/api/login", loginData);

        Console.WriteLine($"Status code: {response.StatusCode}");
        var jsonResponse = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"Response: {jsonResponse}");

        var countriesResponse = await client.GetAsync("http://localhost:5091/api/countries");
        Console.WriteLine($"Status code: {countriesResponse.StatusCode}");
        var content = await countriesResponse.Content.ReadAsStringAsync();
        Console.WriteLine("Response:");
        Console.WriteLine(content);
    }
}