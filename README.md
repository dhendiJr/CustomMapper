# CustomMapper

A lightweight, convention-free, expression-ready object mapping library designed for use across multiple .NET solutions.

---

## 📦 Building the NuGet Package

To generate a `.nupkg` file for distribution:

1. Open your terminal and navigate to the root of the `CustomMapper` project:
```bash
    cd /path/to/CustomMapper
```
2. Run the following command to create the NuGet package:
```bash
    dotnet pack -c Release --output "/path/to/localNuget/directory"
```
3. To consume the package in other solutions, add the local NuGet source:
```bash
    dotnet nuget add source "/path/to/localNuget/directory" --name CustomLocal
    
    dotnet add package CustomMapper
```
4. To register the service in your Program.cs or Startup.cs file, add the following line:
```csharp
builder.Services.AddCustomMapper();
```

## 🧪 Examples

### ✅ Defining a Mapping

Create a file like `PersonMap.cs` in your application or domain layer:

```csharp
public class PersonMap : ICustomMapRegistration
{
    public void Register(ICustomMapperRegistry registry)
    {
        // Runtime object-to-object mapping
        registry.Register<Person, PersonDto>(
            mapAction: (src, dest) =>
            {
                dest.Fullname = $"{src.Firstname} {src.Lastname}";
                dest.Age = DateTimeOffset.Now.Year - src.DateOfBirth.Year;
            },
            //Optional:
            // Expression-based projection for use with EF Core
            projection: src => new PersonDto
            {
                Fullname = src.Firstname + " " + src.Lastname,
                Age = DateTimeOffset.Now.Year - src.DateOfBirth.Year
            }
        );
    }
}
```
### ✅ Using the Mapper
In your application layer, you can use the mapper like this:

```csharp
public class MyService
{
    private readonly ICustomMapperService _mapper;

    public MyService(ICustomMapperService mapper)
    {
        _mapper = mapper;
    }

    public PersonDto MapPerson(Person person)
    {
        var dto = new PersonDto();
        _mapper.Map(person, dto);
        return dto;
    }
}

```
### ✅ Using the Mapper with EF Core
In your application layer, you can use the mapper like this:

```csharp
var people = await _context.Persons
    .Where(p => p.Firstname.StartsWith("J"))
    .Select(_mapper.Project<Person, PersonDto>())
    .ToListAsync();