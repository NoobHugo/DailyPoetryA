using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using DailyPoetryA.Library.Models;
using DailyPoetryA.Library.Services;

namespace DailyPoetryA.Library.ViewModels;

public class ResultViewModel : ViewModelBase
{
    private readonly IPoetryStorage _poetryStorage;

    public ResultViewModel(IPoetryStorage poetryStorage)
    {
        _poetryStorage = poetryStorage;
        OnInitializedCommand = new AsyncRelayCommand(OnInitializedAsync);
    }

    public ObservableCollection<Poetry> PoetryCollection { get; } = [];

    public ICommand OnInitializedCommand { get; }

    public async Task OnInitializedAsync()
    {
        await _poetryStorage.InitializeAsync();
        var poetries =
            await _poetryStorage.GetPoetriesAsync(
                Expression.Lambda<Func<Poetry, bool>>(Expression.Constant(true),
                    Expression.Parameter(typeof(Poetry), "p")),
                0,
                int.MaxValue);
        foreach (var poetry in poetries)
        {
            PoetryCollection.Add(poetry);
        }

        // windows平台下需要关闭数据库连接才能正常运行
        await _poetryStorage.CloseAsync();
    }
}