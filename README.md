# Camellia Wiki - Backend
The magic behind the curtain.

## Getting Started
> [!IMPORTANT]
> Please install [.NET](https://aka.ms/dotnet-download) (Not .NET Framework) and [MongoDB Community Server](https://www.mongodb.com/try/download/community).
> Additionally, you may install [MongoDB Compass](https://www.mongodb.com/try/download/compass).

To begin development and testing locally, please follow these steps in your terminal of choice:
1. Clone the repo by running `git clone https://github.com/FlowerShaper/wiki-backend`.
2. Go inside the newly cloned folder (`cd wiki-backend`).
3. Run `dotnet restore --project CamelliaWiki.Backend` to install the packages.
4. Great! Time for configuration by creating a `config.json` in the root directory.
   - **Data Directory**: Take the full path to the [Wiki Articles](https://github.com/FlowerShaper/wiki-articles) and place it into the value with a key of `data-dir`.
   - **Discord Bot Token**: Obtain a bot's Discord Token from the [Discord Developer Portal](https://discord.com/developers/applications) and place it into the value with a key of `token`.
5. Finally, to run the backend, you'll need to run `dotnet run --project CamelliaWiki.Backend`. You can then access it at `http://localhost:1984`.

## Additional Info
- [C# Official Docs](https://learn.microsoft.com/en-us/dotnet/csharp/)
- [MongoDB C# Official Docs](https://www.mongodb.com/docs/drivers/csharp/current/quick-start/)
- [#💻dev-discussion](https://discord.com/channels/435720333786480641/1174624963584610334) in our [Discord](https://discord.gg/camellia)
