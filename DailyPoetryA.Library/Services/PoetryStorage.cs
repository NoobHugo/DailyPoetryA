﻿using System.Linq.Expressions;
using DailyPoetryA.Library.Helpers;
using DailyPoetryA.Library.Models;
using SQLite;

namespace DailyPoetryA.Library.Services;

public class PoetryStorage(IPreferenceStorage preferenceStorage) : IPoetryStorage
{
    private const int NumberPoetry = 30;

    private const string DbName = "poetrydb.sqlite3";

    public static readonly string PoetryDbPath = PathHelper.GetLocalFilePath(DbName);

    private SQLiteAsyncConnection _connection;

    public SQLiteAsyncConnection Connection => _connection ??= new SQLiteAsyncConnection(PoetryDbPath);

    public bool IsInitialized =>
        preferenceStorage.Get(PoetryStorageConstant.VersionKey, default(int)) == PoetryStorageConstant.Version;

    public async Task InitializeAsync()
    {
        await using var dbAssetFileStream = typeof(PoetryStorage).Assembly.GetManifestResourceStream(DbName) !;
        await using var dbFileStream = new FileStream(PoetryDbPath, FileMode.OpenOrCreate);
        await dbAssetFileStream.CopyToAsync(dbFileStream);
        preferenceStorage.Set(PoetryStorageConstant.VersionKey, PoetryStorageConstant.Version);
    }

    public async Task<Poetry> GetPoetryAsync(int id)
        => await Connection.Table<Poetry>().FirstOrDefaultAsync(p => p.Id == id);


    public Task<IList<Poetry>> GetPoetriesAsync(Expression<Func<Poetry, bool>> where, int skip, int take)
    {
        throw new NotImplementedException();
    }
}

public static class PoetryStorageConstant
{
    public const int Version = 1;
    public const string VersionKey = nameof(PoetryStorageConstant) + "." + nameof(Version);
}