using rpa_fotografia.Automation.WebScraping;
using rpa_fotografia.Utils;

namespace rpa_fotografia
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Logger.Log("Aplicação iniciada.");

            var credentials = Configuration.GetSection<LoginCredentials>("LoginCredentials");
            var hostDataBase = Configuration.GetSection<ConnectionStringsJ>("ConnectionStrings");

            IWebScraper scraper = new WebScraper();
            await scraper.NavigateToUrlAsync("https://auth.alboompro.com/login?srv=prosite&redir=/&host=https://prosite.alboompro.com");
            await scraper.LoginAsync(credentials.Email, credentials.Password, hostDataBase);
            Logger.Log("Aplicação finalizada.");
        }
    }

    public class LoginCredentials
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class ConnectionStringsJ
    {
        public string DefaultConnection { get; set; }
    }
}
