using System;

namespace Dusza2017_2018_Backup
{
    public class DiffItem
    {
        public readonly DiffItemType Type;
        public readonly string Path;
        public readonly int? Time;
        public readonly int? Size;

        public static DiffItem created(string path, int time, int size)
        {
            return new DiffItem(DiffItemType.CREATED, path, time, size);
        }

        public static DiffItem modified(string path, int time, int size)
        {
            return new DiffItem(DiffItemType.MODIFIED, path, time, size);
        }

        public static DiffItem deleted(string path)
        {
            return new DiffItem(DiffItemType.DELETED, path, null, null);
        }

        public static DiffItem deletedRecursively(string path) {
            return new DiffItem(DiffItemType.DELETED_RECURSIVELY, path, null, null);
        }

        private DiffItem(DiffItemType type, string path, int? time, int? size) {
            Type = type;
            Path = path;
            Time = time;
            Size = size;
        }

        public override string ToString()
        {
            switch (Type) {
                case DiffItemType.CREATED:
                    return $"LETREHOZ {Path} {Size} {Time}";
                case DiffItemType.MODIFIED:
                    return $"MODOSIT {Path} {Size} {Time}";
                case DiffItemType.DELETED:
                    return $"TOROL {Path}";
                case DiffItemType.DELETED_RECURSIVELY:
                    return $"REKURZIV-TOROL {Path}";
                default:
                    throw new NotImplementedException();
            }
        }  
    }

    public enum DiffItemType {
        CREATED,
        MODIFIED,
        DELETED,
        DELETED_RECURSIVELY
    }
}