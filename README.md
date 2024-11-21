# **CLI Tool Overview**  
This CLI tool bundles code files into a single file, with options to customize the output. It supports:  
- **Filtering by programming language (e.g., C++, Python).**  
- **Sorting files by name or programming language.**  
- **Adding comments with file paths as a source reference.**  
- **Removing empty lines from source files.**  
- **Adding author information to the bundled output.**

---

## **Features**
- **Bundle multiple code files into a single output file.**
- **Filter files by programming language.**
- **Include the source file path as a comment.**
- **Remove empty lines.**
- **Sort files by name or language.**
- **Add author metadata.**

---

## **Usage**  
### **Basic Command Structure**  
```bash
dotnet run -- <command> [options]
```

## **Available Commands**
bundle: Bundle code files into a single file.  
create-rsp: Generate a response file for easier command execution.  

## **Options for bundle**
| Option             | Description                                                                                       | Required |
|--------------------|---------------------------------------------------------------------------------------------------|----------|
| --language         | Comma-separated list of programming languages to include (e.g., C++, Java). Use 'all' to include all files. | Yes      |
| --output           | The output file name and path.                                                                    | Yes      |
| --note             | Include the source file path as a comment in the bundle.                                           | No       |
| --sort             | Sort files by name or programming language.                                                       | No       |
| --remove-empty-lines | Remove empty lines from code files.                                                              | No       |
| --author           | Add author information to the bundled output.                                                     | No       |


### Using a Response File

Instead of typing the command, you can create a response file (.rsp) to store options.

#### Example response.rsp:

```text
bundle
--language C++  
--output bundle.txt  
--note  
--sort  
--remove-empty-lines  
--author John Doe
```

#### Run the command:
```bash
dotnet run -- @path\to\response.rsp
```

#### Options for create-rsp
Generates a .rsp file interactively by asking the user for the required inputs.

```bash
dotnet run -- create-rsp
```
Follow the prompts to generate the response.rsp file.

### Supported Programming Languages

| Language    | Extensions        |
|-------------|-------------------|
| C#          | .cs               |
| Java        | .java             |
| Python      | .py               |
| JavaScript  | .js               |
| C++         | .cpp, .h          |
| HTML        | .html             |

### Requirements

- .NET SDK installed on your system.
- A directory with code files available for bundling.

### Error Handling

- Ensure the output directory exists.
- Verify the language option matches the desired file extensions.
- Double-check the response.rsp file formatting and values.

### Contributing

Feel free to fork the repository and make improvements!
