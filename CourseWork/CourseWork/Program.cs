using CourseWork.BLL;
using CourseWork.Data;
using CourseWork.Models;
using CourseWork.Objects;
using Newtonsoft.Json;


namespace CourseWork
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var data = JsonConvert.DeserializeObject<ParseData>(File.ReadAllText("json.txt"));
            data.AnalyzeParseData();
            new UserInteraction(data).StartInteraction();
        }
    }
}
