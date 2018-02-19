namespace Dusza20172018Backup
{
    public class FileItem
    {
        public readonly FileType Type;
        public readonly string FullPath;
        public readonly string ContainingDir;
        public readonly string FileName;
        public readonly int Size;
        public readonly int LastUpdated;

        public FileItem(FileType type, string fullPath, string containingDir, string fileName, int size, int lastUpdated) {
            Type = type;
            FullPath = fullPath;
            ContainingDir = containingDir;
            FileName = fileName;
            Size = size;
            LastUpdated = lastUpdated;
        }

        public enum FileType {
            DIRECTORY,
            FILE
        }
    }
}