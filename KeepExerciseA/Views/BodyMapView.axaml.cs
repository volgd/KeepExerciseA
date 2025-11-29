using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace KeepExerciseA.Views;

public partial class BodyMapView : UserControl
{
    // 静态缓存：程序运行期间只加载一次图片，避免频繁创建 / 释放
    private static readonly Bitmap BodyBitmap =
        new Bitmap(AssetLoader.Open(
            new Uri("avares://KeepExerciseA/Assets/body_front.png")));

    public BodyMapView()
    {
        InitializeComponent();

        // 使用已经缓存好的 Bitmap，不会被 Dispose 掉
        BodyImage.Source = BodyBitmap;
    }
}