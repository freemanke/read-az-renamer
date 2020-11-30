using System;
using System.IO;
using System.Linq;

namespace ReadAZRenamer
{
    class Program
    {
        static void Main(string[] args)
        {
            var root = @"C:\Users\freeman\Downloads\f\f";
            var storyDirectories = Directory.GetDirectories(root);

            foreach (var storyDirectory in storyDirectories)
            {
                Console.WriteLine($"{storyDirectory}: ");
                var files = Directory.GetFiles(storyDirectory);
                foreach(var path in files)
                {
                    var fileName = Path.GetFileName(path);
                    if (fileName.ToLower().EndsWith(".jpg"))
                    {
                        var start = 5;
                        var end = fileName.IndexOf(".");
                        var item = fileName.Substring(start, end - start);
                        if (item.Length >= 2) continue;

                        var page = int.Parse(fileName.Substring(start, end - start));
                        var newFileName = $"page-{page:D2}.jpg";
                        File.Move(path, path.Replace(fileName, newFileName));
                        Console.WriteLine($"Move file: {path}");
                    }
                    else if (fileName.EndsWith(".mp3"))
                    {
                        var items = fileName.Split("_");
                        string newFileName;
                        if (items[3].ToLower() == "title")
                        {
                            newFileName = fileName.Replace("_" + items[3], $"_p01");
                            File.Move(path, path.Replace(fileName, newFileName));
                            Console.WriteLine($"Move file: {path}");
                            continue;
                        }

                        if (items[3].Length >= 3) continue;

                        var page = int.Parse(items[3].Substring(1));
                        newFileName = fileName.Replace("_" + items[3], $"_p{page:D2}");
                        File.Move(path, path.Replace(fileName, newFileName));
                        Console.WriteLine($"Move file: {path}");
                    }
                }
            }
            
            foreach (var storyDirectory in storyDirectories)
            {
                var files = Directory.GetFiles(storyDirectory).ToList().Where(a => a.EndsWith(".mp3"));
                var stream = File.OpenWrite(Path.Combine(storyDirectory, "00.mp3"));
                foreach (var v in files)
                {
                    Console.WriteLine(v);
                    var bytes = File.ReadAllBytes(v);
                    stream.Write(bytes, 0, bytes.Length);
                    File.Delete(v);
                }

                stream.Close();
            }


        }
    }
}
