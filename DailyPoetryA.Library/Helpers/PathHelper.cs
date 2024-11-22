namespace DailyPoetryA.Library.Helpers;

public static class PathHelper
{
    private static string _localFolder = string.Empty;

    private static string LocalFolder
    {
        get
        {
            if (!string.IsNullOrEmpty(_localFolder))
            {
                return _localFolder;
            }

            _localFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                // AppDomain.CurrentDomain.FriendlyName 开始使用的是左侧代码，使用中发现不利于单元测试，还是使用下面的代码
                nameof(DailyPoetryA)
            );

            if (!Directory.Exists(_localFolder))
            { 
                Directory.CreateDirectory(_localFolder);
            }

            return _localFolder;
        }
    }

    public static string GetLocalFilePath(string fileName)
    {
        return Path.Combine(LocalFolder, fileName);
    }
}