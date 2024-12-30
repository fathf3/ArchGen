using ArchGen;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace ArchGenTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Setup dependency injection
                var services = new ServiceCollection();
                ConfigureServices(services);
                var serviceProvider = services.BuildServiceProvider();

                // Get generator instance
                var generator = serviceProvider.GetRequiredService<IArchitectureGenerator>();

                if (args.Length == 0)
                {
                    ShowHelp();
                    return;
                }

                string command = args[0].ToLower();
                string projectName = args.Length > 1 ? args[1] : "MyProject";
                string projectPath = Directory.GetCurrentDirectory();

                switch (command)
                {
                    case "nlayer":
                        Console.WriteLine($"N-Layer mimarisi oluşturuluyor: {projectPath}");
                        Console.WriteLine($"Proje adı: {projectName}");
                        generator.GenerateNLayerArchitecture(projectPath, projectName);
                        Console.WriteLine("N-Layer mimarisi başarıyla oluşturuldu!");
                        ListFiles(projectPath);
                        break;

                    case "onion":
                        Console.WriteLine($"Onion mimarisi oluşturuluyor: {projectPath}");
                        Console.WriteLine($"Proje adı: {projectName}");
                        generator.GenerateOnionArchitecture(projectPath, projectName);
                        Console.WriteLine("Onion mimarisi başarıyla oluşturuldu!");
                        ListFiles(projectPath);
                        break;

                    case "help":
                        ShowHelp();
                        break;

                    default:
                        Console.WriteLine("Geçersiz komut!");
                        ShowHelp();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Hata oluştu: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<IProjectService, ProjectService>();
            services.AddSingleton<IArchitectureGenerator, ArchitectureGenerator>();
        }

        static void ShowHelp()
        {
            Console.WriteLine("ArchGen - .NET Mimari Oluşturucu");
            Console.WriteLine("\nKullanım:");
            Console.WriteLine("  archgen <komut> [proje_adı]");
            Console.WriteLine("\nKomutlar:");
            Console.WriteLine("  nlayer     N-Layer mimarisi oluşturur");
            Console.WriteLine("  onion      Onion mimarisi oluşturur");
            Console.WriteLine("  help       Bu yardım mesajını gösterir");
            Console.WriteLine("\nÖrnekler:");
            Console.WriteLine("  archgen nlayer MyProject");
            Console.WriteLine("  archgen onion MyProject");
            Console.WriteLine("\nNot: Proje bulunduğunuz klasörde oluşturulacaktır.");
        }

        static void ListFiles(string path)
        {
            if (Directory.Exists(path))
            {
                Console.WriteLine("\nOluşturulan dosya yapısı:");
                foreach (var dir in Directory.GetDirectories(path))
                {
                    Console.WriteLine($"- {Path.GetFileName(dir)}");
                    foreach (var file in Directory.GetFiles(dir))
                    {
                        Console.WriteLine($"  * {Path.GetFileName(file)}");
                    }
                }
            }
            else
            {
                Console.WriteLine($"Klasör bulunamadı: {path}");
            }
        }
    }
}
