open NUnit.Framework
open FsUnit

open MiniWebCrawler.MiniWebCrawler

module MiniWebCrawlerTests =

    let text = """<h2>Absolute URLs</h2>
                <p><a href="https://www.w3.org/">W3C</a></p>
                <p><a href="https://www.google.com/">Google</a></p>

                <h2>Relative URLs</h2>
                <p><a href="html_images.asp">HTML Images</a></p>
                <p><a href="/css/default.asp">CSS Tutorial</a></p>"""


    [<Test>]
    let ``Extract url from HTML`` () =
        let urls = ["https://www.w3.org/"; "https://www.google.com/"]
        text |> getAllURLsFrom |> should equivalent urls

    [<Test>]
    let ``[internet connection required] Try to get google.com`` () = 
        let result = "https://google.com" |> getPageByURL |> Async.RunSynchronously
        result.IsSome |> should be True

    [<Test>]
    let ``[internet connection required] Try to download all pages from site which does not exist`` () = 
        let result = "https://aa1231asdasd12321.asda31asdasd" |> downloadPages
        result |> should equal Seq.empty

    [<Test>]
    let ``[internet connection required] Try to download all pages from google.com`` () = 
        let result = "https://google.com" |> downloadPages
        result |> should not' (equal Seq.empty)
