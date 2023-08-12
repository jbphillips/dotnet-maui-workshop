using System.Net.Http.Json;

namespace MonkeyFinder.Services;

public class MonkeyService
{
    HttpClient httpClient;

    public MonkeyService()
    {
        httpClient = new HttpClient();
    }

    List<Monkey> monkeyList = new List<Monkey>();

    public async Task<List<Monkey>> GetMonkeys()
    {
        if(monkeyList.Count > 0)
        {
            return monkeyList;
        }

        var url = "https://montemagno.com/monkeys.json";

        // fetch monkey json
        // but instead of using newsoft like I've always done, use microsofts through the response
        // it will auto deserialize the json
        var response = await httpClient.GetAsync(url);

        if(response.IsSuccessStatusCode) 
        { 
            // this call will deserialize the json for me
            monkeyList = await response.Content.ReadFromJsonAsync<List<Monkey>>();
        }

        // Offline. If troubles with https
        //using var stream = await FileSystem.OpenAppPackageFileAsync("monkeydata.json");
        //using var reader = new StreamReader(stream);
        //var contents = await reader.ReadToEndAsync();
        //monkeyList = JsonSerializer.Deserialize(contents, MonkeyContext.Default.ListMonkey);

        return monkeyList;
    }
}
