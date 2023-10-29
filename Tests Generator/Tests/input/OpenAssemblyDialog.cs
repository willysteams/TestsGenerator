using System;
using System.Windows;
using Microsoft.Win32;

namespace AssemblyBrowserGui;

public class OpenAssemblyDialog
{
    public string FilePath { get; private set; } = string.Empty;
 
    public bool OpenFileDialog()
    {
        var openFileDialog = new OpenFileDialog();
        if (openFileDialog.ShowDialog() != true) return false;
        
        FilePath = openFileDialog.FileName;
        return true;
    }
    
    public static void ShowMessage(string message)
    {
        MessageBox.Show(message);
    }
}