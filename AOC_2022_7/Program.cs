using AOC.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AOC_2022_7
{
    class Program
    {
        abstract class DirectoryEntry
        {
            public DirectoryEntry(string name, Folder parentFolder)
            {
                Name = name;
                ParentFolder = parentFolder;
            }
            public Folder ParentFolder { get; }
            public string Name { get; }
            public List<DirectoryEntry> Entries { get; } = new List<DirectoryEntry>();
            public abstract Lazy<long> Size { get; }
        }
        class File : DirectoryEntry
        {
            public File(string name, Folder parentFolder, long size) : base(name, parentFolder)
            {
                Size = new Lazy<long>(size);
            }
            public override Lazy<long> Size { get; }
        }
        class Folder : DirectoryEntry
        {
            public Folder(string name, Folder parentFolder) : base(name, parentFolder)
            {
                Size = new Lazy<long>(() => Entries.Sum(x => x.Size.Value));
            }
            public override Lazy<long> Size { get; }
            public IReadOnlyList<Folder> AllDistinctFolders()
            {
                var folders = Entries.Where(x => x is Folder).Cast<Folder>().ToList();
                folders.AddRange(folders.SelectMany(x => x.AllDistinctFolders()).ToList());
                return folders;
            }
        }
        static async Task Main(string[] args)
        {
            string inputUrl = "https://adventofcode.com/2022/day/7/input";

            var input = await InputHelper.GetInputLines<string[]>(inputUrl);
            Folder root = new Folder("/", null);
            Folder directory = root;
            foreach(string[] line in input.Skip(1))
            {
                switch(line[0])
                {
                    case "dir":
                        directory.Entries.Add(new Folder(line[1], directory));
                        break;
                    case "$" when line[1] == "ls":
                        break;
                    case "$" when line[1] == "cd":
                        if (line[2] == "..")
                            directory = directory.ParentFolder;
                        else if (line[2] == "/")
                            throw new NotImplementedException();
                        else
                            directory = (Folder)directory.Entries.First(x => x.Name == line[2]);
                        break;
                    default:
                        {
                            if (int.TryParse(line[0], out int size))
                                directory.Entries.Add(new File(line[1], directory, size));
                            break;
                        }
                }
            }
            //part 1
            Console.WriteLine(root.AllDistinctFolders().Sum(x =>
            {
                long size = x.Size.Value;
                if (size <= 100_000) return size;
                else return 0;
            }));
            //part 2
            long totalSize = root.Size.Value;
            long missingSize = 30_000_000L - (70_000_000 - totalSize);
            var foldersOrdered = root.AllDistinctFolders().OrderBy(x => x.Size.Value).ToList();
            Console.WriteLine(foldersOrdered.First(x => x.Size.Value >= missingSize).Size);
        }
    }
}
