using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using maison_quizz;
using maison_quizz.Services;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IQuizService, QuizService>();
builder.Services.AddScoped<ResultatService>();
builder.Services.AddScoped<HistoryService>();
builder.Services.AddBlazoredLocalStorage();

await builder.Build().RunAsync();

