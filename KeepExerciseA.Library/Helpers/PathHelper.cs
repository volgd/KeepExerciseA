namespace KeepExerciseA.Library.Helpers;

public class PathHelper
{
    private static string _localFolder = string.Empty;

    private static string localFolder //懒加载模式，在系统为应用提供的数据公共存储库中创建路径
    {
        get
        {
            if (!string.IsNullOrEmpty(_localFolder))
            {
                return _localFolder;
            }
            _localFolder =Path.Combine(Environment.GetFolderPath
                (Environment.SpecialFolder.LocalApplicationData), nameof(KeepExerciseA));
            if (!Directory.Exists(_localFolder))
            {
                Directory.CreateDirectory(_localFolder);
            }
            return _localFolder;
        }
    }

    public static string GetLocalFilePath(string fileName) //获取数据库的路径
    {
        return Path.Combine(localFolder, fileName);
    }
}