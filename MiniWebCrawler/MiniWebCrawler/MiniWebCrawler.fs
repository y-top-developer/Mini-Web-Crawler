open System
open System.IO
open System.Net
open System.Text.RegularExpressions

module MiniWebCrawler = 
    let regexURLs = Regex("<a href\s*=\s*\"?(https?://[^\"]+)\"?\s*>", RegexOptions.Compiled)
    let regexBadChars = Regex(@"[^\w\.@-]", RegexOptions.Compiled)

    let getAllURLsFrom text =
        regexURLs.Matches(text)
        |> Seq.map (fun data  -> data.Groups.[1].Value)
        |> Seq.filter (fun url -> url <> "")

    let getPageByURL (url:string) =
        async {
            try
                let request = WebRequest.Create url
                use! response = request.AsyncGetResponse()
                use stream = response.GetResponseStream()
                use reader = new StreamReader(stream)
                return Some (url, reader.ReadToEnd())
            with 
                | _ -> return None
        }

    let downloadPages initURL = 
        let initPage = initURL |> getPageByURL |> Async.RunSynchronously
        match initPage with
        | Some (_, data) -> getAllURLsFrom data 
                            |> Seq.map (fun subURL -> getPageByURL subURL) 
                            |> Async.Parallel 
                            |> Async.RunSynchronously
                            |> Seq.append [initPage]
        | None -> Seq.empty

    let processPage url (data:string) = 
        printf $"%A{url} --- %d{data.Length}\n"
        File.WriteAllText(regexBadChars.Replace(url, "_") + ".txt", data)

    let savePage data = 
        match data with
        | Some(url, page) -> processPage url page
        | _ -> printf "Can not save page\n"

    let savePages initURL = 
        downloadPages initURL |> Seq.map(fun (x:(string * string) option) -> savePage x) |> List.ofSeq |> ignore

[<EntryPoint>]
let main _ =
    printf "Enter link:"
    let request = Console.ReadLine()
    request |> MiniWebCrawler.savePages
    0