using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

//TODO: Implement JSON reader to make it faster and possibly include even more information.

namespace TreeViewMenu
{
    /// <summary>
    /// Interaction logic for View_TrainStationRenovationProgress.xaml
    /// </summary>
    public partial class View_TrainStationRenovationProgress : UserControl
    {
        private static string SavePath;


        public View_TrainStationRenovationProgress()
        {
            InitializeComponent();
            this.tb_Out.Text += "Detected savegames: " + "\n\n" + GetSavePath() + "\n";
            List<string> SavesList = GetSaveFiles();

            foreach(string savefile in SavesList)
            {
                FileInfo info = new FileInfo(SavePath + savefile);
                tb_Out.Text += savefile;
                tb_Out.Text += " (" + info.LastWriteTime.ToLocalTime() +")\n";
            }

            this.tb_Out.Text += "\n";


            foreach (string savefile in SavesList)
            {
                FileInfo info = new FileInfo(SavePath + savefile);
                tb_Out.Text += savefile;
                tb_Out.Text += "\nTotal Small Trash: " + GetAchData(SavePath + savefile, "amountOfSmallTrash");
                tb_Out.Text += "\nTotal Segregated Trash: " + GetAchData(SavePath + savefile, "amountOfSegregableTrash");
                tb_Out.Text += "\nTotal Stars: " + GetAchData(SavePath + savefile, "playerStarsAmount");
                tb_Out.Text += "\nTutorial starts: " + GetAchData(SavePath + savefile, "playerStartAmountInTutorial");
                tb_Out.Text += "\nTrain Parts: " + GetAchData(SavePath + savefile, "trainParts");
                tb_Out.Text += "\nLevel states:\n\t" + GetLevelStates(SavePath + savefile);
                tb_Out.Text += "\n\n";
            }
        }

        private string GetSavePath()
        {
            string text = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).Replace(@"\Roaming",@"\LocalLow") + @"\Live Motion Games\TrainStationRenovation\Save\";
            if (!global::System.IO.Directory.Exists(text))
            {
                SavePath = "Savegame folder not found!";
            }
            SavePath = text;

            return SavePath;
        }

        private List<string> GetSaveFiles()
        {
            List<string> szaResult = new List<string>();

            string[] allfiles = System.IO.Directory.GetFiles(SavePath, "*.save", System.IO.SearchOption.TopDirectoryOnly);
            foreach (var file in allfiles)
            {
                szaResult.Add(file.Replace(SavePath,""));
            }
            return szaResult;
        }

        public string GetAchData(string FilePath, string jsonval)
        {
            List<string> lines = File.ReadLines(FilePath).SkipWhile(line => !line.Contains(jsonval)).ToList();
            string szResult = lines[0].Replace(jsonval, "").Replace(",", "").Replace("\"", "").Replace(":","").Replace(" ","");
            return szResult;
        }

        private string GetLevelStates(string FilePath)
        {
            string szaResult = "";
            List<string> current = null;
            foreach (var line in File.ReadAllLines(FilePath))
            {
                if (line.Contains("state\":") && current == null)
                szaResult += line.Replace("\"state\":", "").Replace(",", "").Replace("\"", "").Replace(":", "").Replace(" ", "").Replace("0","Not available").Replace("1", "Available").Replace("2", "Started").Replace("3", "Repaired") + "\n\t";
            }
            return szaResult;
        }
    }
}
