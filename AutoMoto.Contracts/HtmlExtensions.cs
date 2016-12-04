namespace AutoMoto.Contracts
{
    public static class HtmlExtensions
    {
        public static string GetDisplayForStatus(this bool status)
        {
            return status ? "Aktywny" : "Nieaktywny";
        }



    }
}
