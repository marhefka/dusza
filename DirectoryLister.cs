using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dusza20172018Backup;

namespace Dusza2017_2018_Backup
{
    public class DirectoryLister 
    {
        private readonly RowParser rowParser;

        public DirectoryLister() {
            rowParser = new RowParser();
        }

        public List<FileItem> getFilesUnder(int stateId, string directory)
        {
            return rowParser.parseRows(stateId)
                        .Where(s => s.ContainingDir == directory)
                        .OrderBy(s => s.Type)
                        .ThenBy(s => s.FileName)
                        .ToList();
        }
    }
}