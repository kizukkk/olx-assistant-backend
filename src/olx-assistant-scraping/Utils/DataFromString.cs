using System.Globalization;

namespace olx_assistant_scraping.Utils;
public static class DataFromString
{
    public static DateTime GetDateFromStr(string str)
    {
        //TODO : Process the date case "Сьогодні" (today) 
        str = str.Replace("р.", String.Empty).TrimEnd();
        DateTime date = DateTime.ParseExact(str, "d MMMM yyyy", new CultureInfo("uk-UA"));
        return date;
    }

}
