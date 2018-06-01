# This is Bulls and cows game made over Neo blockchain

This is a realtime online game made with websockets and handling of blockchain events. The old and gold game played in school on sheets of paper done over again but better.

## Getting Started

These instructions will let you know on how to run the project locally.

### Prerequisites

What things you need to install before hand.

* [dotnet core 2.1](https://www.microsoft.com/net/download/dotnet-core/) - for the webapp and signalR
* [VS 2017 update 15.7](https://docs.microsoft.com/en-us/visualstudio/releasenotes/vs2017-relnotes) - for your prefered IDE
* [Docker](https://docs.docker.com/docker-for-windows/) - For running Neo private network

#### Docker

Follow instructions from [here](https://hub.docker.com/r/cityofzion/neo-privatenet/) to run your private net.


#### Bulding and runing the app

Building the app is easy just build in VS 2017 you will need update 15.7 for signalR and dotnet core 2.1.

After building you can go into the project folder and run in the console.

```
dotnet publish
```

This will make a publish folder in 

```
~\BullsAndCowsNeo.Web\bin\Debug\netcoreapp2.1\publish
```

Then go into that folder and run in the console.

```
dotnet BullsAndCowsNeo.Web.dll
```

This will start a localhost:5000 with the app.

And you are done.

## The end.
