namespace WpfClientApp.Endpoints
{
    public class ItemEndpoints : BaseEndpoints
    {
        private const string ControllerName = "File";
        public const string ControllerRoute = $"{ApiRoute}/{ControllerName}";


        public const string CreateItem = $"CreateItem";
        public static string CreateItemRoute()
        {
            return $"https://localhost:7279/{ControllerRoute}/{CreateItem}";
        }

        public const string GetAllItems = $"GetAllItems";
        public static string GetAllItemsRoute()
        {
            return $"https://localhost:7279/{ControllerRoute}/{GetAllItems}";
        }
    }
}
