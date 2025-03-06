# About

Test Connectivity to [Azure Cache for Redis](https://learn.microsoft.com/en-us/azure/azure-cache-for-redis/cache-overview).

## How to

Create `appsettings.Development.json` with your redis information:

```json
{
    "RedisCacheName": "foo.cache.windows.net:6380",
    "RedisCachePassword": "",
    "TenantId": "00000000-0000-0000-0000-000000000000"
}
```

`TenantId` is only needed for Azure Identity Access using External (Guest) Accounts.

## Running the sample

Modify `Program.cs` to select sample to run.

Execute with `dotnet run`.