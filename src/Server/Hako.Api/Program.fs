namespace Hako.Api

#nowarn "20"

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.Hosting

module Program =
    open Routes
    [<EntryPoint>]
    let main args =
        
        let builder = WebApplication.CreateBuilder(args)

        let app = builder.Build()
        app.MapGroup("api") |> Routes.routes.Apply |> ignore
        app.UseHttpsRedirection()

        app.UseAuthorization()

        app.Run()

        0
