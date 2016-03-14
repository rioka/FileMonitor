using CommandLine;
using CommandLine.Text;

namespace FileMonitor.ConsoleApp
{
  internal class Options
  {
    /// <summary>
    /// Name of the folder to monitor
    /// </summary>
    [Option('s', "source", Required = true, HelpText = "Folder to monitor.")]
    public string SourceFolder { get; set; }

    /// <summary>
    /// # of seconds to check for changes in file length
    /// </summary>
    [Option('i', "interval", Required = false, DefaultValue = 10, HelpText = "# of seconds between checks.")]
    public long Seconds { get; set; }

    /// <summary>
    /// Name of the folder to copy files to
    /// </summary>
    [Option('t', "target", Required = true, HelpText = "Folder to copy files to.")]
    public string TargetFolder { get; set; }

    /// <summary>
    /// Extensions for copied files
    /// </summary>
    [Option('e', "extension", Required = true, HelpText = "Extensions for copied files.")]
    public string FileExtension { get; set; }

    [HelpOption]
    public string GetUsage()
    {
      //var help = new HelpText {
      //  AdditionalNewLineAfterOption = true,
      //  AddDashesToOption = true
      //};
      //help.AddOptions(this);
      //return help;
      return HelpText.AutoBuild(this);
    }

  }
}
