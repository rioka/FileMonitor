using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace FileMonitor.ConsoleApp
{
  class Program
  {
    private static Timer _timer;
    private static ProgramState _state;

    private static long _checkInterval;

    static void Main(string[] args)
    {

      var options = new Options();
      if (!CommandLine.Parser.Default.ParseArguments(args, options))
      {
        return;
      }

      _state = new ProgramState(new List<FileDetail>(), options.TargetFolder, options.FileExtension);

      _checkInterval = options.Seconds * 1000;

      var monitor = new Monitor(options.SourceFolder, _state);

      _timer = new Timer(CheckFile, _state, _checkInterval, Timeout.Infinite);

      Console.WriteLine("Running for path '{0}'...\nPress 'q' to quit", options.SourceFolder);

      while (Console.Read() != 'q')
      { }

      monitor.Dispose();
    }

    private static void CheckFile(object state)
    {
      var currentState = (ProgramState) state;
      if (!currentState.NoFiles)
      {
        // to copy a file, we wait til the its length has not changed
        // not bullet-proof, but should work in most cases
        var trackedFile = currentState.GetLastFile();
        var fileInfo = new FileInfo(trackedFile.Name);
        if (fileInfo.Exists)
        {
          if (trackedFile.Size != fileInfo.Length)
          {
            trackedFile.Size = fileInfo.Length;
            Console.WriteLine("Still writing to file {0}, current size is {1:#,###}", fileInfo.Name, fileInfo.Length);
          }
          else
          {
            var targetFile = Path.Combine(currentState.TargetFolder, fileInfo.Name + _state.FileExtension);
            if (File.Exists(targetFile))
            {
              Console.WriteLine("File {0} already copied", trackedFile.Name);
              // maybe the download was slow, and we've already copied an uncomplete file
              if (new FileInfo(targetFile).Length != trackedFile.Size)
              {
                Console.Write("File size has changed, copying {0} into {1} again...", trackedFile.Name, targetFile);
                File.Copy(trackedFile.Name, targetFile, true);
                Console.WriteLine(" Copied!");
              }
            }
            else
            {
              Console.Write("File size has not changed, copying {0} into {1}...", trackedFile.Name, targetFile);
              File.Copy(trackedFile.Name, targetFile, true);
              Console.WriteLine(" Copied!");
            }
          }
        }
        else
        {
          Console.WriteLine("File {0} does not exist any longer", trackedFile.Name);
        }
      }
      // restart
      _timer.Change(_checkInterval, Timeout.Infinite);
    }
  }
}
