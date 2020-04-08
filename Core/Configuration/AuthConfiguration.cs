namespace Core.Configuration
{
    public class OpenIddictServerConfig
    {
        public string SecretKey { get; set; }
        public string Authority { get; set; }
        public bool RequireHttps { get; set; }
    }
}