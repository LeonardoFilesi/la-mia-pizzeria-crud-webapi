namespace la_mia_pizzeria_crud_mvc.CustomLoggers
{
    public class CustomFileLogger : ICustomLogger
    {
        public void WriteLog(string message)
        {
            File.WriteAllText("C:\\EsercizioLogger\\my-log.txt", $"{DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}LOG: {message}\n");
        }
    }
}
