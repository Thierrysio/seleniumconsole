using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

const string DefaultSearchText = "météo";
const string DefaultCity = "Lyon";

Console.WriteLine("Selenium Console - Activités");
Console.WriteLine("1) Activité 1 : Personnalisation météo");
Console.WriteLine("2) Activité 2 : Bouton cinéma / restaurants");
Console.WriteLine("3) Activité 3 : Challenge");
Console.Write("Choisis une activité (1-3) : ");

var choice = Console.ReadLine();
switch (choice)
{
    case "1":
        RunActivity1();
        break;
    case "2":
        RunActivity2();
        break;
    case "3":
        RunActivity3();
        break;
    default:
        Console.WriteLine("Choix non valide. Relance le programme.");
        break;
}

static void RunActivity1()
{
    Console.WriteLine("Activité 1 : personnaliser le texte et la ville pour la météo.");
    Console.WriteLine($"Texte par défaut : {DefaultSearchText}");
    Console.WriteLine($"Ville par défaut : {DefaultCity}");
    Console.WriteLine("Astuce : modifie ces constantes en haut du fichier pour personnaliser.");

    var query = $"{DefaultSearchText} {DefaultCity}";
    using var driver = CreateDriver();
    NavigateAndSearch(driver, query);
    Console.WriteLine("Recherche lancée. Ferme la fenêtre du navigateur pour terminer.");
    WaitUntilClosed(driver);
}

static void RunActivity2()
{
    Console.WriteLine("Activité 2 : bouton rapide (cinéma ou restaurants).");
    Console.Write("Ville pour la recherche : ");
    var city = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(city))
    {
        city = DefaultCity;
    }

    Console.WriteLine("1) Horaires de cinéma");
    Console.WriteLine("2) Restaurants locaux");
    Console.Write("Choisis une option : ");
    var option = Console.ReadLine();

    var query = option == "1"
        ? $"horaires cinéma {city}"
        : $"restaurants {city}";

    using var driver = CreateDriver();
    NavigateAndSearch(driver, query);
    Console.WriteLine("Recherche lancée. Ferme la fenêtre du navigateur pour terminer.");
    WaitUntilClosed(driver);
}

static void RunActivity3()
{
    Console.WriteLine("Activité 3 : challenge en groupe.");
    Console.WriteLine("1) Récupérer les tendances YouTube");
    Console.WriteLine("2) Vérifier la disponibilité d'un produit");
    Console.WriteLine("3) Surveiller un forum/site d'actualités");
    Console.Write("Choisis une option : ");
    var option = Console.ReadLine();

    switch (option)
    {
        case "1":
            RunYouTubeTrends();
            break;
        case "2":
            RunProductAvailability();
            break;
        case "3":
            RunNewsMonitoring();
            break;
        default:
            Console.WriteLine("Option non valide.");
            break;
    }
}

static void RunYouTubeTrends()
{
    using var driver = CreateDriver();
    driver.Navigate().GoToUrl("https://www.youtube.com/feed/trending");
    Console.WriteLine("Page des tendances YouTube ouverte.");
    WaitUntilClosed(driver);
}

static void RunProductAvailability()
{
    Console.Write("URL du produit : ");
    var url = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(url))
    {
        Console.WriteLine("URL manquante.");
        return;
    }

    Console.Write("Sélecteur CSS du statut (ex: .stock, #availability) : ");
    var selector = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(selector))
    {
        Console.WriteLine("Sélecteur manquant.");
        return;
    }

    using var driver = CreateDriver();
    driver.Navigate().GoToUrl(url);

    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    var statusElement = wait.Until(d => d.FindElement(By.CssSelector(selector)));
    Console.WriteLine($"Statut détecté : {statusElement.Text}");
    WaitUntilClosed(driver);
}

static void RunNewsMonitoring()
{
    Console.Write("URL du site/forum : ");
    var url = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(url))
    {
        Console.WriteLine("URL manquante.");
        return;
    }

    Console.Write("Sélecteur CSS du titre principal (ex: h1, .headline) : ");
    var selector = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(selector))
    {
        Console.WriteLine("Sélecteur manquant.");
        return;
    }

    using var driver = CreateDriver();
    driver.Navigate().GoToUrl(url);

    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    var headline = wait.Until(d => d.FindElement(By.CssSelector(selector)));
    Console.WriteLine($"Dernière actu : {headline.Text}");
    WaitUntilClosed(driver);
}

static IWebDriver CreateDriver()
{
    var options = new ChromeOptions();
    options.AddArgument("--start-maximized");
    return new ChromeDriver(options);
}

static void NavigateAndSearch(IWebDriver driver, string query)
{
    driver.Navigate().GoToUrl("https://www.google.com");
    TryAcceptGoogleConsent(driver);

    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
    var searchBox = wait.Until(d => d.FindElement(By.Name("q")));
    searchBox.SendKeys(query);
    searchBox.SendKeys(Keys.Enter);
}

static void TryAcceptGoogleConsent(IWebDriver driver)
{
    try
    {
        var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(3));
        var agreeButton = wait.Until(d =>
            d.FindElement(By.CssSelector("button#L2AGLb, button[aria-label*='Accept']")));
        agreeButton.Click();
    }
    catch (WebDriverTimeoutException)
    {
        // Aucun bandeau détecté.
    }
}

static void WaitUntilClosed(IWebDriver driver)
{
    while (driver.WindowHandles.Count > 0)
    {
        Thread.Sleep(500);
    }
}
