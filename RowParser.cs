using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dusza20172018Backup
{
    public class RowParser
    {
        public List<FileItem> parseRows(int stateId) {
            string[] lines = File.ReadAllLines($"filerendszer-{stateId}.txt");
            return lines.Select(row => parseRow(row)).ToList();
        }

        private FileItem parseRow(string row)
        {
            var items = row.Split(" ");

            var fullPath = items[0];
            var type = fullPath.EndsWith('/') ? FileItem.FileType.DIRECTORY : FileItem.FileType.FILE;

            string fullPathWithoutTrailingSlash;
            if (type == FileItem.FileType.FILE)
            {
                fullPathWithoutTrailingSlash = fullPath;
            }
            else
            {
                fullPathWithoutTrailingSlash = fullPath.Substring(0, fullPath.Length - 1);
            }

            var containingDir = fullPathWithoutTrailingSlash.Substring(0, fullPathWithoutTrailingSlash.LastIndexOf('/') + 1);
            var name = fullPathWithoutTrailingSlash.Substring(fullPathWithoutTrailingSlash.LastIndexOf('/') + 1);
            var size = int.Parse(items[1]);
            var lastUpdated = int.Parse(items[2]);

            return new FileItem(type, fullPath, containingDir, name, size, lastUpdated);
        }    
    }
}
