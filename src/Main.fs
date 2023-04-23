module Main

open Feliz
open App
open Browser.Dom
open Feliz.React.Msal.Config
open Feliz.React.Msal
open Fable.Core.JsInterop

importAll "@azure/msal-react"
importAll "@azure/msal-browser"

let msalConfig :MsalConfig ={
    auth={
            clientId=""
            authority="https://<Domain>.b2clogin.com/<Domain>.onmicrosoft.com/<Flow>"
            knownAuthorities=[|"https://<Domain>.b2clogin.com"|]
            redirectUri= "https://localhost:8080/"
            postLogoutRedirectUri = "https://localhost:8080/"};
    cache={cacheLocation="sessionStorage"; storeAuthStateInCookie=true}
    }
let client:IPublicClientApplication = createClient msalConfig
let root = ReactDOM.createRoot(document.getElementById "feliz-app")
root.render(
        MsalProvider.create[
            MsalProvider.instance client
            MsalProvider.children[
                Components.Counter( {|config=msalConfig |})
            ]
        ]
    )