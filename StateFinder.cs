using System.IO;
using Dusza20172018Backup;

namespace Dusza2017_2018_Backup
{
    public class StateFinder 
    {
        public int getNumberOfStates()
        {
            int i = 0;
            while (File.Exists($"filerendszer-{i}.txt")) {
                i++;
            }
            return i;
        }
    }
}