using System;
using System.IO;

namespace FileMonitor.ConsoleApp
{
  class Monitor : IDisposable
  {
    private readonly ProgramState _state;
    private readonly FileSystemWatcher _watcher;

    public Monitor(string path, ProgramState state)
    {
      _state = state;
      var watcher = new FileSystemWatcher(path) {
        EnableRaisingEvents = true,
        NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName |
                       NotifyFilters.DirectoryName | NotifyFilters.Size
      };
      //watcher.Changed += FileChanged;
      watcher.Created += FileCreated;
      watcher.Deleted += FileDeleted;
      watcher.Error += FileError;

      _watcher = watcher;
    }

    #region Watcher events

    //void FileChanged(object sender, FileSystemEventArgs e)
    //{

    //}

    void FileCreated(object sender, FileSystemEventArgs e)
    {
      Console.WriteLine(e.Name + " created");

      var fileInfo = new FileInfo(e.FullPath);
      var file = new FileDetail(e.FullPath) {
        Size = fileInfo.Length
      };

      _state.Add(file);
    }

    void FileDeleted(object sender, FileSystemEventArgs e)
    {
      Console.WriteLine(e.Name + " deleted");
      var file = _state.GetFileByName(e.FullPath);
      if (file != null)
      {
        Console.WriteLine("Last file size was {0:#,###}", file.Size);
      }
      else
      {
        Console.WriteLine("{0} was not tracked", e.Name);
      }
    }

    void FileError(object sender, ErrorEventArgs e)
    {
      Console.WriteLine("*** {0}", e.GetException().Message);
    }

    #endregion

    #region IDisposable

    public void Dispose()
    {
      _watcher.Dispose();
    }

    #endregion
  }
}
