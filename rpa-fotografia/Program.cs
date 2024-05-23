using Org.BouncyCastle.Asn1.Cms;
using rpa_fotografia.Automation.WebScraping;
using rpa_fotografia.Utils;

namespace rpa_fotografia
{
    class Program
    {
        static async Task Main(string[] args)
        {
            while (true)
            {
            Inicio:

                IWebScraper scraper = new WebScraper();

                try
                {
                    Logger.Log("Aplicação iniciada.");

                    var credentials = Configuration.GetSection<LoginCredentials>("LoginCredentials");
                    var hostDataBase = Configuration.GetSection<ConnectionStringsJ>("ConnectionStrings");

                    ;
                    await scraper.NavigateToUrlAsync("https://auth.alboompro.com/login?srv=prosite&redir=/&host=https://prosite.alboompro.com");
                    await scraper.LoginAsync(credentials.Email, credentials.Password, hostDataBase);
                    await scraper.CloseWebDrive();
                    Logger.Log("Aplicação finalizada.");

                    TimeSpan Atraso = TimeSpan.FromMinutes(10);
                    Logger.Log("aguardando " + Atraso.ToString() + " min");
                    Thread.Sleep(Atraso);
                }
                catch (Exception ex)
                {
                   await scraper.CloseWebDrive();
                    Logger.Log(ex.ToString());
                    Logger.Log("Reiniciando Aplicação por falha");
                    goto Inicio;
                } 

            }
            

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
