﻿using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using Tekla.Structures.Dialog;
using TD = Tekla.Structures.Datatype;

namespace dotbimTekla.UI;
public class MainWindowViewModel : INotifyPropertyChanged
{
    private readonly PropertySetsDefinitionSearcher _propertySetsDefinitionSearcher;

    private int _selectionMode = 0;
    [StructuresDialog("SelectionMode", typeof(TD.Integer))]
    public int SelectionMode
    {
        get { return _selectionMode; }
        set { _selectionMode = value; OnPropertyChanged(); }
    }

    private string _outputPath = string.Empty;
    [StructuresDialog("OutputPath", typeof(TD.String))]
    public string OutputPath
    {
        get { return _outputPath; }
        set { _outputPath = value; OnPropertyChanged(); }
    }

    private string _propertySets = string.Empty;
    [StructuresDialog("PropertySets", typeof(TD.String))]
    public string PropertySets
    {
        get { return _propertySets; }
        set { _propertySets = value; OnPropertyChanged(); }
    }

    private ObservableCollection<string> _availableSettings = new ObservableCollection<string>();
    public ObservableCollection<string> AvailableSettings
    {
        get { return _availableSettings; }
        set { _availableSettings = value; OnPropertyChanged(); }
    }

    public MainWindowViewModel()
    {
        _propertySetsDefinitionSearcher = new PropertySetsDefinitionSearcher();

        PopulateSettings();
    }

    private void PopulateSettings()
    {
        AvailableSettings.Add("<empty>");

        foreach (var settings in _propertySetsDefinitionSearcher.GetFullSettingPaths())
        {
            var fileName = Path.GetFileNameWithoutExtension(settings);
            AvailableSettings.Add(fileName);
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? property = null)
    {
        var handler = PropertyChanged;
        if (handler != null)
            handler(this, new PropertyChangedEventArgs(property));
    }
}
