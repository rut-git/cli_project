using System.CommandLine;
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
