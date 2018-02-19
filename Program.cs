using System;
using System.Collections.Generic;
using System.Linq;
using Dusza20172018Backup;

namespace Dusza2017_2018_Backup
{
    public class Program
    {
        private readonly DirectoryLister directoryLister;
        private readonly StateFinder stateFinder;
        private readonly DiffFinder diffFinder;

        private int numberOfStates;
        private int currentStateId;
        private string currentDir;

        public Program() {
            directoryLister = new DirectoryLister();
            stateFinder = new StateFinder();
            diffFinder = new DiffFinder();
        }

        public void start()
        {
            numberOfStates = stateFinder.getNumberOfStates();
            currentStateId = 0;
            currentDir = "/";

            while (true)
            {
                var files = directoryLister.getFilesUnder(currentStateId, currentDir);

                printCurrentDirectory(files);
                printMenu();

                string s = Console.ReadLine().Trim();

                switch (s) {
                    case "":
                        sayGoodBye();
                        return;
                    case "^":
                        goOneLevelUp();
                        break;
                    case "+":
                        goToNextState();
                        break;
                    case "-":
                        goToPreviousState();
                        break;
                    case "*":
                        printChangeList();
                        break;
                    default:
                        changeDirectory(files, s);
                        break;
                }

                Console.WriteLine("-------------------------------------------------------");
            }
        }

        private void printCurrentDirectory(List<FileItem> files) {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Állapotok száma: {0}", numberOfStates);
            Console.WriteLine("A {0} könyvtár tartalma a(z) {1}. állapotban: ", currentDir, currentStateId);
            Console.WriteLine();

            for (int i = 0; i < files.Count; i++)
            {
                var type = files[i].Type == FileItem.FileType.DIRECTORY ? "DIR " : "FILE";
                Console.WriteLine(string.Format("{0} {1}", type, files[i].FileName));
            }
        }

        private void printMenu()
        {
            Console.WriteLine();
            Console.WriteLine("Menü");
            Console.WriteLine("====");
            Console.WriteLine();

            if (currentDir != "/")
            {
                Console.WriteLine("^: egy szinttel feljebb lépés");
            }

            if (currentStateId < numberOfStates - 1)
            {
                Console.WriteLine("+: következő állapot (->{0})", currentStateId + 1);
            }

            if (currentStateId > 0)
            {
                Console.WriteLine("-: előző állapot ({0}<-)", currentStateId - 1);
                Console.WriteLine("*: változáslista megtekintése az előző állapothoz képest");
            }
            Console.WriteLine("könyvtárnév: adott könyvtárba lépés");
            Console.WriteLine("ENTER: kilépés");
            Console.WriteLine();
            Console.Write("Válasszon: ");
        }

        private void sayGoodBye()
        {
            Console.WriteLine("Viszlát!");
        }

        private void goOneLevelUp()
        {
            if (currentDir != "/")
            {
                var withoutTrailingSlash = currentDir.Substring(0, currentDir.Length - 1);
                currentDir = currentDir.Substring(0, withoutTrailingSlash.LastIndexOf('/') + 1);
            }
            else
            {
                Console.WriteLine("HIBA: A GYÖKÉRKÖNYVTÁRBÓL NEM LEHET FELJEBB LÉPNI.");
            }
        }
                                           
        private void goToNextState()
        {
            if (currentStateId < numberOfStates - 1)
            {
                currentStateId++;
            }
            else
            {
                Console.WriteLine("HIBA: NINCSENEK TOVÁBBI ÁLLAPOTOK.");
            }
        }

        private void goToPreviousState()
        {
            if (currentStateId > 0)
            {
                currentStateId--;
            }
            else
            {
                Console.WriteLine("HIBA: NEM LEHET VISSZALÉPNI, MÁR A 0. ÁLLAPOTBAN VAGYUNK.");
            }
        }

        private void printChangeList()
        {
            if (currentStateId > 0)
            {
                List<DiffItem> diffItems =  diffFinder.findDifferences(currentStateId);
                diffItems.ForEach(item => Console.WriteLine(item));
            }
            else
            {
                Console.WriteLine("HIBA: A 0. ÁLLAPOTBAN VAGYUNK.");
            }
        }

        private void changeDirectory(List<FileItem> files, string s)
        {
            if (files.Exists(item => item.FileName.Equals(s) && item.Type == FileItem.FileType.DIRECTORY))
            {
                currentDir = currentDir + s + "/";
                return;
            }

            Console.WriteLine("HIBA: ÉRVÉNYTELEN KÖNYVTÁRNÉV.");
        }
    }
}
