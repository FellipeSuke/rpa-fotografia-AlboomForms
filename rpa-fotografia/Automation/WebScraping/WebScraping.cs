using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using rpa_fotografia.DataAccess;
using rpa_fotografia.Models;
using rpa_fotografia.Utils;
using System.Text.RegularExpressions;

namespace rpa_fotografia.Automation.WebScraping
{
    public class WebScraper : IWebScraper, IDisposable
    {
        private readonly IWebDriver _driver;
        private readonly WebDriverWait _wait;

        public WebScraper()
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArgument("--headless"); // Executar em modo headless
            chromeOptions.AddArgument("--no-sandbox"); // Necessário para rodar no Docker
            chromeOptions.AddArgument("--disable-dev-shm-usage"); // Necessário para evitar problemas de memória
            chromeOptions.AddArgument("--disable-gpu"); // Desabilitar GPU
            chromeOptions.AddArgument("--disable-software-rasterizer"); // Desabilitar rasterizador de software

            _driver = new ChromeDriver(chromeOptions);
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(600));
        }

        public async Task NavigateToUrlAsync(string url)
        {
            try
            {
                _driver.Navigate().GoToUrl(url);
                _wait.Until(d => d.FindElement(By.TagName("body"))); // Esperar até que o corpo da página seja carregado
                Logger.Log($"Navegado para a URL: {url}");

                // Capturar e registrar o título da página
                string title = await GetPageTitleAsync();
                Logger.Log($"Título da página: {title}");
            }
            catch (Exception ex)
            {
                Logger.Log($"Erro ao navegar para a URL: {ex.Message}");
            }
        }

        public async Task CloseWebDrive()
        {
            try
            {
                _driver.Quit();
                Logger.Log($"WebDrive Encerrado");

            }
            catch (Exception ex)
            {
                Logger.Log($"Erro ao fechar WebDrive: {ex.Message}");
                throw;
            }
        }

        public async Task LoginAsync(string email, string password, ConnectionStringsJ host)
        {
            try
            {
                IWebElement emailField = _wait.Until(d => d.FindElement(By.Id("email")));
                IWebElement passwordField = _wait.Until(d => d.FindElement(By.Id("password")));
                IWebElement loginButton = _wait.Until(d => d.FindElement(By.CssSelector("button[type='submit']")));

                emailField.Clear();
                emailField.SendKeys(email);
                passwordField.Clear();
                passwordField.SendKeys(password);
                loginButton.Click();

                // Esperar até que a página de login seja concluída (ajustar conforme a lógica da página)
                try
                {
                    var profileNameElement = _driver.WaitForElementToBeVisible(By.CssSelector(".bh-profile__name"), 30);
                    string profileName = profileNameElement.Text;
                    Logger.Log("Login realizado com sucesso: " + profileName);
                }
                catch
                {
                    try
                    {
                        var profileNameElement = _driver.WaitForElementToBeVisible(By.CssSelector(".bh-profile__name"), 30);
                        string profileName = profileNameElement.Text;
                        Logger.Log("Login realizado com sucesso: " + profileName);

                    }
                    catch (TimeoutException)
                    {

                        Logger.Log("O elemento .bh-profile__name não foi encontrado ou não está visível.");
                        throw;
                    }
                }



            }
            catch (Exception ex)
            {
                Logger.Log($"Erro ao realizar login: {ex.Message}");
                throw;
            }

            // Navegação para pagina inbox e aguardar carga
            try
            {



                var inboxElement = _wait.Until(d => d.FindElement(By.XPath("//span[@class='menu-item-title' and contains(text(),'Inbox')]")));
                inboxElement.Click();
                _wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));
            }
            catch
            {
                try
                {
                    Logger.Log("Elemento  INBOX   não encontrado, Navegando manualmente.");
                    _driver.Navigate().GoToUrl("https://prosite.alboompro.com/business/inbox");
                    Thread.Sleep(9000);
                }
                catch (Exception)
                {
                    Logger.Log("Elemento  INBOX   não encontrado, Navegando manualmente.");
                    throw;
                }

            }


            try
            {

                var formTitleElement = _driver.WaitForElementToBeVisible(By.XPath("//*[@id=\"app-viewport\"]/div[2]/div[1]/header/div/div[2]/div[1]/h3"), 30);
                Logger.Log(formTitleElement.Text);
            }
            catch
            {
                try
                {
                    var formTitleElement = _driver.WaitForElementToBeVisible(By.XPath("//*[@id=\"app-viewport\"]/div[2]/div[1]/header/div/div[2]/div[1]/h3"), 30);
                    Logger.Log(formTitleElement.Text);
                }
                catch (TimeoutException)
                {
                    Logger.Log("");
                    Logger.Log("#######################O elemento de inbox não foi encontrado ou não está visível.###########################");
                    throw;
                }
            }
            

            // Extração de dados do novo cliente!
            try
            {

                var respostasForm = _driver.WaitForElementToBeVisible(By.XPath("/html/body/div[2]/div/div/div/div[3]/div[2]/div[2]/div/div[1]/div[1]/div/div/div"), 30);
                Logger.Log(respostasForm.Text);
                int quantidadeForm = int.Parse(Regex.Replace(respostasForm.Text, "[^0-9]", ""));
               // quantidadeForm = 0;

                if (quantidadeForm > 0)
                {
                    for (int i = 1; i <= quantidadeForm; i++)
                    {
                        try
                        {
                            Thread.Sleep(4000);
                            Console.WriteLine();
                            Logger.Log("####  Cliente Numero " + i + " de " + quantidadeForm + " ####");
                            var clientForm1 = _driver.WaitForElementToBeVisible(By.XPath("//*[@id=\"app-viewport\"]/div[2]/div[2]/div/div[2]/div[1]/div/div[1]/div[2]/span"), 30);
                            //clientForm1.Click();
                            Thread.Sleep(5000);
                            var client = _driver.WaitForElementToBeVisible(By.XPath("//*[@id='app-viewport']/div[2]/div[2]/div/div[2]/div[2]/div/div/div/div[2]"), 30);
                            Cliente novoCliente = ColetarInformacoesCliente();
                            ExecutarColetaInformacoes(host);
                            var arquivarButton = _driver.WaitForElementToBeVisible(By.XPath("//*[@id=\"app-viewport\"]/div[2]/div[2]/div/div[2]/div[2]/div/div/div/div[1]/div[1]"), 30);
                            arquivarButton.Click();
                            Thread.Sleep(5000);
                        }
                        catch (Exception ex)
                        {
                            Logger.Log("Falha na extração de formulario do cliente, numero de cliente (" + quantidadeForm + ")");
                            Logger.Log(ex.ToString());
                        }
                    }
                }
                else
                {
                    Logger.Log("Não há formulario novos ou não tratados no Inbox");
                }



            }
            catch (Exception)
            {

                Logger.Log("###############    Falha ao Carrgar INBOX.    ###################");
                throw;
            }



        }


        public Task<string> GetPageTitleAsync()
        {
            return Task.FromResult(_driver.Title);
        }

        public void Dispose()
        {
            _driver.Quit();
        }

        public Cliente ColetarInformacoesCliente()
        {
            try
            {
                IWebElement clienteInfoElement;
                try
                {
                    // XPath do elemento que contém as informações do cliente
                     clienteInfoElement = _driver.WaitForElementToBeVisible(By.XPath("//*[@id='app-viewport']/div[2]/div[2]/div/div[2]/div[2]/div/div/div/div[2]"), 30);
                }
                catch
                {
                     clienteInfoElement = _driver.WaitForElementToBeVisible(By.XPath("//*[@id='app-viewport']/div[2]/div[2]/div/div[2]/div[2]/div/div/div/div[2]"), 30);

                }
               

                // Coletar as informações individuais
                var data = clienteInfoElement.FindElement(By.XPath("//*[@id=\"app-viewport\"]/div[2]/div[2]/div/div[2]/div[2]/div/div/div/div[2]/div[1]/div")).Text;
                var formulario = clienteInfoElement.FindElement(By.XPath("//*[@id=\"app-viewport\"]/div[2]/div[2]/div/div[2]/div[2]/div/div/div/div[2]/div[3]")).Text;
                var link = clienteInfoElement.FindElement(By.XPath("//*[@id=\"app-viewport\"]/div[2]/div[2]/div/div[2]/div[2]/div/div/div/div[2]/div[5]")).Text;
                var dataBd = ConvertClass.ConvertToDateTime(data);
                var nome = clienteInfoElement.FindElement(By.XPath("//*[@id=\"app-viewport\"]/div[2]/div[2]/div/div[2]/div[2]/div/div/div/div[3]/div[2]")).Text;
                var sobreNome = clienteInfoElement.FindElement(By.XPath("//*[@id=\"app-viewport\"]/div[2]/div[2]/div/div[2]/div[2]/div/div/div/div[3]/div[4]")).Text;
                var cpf = clienteInfoElement.FindElement(By.XPath("//*[@id=\"app-viewport\"]/div[2]/div[2]/div/div[2]/div[2]/div/div/div/div[3]/div[6]")).Text;
                var email = clienteInfoElement.FindElement(By.XPath("//*[@id=\"app-viewport\"]/div[2]/div[2]/div/div[2]/div[2]/div/div/div/div[3]/div[8]")).Text;
                var whatsApp = clienteInfoElement.FindElement(By.XPath("//*[@id=\"app-viewport\"]/div[2]/div[2]/div/div[2]/div[2]/div/div/div/div[3]/div[10]")).Text;
                var cep = clienteInfoElement.FindElement(By.XPath("//*[@id=\"app-viewport\"]/div[2]/div[2]/div/div[2]/div[2]/div/div/div/div[3]/div[12]")).Text;
                var rua = clienteInfoElement.FindElement(By.XPath("//*[@id=\"app-viewport\"]/div[2]/div[2]/div/div[2]/div[2]/div/div/div/div[3]/div[14]")).Text;
                var bairro = clienteInfoElement.FindElement(By.XPath("//*[@id=\"app-viewport\"]/div[2]/div[2]/div/div[2]/div[2]/div/div/div/div[3]/div[16]")).Text;
                var cidade = clienteInfoElement.FindElement(By.XPath("//*[@id=\"app-viewport\"]/div[2]/div[2]/div/div[2]/div[2]/div/div/div/div[3]/div[18]")).Text;
                var estado = clienteInfoElement.FindElement(By.XPath("//*[@id=\"app-viewport\"]/div[2]/div[2]/div/div[2]/div[2]/div/div/div/div[3]/div[20]")).Text;
                var pais = clienteInfoElement.FindElement(By.XPath("//*[@id=\"app-viewport\"]/div[2]/div[2]/div/div[2]/div[2]/div/div/div/div[3]/div[22]")).Text;
                var numero = clienteInfoElement.FindElement(By.XPath("//*[@id=\"app-viewport\"]/div[2]/div[2]/div/div[2]/div[2]/div/div/div/div[3]/div[24]")).Text;
                var complemento = clienteInfoElement.FindElement(By.XPath("//*[@id=\"app-viewport\"]/div[2]/div[2]/div/div[2]/div[2]/div/div/div/div[3]/div[26]")).Text;

                // Criar uma instância da classe Cliente e atribuir os valores coletados
                var cliente = new Cliente
                {
                    Data = (DateTime)dataBd,
                    NomeFormulario = formulario,
                    Link = link,
                    Nome = nome,
                    SobreNome = sobreNome,
                    CPF = Regex.Replace(cpf, "[^0-9]", ""),
                    Email = email,
                    WhatsApp = whatsApp,
                    CEP = cep,
                    Rua = rua,
                    Bairro = bairro,
                    Cidade = cidade,
                    Estado = estado,
                    Pais = pais,
                    Numero = numero,
                    Complemento = complemento
                };

                return cliente;
            }
            catch (NoSuchElementException ex)
            {
                Logger.Log("Um ou mais elementos não foram encontrados na página: " + ex.Message);
                throw;
            }
        }

        // Exemplo de uso
        public void ExecutarColetaInformacoes(ConnectionStringsJ host)
        {
            try
            {
                Cliente cliente = ColetarInformacoesCliente();
                if (cliente != null)
                {
                    Logger.Log("Data: " + cliente.Data);
                    Logger.Log("Formulario: " + cliente.NomeFormulario);
                    Logger.Log("Nome: " + cliente.Nome);
                    Logger.Log("Sobre Nome: " + cliente.SobreNome);
                    Logger.Log("CPF: " + cliente.CPF);
                    Logger.Log("E-mail: " + cliente.Email);
                    Logger.Log("WhatsApp: " + cliente.WhatsApp);
                    Logger.Log("CEP: " + cliente.CEP);
                    Logger.Log("Rua: " + cliente.Rua);
                    Logger.Log("Bairro: " + cliente.Bairro);
                    Logger.Log("Cidade: " + cliente.Cidade);
                    Logger.Log("Estado: " + cliente.Estado);
                    Logger.Log("País: " + cliente.Pais);
                    Logger.Log("N°: " + cliente.Numero);
                    Logger.Log("Complemento: " + cliente.Complemento);

                    var database = new Database(host);
                    var clienteService = new ClienteService(database);
                    Logger.Log("Linhas afetadas: " + clienteService.AdicionarCliente(cliente));

                }
            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}
