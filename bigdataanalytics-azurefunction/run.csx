#load "services\DataIngestService.csx"
#load "services\DataLakeService.csx"
using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;

public static void Run(TimerInfo myTimer, TraceWriter log)
{
    MainAsync(log).Wait();

    if (myTimer.IsPastDue)
    {
        log.Info("Timer is running late!");
    }
    else
    {
        log.Info("Timer is on time!");
    }
    log.Info($"C# Timer trigger function executed at: {DateTime.Now}");
}

static async Task MainAsync(TraceWriter log)
{
    log.Info("Getting da news and tweets . . .");

    DataIngestService ds = new DataIngestService();
    string allDaNews = ds.getNews();

    NewsModel newsModel = new NewsModel();
    newsModel = JsonConvert.DeserializeObject<NewsModel>(allDaNews);
    log.Info(allDaNews);

    DataLakeService dataIngestService = new DataLakeService();
    await dataIngestService.CreateDirectory("superMarioBros");
    dataIngestService.UploadFile();
}