# Mini Web Crawler 🕷 
This is a tool to download the page and all linked pages

## Usage:

```powershell
powershell> MiniWebCrawler.exe

Enter link:https://google.com
"https://google.com" --- 51966
"http://www.google.ru/intl/ru/services/" --- 75846

powershell> ls

Length Name
------ ----
51966 https___google.com.txt
80004 http___www.google.ru_intl_ru_services_.txt
```

## Build:

```powershell
powershell> dotnet publish -c Release --runtime win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true
```

