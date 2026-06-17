using Avalonia.Controls;
using Avalonia.Platform.Storage;
using GyorsulasEVA_AVA.ViewModels;
using System;

namespace GyorsulasEVA_AVA.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();

        this.DataContextChanged += OnDataContextChanged;
    }

    private void OnDataContextChanged(object? sender, EventArgs e)
    {
        if (DataContext is MainViewModel vm)
        {
            // Feliratkozunk a ViewModel eseményeire
            vm.LoadGameRequested += OnLoadGameRequested;
            vm.SaveGameRequested += OnSaveGameRequested;
        }
    }
    private async void OnLoadGameRequested(object? sender, EventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel == null) return;
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Load window",
            AllowMultiple = false,
            FileTypeFilter = new[] { new FilePickerFileType("Text files") { Patterns = new[] { "*.txt" } } }
        });

        if (files.Count >= 1)
        {
            if (DataContext is MainViewModel vm)
            {               
                await vm.LoadGameFromPathAsync(files[0].Path.LocalPath);
            }
        }
    }

    private async void OnSaveGameRequested(object? sender, EventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel == null) return;

        var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save window",
            DefaultExtension = "txt",
            FileTypeChoices = new[] { new FilePickerFileType("Text files") { Patterns = new[] { "*.txt" } } }
        });

        if (file != null)
        {
            if (DataContext is MainViewModel vm)
            {
                await vm.SaveGameToPathAsync(file.Path.LocalPath);
            }
        }
    }
}
