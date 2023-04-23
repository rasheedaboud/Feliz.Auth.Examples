# Introduction
This is an example app using minimal requirements to get up and running using MSAL for React and Feliz. Please refer to [Feliz.React.Msal](https://github.com/rasheedaboud/Feliz.React.Msal) for more details regarding library I built.
Some things to keep in mind:
-	This package work with Azure B2C or Azure Ad just change the config object, following the docs for AD or B2C respectively.
-	The msal-react package is meant to work with react and utilize hooks. If you need to integrate with Elmish use a useEffect() hook in conjunction with isAuthenticated() hook to dispatch events. There are many examples of how to do this using redux and @azure/msal-react. Elmish follows same pattern.

# Instructions

1.	Follow instructions to create a Feliz App using default project template provided by Zaid [here](https://zaid-ajaj.github.io/Feliz/).
2.	Install pkg `vite-plugin-mkcert`. Update vite config to example below. This will achieved two things. The first is that msal requires https in order to function, the second is we can now access our app on localhost port 8080.

```javascript
import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";
import mkcert from "vite-plugin-mkcert";

export default defineConfig({
  plugins: [react(), mkcert()],
  server: {
    port: "8080",
  },
  root: "./src",
  build: {
    outDir: "../dist",
  },
});

```
3.	Cd into src directory and run ‘femto install Feliz.React.Msal’. Verify  "@azure/msal-react" and "@azure/msal-browser" were installed into package.json. If not add them ‘npm i @azure/msal-browser @azure/msal-react’.
4.	In ‘main.fs’ add following code and open ‘Feliz.React.Msal’
5.	Update the required ‘clientId’, ‘authority’ and ‘knownAuthority’ with values from your config. 

```F#
open Fable.Core.JsInterop
open Feliz.React.Msal.Config
open Feliz.React.Msal

importAll "@azure/msal-react"
importAll "@azure/msal-browser"

//Azure Ad Example
let config = {
  auth: {
    authority: 'https://login.microsoftonline.com/common',
    clientId: '<Client ID>',
    redirectUri: 'http://localhost:8080'
  },
  cache={cacheLocation="sessionStorage"; storeAuthStateInCookie=false}
};
//Azure AD B2C Example
let msalConfig ={
    auth={
            clientId='<Client ID>'
            authority="https://<domain>.b2clogin.com/<domain>.onmicrosoft.com/<Sign in flow>"
            knownAuthorities=[|"https://<domain>.b2clogin.com"|]
            redirectUri= "https://localhost:8080/"
            postLogoutRedirectUri = "https://localhost:8080/"};
    cache={cacheLocation="sessionStorage"; storeAuthStateInCookie=false}
    }
root.render(
        MsalProvider.create[
            MsalProvider.instance client
            MsalProvider.children[
                Components.Counter( {|config=msalConfig |})
            ]
        ]
    )


```

6.	Update the counter component with following code. It is recommended to put the config in its own file that can be referenced where needed. For simplicity we are sticking it in main.fs and passing it down as props.
I would recommend using redirect request. The popup request does not work well on mobile devices in my experience. 

```F#
/// <summary>
    /// A stateful React component that maintains a counter
    /// </summary>
    [<ReactComponent>]
    static member Counter(props:{|config:MsalConfig|}) =
        let (count, setCount) = React.useState(0)

        let client = useMsal()

        let handleLogin (evt:MouseEvent) = 
            evt.preventDefault()
            let request = redirectRequest props.config "https://localhost:8080/"
            client.instance.loginRedirect(request)

        Html.div[
            AuthenticatedTemplate.create [
                AuthenticatedTemplate.children [
                    Html.div [
                        Html.h1 count
                        Html.button [
                            prop.onClick (fun _ -> setCount(count + 1))
                            prop.text "Increment"
                        ]
                    ]
                ]
            ]

            UnauthenticatedTemplate.create [
                UnauthenticatedTemplate.children [
                    Html.p "Login to get started"
                    Html.button[
                        prop.onClick handleLogin
                    ]
                ]
            ]    
        ]


```

7.	With all that done you should now be able to run ‘npm run start’. Navigate to ‘https://localhost:8080/’ and you should see the following:
