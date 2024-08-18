# HackerRank Best Stories Api

## How to run?
There a no additional steps, open solution, set up Api as Startup project and run.

## How to use
Use swagger for testing, there is only one GET endpoint (/beststories) with optional nBestStories parameter

### Assumptions and implementation considerations
- Has to be as simple as possible but meet requirements, no over-engineering
- For simplicity MemoryCache was used as DistributedCache, for multi tenant some real distributed cache should be used (Redis/Sql)
- For simplicity Singleton was used as storage, for multi tenant some real db should be used (Redis/Sql/DocumentDb)
- HakerRank Firebase endpoints don't return ETag header so refresh need to download full data set instead of headers first
- Data update runs in background service, Check for changes happens every minute, can be changed in app configuration, data may be delayed by this time
- Actual update run only when best stoires endponit has any changes in order, this may lead to some more data delays if eg. only number of comments changes but no order change in best stories
- Custom JsonConverter with different read/write json structure was used to streamline use of single POCO model (`Story`) to read data from source and present in api in desired format

### Improvements
- Add more logging
- Add better validation
- Store last update and expose it in status endpoint
- Add Auth if api should be private
- Optionally subscribe to firebase story updates to limit delays with comments number
- Split application for proper layers and mappings
- Add support (make use of provided) for json options in JsonConverter Read/Write method