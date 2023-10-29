using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using AssemblyBrowserCore;

namespace AssemblyBrowserGui;

public sealed class Model : INotifyPropertyChanged
{
    private ObservableCollection<IElementInfo> _namespaces;
    public ObservableCollection<IElementInfo> Namespaces
    {
        get => _namespaces;
        private set
        {
            _namespaces = value;
            OnPropertyChanged();
        }
    }

    public Model()
    {
        _namespaces = new ObservableCollection<IElementInfo>();
    }

    public void UpdateNamespace(string assemblyPath)
    {
        Namespaces = AssemblyLoader.GetNamespaces(assemblyPath);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}