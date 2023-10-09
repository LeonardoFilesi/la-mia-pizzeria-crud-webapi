using System.Diagnostics;
namespace la_mia_pizzeria_crud_mvc.CustomLoggers
{
    public class CustomConsoleLogger : ICustomLogger
    {
        public void WriteLog(string message)
        {
            Debug.WriteLine("LOG" + message);
        }
    }
}
