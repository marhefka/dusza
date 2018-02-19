using System;
using System.Collections.Generic;
using System.Linq;
using Dusza2017_2018_Backup;

namespace Dusza20172018Backup
{
    public class DiffFinder
    {
        private readonly RowParser rowParser;

        public DiffFinder() {
            rowParser = new RowParser();
        }

        public List<DiffItem> findDifferences(int currentStateId)
        {
            Dictionary<string, FileItem> dict0 = getFiles(currentStateId - 1);
            Dictionary<string, FileItem> dict1 = getFiles(currentStateId);

            HashSet<string> deletedTopFolders = findDeletedTopFolders(dict0, dict1);

            List<DiffItem> ret = new List<DiffItem>();
            foreach (FileItem oldItem in dict0.Values)
            {
                if (dict1.ContainsKey(oldItem.FullPath))
                {
                    var newItem = dict1.GetValueOrDefault(oldItem.FullPath);

                    if (areDifferent(oldItem, newItem))
                    {
                        DiffItem diff1 = DiffItem.modified(newItem.FullPath, newItem.LastUpdated, newItem.Size);
                        ret.Add(diff1);
                        continue;
                    }

                    continue;
                }

                if (isTopLevelFolder(oldItem, deletedTopFolders)) {
                    DiffItem diff2 = DiffItem.deletedRecursively(oldItem.FullPath);
                    ret.Add(diff2);
                    continue;
                }

                if (isDescendantOf(oldItem.FullPath, deletedTopFolders)) {
                    continue;
                }

                DiffItem diff3 = DiffItem.deleted(oldItem.FullPath);
                ret.Add(diff3);
            }

            foreach (FileItem newItem in dict1.Values)
            {
                if (!dict0.ContainsKey(newItem.FullPath))
                {
                    DiffItem diff3 = DiffItem.created(newItem.FullPath, newItem.LastUpdated, newItem.Size);
                    ret.Add(diff3);
                    continue;
                }
            }

            return ret;
        }

        private HashSet<string> findDeletedTopFolders(Dictionary<string, FileItem> dict0, Dictionary<string, FileItem> dict1)
        {
            Dictionary<string, FileItem> deletedFolders = new Dictionary<string, FileItem>();

            foreach (FileItem oldItem in dict0.Values)
            {
                if (oldItem.Type == FileItem.FileType.DIRECTORY && !dict1.ContainsKey(oldItem.FullPath))
                {
                    deletedFolders.Add(oldItem.FullPath, oldItem);            
                }
            }

            HashSet<string> ret = new HashSet<string>();

            foreach(FileItem dir in deletedFolders.Values) {
                if (!deletedFolders.ContainsKey(dir.ContainingDir)) {
                    ret.Add(dir.FullPath);
                }
            }

            return ret;
        }

        private bool isTopLevelFolder(FileItem fileItem, HashSet<string> deletedTopFolders)
        {
            return fileItem.Type == FileItem.FileType.DIRECTORY && deletedTopFolders.Any(folderName => fileItem.FullPath == folderName);
        }

        private bool isDescendantOf(string fullPath, HashSet<string> deletedTopFolders)
        {
            return deletedTopFolders.Any(folderName => fullPath != folderName && fullPath.StartsWith(folderName));
        }

        private Dictionary<string, FileItem> getFiles(int stateId)
        {
            List<FileItem> items = rowParser.parseRows(stateId)
                                            .Where(item => !isSpecial(item))
                                            .ToList();
            
            Dictionary<string, FileItem> dict = new Dictionary<string, FileItem>();
            items.ForEach(item => dict.Add(item.FullPath, item));
            return dict;
        }

        private bool isSpecial(FileItem item)
        {
            return item.FileName.StartsWith('.') || item.FileName.EndsWith(".tmp") || item.Size > (100 * 1 << 20);
        }

        private bool areDifferent(FileItem old, FileItem @new) {
            return !old.FullPath.Equals(@new.FullPath) || old.Size != @new.Size || old.LastUpdated != @new.LastUpdated;
        }
    }
}
