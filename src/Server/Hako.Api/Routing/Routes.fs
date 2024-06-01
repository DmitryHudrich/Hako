namespace Routes

open FSharp.MinimalApi.Builder

module Routes = 
    let routes = 
        endpoints {
            get "/hello" (fun () -> "world")
        }    
     
