using DailyPoetryA.Library.Services;

namespace DailyPoetryA.Library.ViewModels;

public class ResultViewModel : ViewModelBase
{
    private readonly IPoetryStorage _poetryStorage;

    public ResultViewModel(IPoetryStorage poetryStorage)
    {
        _poetryStorage = poetryStorage;
    }
}