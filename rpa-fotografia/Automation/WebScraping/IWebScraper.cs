using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rpa_fotografia.Automation.WebScraping
{
    public interface IWebScraper
    {
        Task NavigateToUrlAsync(string url);
        Task<string> GetPageTitleAsync();
        Task LoginAsync(string email, string password, ConnectionStringsJ host);
    }
}
