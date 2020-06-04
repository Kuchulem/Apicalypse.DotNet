# Apicalypse.DotNet
A .Net Stadard library to query APIs based on Apycalipse.

# How to install

more info [here](https://github.com/Kuchulem/Apicalypse.DotNet/wiki/Get-Started)

## Nuget package

In *Nuget package manager* search for `Apicalypse.DotNet` and click on install the package.


## Embeded library

Clone the repository :

```sh
git clone git@github.com:Kuchulem/Apicalypse.DotNet.git
```

Add the project to your solution in VisualStudio and add a reference to it in your own project.

If you prefere command line :
Add it to your solution

```sh
dotnet sln add ~/source/repo/Apicalypse.DotNet/Apicalypse.DotNet.csproj
```

Add a reference to the library

```sh
dotnet add Contoso.MyProject/Contoso.MyProject.csproj reference Apicalypse.DotNet/Apicalypse.DotNet.csproj
```

# Usage

For a full reference to the library go [there](https://github.com/Kuchulem/Apicalypse.DotNet/wiki/Library-Reference)

The entry point of the library is the `RequestBuilder<T>`. The Generic `T` should be a class of the api endpoint.

Let's say our Api endpoint as a class model like : 
```csharp

public class Game
{
    public string Name { get; set; }
    public string Slug { get; set; }
    public int Follows { get; set; }
    public double Score { get; set; }
    public DateTime ReleaseDate { get; set; }
}
```

Our builder would be :

```csharp
var builder = new RequestBuilder<Game>();
```

You can then build the query. If you are familliar to Linq you won't get lost :

```csharp
// the `o` var is of type `Game` (the `T` generic of the constructor of `RequestBulder`)
builder
    .Select(new { // the list of fields to gather
        o => o.Name,
        o => o.Slug
    })
    .Where( // conditions
        o => o.Follows < 3
        && o.Follow > 10
        || o.Score > 90
    )
    .OrderByDescending(   // Descending sort orer
        o => o.ReleaseDate
    )
    .OrderBy( // order by ascending
        o => o.Name
    )
    .Take(8) // limit to 8 results
    .Skip(3); // gather results after the third one
```

After the builder is ready your can buld the `ApicalypseRequest` and send the request

```csharp
ApicalypseRequest request = builder.Build();

using(var httpClient = new HttpClient() {
    // prepare your client with API base URL
    // ...

    var response = await request.Send(httpClient, "game");
    // response.Content will get the response json/xml content
}
```

You can put it together with the chaining :

```csharp

using(var httpClient = new HttpClient() {
    // prepare your client with API base URL
    // ...

    var response = await new RequestBuilder<Game>()
        // prepare the request
        .Select(new { 
            o => o.Name,
            o => o.Slug
        })
        .Where( 
            o => o.Follows < 3
            && o.Follow > 10
            || o.Score > 90
        )
        .OrderByDescending( 
            o => o.ReleaseDate
        )
        .OrderBy( 
            o => o.Name
        )
        .Take(8) 
        .Skip(3)
        // build
        .Build()
        // send
        .Send(httpClient, "game");
    // response.Content will get the response json/xml content
}
```

If you wish to get the response content in an object you can call the `RequestBuilder<T>.Select<TSelect>()` `ApicalypseRequest.Send<TSelect>(HttpClient, string)` variant.

ie : for a local model like :

```csharp
class GameShort
{
    public string Name { get; set; }
    public string Slug { get; set; }
}
```

you will get : 

```csharp
using(var httpClient = new HttpClient() {
    // prepare your client with API base URL
    // ...

    var list = await new RequestBuilder<Game>()
        // prepare the request
        .Select<GameShort>() // will add all public properties of `GameShort` to the fields list to gather
        // the other methods are still based on the model from the API : `Game` class
        .Where( 
            o => o.Follows < 3
            && o.Follow > 10
            || o.Score > 90
        )
        .OrderByDescending( 
            o => o.ReleaseDate
        )
        .OrderBy( 
            o => o.Name
        )
        .Take(8) 
        .Skip(3)
        // build
        .Build()
        // send
        .Send<GameShort>(httpClient, "game");
    // `list` will contain an IEnumerable<GameShort> list from the response content.
}
```

