/*using System.CommandLine;
var bundleOption = new Option<FileInfo>("--output", "File path and name");
var bundleCommand = new Command("bundle", "bundle code files to a sinle file");
bundleCommand.AddOption(bundleOption);
bundleCommand.SetHandler((output) =>
{
    try
    {
        File.Create(output.FullName);
        Console.WriteLine("File was created");

    }
    catch (Exception ex) { }
}, bundleOption);
var rootCommand = new RootCommand("Root command for file bundler CLI");
rootCommand.AddCommand(bundleCommand);
rootCommand.InvokeAsync(args);
*/
using System.CommandLine;
using System.IO;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        var languageOption = new Option<string>("--language", "Comma-separated list of programming languages (e.g., C#, Java, Python). Use 'all' for all files.")
        {
            IsRequired = true
        };

        var outputOption = new Option<FileInfo>("--output", "The output file name and path.")
        {
            IsRequired = true
        };

        var noteOption = new Option<bool>("--note", "Include code source as a comment in the bundle.");
        var sortOption = new Option<bool>("--sort", "Sort files by name or language.");
        var removeEmptyLinesOption = new Option<bool>("--remove-empty-lines", "Remove empty lines from code.");
        var authorOption = new Option<string>("--author", "Author name for the bundle.");

        var bundleCommand = new Command("bundle", "Bundle code files into a single file.");
        bundleCommand.AddOption(languageOption);
        bundleCommand.AddOption(outputOption);
        bundleCommand.AddOption(noteOption);
        bundleCommand.AddOption(sortOption);
        bundleCommand.AddOption(removeEmptyLinesOption);
        bundleCommand.AddOption(authorOption);

        bundleCommand.SetHandler((string language, FileInfo output, bool note, bool sort, bool removeEmptyLines, string author) =>
        {
            // Validate and create output directory
            var outputPath = output.DirectoryName;
            if (!Directory.Exists(outputPath))
            {
                Console.WriteLine($"Error: The specified directory does not exist: {outputPath}");
                return;
            }

            var filesToBundle = GetCodeFiles(language);
            var bundledCode = string.Join("\n\n", filesToBundle.Select(file =>
            {
                var code = File.ReadAllText(file.FullName);
                if (removeEmptyLines) code = RemoveEmptyLines(code);
                if (note) code = $"// Source: {file.FullName}\n{code}";
                return code;
            }));

            if (sort)
                bundledCode = SortFiles(filesToBundle, bundledCode);

            try
            {
                File.WriteAllText(output.FullName, bundledCode);
                Console.WriteLine($"Bundle created successfully: {output.FullName}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating bundle: {ex.Message}");
            }
        }, languageOption, outputOption, noteOption, sortOption, removeEmptyLinesOption, authorOption);

        var createRspCommand = new Command("create-rsp", "Create a response file for the bundle command.");
        createRspCommand.SetHandler(() =>
        {
            var languages = PromptForInput("Enter programming languages (comma separated, 'all' for all): ");
            var output = PromptForInput("Enter output file name or full path: ");
            var note = PromptForConfirmation("Include code source as a comment? (y/n): ");
            var sort = PromptForConfirmation("Sort files by name or language? (y/n): ");
            var removeEmptyLines = PromptForConfirmation("Remove empty lines? (y/n): ");
            var author = PromptForInput("Enter author name: ");

            string rspContent = $"--language {languages}\n--output {output}\n--note {note}\n--sort {sort}\n--remove-empty-lines {removeEmptyLines}\n--author {author}";
            string rspFileName = "response.rsp";

            // Check if the file already exists
            if (File.Exists(rspFileName))
            {
                var replace = PromptForConfirmation("The response file already exists. Do you want to overwrite it? (y/n): ");
                if (!replace) return;
            }

            File.WriteAllText(rspFileName, rspContent);
            Console.WriteLine($"Response file created: {rspFileName}");
        });

        var rootCommand = new RootCommand
        {
            bundleCommand,
            createRspCommand
        };

        rootCommand.InvokeAsync(args).Wait();
    }

    static string PromptForInput(string message)
    {
        Console.Write(message);
        return Console.ReadLine();
    }

    static bool PromptForConfirmation(string message)
    {
        Console.Write(message);
        var response = Console.ReadLine();
        return response?.ToLower() == "y";
    }

    static IEnumerable<FileInfo> GetCodeFiles(string language)
    {
        var directoryPath = Directory.GetCurrentDirectory();
        var validExtensions = language.ToLower() == "all"
            ? new[] { ".cs", ".java", ".py", ".js", ".cpp", ".html" }
            : language.Split(',').SelectMany(l => GetExtensionsForLanguage(l.Trim()));

        var files = Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories)
                             .Where(f => validExtensions.Contains(Path.GetExtension(f).ToLower()))
                             .Select(f => new FileInfo(f))
                             .Where(f => !f.DirectoryName.Contains("bin") && !f.DirectoryName.Contains("debug"));

        return files;
    }

    static IEnumerable<string> GetExtensionsForLanguage(string language)
    {
        return language switch
        {
            "C#" => new[] { ".cs" },
            "Java" => new[] { ".java" },
            "Python" => new[] { ".py" },
            "JavaScript" => new[] { ".js" },
            "C++" => new[] { ".cpp", ".h" },
            "HTML" => new[] { ".html" },
            _ => new[] { ".cs", ".java", ".py", ".js", ".cpp", ".html" }
        };
    }

    static string RemoveEmptyLines(string code)
    {
        return string.Join("\n", code.Split('\n').Where(line => !string.IsNullOrWhiteSpace(line)));
    }

    static string SortFiles(IEnumerable<FileInfo> files, string bundledCode)
    {
        var sortedFiles = files.OrderBy(f => f.Name); // Sort by file name, or customize as needed
        return string.Join("\n\n", sortedFiles.Select(file => File.ReadAllText(file.FullName)));
    }
}


