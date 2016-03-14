
namespace FileMonitor.ConsoleApp
{
  class FileDetail
  {
    public string Name { get; private set; }
    public long Size { get; set; }

    public FileDetail(string name)
    {
      Name = name;
    }
  }
}
