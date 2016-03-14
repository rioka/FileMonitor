using System.Collections.Generic;
using System.Linq;

namespace FileMonitor.ConsoleApp
{
  class ProgramState
  {
    public IList<FileDetail> TrackedFiles { get; private set; }
    public string TargetFolder { get; private set; }
    public string FileExtension { get; private set; }

    public bool NoFiles
    {
      get { return !TrackedFiles.Any(); }
    }

    public ProgramState(IList<FileDetail> trackedFiles, string targetFolder, string fileExtension)
    {
      TrackedFiles = trackedFiles;
      TargetFolder = targetFolder;

      if (fileExtension.First() != '.')
        fileExtension = '.' + fileExtension;
      FileExtension = fileExtension;
    }

    public void Add(FileDetail file)
    {
      TrackedFiles.Add(file);
    }

    public FileDetail GetLastFile()
    {
      return TrackedFiles.Last();
    }

    public FileDetail GetFileByName(string fullPath)
    {
      return TrackedFiles.FirstOrDefault(f => f.Name == fullPath);
    }
  }
}
