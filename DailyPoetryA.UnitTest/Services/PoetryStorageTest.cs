using System.Linq.Expressions;
using DailyPoetryA.Library.Models;
using DailyPoetryA.Library.Services;
using DailyPoetryA.UnitTest.Helpers;
using Moq;

namespace DailyPoetryA.UnitTest.Services;

public class PoetryStorageTest : IDisposable
{
    public PoetryStorageTest()
    {
        PoetryStorageHelper.RemoveDatabaseFile();
    }

    [Fact]
    public void IsInitialized_Default()
    {
        var preferenceStorageMock = new Mock<IPreferenceStorage>();
        // 这里使用 Setup 和 Returns 要求调用 Get 方法时返回指定的值
        // 如果不使用这两个方法，那么当该方法被调用时会返回要求返回类型的默认值
        preferenceStorageMock
            .Setup(p => p.Get(PoetryStorageConstant.VersionKey, default(int)))
            .Returns(PoetryStorageConstant.Version);
        var mockPreferenceStorage = preferenceStorageMock.Object;
        var poetryStorage = new PoetryStorage(mockPreferenceStorage);

        // 由于前面的要求 Get 方法返回的值是PoetryStorageConstant.Version
        // 所以 poetryStorage.IsInitialized 的返回结果必定为 true
        Assert.True(poetryStorage.IsInitialized);

        // 确保 poetryStorage.IsInitialized 中的确实调用了 Get 方法
        preferenceStorageMock.Verify(p => p.Get(PoetryStorageConstant.VersionKey, default(int)), Times.Once);
    }

    [Fact]
    public async Task InitializeAsync_Default()
    {
        var preferenceStorageMock = new Mock<IPreferenceStorage>();
        var mockPreferenceStorage = preferenceStorageMock.Object;
        var poetryStorage = new PoetryStorage(mockPreferenceStorage);

        Assert.False(File.Exists(PoetryStorage.PoetryDbPath));
        await poetryStorage.InitializeAsync();
        Assert.True(File.Exists(PoetryStorage.PoetryDbPath));
    }

    [Fact]
    public async Task GetPoetryAsync_Default()
    {
        var poetryStorage = await PoetryStorageHelper.GetInitializedPoetryStorageAsync();
        var poetry = await poetryStorage.GetPoetryAsync(10001);
        Assert.Equal("临江仙 · 夜归临皋", poetry.Name);
        await poetryStorage.CloseAsync();
    }

    [Fact]
    public async Task GetPoetriesAsync_Default()
    {
        var poetryStorage = await PoetryStorageHelper.GetInitializedPoetryStorageAsync();
        var poetries =
            await poetryStorage.GetPoetriesAsync(
                Expression.Lambda<Func<Poetry, bool>>(Expression.Constant(true),
                    Expression.Parameter(typeof(Poetry), "p")),
                0,
                int.MaxValue);
        Assert.Equal(PoetryStorage.NumberPoetry, poetries.Count);
        await poetryStorage.CloseAsync();
    }

    public void Dispose()
    {
        PoetryStorageHelper.RemoveDatabaseFile();
    }
}