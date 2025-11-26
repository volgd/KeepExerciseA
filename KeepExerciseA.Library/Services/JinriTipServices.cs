using System.Text;
using System.Text.Json;
using KeepExerciseA.Library.Helpers;
using KeepExerciseA.Library.Models;

namespace KeepExerciseA.Library.Services;

public class JinriTipServices(IAlertServices _alertServices) : ITodayExercisesTipServices
{
    private string Apikey = "b3efab1c-102e-4a3c-9a4c-3216c47852e3";

    public const string Server = "doubao";
    
    
    public async Task<TodayExerciseTips> GetTodayExerciseTipsAsync()
    {
        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {Apikey}");
        httpClient.DefaultRequestHeaders.Add("Accept", "application/json");

        HttpResponseMessage response = new HttpResponseMessage();
        RootObject rootObject;
        try
        {
            var requestBody = new
            {
                model = "doubao-seed-1-6-251015",  // 根据实际模型调整
                messages = new[]
                {
                    new { role = "user", content = "请给我一个部位肌肉的锻炼方法和注意事项" }
                },
                max_tokens = 2000
            };
            var jsonContent = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            response =
                await httpClient.PostAsync("https://ark.cn-beijing.volces.com/api/v3/chat/completions",content);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            await _alertServices.AlertAsync(ErrorMesssageHelper.HttpClientErrorTitle,
                ErrorMesssageHelper.GetHttpClientError(Server,e.Message));
        }
        var json = await response.Content.ReadAsStringAsync();
        try
        {
            rootObject = JsonSerializer.Deserialize<RootObject>(json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                })?? throw new JsonException();
        }
        catch (Exception e)
        {
            await _alertServices.AlertAsync(
                ErrorMesssageHelper.JsonDeserializationErrorTitle,
                ErrorMesssageHelper.GetJsonDeserializationError(Server, e.Message));
        }
        new TodayExerciseTips
        {
            Snippet = rootObject.Choices[0]?.Message?.Content?? throw new JsonException(),
        }
    }
}

public class RootObject
{
    public Choices[]? Choices { get; set; } 
}

public class Choices
{
    public Message? Message { get; set; }
}

public class Message
{
    public string? Content { get; set; }
    public string? Reasoning_content { get; set; }
}


