namespace App

open Feliz
open Feliz.Router
open Feliz.React.Msal
open Feliz.React.Msal.Hooks
open Browser.Types
open Fable.React

type Components =
    /// <summary>
    /// The simplest possible React component.
    /// Shows a header with the text Hello World
    /// </summary>
    [<ReactComponent>]
    static member HelloWorld() = Html.h1 "Hello World"

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
                        prop.text "Login"
                    ]
                ]
            ]    
        ]
    


    /// <summary>
    /// A React component that uses Feliz.Router
    /// to determine what to show based on the current URL
    /// </summary>
    [<ReactComponent>]
    static member Router(props:{|config:MsalConfig|}) =
        let (currentUrl, updateUrl) = React.useState(Router.currentUrl())
        React.router [
            router.onUrlChanged updateUrl
            router.children [
                match currentUrl with
                | [ ] -> Html.h1 "Index"
                | [ "hello" ] -> Components.HelloWorld()
                | [ "counter" ] -> Components.Counter(props)
                | otherwise -> Html.h1 "Not found"
            ]
        ]