using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;

namespace heroclixAI
{
    public partial class mainForm : Form
    {
        //For Item handling
        LinkedList<ItemNode> ItemList = new LinkedList<ItemNode>();

        //For keeping track of occupied squares on the map
        LinkedList<MapNode> IsOccupiedList = new LinkedList<MapNode>();

        //For Hindrance, keeping a record besides the map of location to reference quickly for AI
        LinkedList<MapNode> HinderanceList = new LinkedList<MapNode>();

        //For Individual Characters
        Character Thor = new Character();
        Character EnemyThor = new Character();
        Character CaptainAmerica = new Character();
        Character EnemyCaptainAmerica = new Character();
        Character IronMan = new Character();
        Character EnemyIronMan = new Character();

        public mainForm()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
            
        }

        //Updates the gameMap to what squares/MapNodes that are occupied by characters
        private void UpdateCharacterLocations()
        {
            //Load map
            MapNode[,] gameMap = new MapNode[16, 16];
            gameMap = RetrieveMap();

            //Clear all previous occupied squares
            foreach (MapNode mapNode in IsOccupiedList)
            {
                gameMap[mapNode.x, mapNode.y].IsOccupied = false;
            }

            IsOccupiedList.Clear();

            //Retrieve data on all character locations, add them to the list, and update the gameMap
            MapNode captainAmerica = new MapNode();

            captainAmerica.x = ConvertColumnLetter(locationAlphaFriendlyCapCB.Text);
            captainAmerica.y = Convert.ToInt32(locationNumericFriendlyCapCB.Text) - 1;
            gameMap[captainAmerica.x, captainAmerica.y].IsOccupied = true;
            captainAmerica.IsOccupied = true;
            captainAmerica.OccupiedByCharacter = CaptainAmerica;
            IsOccupiedList.AddLast(captainAmerica);

            MapNode ironMan = new MapNode();

            ironMan.x = ConvertColumnLetter(locationAlphaFriendlyIronManCB.Text);
            ironMan.y = Convert.ToInt32(locationNumericFriendlyIronManCB.Text) - 1;
            gameMap[ironMan.x, ironMan.y].IsOccupied = true;
            ironMan.IsOccupied = true;
            ironMan.OccupiedByCharacter = IronMan;
            IsOccupiedList.AddLast(ironMan);

            MapNode thor = new MapNode();

            thor.x = ConvertColumnLetter(locationAlphaFriendlyThorCB.Text);
            thor.y = Convert.ToInt32(locationNumericFriendlyThorCB.Text) - 1;
            gameMap[thor.x, thor.y].IsOccupied = true;
            thor.IsOccupied = true;
            thor.OccupiedByCharacter = Thor;
            IsOccupiedList.AddLast(thor);

            MapNode enemyCaptainAmerica = new MapNode();

            enemyCaptainAmerica.x = ConvertColumnLetter(locationAlphaOpposingCapCB.Text);
            enemyCaptainAmerica.y = Convert.ToInt32(locationNumericOpposingCapCB.Text) - 1;
            gameMap[enemyCaptainAmerica.x, enemyCaptainAmerica.y].IsOccupied = true;
            enemyCaptainAmerica.IsOccupied = true;
            enemyCaptainAmerica.OccupiedByCharacter = EnemyCaptainAmerica;
            IsOccupiedList.AddLast(enemyCaptainAmerica);

            MapNode enemyIronMan = new MapNode();

            enemyIronMan.x = ConvertColumnLetter(locationAlphaOpposingIronManCB.Text);
            enemyIronMan.y = Convert.ToInt32(locationNumericOpposingIronManCB.Text) - 1;
            gameMap[enemyIronMan.x, enemyIronMan.y].IsOccupied = true;
            enemyIronMan.IsOccupied = true;
            enemyIronMan.OccupiedByCharacter = EnemyIronMan;
            IsOccupiedList.AddLast(enemyIronMan);

            MapNode enemyThor = new MapNode();

            enemyThor.x = ConvertColumnLetter(locationAlphaOpposingThorCB.Text);
            enemyThor.y = Convert.ToInt32(locationNumericOpposingThorCB.Text) - 1;
            gameMap[enemyThor.x, enemyThor.y].IsOccupied = true;
            enemyThor.IsOccupied = true;
            enemyThor.OccupiedByCharacter = EnemyThor;
            IsOccupiedList.AddLast(enemyThor);

            //Save the map data
            SaveMap(gameMap);

        }

        private void whatToDoCaptainAmerica_Click(object sender, EventArgs e)
        {
            DetermineCharactersTurn(CaptainAmerica);
            whatToDoCaptainAmerica.Enabled = false;
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            actionTokensCapFriendly.SelectedIndex = 0;
            actionTokensIronManFriendly.SelectedIndex = 0;
            actionTokensThorFriendly.SelectedIndex = 0;

            CreateNewMap();
            CreateCharacters();
            UpdateListViewItems();
            UpdateCharacterLocations();
        }

        private void nextTurnButton_Click(object sender, EventArgs e)
        {
            this.turnNumberLabel.Text = Convert.ToString(Convert.ToInt16(this.turnNumberLabel.Text) + 1);
        }

        private void previousTurnButton_Click(object sender, EventArgs e)
        {
            this.turnNumberLabel.Text = Convert.ToString(Convert.ToInt16(this.turnNumberLabel.Text) - 1);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.clickNumberFriendlyCap.Text = Convert.ToString(Convert.ToInt16(clickNumberFriendlyCap.Text) + 1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.clickNumberFriendlyCap.Text = Convert.ToString(Convert.ToInt16(clickNumberFriendlyCap.Text) - 1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.clickNumberFriendlyIronMan.Text = Convert.ToString(Convert.ToInt16(clickNumberFriendlyIronMan.Text) + 1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.clickNumberFriendlyIronMan.Text = Convert.ToString(Convert.ToInt16(clickNumberFriendlyIronMan.Text) - 1);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.clickNumberFriendlyThor.Text = Convert.ToString(Convert.ToInt16(clickNumberFriendlyThor.Text) + 1);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.clickNumberFriendlyThor.Text = Convert.ToString(Convert.ToInt16(clickNumberFriendlyThor.Text) - 1);
        }

        private void CreateCharacters()
        {
            //Begin setting up the characters
            CharacterAbility chAbility = new CharacterAbility();

            //Setup Thor
            Thor._CharacterName = "Thor";
            EnemyThor._CharacterName = "Enemy Thor";

            //Click 1
            chAbility._Charge = true;
            chAbility._SuperStrength = true;
            chAbility._Impervious = true;

            Thor.AddClick(1, 11, 11, 18, 4, 6, chAbility);
            EnemyThor.AddClick(1, 11, 11, 18, 4, 6, chAbility);

            //Click 2
            Thor.AddClick(2, 11, 11, 17, 4, 6, chAbility);
            EnemyThor.AddClick(2, 11, 11, 17, 4, 6, chAbility);

            //Click 3
            Thor.AddClick(3, 11, 11, 17, 3, 6, chAbility);
            EnemyThor.AddClick(3, 11, 11, 17, 3, 6, chAbility);

            //Click 4
            chAbility._Charge = false;
            chAbility._RunningShot = true;
            chAbility._SuperStrength = false;
            chAbility._EnergyExplosion = true;
            chAbility._Impervious = false;
            chAbility._Invulnerability = true;

            Thor.AddClick(4, 11, 10, 17, 3, 6, chAbility);
            EnemyThor.AddClick(4, 11, 10, 17, 3, 6, chAbility);

            //Click 5
            Thor.AddClick(5, 11, 10, 17, 3, 6, chAbility);
            EnemyThor.AddClick(5, 11, 10, 17, 3, 6, chAbility);

            //Click 6
            Thor.AddClick(6, 11, 10, 17, 3, 6, chAbility);
            EnemyThor.AddClick(6, 11, 10, 17, 3, 6, chAbility);

            //Click 7
            chAbility._RunningShot = false;
            chAbility._SideStep = true;
            chAbility._EnergyExplosion = false;
            chAbility._LightningSmash = true;
            chAbility._Invulnerability = false;
            chAbility._WillPower = true;

            Thor.AddClick(7, 10, 9, 17, 3, 6, chAbility);
            EnemyThor.AddClick(7, 10, 9, 17, 3, 6, chAbility);

            //Click 8
            Thor.AddClick(8, 10, 9, 17, 3, 6, chAbility);
            EnemyThor.AddClick(8, 10, 9, 17, 3, 6, chAbility);

            //Click 9
            Thor.AddClick(9, 10, 9, 16, 3, 6, chAbility);
            EnemyThor.AddClick(9, 10, 9, 16, 3, 6, chAbility);

            //Setup Iron Man
            IronMan._CharacterName = "Iron Man";
            EnemyIronMan._CharacterName = "Enemy Iron Man";

            //Click 1 Added ClearAbilites Method to help clear abilites in the chAbility class
            chAbility.ClearAbilites();
            chAbility._RunningShot = true;
            chAbility._EnergyExplosion = true;
            chAbility._Invulnerability = true;

            IronMan.AddClick(1, 11, 10, 18, 4, 7, chAbility);
            EnemyIronMan.AddClick(1, 11, 10, 18, 4, 7, chAbility);

            //Click 2
            IronMan.AddClick(2, 11, 10, 17, 3, 7, chAbility);
            EnemyIronMan.AddClick(2, 11, 10, 17, 3, 7, chAbility);

            //Click 3
            IronMan.AddClick(3, 11, 10, 17, 3, 7, chAbility);
            EnemyIronMan.AddClick(3, 11, 10, 17, 3, 7, chAbility);

            //Click 4
            chAbility.ClearAbilites();
            chAbility._SideStep = true;
            chAbility._Toughness = true;
            chAbility._RangedCombatExpert = true;

            IronMan.AddClick(4, 10, 9, 17, 2, 7, chAbility);
            EnemyIronMan.AddClick(4, 10, 9, 17, 2, 7, chAbility);

            //Click 5
            IronMan.AddClick(5, 10, 9, 17, 2, 7, chAbility);
            EnemyIronMan.AddClick(5, 10, 9, 17, 2, 7, chAbility);

            //Click 6
            IronMan.AddClick(6, 9, 9, 16, 2, 7, chAbility);
            EnemyIronMan.AddClick(6, 9, 9, 16, 2, 7, chAbility);

            //Click 7
            IronMan.AddClick(7, 9, 9, 16, 2, 7, chAbility);
            EnemyIronMan.AddClick(7, 9, 9, 16, 2, 7, chAbility);

            //Setup Captain America
            CaptainAmerica._CharacterName = "Captain America";
            EnemyCaptainAmerica._CharacterName = "Enemy Captain America";

            //Click 1
            chAbility.ClearAbilites();
            chAbility._Deflection = true;
            chAbility._Charge = true;
            chAbility._CombatReflexes = true;
            chAbility._Leadership = true;

            CaptainAmerica.AddClick(1, 9, 11, 17, 3, 5, chAbility);
            EnemyCaptainAmerica.AddClick(1, 9, 11, 17, 3, 5, chAbility);

            //Click 2
            CaptainAmerica.AddClick(2, 8, 10, 17, 3, 5, chAbility);
            EnemyCaptainAmerica.AddClick(2, 8, 10, 17, 3, 5, chAbility);

            //Click 3
            CaptainAmerica.AddClick(3, 8, 10, 17, 3, 5, chAbility);
            EnemyCaptainAmerica.AddClick(3, 8, 10, 17, 3, 5, chAbility);

            //Click 4
            chAbility.ClearAbilites();
            chAbility._Deflection = true;
            chAbility._SideStep = true;
            chAbility._WillPower = true;
            chAbility._CloseCombatExpert = true;

            CaptainAmerica.AddClick(4, 7, 9, 16, 2, 5, chAbility);
            EnemyCaptainAmerica.AddClick(4, 7, 9, 16, 2, 5, chAbility);

            //Click 5
            CaptainAmerica.AddClick(5, 7, 9, 16, 2, 5, chAbility);
            EnemyCaptainAmerica.AddClick(5, 7, 9, 16, 2, 5, chAbility);

            //Click 6
            CaptainAmerica.AddClick(6, 6, 9, 17, 2, 5, chAbility);
            EnemyCaptainAmerica.AddClick(6, 6, 9, 17, 2, 5, chAbility);
        }

        private void CreateNewMap()
        {
            //Creates the BinaryFormatter and Filestream, and writes the gameMap array retrieved from the CreateMap() method to a text file
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            filePath = filePath + @"\\HeroClixMapData.txt";
            BinaryFormatter bf = new BinaryFormatter();
            Stream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            bf.Serialize(stream, CreateMap());
            stream.Close();
        }

        private MapNode[,] RetrieveMap()
        {
            //Creates a FileStream to open and read from a text file and returns the gameMap array.
            try
            {
                string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                filePath = filePath + @"\\HeroClixMapData.txt";
                BinaryFormatter bf = new BinaryFormatter();
                MapNode[,] gameMap = new MapNode[16, 16];
                Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                gameMap = (MapNode[,])bf.Deserialize(stream);
                stream.Close();
                return gameMap;
            }
            catch (FileNotFoundException)
            {
                //Notify error and fix it
                MessageBox.Show("Map data file not found. Creating new map file.", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);

                CreateNewMap();

                //Rerun the method
                MapNode[,] gameMap = new MapNode[16, 16];
                gameMap = RetrieveMap();
                return gameMap;

            }
        }

        private void SaveMap(MapNode[,] gameMap)
        {
            //Creates or overwrites file at specified file path with the array given.
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            filePath = filePath + @"\\HeroClixMapData.txt";
            BinaryFormatter bf = new BinaryFormatter();
            Stream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            bf.Serialize(stream, gameMap);
            stream.Close();
        }

        //This is for the test button and used for testing code
        private void Button13_Click(object sender, EventArgs e)
        {
            MapNode[,] gameMap = new MapNode[16,16];
            gameMap = RetrieveMap();
            MapNode Node = new MapNode();
            Node = gameMap[0, 2];
            LinkedList<MapNode> AvoidanceList = new LinkedList<MapNode>();

            AvoidanceList = FindEnemyMovementThreatZoneForCharacter(Node, false);

            foreach (MapNode mapNode in AvoidanceList)
            {
                MessageBox.Show(ConvertColumnNumberToLetter(mapNode.x) + (mapNode.y + 1).ToString());
            }
        }

        private MapNode[,] CreateMap()
        {
            MapNode[,] gameMap = new MapNode[16, 16];

            int x = 0;
            int y = 0;

            while (x < 16)
            {
                while (y < 16)
                {
                    gameMap[x, y] = new MapNode();
                    gameMap[x, y].x = x;
                    gameMap[x, y].y = y;
                    CreateMapConditions(gameMap[x, y], x, y);
                    y = y + 1;
                }
                y = 0;
                x = x + 1;
            }

            return gameMap;
        }

        private void CreateMapConditions(MapNode node, int x, int y)
        {
            //Set North Connection To False
            if ((x+1 > 2 && x+1 < 8) && y+1 == 4 || //C4 - G4
                (x+1 > 9 && x+1 < 15) && y+1 == 4 || //J4 - N4
                (x+1 > 11 && x+1 < 15) && y+1 == 6 || //L6 - N6
                (x+1 > 3 && x+1 < 6) && y+1 == 7 || //D7 - E7
                x+1 == 11 && y+1 == 7 || //K7
                x+1 == 13 && y+1 == 7 || //M7
                (x+1 > 3 && x+1 < 7) && y+1 == 8 || //D8 - F8
                (x+1 > 7 && x+1 < 10) && y+1 == 8 || //H8 - I8
                x+1 == 13 && y+1 == 8 || //M8
                (x+1 > 7 && x+1 < 10) && y+1 == 10 || //H10 - I10
                (x+1 > 10 && x+1 < 15) && y+1 == 10 || //K10 - N10
                (x+1 > 2 && x+1 < 8) && y+1 == 14 || //C14 - G14
                (x+1 > 9 && x+1 < 15) && y+1 == 14 || //J14 - N14
                (x+1 > 0 && x+1 < 17) && y+1 == 1) //A1 - P16
            {
                node.IsConnectedToNorthNode = false;
            }
            //Set NorthEast Connection To False
            if  (x+1 == 2 && (y+1 > 4 && y+1 < 7) || //B5 - B6
                x+1 == 2 && (y+1 > 8 && y+1 < 15) || //B9 - B14
                (x+1 > 2 && x+1 < 7) && y+1 == 4 || //C4 - F4
                (x+1 > 9 && x+1 < 15) && y+1 == 4 || //I4 - N4
                x+1 == 14 && (y+1 > 3 && y+1 < 6) || //N4 - N5
                x+1 == 14 && (y+1 > 7 && y+1 < 11) || //N8 - N10
                x+1 == 14 && y+1 == 13 || //N13
                (x+1 > 2 && x+1 < 14) && y+1 == 14 || //B14 - M14
                x+1 == 5 && y+1 == 5 || //E5
                x+1 == 4 && y+1 == 7 || //D7
                x+1 == 10 && y+1 == 5 || //J5
                (x+1 > 11 && x+1 < 14) && y+1 == 6 || //L6 - M6
                x+1 == 13 && y+1 == 7 || //M7
                x+1 == 10 && (y+1 > 7 && y+1 < 11) || //J8 - J10
                x+1 == 12 && y+1 == 9 || //L9
                (x+1 > 9 && x+1 < 15) && y+1 == 10 || //J10 - N10
                x+1 == 10 && y+1 == 13 || //J13
                x+1 == 8 && y+1 == 8 || //H8
                x+1 == 7 && y+1 == 9 || //G9
                (x+1 > 6 && x+1 < 10) && y+1 == 10 || //G10 - I10
                (x+1 > 3 && x+1 < 7) && y+1 == 8 || //D8 - F8
                x+1 == 6 && (y+1 > 8 && y+1 < 11) || //F9 - F10
                (x+1 > 0 && x+1 < 17) && y+1 == 1 || //A1 - P16
                x+1 == 16 && (y+1 > 0 && y+1 < 17)) //P1 - P16
            {
                node.IsConnectedToNorthEastNode = false;
            }
            //Set East Connection To False
            if (x+1 == 2 && (y+1 > 3 && y+1 <7) || //B4 - B6
                x+1 == 2 && (y+1 > 7 && y+1 < 14) || //B8 - B13
                x+1 == 5 && (y+1 > 3 && y+1 < 6) || //E4 - E5
                x+1 == 6 && (y+1 > 7 && y+1 < 11) || //F8 - F10
                x+1 == 6 && y+1 == 13 || //F13
                x+1 == 7 && (y+1 > 7 && y+1 < 11) || //G8 - G10
                x+1 == 9 && (y+1 > 8 && y+1 < 11) || //I9 - I10
                x+1 == 10 && (y+1 > 3 && y+1 < 6) || //J4 - J5
                x+1 == 10 && (y+1 > 6 && y+1 < 10) || //J7 - J9
                x+1 == 10 && (y+1 > 11 && y+1 < 14) || //J12 - J13
                x+1 == 12 && (y+1 > 7 && y+1 < 10) || //L8 - L9
                x+1 == 13 && y+1 == 4 || //M4
                x+1 == 13 && y+1 == 7 || //M7
                x+1 == 14 && (y+1 > 3 && y+1 < 6) || //N4 - N5
                x+1 == 14 && (y+1 > 6 && y+1 < 11) || //N7 - N10
                x+1 == 14 && (y+1 > 11 && y+1 < 14) || //N12 - N13
                x+1 == 16 && (y+1 > 0 && y+1 < 17)) //P1 - P16
            {
                node.IsConnectedToEastNode = false;
            }
            //Set SouthEast Connection To False
            if (x+1 == 2 && (y+1 > 2 && y+1 < 6) || //B3 - B5
                x+1 == 2 && (y+1 > 7 && y+1 < 13) || //B8 - B12
                (x+1 > 1 && x+1 < 7) && y+1 == 3 || //B3 - F3
                (x+1 > 9 && x+1 < 14) && y+1 == 3 || //J3 - M3
                x+1 == 14 && (y+1 > 3 && y+1 < 6) || //N4 - N5
                x+1 == 14 && (y+1 > 6 && y+1 < 10) || //N7 - N9
                x+1 == 14 && (y+1 > 11 && y+1 < 14) || //N12 - N13
                (x+1 > 2 && x+1 < 7) && y+1 == 13 || //C13 - F13
                (x+1 > 9 && x+1 < 15) && y+1 == 13 || //J13 - N13
                x+1 == 4 && y+1 == 6 || //D6
                (x+1 > 11 && x+1 < 15) && y+1 == 5 || //L5 - N5
                x+1 == 10 && (y+1 > 5 && y+1 < 9) || //J6 - J8
                x+1 == 13 && y+1 == 7 || //M7
                x+1 == 12 && (y+1 > 6 && y+1 < 10) || //L7 - L9
                (x+1 > 10 && x+1 < 15) && y+1 == 9 || //K9 - N9
                x+1 == 10 && y+1 == 12 || //J12
                x+1 == 8 && y+1 == 7 || //H7
                x+1 == 7 && (y+1 > 6 && y+1 < 10) || //G7 - G9
                (x+1 > 7 && x+1 < 10) && y+1 == 9 || //H9 - I9
                (x+1 > 3 && x+1 < 6) && y+1 == 7 || //D7 - E7
                x+1 == 6 && (y+1 > 7 && y+1 < 10) || //F8 - F9
                x+1 == 16 && (y+1 > 0 && y+1 < 17) || //P1 - P16
                (x+1 > 0 && x+1 < 17) && y+1 == 16) // A16 - P16
            {
                node.IsConnectedToSouthEastNode = false;
            }
            //Set South Connection To False
            if ((x+1 > 2 && x+1 < 8) && y+1 == 3 || //C3 - G3
                (x+1 > 9 && x+1 < 15) && y+1 == 3 || //J3 - N3
                (x+1 > 11 && x+1 < 15) && y+1 == 5 || //L5 - N5
                (x+1 > 3 && x+1 < 6) && y+1 == 6 || //D6 - E6
                x+1 == 11 && y+1 == 6 || //K6
                x+1 == 13 && y+1 == 6 || //M6
                x+1 == 13 && y+1 == 7 || //M7
                (x+1 > 3 && x+1 < 7) && y+1 == 7 || //D7 - F7
                (x+1 > 7 && x+1 < 10) && y+1 == 7 || //H7 - I7
                (x+1 > 7 && x+1 < 10) && y+1 == 9 || //H9 - I9
                (x+1 > 10 && x+1 < 15) && y+1 == 9 || //K9 - N9
                (x+1 > 2 && x+1 < 8) && y+1 == 13 || //C13 - G13
                (x+1 > 9 && x+1 < 15) && y+1 == 13 || //J13 - N13
                (x+1 > 0 && x+1 < 17) && y+1 == 16) // A16 - P16
            {
                node.IsConnectedToSouthNode = false;
            }
            //Set SouthWest Connection to False
            if (x+1 == 3 && (y+1 > 3 && y+1 < 6) || //C4 - C5
                x+1 == 3 && (y+1 > 7 && y+1 < 14) || //C8 - C13
                (x+1 > 3 && x+1 < 8) && y+1 == 3 || //D3 - G3
                (x+1 > 10 && x+1 < 16) && y+1 == 3 || //K3 - O3
                x+1 == 15 && (y+1 > 2 && y+1 < 5) || //O3 - O4
                x+1 == 15 && (y+1 > 6 && y+1 < 10) || //O7 - O9
                x+1 == 15 && y+1 == 12 || //O12
                (x+1 > 2 && x+1 < 8) && y+1 == 13 || //C13 - G13
                (x+1 > 10 && x+1 < 15) && y+1 ==13 || //K13 - N13
                x+1 == 6 && y+1 == 4 || //F4
                x+1 == 5 && y+1 == 6 || //E6
                (x+1 > 12 && x+1 < 15) && y+1 == 5 || //M5 - N5
                x+1 == 11 && y+1 == 4 || //K4
                x+1 == 14 && y+1 == 6 || //N6
                x+1 == 11 && (y+1 > 6 && y+1 < 10) || //K7 - K9
                (x+1 > 10 && x+1 < 15) && y+1 == 9 || //K9 - N9
                x+1 == 13 && y+1 == 8 || //M8
                x+1 == 11 && y+1 == 12 || //K12
                x+1 == 9 && y+1 == 7 || //I7
                x+1 == 8 && y+1 == 8 || //H8
                (x+1 > 7 && x+1 < 11) && y+1 == 9 || //H9 - J9
                (x+1 > 4 && x+1 < 8) && y+1 == 7 || //E7 - G7
                x+1 == 7 && (y+1 > 6 && y+1 < 10) || //G7 - G9
                x+1 == 1 && (y+1 > 0 && y+1 < 17) || //A1 - A16
                (x+1 > 0 && x+1 < 17) && y+1 == 16) //A16 - P16
            {
                node.IsConnectedToSouthWestNode = false;
            }
            //Set West Connection to False
            if (x+1 == 3 && (y+1 > 3 && y+1 < 7) || //C4 - C7
                x+1 == 3 && (y+1 > 7 && y+1 < 14) || //C8 - C13
                x+1 == 15 && (y+1 > 3 && y+1 < 6) || //O4 - O5
                x+1 == 15 && (y+1 > 6 && y+1 < 11) || //O7 - O10
                x+1 == 15 && (y+1 > 11 && y+1 < 14) || //O12 - O13
                x+1 == 6 && (y+1 > 3 && y+1 < 6) || //F4 - F5
                x+1 == 11 && (y+1 > 3 && y+1 < 6) || //K4 - K5
                x+1 == 14 && y+1 == 4 || //N4
                x+1 == 14 && y+1 == 7 || //N7
                x+1 == 11 && (y+1 > 6 && y+1 < 10) || //K7 - K9
                x+1 == 13 && (y+1 > 7 && y+1 < 10) || //M8 - M9
                x+1 == 11 && (y+1 > 11 && y+1 < 14) || //K12 - K13
                x+1 == 10 && (y+1 > 8 && y+1 < 11) || //J9 - J10
                x+1 == 8 && (y+1 > 7 && y+1 < 11) || //H8 - H10
                x+1 == 7 && y+1 == 13 || //G13
                x+1 == 7 && (y+1 > 7 && y+1 < 11) || //G8 - G10
                x+1 == 1 && (y+1 > 0 && y+1 < 17)) //A1 - A16
            {
                node.isConnectedToWestNode = false;
            }
            //Set NorthWest Connection to False
            if (x + 1 == 3 && (y + 1 > 3 && y + 1 < 7) || //C4 - C6
                x + 1 == 3 && (y + 1 > 8 && y + 1 < 14) || //C9 - C13
                (x + 1 > 2 && x + 1 < 8) && y + 1 == 4 || //C4 - G4
                (x + 1 > 10 && x + 1 < 15) && y + 1 == 4 || //K4 - N4
                x + 1 == 15 && (y + 1 > 4 && y + 1 < 7) || //O5 - O6
                x + 1 == 15 && (y + 1 > 7 && y + 1 < 11) || //O8 - O10
                x + 1 == 15 && (y + 1 > 12 && y + 1 < 15) || //O13 - O14
                (x + 1 > 3 && x + 1 < 8) && y + 1 == 14 || //D14 - G14
                (x + 1 > 10 && x + 1 < 16) && y + 1 == 14 || //K14 - O14
                x + 1 == 6 && y + 1 == 5 || //F5
                x + 1 == 5 && y + 1 == 7 || //E7
                x + 1 == 11 && y + 1 == 5 || //K5
                (x + 1 > 12 && x + 1 < 16) && y + 1 == 6 || //M6 - O6
                x + 1 == 11 && (y + 1 > 6 && y + 1 < 10) || //K7 - K9
                x + 1 == 13 && (y + 1 > 7 && y + 1 < 10) || //M8 - M9
                x + 1 == 14 && y + 1 == 8 || //N8
                (x + 1 > 11 && x + 1 < 15) && y + 1 == 10 || //L10 - N10
                x + 1 == 11 && y + 1 == 13 || //K13
                x + 1 == 8 && (y+1 > 7 && y+1 < 11) || // H8 - H10
                x+1 == 9 && y+1 == 8 || //I8
                x+1 == 10 && y+1 == 10 || //J10
                x+1 == 9 && y+1 == 10 || //I10
                x+1 == 7 && (y+1 > 8 && y+1 < 11) || //G9 - G10
                (x+1 > 4 && x+1 < 7) && y+1 == 8 || //E8 - F8
                x+1 == 1 && (y+1 > 0 && y+1 < 17) || //A1 - A16
                (x+1 > 0 && x+1 < 17) && y+1 == 1) //A1 - P16
            {
                node.IsConnectedToNorthWestNode = false;
            }
            //Set Is Hinderance to True
            if ((x+1 > 4 && x+1 < 10) && y+1 < 3 || //E1:I2
                x+1 == 11 && y+1 == 3 || //K3
                x+1 == 6 && y+1 == 4 || //F4
                (x+1 > 9 && x+1 < 12) && y+1 == 4 || //J4 - K4
                x+1 == 13 && y+1 == 4 || //M4
                (x+1 > 7 && x+1 < 10) && y+1 == 6 || //H6 - I6
                x+1 == 6 && y+1 == 8 || //F8
                x+1 == 8 && (y+1 > 7 && y+1 < 10) || //H8 - H9
                x+1 == 9 && y+1 == 9 || //I9
                x+1 == 11 & y+1 == 9 || //K9
                x+1 == 14 && y+1 == 9 || //N9
                x+1 == 15 && y+1 == 8 || //O8
                x+1 == 3 && (y+1 > 9 && y+1 < 12) || //C10 - C11
                x+1 == 3 && y+1 == 13 || //C13
                x+1 == 5 && (y+1 > 9 && y+1 < 12) || //E10 - E11
                x+1 == 6 && y+1 == 13 || //F13
                (x+1 > 11 && x+1 < 14) && y+1 == 10 || //L10 - M10
                x+1 == 15 && y+1 == 10 || //O10
                (x+1 > 11 && x+1 < 14) && y+1 == 13 || //L13 - M13
                x+1 == 6 & y+1 == 14 || //F14
                x+1 == 6 & y+1 == 16 || //F16
                x+1 == 11 & y+1 == 14 || //K14
                x+1 == 11 & y+1 == 16) //K16
            {
                node.IsHinderance = true;
            }
            //Set Is Blocked to True
            if (x+1 == 1 && (y+1 > 4 && y+1 < 8) || //A5 - A7
                x+1 == 4 && y+1 == 6 || //D6
                x+1 == 6 && y+1 == 15 || //F15
                x+1 == 11 && y+1 == 15) //K15
            {
                node.IsBlocked = true;
            }
        }

        private void FinishTurnButton_Click(object sender, EventArgs e)
        {
            //Re-enable buttons
            whatToDoThor.Enabled = true;
            whatToDoIronMan.Enabled = true;
            whatToDoCaptainAmerica.Enabled = true;

            //Increase turn number
            turnNumberLabel.Text = (Convert.ToInt32(turnNumberLabel.Text) + 1).ToString();
        }

        private void ButtonWCCheck_Click(object sender, EventArgs e)
        {
            WallConnection();
        }

        private int ConvertColumnLetter(string columnLetter)
        {
            return ((int)(columnLetter[0]) % 32) - 1;
        }
        private void WallConnection(int createWall_1_or_BreakWall_2 = 0)
        {
            //Check for appropriate input data
            string firstWCCell = textBoxWCFirstCell.Text;
            string secondWCCell = textBoxWCSecondCell.Text;

            if (firstWCCell.Length < 2 || firstWCCell.Length > 3 || secondWCCell.Length < 2 || secondWCCell.Length > 3 ||
                Char.IsLetter(firstWCCell, 0) == false || Char.IsDigit(firstWCCell, 1) == false ||
                Char.IsLetter(secondWCCell, 0) == false || Char.IsDigit(secondWCCell, 1) == false)
            {
                MessageBox.Show("Please enter the location information in the format similar to: A1", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxWCCondition.Clear();
                return;
            }

            //Convert Cell Address to Index number
            int firstWCCellx = ((int)firstWCCell[0] % 32) - 1;
            int firstWCCelly = ((int)int.Parse((new string(firstWCCell.Where(Char.IsDigit).ToArray()))) - 1);
            int secondWCCellx = ((int)secondWCCell[0] % 32) - 1;
            int secondWCCelly = ((int)int.Parse((new string(secondWCCell.Where(Char.IsDigit).ToArray()))) - 1);

            //Determine if both cells are next to each other and the direction between the two Cells
            int xValue = firstWCCellx - secondWCCellx;
            int yValue = firstWCCelly - secondWCCelly;
                    
            if (xValue > 1 || xValue < -1 || yValue > 1 || yValue < -1 || (xValue == 0 && yValue == 0))
            {
                MessageBox.Show("The locations entered are not adjacent", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxWCCondition.Clear();
                return;
            }

            //Load recent map data
            MapNode[,] gameMap = new MapNode[16, 16];
            gameMap = RetrieveMap();

            //If the map data file could not be found, exit method.
            if (gameMap == null)
            {
                return;
            }

            //Select the correct direction and then run the related algorithm checks
            string combinedXY = xValue.ToString() + yValue.ToString();
            var check1 = false;
            var check2 = false;

            switch (combinedXY)
            {
                case "10":

                    //Runs additional code depending upon the createWall_1_or_BreakWall integer used. This continues on through the switch.
                    if (createWall_1_or_BreakWall_2 == 1)
                    {
                        gameMap[firstWCCellx, firstWCCelly].isConnectedToWestNode = false;
                        gameMap[secondWCCellx, secondWCCelly].IsConnectedToEastNode = false;
                    }
                    if (createWall_1_or_BreakWall_2 == 2)
                    {
                        gameMap[firstWCCellx, firstWCCelly].isConnectedToWestNode = true;
                        gameMap[secondWCCellx, secondWCCelly].IsConnectedToEastNode = true;
                    }

                    //Runs no matter what the createOrBreakWallIndicator variable value is. This continues on through the switch.
                    check1 = gameMap[firstWCCellx, firstWCCelly].isConnectedToWestNode;
                    check2 = gameMap[secondWCCellx, secondWCCelly].IsConnectedToEastNode;
                    break;

                case "11":

                    if (createWall_1_or_BreakWall_2 == 1)
                    {
                        gameMap[firstWCCellx, firstWCCelly].IsConnectedToNorthWestNode = false;
                        gameMap[secondWCCellx, secondWCCelly].IsConnectedToSouthEastNode = false;
                    }
                    if (createWall_1_or_BreakWall_2 == 2)
                    {
                        gameMap[firstWCCellx, firstWCCelly].IsConnectedToNorthWestNode = true;
                        gameMap[secondWCCellx, secondWCCelly].IsConnectedToSouthEastNode = true;
                    }

                    check1 = gameMap[firstWCCellx, firstWCCelly].IsConnectedToNorthWestNode;
                    check2 = gameMap[secondWCCellx, secondWCCelly].IsConnectedToSouthEastNode;
                    break;

                case "01":

                    if (createWall_1_or_BreakWall_2 == 1)
                    {
                        gameMap[firstWCCellx, firstWCCelly].IsConnectedToNorthNode = false;
                        gameMap[secondWCCellx, secondWCCelly].IsConnectedToSouthNode = false;
                    }
                    if (createWall_1_or_BreakWall_2 == 2)
                    {
                        gameMap[firstWCCellx, firstWCCelly].IsConnectedToNorthNode = true;
                        gameMap[secondWCCellx, secondWCCelly].IsConnectedToSouthNode = true;
                    }

                    check1 = gameMap[firstWCCellx, firstWCCelly].IsConnectedToNorthNode;
                    check2 = gameMap[secondWCCellx, secondWCCelly].IsConnectedToSouthNode;
                    break;

                case "-11":

                    if (createWall_1_or_BreakWall_2 == 1)
                    {
                        gameMap[firstWCCellx, firstWCCelly].IsConnectedToNorthEastNode = false;
                        gameMap[secondWCCellx, secondWCCelly].IsConnectedToSouthWestNode = false;
                    }
                    if (createWall_1_or_BreakWall_2 == 2)
                    {
                        gameMap[firstWCCellx, firstWCCelly].IsConnectedToNorthEastNode = true;
                        gameMap[secondWCCellx, secondWCCelly].IsConnectedToSouthWestNode = true;
                    }

                    check1 = gameMap[firstWCCellx, firstWCCelly].IsConnectedToNorthEastNode;
                    check2 = gameMap[secondWCCellx, secondWCCelly].IsConnectedToSouthWestNode;
                    break;

                case "-10":

                    if (createWall_1_or_BreakWall_2 == 1)
                    {
                        gameMap[firstWCCellx, firstWCCelly].IsConnectedToEastNode = false;
                        gameMap[secondWCCellx, secondWCCelly].isConnectedToWestNode = false;
                    }
                    if (createWall_1_or_BreakWall_2 == 2)
                    {
                        gameMap[firstWCCellx, firstWCCelly].IsConnectedToEastNode = true;
                        gameMap[secondWCCellx, secondWCCelly].isConnectedToWestNode = true;
                    }

                    check1 = gameMap[firstWCCellx, firstWCCelly].IsConnectedToEastNode;
                    check2 = gameMap[secondWCCellx, secondWCCelly].isConnectedToWestNode;
                    break;

                case "-1-1":

                    if (createWall_1_or_BreakWall_2 == 1)
                    {
                        gameMap[firstWCCellx, firstWCCelly].IsConnectedToSouthEastNode = false;
                        gameMap[secondWCCellx, secondWCCelly].IsConnectedToNorthWestNode = false;
                    }
                    if (createWall_1_or_BreakWall_2 == 2)
                    {
                        gameMap[firstWCCellx, firstWCCelly].IsConnectedToSouthEastNode = true;
                        gameMap[secondWCCellx, secondWCCelly].IsConnectedToNorthWestNode = true;
                    }

                    check1 = gameMap[firstWCCellx, firstWCCelly].IsConnectedToSouthEastNode;
                    check2 = gameMap[secondWCCellx, secondWCCelly].IsConnectedToNorthWestNode;
                    break;

                case "0-1":

                    if (createWall_1_or_BreakWall_2 == 1)
                    {
                        gameMap[firstWCCellx, firstWCCelly].IsConnectedToSouthNode = false;
                        gameMap[secondWCCellx, secondWCCelly].IsConnectedToNorthNode = false;
                    }
                    if (createWall_1_or_BreakWall_2 == 2)
                    {
                        gameMap[firstWCCellx, firstWCCelly].IsConnectedToSouthNode = true;
                        gameMap[secondWCCellx, secondWCCelly].IsConnectedToNorthNode = true;
                    }

                    check1 = gameMap[firstWCCellx, firstWCCelly].IsConnectedToSouthNode;
                    check2 = gameMap[secondWCCellx, secondWCCelly].IsConnectedToNorthNode;
                    break;

                case "1-1":

                    if (createWall_1_or_BreakWall_2 == 1)
                    {
                        gameMap[firstWCCellx, firstWCCelly].IsConnectedToSouthWestNode = false;
                        gameMap[secondWCCellx, secondWCCelly].IsConnectedToNorthEastNode = false;
                    }
                    if (createWall_1_or_BreakWall_2 == 2)
                    {
                        gameMap[firstWCCellx, firstWCCelly].IsConnectedToSouthWestNode = true;
                        gameMap[secondWCCellx, secondWCCelly].IsConnectedToNorthEastNode = true;
                    }

                    check1 = gameMap[firstWCCellx, firstWCCelly].IsConnectedToSouthWestNode;
                    check2 = gameMap[secondWCCellx, secondWCCelly].IsConnectedToNorthEastNode;
                    break;
            }

            //Check for consistancy between the two checks
            if (check1 != check2)
            {
                MessageBox.Show("Connection is inconsistant. One of the connection nodes returns a true connection while the other returns a false connection", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBoxWCCondition.Text = "Inconsistant";
                return;
            }

            //Save current state of game map if a wall was created or destroyed
            if (createWall_1_or_BreakWall_2 > 0)
            {
                SaveMap(gameMap);
            }

            //Changes textBoxWCCondition to true or false on whether a wall/connection exsists
            if (check1 == true)
            {
                textBoxWCCondition.Text = "False";
            }
            else
            {
                textBoxWCCondition.Text = "True";
            }
        }

        private void ButtonBreakWall_Click(object sender, EventArgs e)
        {
            WallConnection(2);
        }

        private void ButtonCreateWall_Click(object sender, EventArgs e)
        {
            WallConnection(1);
        }

        private void ButtonCreateNewMap_Click(object sender, EventArgs e)
        {
            UpdateCharacterLocations();
            CreateNewMap();
        }

        private void CreateItem(string weaponType, MapNode mapLocation)
        {
            //Checks for appropiate data
            if (weaponType != "Light" && weaponType != "Heavy")
            {
                MessageBox.Show("Weapon Type selected does not exsist", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Find next highest ID then create new item with +1 to that number
            ItemNode Item = new ItemNode();
            int highest = 1;

            if (ItemList.Count != 0)
            {
                highest = ItemList.Max(l => l.ID);
                Item.ID = highest + 1;
            }

            //Assign parameters depending on weapon type
            switch (weaponType)
            {
                case "Light":
                    {
                        Item.weaponType = "Light";
                        Item.meleeDamageBonus = 1;
                        Item.rangeDamageBonus = 2;
                        Item.range = 6;
                        Item.defenseBonus = 0;
                        Item.hinderance = false;
                        Item.mapLocation = mapLocation;
                        break;
                    }
                case "Heavy":
                    {
                        Item.weaponType = "Heavy";
                        Item.meleeDamageBonus = 2;
                        Item.rangeDamageBonus = 3;
                        Item.range = 4;
                        Item.defenseBonus = 0;
                        Item.hinderance = true;
                        Item.mapLocation = mapLocation;

                        //Update Map information based on hinderance for heavy weapons
                        MapNode[,] gameMap = new MapNode[16, 16];
                        gameMap = RetrieveMap();
                        gameMap[mapLocation.x, mapLocation.y].IsHinderance = true;
                        SaveMap(gameMap);
                        break;
                    }
            }

            //Adds the item to the ItemList
            ItemList.AddLast(Item);
            UpdateListViewItems();
        }

        //Updates the ListViewItems
        private void UpdateListViewItems()
        {
            //Starts of fresh by erasing everything and creating the column headers
            listViewItems.Clear();
            listViewItems.Columns.Add("ID", 30, HorizontalAlignment.Left);
            listViewItems.Columns.Add("M. Damage", 75, HorizontalAlignment.Left);
            listViewItems.Columns.Add("R. Damage", 75, HorizontalAlignment.Left);
            listViewItems.Columns.Add("Range", 50, HorizontalAlignment.Left);
            listViewItems.Columns.Add("Weapon Type", 90, HorizontalAlignment.Left);
            listViewItems.Columns.Add("Creates Hindrance", 110, HorizontalAlignment.Left);
            listViewItems.Columns.Add("Map Location", 110, HorizontalAlignment.Left);

            //Repopulate the ListView with information from ItemList
            foreach (var Node in ItemList)
            {
                ListViewItem Item = new ListViewItem(Node.ID.ToString());
                Item.SubItems.Add(Node.meleeDamageBonus.ToString());
                Item.SubItems.Add(Node.rangeDamageBonus.ToString());
                Item.SubItems.Add(Node.range.ToString());
                Item.SubItems.Add(Node.weaponType.ToString());
                Item.SubItems.Add(Node.hinderance.ToString());

                //Determines whether to display the mapLocation or the character who has the item currently
                if (Node.mapLocation != null)
                {
                    Item.SubItems.Add(Node.mapLocation.x.ToString() + "," + Node.mapLocation.y.ToString());
                }
                else
                {
                    Item.SubItems.Add(Node.character._CharacterName.ToString());
                }

                listViewItems.Items.Add(Item);
            }

            //Display the new information
            listViewItems.View = View.Details;
        }

        private void ButtonCreateItem_Click(object sender, EventArgs e)
        {
            //Check for appropriate information
            if (radioButtonLightIH.Checked == false && radioButtonHeavyIH.Checked == false)
            {
                MessageBox.Show("Please select a weapon type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (comboBoxRowIH.Text == "" || comboBoxColumnIH.Text == "")
            {
                MessageBox.Show("Please enter the map location x and y", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Assign which radiobutton is selected
            string weaponType = null;

            if (radioButtonLightIH.Checked == true)
            {
                weaponType = "Light";
            }

            if (radioButtonHeavyIH.Checked == true)
            {
                weaponType = "Heavy";
            }

            //Convert Column Letter and Row Number to array integers
            int x = ConvertColumnLetter(comboBoxColumnIH.Text);
            int y = Convert.ToInt32((comboBoxRowIH.Text.ToString())) - 1;

            //Retrieve the correct MapNode and save to pass on to Create Item Method
            MapNode[,] gameMap = new MapNode[16, 16];
            gameMap = RetrieveMap();

            CreateItem(weaponType, gameMap[x, y]);
        }

        private void DestroyItem(int itemID)
        {
            //Retrieves Item infromation from ItemList
            ItemNode Item = new ItemNode();
            Item = ItemList.First(l => l.ID == itemID);

            //Remove From Character, ItemList, and Map
            switch (Item.weaponType.ToString())
            {
                case "Light":
                    {
                        if (Item.character != null)
                        {
                            Item.character._LightWeapon = false;
                        }
                        
                        ItemList.Remove(Item);
                        break;
                    }

                case "Heavy":
                    {
                        if (Item.character != null)
                        {
                            Item.character._HeavyWeapon = false;
                            ItemList.Remove(Item);
                            break;
                        }
                        else
                        {
                            int x = Item.mapLocation.x;
                            int y = Item.mapLocation.y;

                            if ((x + 1 > 4 && x + 1 < 10) && y + 1 < 3 || //E1:I2
                                x + 1 == 11 && y + 1 == 3 || //K3
                                x + 1 == 6 && y + 1 == 4 || //F4
                                (x + 1 > 9 && x + 1 < 12) && y + 1 == 4 || //J4 - K4
                                x + 1 == 13 && y + 1 == 4 || //M4
                                (x + 1 > 7 && x + 1 < 10) && y + 1 == 6 || //H6 - I6
                                x + 1 == 6 && y + 1 == 8 || //F8
                                x + 1 == 8 && (y + 1 > 7 && y + 1 < 10) || //H8 - H9
                                x + 1 == 9 && y + 1 == 9 || //I9
                                x + 1 == 11 & y + 1 == 9 || //K9
                                x + 1 == 14 && y + 1 == 9 || //N9
                                x + 1 == 15 && y + 1 == 8 || //O8
                                x + 1 == 3 && (y + 1 > 9 && y + 1 < 12) || //C10 - C11
                                x + 1 == 3 && y + 1 == 13 || //C13
                                x + 1 == 5 && (y + 1 > 9 && y + 1 < 12) || //E10 - E11
                                x + 1 == 6 && y + 1 == 13 || //F13
                                (x + 1 > 11 && x + 1 < 14) && y + 1 == 10 || //L10 - M10
                                x + 1 == 15 && y + 1 == 10 || //O10
                                (x + 1 > 11 && x + 1 < 14) && y + 1 == 13 || //L13 - M13
                                x + 1 == 6 & y + 1 == 14 || //F14
                                x + 1 == 6 & y + 1 == 16 || //F16
                                x + 1 == 11 & y + 1 == 14 || //K14
                                x + 1 == 11 & y + 1 == 16) //K16
                            {
                                ItemList.Remove(Item);
                                break;
                            }
                            else
                            {
                                MapNode[,] gameMap = new MapNode[16, 16];
                                gameMap = RetrieveMap();
                                gameMap[Item.mapLocation.x, Item.mapLocation.y].IsHinderance = false;
                                SaveMap(gameMap);
                                ItemList.Remove(Item);
                                break;
                            }
                        }
                    }
            }

            //Refresh the ListViewItems
            UpdateListViewItems();
        }

        private void ButtonDestroyItem_Click(object sender, EventArgs e)
        {
            //Check to see if an item is selected
            if (listViewItems.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select an item.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Retrieve the ID number from the selected Item and pass it to DestroyItem method
            ListViewItem Item = listViewItems.SelectedItems[0];
            DestroyItem(Convert.ToInt32(Item.SubItems[0].Text));
        }

        private void PickUpItem(int itemID, string characterName)
        {
            //Retrieves Item infromation from ItemList
            ItemNode Item = new ItemNode();
            Item = ItemList.First(l => l.ID == itemID);

            //Check to make sure no one else is holding the weapon
            if (Item.character != null)
            {
                MessageBox.Show("Item is already being held by another character.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Sets which character currently owns the item in the ItemList and the Character Class
            switch (characterName)
            {
                case "Thor":
                    {
                        if (Item.weaponType == "Light" && Thor._LightWeapon == false && Thor._HeavyWeapon == false)
                        {
                            Thor._LightWeapon = true;
                            Item.character = Thor;
                            break;
                        }

                        if (Item.weaponType == "Heavy" && Thor._LightWeapon == false && Thor._HeavyWeapon == false)
                        {
                            Thor._HeavyWeapon = true;
                            Item.character = Thor;
                            break;
                        }

                        MessageBox.Show("Character already has a weapon.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                case "Iron Man":
                    {
                        if (Item.weaponType == "Light" && IronMan._LightWeapon == false && IronMan._HeavyWeapon == false)
                        {
                            IronMan._LightWeapon = true;
                            Item.character = IronMan;
                            break;
                        }

                        if (Item.weaponType == "Heavy" && IronMan._LightWeapon == false && IronMan._HeavyWeapon == false)
                        {
                            IronMan._HeavyWeapon = true;
                            Item.character = IronMan;
                            break;
                        }

                        MessageBox.Show("Character already has a weapon.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                case "Captain America":
                    {
                        if (Item.weaponType == "Light" && CaptainAmerica._LightWeapon == false && CaptainAmerica._HeavyWeapon == false)
                        {
                            CaptainAmerica._LightWeapon = true;
                            Item.character = CaptainAmerica;
                            break;
                        }

                        if (Item.weaponType == "Heavy" && CaptainAmerica._LightWeapon == false && CaptainAmerica._HeavyWeapon == false)
                        {
                            CaptainAmerica._HeavyWeapon = true;
                            Item.character = CaptainAmerica;
                            break;
                        }

                        MessageBox.Show("Character already has a weapon.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                case "Enemy Thor":
                    {
                        if (Item.weaponType == "Light" && EnemyThor._LightWeapon == false && EnemyThor._HeavyWeapon == false)
                        {
                            EnemyThor._LightWeapon = true;
                            Item.character = EnemyThor;
                            break;
                        }

                        if (Item.weaponType == "Heavy" && EnemyThor._LightWeapon == false && EnemyThor._HeavyWeapon == false)
                        {
                            EnemyThor._HeavyWeapon = true;
                            Item.character = EnemyThor;
                            break;
                        }

                        MessageBox.Show("Character already has a weapon.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                case "Enemy Iron Man":
                    {
                        if (Item.weaponType == "Light" && EnemyIronMan._LightWeapon == false && EnemyIronMan._HeavyWeapon == false)
                        {
                            EnemyIronMan._LightWeapon = true;
                            Item.character = EnemyIronMan;
                            break;
                        }

                        if (Item.weaponType == "Heavy" && EnemyIronMan._LightWeapon == false && EnemyIronMan._HeavyWeapon == false)
                        {
                            EnemyIronMan._HeavyWeapon = true;
                            Item.character = EnemyIronMan;
                            break;
                        }

                        MessageBox.Show("Character already has a weapon.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                case "Enemy Captain America":
                    {
                        if (Item.weaponType == "Light" && EnemyCaptainAmerica._LightWeapon == false && EnemyCaptainAmerica._HeavyWeapon == false)
                        {
                            EnemyCaptainAmerica._LightWeapon = true;
                            Item.character = EnemyCaptainAmerica;
                            break;
                        }

                        if (Item.weaponType == "Heavy" && EnemyCaptainAmerica._LightWeapon == false && EnemyCaptainAmerica._HeavyWeapon == false)
                        {
                            EnemyCaptainAmerica._HeavyWeapon = true;
                            Item.character = EnemyCaptainAmerica;
                            break;
                        }

                        MessageBox.Show("Character already has a weapon.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
            }

            //Determines if the gameMap needs to be updated due to a heavy weapon and hinderance
            if (Item.weaponType == "Heavy")
            {
                MapNode[,] gameMap = new MapNode[16, 16];
                gameMap = RetrieveMap();

                int x = Item.mapLocation.x;
                int y = Item.mapLocation.y;

                if ((x + 1 > 4 && x + 1 < 10) && y + 1 < 3 || //E1:I2
                    x + 1 == 11 && y + 1 == 3 || //K3
                    x + 1 == 6 && y + 1 == 4 || //F4
                    (x + 1 > 9 && x + 1 < 12) && y + 1 == 4 || //J4 - K4
                    x + 1 == 13 && y + 1 == 4 || //M4
                    (x + 1 > 7 && x + 1 < 10) && y + 1 == 6 || //H6 - I6
                    x + 1 == 6 && y + 1 == 8 || //F8
                    x + 1 == 8 && (y + 1 > 7 && y + 1 < 10) || //H8 - H9
                    x + 1 == 9 && y + 1 == 9 || //I9
                    x + 1 == 11 & y + 1 == 9 || //K9
                    x + 1 == 14 && y + 1 == 9 || //N9
                    x + 1 == 15 && y + 1 == 8 || //O8
                    x + 1 == 3 && (y + 1 > 9 && y + 1 < 12) || //C10 - C11
                    x + 1 == 3 && y + 1 == 13 || //C13
                    x + 1 == 5 && (y + 1 > 9 && y + 1 < 12) || //E10 - E11
                    x + 1 == 6 && y + 1 == 13 || //F13
                    (x + 1 > 11 && x + 1 < 14) && y + 1 == 10 || //L10 - M10
                    x + 1 == 15 && y + 1 == 10 || //O10
                    (x + 1 > 11 && x + 1 < 14) && y + 1 == 13 || //L13 - M13
                    x + 1 == 6 & y + 1 == 14 || //F14
                    x + 1 == 6 & y + 1 == 16 || //F16
                    x + 1 == 11 & y + 1 == 14 || //K14
                    x + 1 == 11 & y + 1 == 16) //K16
                {
                    gameMap[x, y].IsHinderance = true;
                    SaveMap(gameMap);
                }
                else
                {
                    gameMap[x, y].IsHinderance = false;
                    SaveMap(gameMap);
                }
            }

            //Deletes the MapNode associated with the item since it is being held
            Item.mapLocation = null;

            //Refresh the listViewItem
            UpdateListViewItems();
        }

        private void ButtonPickUpItem_Click(object sender, EventArgs e)
        {
            //Check to see if an item is selected
            if (listViewItems.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select an item.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Check to see if a character is selected
            if (comboBoxPickUpIH.Text != "Thor" && comboBoxPickUpIH.Text != "Iron Man"  && comboBoxPickUpIH.Text != "Captain America" &&
                comboBoxPickUpIH.Text != "Enemy Thor" && comboBoxPickUpIH.Text != "Enemy Iron Man" && comboBoxPickUpIH.Text != "Enemy Captain America")
            {
                MessageBox.Show("Please select a character.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Retrieve the ID number from the selected Item, assigning a Character to the item, and pass it to the PickUpItem method
            ListViewItem Item = listViewItems.SelectedItems[0];
            PickUpItem(Convert.ToInt32(Item.SubItems[0].Text), comboBoxPickUpIH.Text);
        }

        private void DropItem(int itemID, MapNode mapLocation)
        {
            //Retrieve Item from ItemList and remove it afterwards
            ItemNode Item = new ItemNode();
            Item = ItemList.FirstOrDefault(l => l.ID == itemID);

            //For heavy weapons update map
            if (Item.weaponType == "Heavy")
            {
                MapNode[,] gameMap = new MapNode[16, 16];
                gameMap = RetrieveMap();
                gameMap[mapLocation.x, mapLocation.y].IsHinderance = true;
                SaveMap(gameMap);
            }

            //Update Item Location
            Item.mapLocation = mapLocation;

            //Delete Character References and reinsert it back into the list
            Item.character._LightWeapon = false;
            Item.character._HeavyWeapon = false;
            Item.character = null;

            //Refresh the ListView
            UpdateListViewItems();
            
        }

        private void ButtonDropItem_Click(object sender, EventArgs e)
        {
            //Check to see if an item is selected
            if (listViewItems.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select an item.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Check to see if a map location has been provided
            if (comboBoxRowDropIH.Text == "" || comboBoxColumnDropIH.Text == "")
            {
                MessageBox.Show("Please enter the map location x and y", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Check to see if the item is being held by a character
            ItemNode ItemWeapon = new ItemNode();
            ListViewItem Item = listViewItems.SelectedItems[0];
            ItemWeapon = ItemList.FirstOrDefault(l => l.ID == Convert.ToInt32(Item.SubItems[0].Text));

            if (ItemWeapon.character == null)
            {
                MessageBox.Show("Item is not held by any character.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Retrieve the correct MapNode and save to pass on to Drop Item Method
            MapNode[,] gameMap = new MapNode[16, 16];
            gameMap = RetrieveMap();
            int x = ConvertColumnLetter(comboBoxColumnDropIH.Text);
            int y = Convert.ToInt32(comboBoxRowDropIH.Text) - 1;

            DropItem(ItemWeapon.ID, gameMap[x, y]);
        }

        //Returns a LineOfSight object that has four booleans for line of sight, hinderance, occupied, and blocked MapNodes
        private LineOfSight IsThereLineOfSight(MapNode attacker, MapNode defender)
        {
            //Create the object that will hold the answers and the MapNode variable to step through with
            LineOfSight lineOfSight = new LineOfSight();
            MapNode current = new MapNode();

            //If the same location is passed return true for line of sight
            if (attacker.x == defender.x && attacker.y == defender.y)
            {
                lineOfSight.lineOfSight = true;
                lineOfSight.isHinderance = false;
                lineOfSight.isOccupied = false;
                lineOfSight.IsBlocked = false;
                return lineOfSight;
            }

            //Retrieve the current game map
            MapNode[,] gameMap = new MapNode[16, 16];
            gameMap = RetrieveMap();

            //Determine if the line of sight is vertical
            if (attacker.x == defender.x)
            {
                current = gameMap[attacker.x, attacker.y];

                //Determine whether to go up or down from the attacker
                int direction = defender.y - attacker.y;

                if (direction > 0)
                {
                    while (current.y != defender.y)
                    {
                        if (current.IsConnectedToSouthNode == false)
                        {
                            lineOfSight.lineOfSight = false;
                            return lineOfSight;
                        }

                        //Addtionally to check hinderance, check to make sure that it doesn't flag the attacker's location with hinderance
                        if (current.IsHinderance == true && (current.x != attacker.x || current.y != attacker.y))
                        {
                            lineOfSight.isHinderance = true;
                        }

                        //Additionally to check occupied, check to make sure that it doesn't flag the attacker's location with occupation
                        if (current.IsOccupied == true && (current.x != attacker.x || current.y != attacker.y))
                        {
                            lineOfSight.isOccupied = true;
                            lineOfSight.lineOfSight = false;
                            return lineOfSight;
                        }

                        if (current.IsBlocked == true)
                        {
                            lineOfSight.IsBlocked = true;
                            lineOfSight.lineOfSight = false;
                            return lineOfSight;
                        }

                        current = gameMap[current.x, current.y + 1];
                    }

                    //If no wall is detected return true after checking the defenders MapNode
                    if (gameMap[defender.x, defender.y].IsHinderance == true)
                    {
                        lineOfSight.isHinderance = true;
                    }

                    lineOfSight.lineOfSight = true;
                    return lineOfSight;
                }

                if (direction < 0)
                {
                    while (current.y != defender.y)
                    {
                        if (current.IsConnectedToNorthNode == false)
                        {
                            lineOfSight.lineOfSight = false;
                            return lineOfSight;
                        }

                        //Additionally to check hinderance, check to make sure that it doesn't flag the attaacker's location with occupation
                        if (current.IsHinderance == true && (current.x != attacker.x || current.y != attacker.y))
                        {
                            lineOfSight.isHinderance = true;
                        }

                        //Additionally to check occupied, check to make sure that it doesn't flag the attacker's location with occupation
                        if (current.IsOccupied == true && (current.x != attacker.x || current.y != attacker.y))
                        {
                            lineOfSight.isOccupied = true;
                            lineOfSight.lineOfSight = false;
                            return lineOfSight;
                        }

                        if (current.IsBlocked == true)
                        {
                            lineOfSight.IsBlocked = true;
                            lineOfSight.lineOfSight = false;
                            return lineOfSight;
                        }

                        current = gameMap[current.x, current.y - 1];
                    }

                    //If no wall is detected return true after checking the defenders MapNode
                    if (gameMap[defender.x, defender.y].IsHinderance == true)
                    {
                        lineOfSight.isHinderance = true;
                    }

                    lineOfSight.lineOfSight = true;
                    return lineOfSight;
                }
            }

            //Determine if the line of sight is horizontal
            if (attacker.y == defender.y)
            {
                current = gameMap[attacker.x, attacker.y];

                //Determine whether to go left or right from the attacker
                int direction = defender.x - attacker.x;

                if (direction > 0)
                {
                    while (current.x != defender.x)
                    {
                        if (current.IsConnectedToEastNode == false)
                        {
                            lineOfSight.lineOfSight = false;
                            return lineOfSight;
                        }

                        //Addtionally to checking hinderance, check to make sure that it doesn't flag the attacker's location with hinderance
                        if (current.IsHinderance == true && (current.x != attacker.x || current.y != attacker.y))
                        {
                            lineOfSight.isHinderance = true;
                        }

                        //Additionally to checking occupied, check to make sure that it doesn't flag the attacker's location with occupation
                        if (current.IsOccupied == true && (current.x != attacker.x || current.y != attacker.y))
                        {
                            lineOfSight.isOccupied = true;
                            lineOfSight.lineOfSight = false;
                            return lineOfSight;
                        }

                        if (current.IsBlocked == true)
                        {
                            lineOfSight.IsBlocked = true;
                            lineOfSight.lineOfSight = false;
                            return lineOfSight;
                        }

                        current = gameMap[current.x + 1, current.y];
                    }

                    //If no wall is detected return true after checking the defenders MapNode
                    if (gameMap[defender.x, defender.y].IsHinderance == true)
                    {
                        lineOfSight.isHinderance = true;
                    }

                    lineOfSight.lineOfSight = true;
                    return lineOfSight;
                }

                if (direction < 0)
                {
                    while (current.x != defender.x)
                    {
                        if (current.isConnectedToWestNode == false)
                        {
                            lineOfSight.lineOfSight = false;
                            return lineOfSight;
                        }

                        //Additionally to check hinderance, check to make sure that it doesn't flag the attacker's location with hinderance
                        if (current.IsHinderance == true && (current.x != attacker.x || current.y != attacker.y))
                        {
                            lineOfSight.isHinderance = true;
                        }

                        //Additionally to check occupied, check to make sure that it doesn't flag the attacker's location with occupation
                        if (current.IsOccupied == true && (current.x != attacker.x || current.y != attacker.y))
                        {
                            lineOfSight.isOccupied = true;
                            lineOfSight.lineOfSight = false;
                            return lineOfSight;
                        }

                        if (current.IsBlocked == true)
                        {
                            lineOfSight.IsBlocked = true;
                            lineOfSight.lineOfSight = false;
                            return lineOfSight;
                        }

                        current = gameMap[current.x - 1, current.y];
                    }

                    //If no wall is detected return true after checking the defenders MapNode
                    if (gameMap[defender.x, defender.y].IsHinderance == true)
                    {
                        lineOfSight.isHinderance = true;
                    }

                    lineOfSight.lineOfSight = true;
                    return lineOfSight;
                }
            }

            //Determine if the Line of Sight is diagonal
            if (Math.Abs(defender.x - attacker.x) == Math.Abs(defender.y - attacker.y))
            {
                current = gameMap[attacker.x, attacker.y];

                //Determine the direction of the diagonal, true = positive while false = negative
                Boolean vertical = false;
                Boolean horizontal = false;

                if (defender.x - attacker.x > 0)
                {
                    horizontal = true;
                }

                if (defender.y - attacker.y > 0)
                {
                    vertical = true;
                }

                //Run the algorithm to determine the walls and MapNodes the line of sight crosses over
                if (horizontal == true && vertical == true)
                {
                    while (current.x != defender.x)
                    {
                        if (current.IsConnectedToSouthEastNode == false)
                        {
                            lineOfSight.lineOfSight = false;
                            return lineOfSight;
                        }

                        //Additionally to check hinderance, check to make sure that it doesn't flag the attacker's location with hinderance
                        if (current.IsHinderance == true && (current.x != attacker.x || current.y != attacker.y))
                        {
                            lineOfSight.isHinderance = true;
                        }

                        //Additionally to check occupied, check to make sure that it doesn't flag the attacker's location with occupation
                        if (current.IsOccupied == true && (current.x != attacker.x || current.y != attacker.y))
                        {
                            lineOfSight.isOccupied = true;
                            lineOfSight.lineOfSight = false;
                            return lineOfSight;
                        }

                        if (current.IsBlocked == true)
                        {
                            lineOfSight.IsBlocked = true;
                            lineOfSight.lineOfSight = false;
                            return lineOfSight;
                        }

                        current = gameMap[current.x + 1, current.y + 1];
                    }

                    //If no wall is detected return true after checking the defenders MapNode
                    if (gameMap[defender.x, defender.y].IsHinderance == true)
                    {
                        lineOfSight.isHinderance = true;
                    }

                    lineOfSight.lineOfSight = true;
                    return lineOfSight;
                }

                if (horizontal == false && vertical == true)
                {
                    while (current.x != defender.x)
                    {
                        if (current.IsConnectedToSouthWestNode == false)
                        {
                            lineOfSight.lineOfSight = false;
                            return lineOfSight;
                        }

                        //Additionally to check hinderance, check to make sure that it doesn't flag the attacker's location with hinderance
                        if (current.IsHinderance == true && (current.x != attacker.x || current.y != attacker.y))
                        {
                            lineOfSight.isHinderance = true;
                        }

                        //Additionally to check occupied, check to make sure that it doesn't flag the attacker's location with occupation
                        if (current.IsOccupied == true && (current.x != attacker.x || current.y != attacker.y))
                        {
                            lineOfSight.isOccupied = true;
                            lineOfSight.lineOfSight = false;
                            return lineOfSight;
                        }

                        if (current.IsBlocked == true)
                        {
                            lineOfSight.IsBlocked = true;
                            lineOfSight.lineOfSight = false;
                            return lineOfSight;
                        }

                        current = gameMap[current.x - 1, current.y + 1];
                    }

                    //If no wall is detected return true after checking the defenders MapNode
                    if (gameMap[defender.x, defender.y].IsHinderance == true)
                    {
                        lineOfSight.isHinderance = true;
                    }

                    lineOfSight.lineOfSight = true;
                    return lineOfSight;
                }

                if (horizontal == true && vertical == false)
                {
                    while (current.x != defender.x)
                    {
                        if (current.IsConnectedToNorthEastNode == false)
                        {
                            lineOfSight.lineOfSight = false;
                            return lineOfSight;
                        }

                        //Additionally to check hinderance, check to make sure that it doesn't flag the attacker's location with hinderance
                        if (current.IsHinderance == true && (current.x != attacker.x || current.y != attacker.y))
                        {
                            lineOfSight.isHinderance = true;
                        }

                        //Additionally to check occupied, check to make sure that it doesn't flag the attacker's location with occupation
                        if (current.IsOccupied == true && (current.x != attacker.x || current.y != attacker.y))
                        {
                            lineOfSight.isOccupied = true;
                            lineOfSight.lineOfSight = false;
                            return lineOfSight;
                        }

                        if (current.IsBlocked == true)
                        {
                            lineOfSight.IsBlocked = true;
                            lineOfSight.lineOfSight = false;
                            return lineOfSight;
                        }

                        current = gameMap[current.x + 1, current.y - 1];
                    }

                    //If no wall is detected return true after checking the defenders MapNode
                    if (gameMap[defender.x, defender.y].IsHinderance == true)
                    {
                        lineOfSight.isHinderance = true;
                    }

                    lineOfSight.lineOfSight = true;
                    return lineOfSight;
                }

                if (horizontal == false && vertical == false)
                {
                    while (current.x != defender.x)
                    {
                        if (current.IsConnectedToNorthWestNode == false)
                        {
                            lineOfSight.lineOfSight = false;
                            return lineOfSight;
                        }

                        //Additionally to check hinderance, check to make sure that it doesn't flag the attacker's location with hinderance
                        if (current.IsHinderance == true && (current.x != attacker.x || current.y != attacker.y))
                        {
                            lineOfSight.isHinderance = true;
                        }

                        //Additionally to check occupied, check to make sure that it doesn't flag the attacker's location with occupation
                        if (current.IsOccupied == true && (current.x != attacker.x || current.y != attacker.y))
                        {
                            lineOfSight.isOccupied = true;
                            lineOfSight.lineOfSight = false;
                            return lineOfSight;
                        }

                        if (current.IsBlocked == true)
                        {
                            lineOfSight.IsBlocked = true;
                            lineOfSight.lineOfSight = false;
                            return lineOfSight;
                        }

                        current = gameMap[current.x - 1, current.y - 1];
                    }

                    //If no wall is detected return true after checking the defenders MapNode
                    if (gameMap[defender.x, defender.y].IsHinderance == true)
                    {
                        lineOfSight.isHinderance = true;
                    }

                    lineOfSight.lineOfSight = true;
                    return lineOfSight;
                }
            }

            //Run the algorithm to determine if there is line of sight with a non cardinal direction line
            current = gameMap[attacker.x, attacker.y];

            //Determine x and y slope as well as the offset for each
            decimal slopeX = defender.x - attacker.x;
            decimal slopeY = defender.y - attacker.y;
            decimal slopeXOffset = 0.5m;
            decimal slopeYOffset = 0.5m;
            
            if (slopeX > 0)
            {
                slopeXOffset = -0.5m;
            }

            if (slopeY > 0)
            {
                slopeYOffset = -0.5m;
            }

            //Find the y-intercept
            decimal b = (((slopeY / slopeX) * attacker.x) - attacker.y) * -1m;

            //Determine and record the horizontal lines that are crossed
            LinkedList<decimal> XLines = new LinkedList<decimal>();
            for (var i = Math.Abs(slopeX); i > 0; i--)
            {
                if (slopeX > 0)
                {
                    decimal line = attacker.x + slopeXOffset + i;
                    XLines.AddLast(line);
                }

                if (slopeX < 0)
                {
                    decimal line = attacker.x + slopeXOffset - i;
                    XLines.AddLast(line);
                }
            }

            //Determine and record the vertical lines that are crossed
            LinkedList<decimal> YLines = new LinkedList<decimal>();
            for (var i = Math.Abs(slopeY); i > 0; i--)
            {
                if (slopeY > 0)
                {
                    decimal line = attacker.y + slopeYOffset + i;
                    YLines.AddLast(line);
                }

                if (slopeY < 0)
                {
                    decimal line = attacker.y + slopeYOffset - i;
                    YLines.AddLast(line);
                }
            }

            //Determine and record all intersects.
            LinkedList<IntersectNode> IntersectList = new LinkedList<IntersectNode>();

            foreach (decimal line in XLines)
            {
                IntersectNode IntersectNode = new IntersectNode();
                decimal y = ((slopeY / slopeX) * line) + b;
                IntersectNode.y = y;
                IntersectNode.x = line;
                IntersectList.AddLast(IntersectNode);
            }

            foreach (decimal line in YLines)
            {
                IntersectNode IntersectNode = new IntersectNode();
                decimal x = (line - b) * (slopeX / slopeY); //Made Change Here with times by reciprical
                IntersectNode.x = x;
                IntersectNode.y = line;
                IntersectList.AddLast(IntersectNode);
            }

            //Find the appropriate wall and MapNode that the line crosses
            LinkedList<MapNode> MapNodeList = new LinkedList<MapNode>();
            int xpoint;
            int ypoint;

            foreach (IntersectNode Node in IntersectList)
            {
                //Make sure the numbers do not go outside of the map array when rounding
                if (Convert.ToInt32(Math.Round(Node.x)) > 15)
                {
                    xpoint = 15;
                }
                else
                {
                    xpoint = Convert.ToInt32(Math.Round(Node.x));
                }

                if (Convert.ToInt32(Math.Round(Node.y)) > 15)
                {
                    ypoint = 15;
                }
                else
                {
                    ypoint = Convert.ToInt32(Math.Round(Node.y));
                }

                //Retrieve the corresponding MapNode from the map
                current = gameMap[xpoint, ypoint];

                //Determine which wall of the current MapNode that the line crosses
                if (Node.x.ToString().Length > 1)
                {
                    //See if the number ends with 0.5. It will determine which algorithmic path to take
                    if (Node.x.ToString().Substring(Node.x.ToString().Length - 2) == ".5")
                    {
                        //Determine either east or west wall to check for the current node
                        if (current.x - Node.x > 0)
                        {
                            if (current.isConnectedToWestNode == false)
                            {
                                lineOfSight.lineOfSight = false;
                                return lineOfSight;
                            }

                            //If the MapNode isn't already in the MapNode list add it
                            if (MapNodeList != null)
                            {
                                if (MapNodeList.FirstOrDefault(l => l.x == current.x && l.y == current.y) == null)
                                {
                                    MapNodeList.AddLast(current);
                                }

                                //Add the MapNode that shares the wall
                                current = gameMap[current.x - 1, current.y];

                                if (MapNodeList.FirstOrDefault(l => l.x == current.x && l.y == current.y) == null)
                                {
                                    MapNodeList.AddLast(current);
                                }
                            }
                            else
                            {
                                //Add the MapNodes that share the wall
                                MapNodeList.AddLast(current);
                                current = gameMap[current.x - 1, current.y];
                                MapNodeList.AddLast(current);
                            }
                        }
                        
                        if (current.x - Node.x < 0)
                        {
                            if (current.IsConnectedToEastNode == false)
                            {
                                lineOfSight.lineOfSight = false;
                                return lineOfSight;
                            }

                            //If the MapNode isn't already in the MapNode list add it
                            if (MapNodeList != null)
                            {
                                if (MapNodeList.FirstOrDefault(l => l.x == current.x && l.y == current.y) == null)
                                {
                                    MapNodeList.AddLast(current);
                                }

                                //Add the MapNode that shares the wall
                                current = gameMap[MaxCap(current.x + 1, 15), current.y];

                                if (MapNodeList.FirstOrDefault(l => l.x == current.x && l.y == current.y) == null)
                                {
                                    MapNodeList.AddLast(current);
                                }
                            }
                            else
                            {
                                //Add the MapNodes that share the wall
                                MapNodeList.AddLast(current);
                                current = gameMap[MaxCap(current.x + 1, 15), current.y];
                                MapNodeList.AddLast(current);
                            }
                        }
                    }

                    if (Node.y.ToString().Substring(Node.y.ToString().Length - 2) == ".5")
                    {
                        //Determine either east or west wall to check for the current node
                        if (current.y - Node.y > 0)
                        {
                            if (current.IsConnectedToNorthNode == false)
                            {
                                lineOfSight.lineOfSight = false;
                                return lineOfSight;
                            }

                            //If the MapNode isn't already in the MapNode list add it
                            if (MapNodeList != null)
                            {
                                if (MapNodeList.FirstOrDefault(l => l.x == current.x && l.y == current.y) == null)
                                {
                                    MapNodeList.AddLast(current);
                                }

                                //Add the MapNode that shares the wall
                                current = gameMap[current.x, current.y - 1];

                                if (MapNodeList.FirstOrDefault(l => l.x == current.x && l.y == current.y) == null)
                                {
                                    MapNodeList.AddLast(current);
                                }
                            }
                            else
                            {
                                //Add the MapNodes that share the wall
                                MapNodeList.AddLast(current);
                                current = gameMap[current.x, current.y - 1];
                                MapNodeList.AddLast(current);
                            }
                        }

                        if (current.y - Node.y < 0)
                        {
                            if (current.IsConnectedToSouthNode == false)
                            {
                                lineOfSight.lineOfSight = false;
                                return lineOfSight;
                            }

                            //If the MapNode isn't already in the MapNode list add it
                            if (MapNodeList != null)
                            {
                                if (MapNodeList.FirstOrDefault(l => l.x == current.x && l.y == current.y) == null)
                                {
                                    MapNodeList.AddLast(current);
                                }

                                //Add the MapNode that shares the wall
                                current = gameMap[current.x, MaxCap(current.y + 1, 15)];

                                if (MapNodeList.FirstOrDefault(l => l.x == current.x && l.y == current.y) == null)
                                {
                                    MapNodeList.AddLast(current);
                                }
                            }
                            else
                            {
                                //Add the MapNodes that share the wall
                                MapNodeList.AddLast(current);
                                current = gameMap[current.x, MaxCap(current.y + 1, 15)];
                                MapNodeList.AddLast(current);
                            }
                        }
                    }
                }                
            }

            //Check for Hinderance and also not to flag the attacker's own occupied MapNode
            if (MapNodeList.FirstOrDefault(l => l.IsHinderance == true && l.x != attacker.x && l.y != attacker.y) != null)
            {
                lineOfSight.isHinderance = true;
            }

            //Check for Occupied and also not to flag the attacker's own occupied MapNode
            if (MapNodeList.FirstOrDefault(l => l.IsOccupied == true && l.x != attacker.x && l.y != attacker.y) != null)
            {
                lineOfSight.isOccupied = true;
                lineOfSight.lineOfSight = false;
                return lineOfSight;
            }

            //Check for Blocked
            if (MapNodeList.FirstOrDefault(l => l.IsBlocked == true) != null)
            {
                lineOfSight.IsBlocked = true;
                lineOfSight.lineOfSight = false;
                return lineOfSight;
            }

            lineOfSight.lineOfSight = true;
            return lineOfSight;
        }

        //Sets a maximum number that is allowed. If number goes over maximum then returns the set maximum number
        private int MaxCap( int numberToBeTested, int maximumAllowed, int lowestAllowed = 0)
        {
            if (numberToBeTested > maximumAllowed)
            {
                return maximumAllowed;
            }

            if(numberToBeTested < lowestAllowed)
            {
                return lowestAllowed;
            }

            return numberToBeTested;
        }

        //Searches for the nearest MapNode to the location. It takes many arguments to edit the search. Returns null if no path found.
        private MapNode FindNearestMapNodeToTargetLocation(MapNode startingPosition, MapNode targetLocation, int distanceFromTargetMin = 1, int distanceFromTargetMax = 1, Boolean collectHinderance = true, Boolean collectOnlyLineOfSight = true)
        {
            //Setup linkedlist, gameMap, lineOfSight, and pathfinder
            LinkedList<MapNode> PossibleTargetLocations = new LinkedList<MapNode>();
            Pathfinder pathfinder = new Pathfinder();
            MapNode[,] gameMap = new MapNode[16, 16];
            gameMap = RetrieveMap();
            LineOfSight lineOfSight = new LineOfSight();

            //First check the target location if it is reachable
            if (pathfinder.FindShortestPath(startingPosition, targetLocation, gameMap).Count > 0 &&
                gameMap[targetLocation.x, targetLocation.y].IsOccupied == false &&
                gameMap[targetLocation.x, targetLocation.y].IsBlocked == false)
            {
                //Check for or not for hindrance
                switch (collectHinderance)
                {
                    case true:
                        {
                            return gameMap[targetLocation.x, targetLocation.y];
                        }
                    case false:
                        {
                            if (gameMap[targetLocation.x, targetLocation.y].IsHinderance == false)
                            {
                                return gameMap[targetLocation.x, targetLocation.y];
                            }

                            break;
                        }
                }
            }

            //Continue on to collect possible Mapnode locations that can be targeted
            for (int i = distanceFromTargetMin; i <= distanceFromTargetMax; i++)
            {
                for (int x = i * -1; x <= i; x++)
                {
                    for (int y = i * -1; y <= i; y++)
                    {
                        //Check to see if it passes to make the possible target node list
                        if (gameMap[MaxCap(targetLocation.x + x, 15), MaxCap(targetLocation.y + y, 15)].IsBlocked)
                        {
                            continue;
                        }

                        if (gameMap[MaxCap(targetLocation.x + x, 15), MaxCap(targetLocation.y + y, 15)].IsOccupied)
                        {
                            continue;
                        }

                        if (gameMap[MaxCap(targetLocation.x + x, 15), MaxCap(targetLocation.y + y, 15)].IsHinderance == true && collectHinderance == false)
                        {
                            continue;
                        }

                        //Used for the following checks
                        MapNode current = new MapNode();
                        current = gameMap[MaxCap(targetLocation.x + x, 15), MaxCap(targetLocation.y + y, 15)];

                        //Check Line of Sight
                        if (collectOnlyLineOfSight == true)
                        {
                            lineOfSight = IsThereLineOfSight(current, targetLocation);

                            if (lineOfSight.lineOfSight == false)
                            {
                                continue;
                            }
                        }

                        //Check if it is reachable from starting location
                        if (pathfinder.FindShortestPath(startingPosition, current, gameMap) == null)
                        {
                            continue;
                        }

                        //Check if it is within the distance from target
                        LinkedList<MapNode> DistanceCount = new LinkedList<MapNode>();
                        DistanceCount = pathfinder.FindShortestPath(targetLocation, current, gameMap);
                        if (DistanceCount.Count < distanceFromTargetMin)
                        {
                            continue;
                        }

                        //Check to see if MapNode already exsists in the linkedList
                        if (PossibleTargetLocations.Count != 0)
                        {
                            if (PossibleTargetLocations.FirstOrDefault(l => l.x == current.x && l.y == current.y) == null)
                            {
                                PossibleTargetLocations.AddLast(current);
                            }
                        }
                        else
                        {
                            PossibleTargetLocations.AddLast(current);
                        }
                    }
                }                
            }

            //If no free spot is found return null
            if (PossibleTargetLocations.Count == 0)
            {
                return null;
            }

            //Select the closest MapNode from the list
            MapNode nearestNode = new MapNode();
            LinkedList<MapNode> Count = new LinkedList<MapNode>();
            nearestNode = PossibleTargetLocations.First();
            Count = pathfinder.FindShortestPath(startingPosition, nearestNode, gameMap);
            int steps = Count.Count;

            foreach (MapNode Node in PossibleTargetLocations)
            {
                Count = pathfinder.FindShortestPath(startingPosition, Node, gameMap);

                if (Count.Count < steps)
                {
                    nearestNode = Node;
                }
            }

            return nearestNode;
        }

        private void LocationAlphaFriendlyCapCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCharacterLocations();
        }

        private void LocationNumericFriendlyCapCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCharacterLocations();
        }

        private void LocationAlphaFriendlyIronManCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCharacterLocations();
        }

        private void LocationNumericFriendlyIronManCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCharacterLocations();
        }

        private void LocationAlphaFriendlyThorCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCharacterLocations();
        }

        private void LocationNumericFriendlyThorCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCharacterLocations();
        }

        private void LocationAlphaOpposingCapCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCharacterLocations();
        }

        private void LocationNumericOpposingCapCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCharacterLocations();
        }

        private void LocationAlphaOpposingIronManCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCharacterLocations();
        }

        private void LocationNumericOpposingIronManCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCharacterLocations();
        }

        private void LocationAlphaOpposingThorCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCharacterLocations();
        }

        private void LocationNumericOpposingThorCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateCharacterLocations();
        }

        //Returns the enemy character with it's threatLevel assign that presents the greatest threat to a single ally character
        private Character ReturnGreatestThreat(Character allyCharacter)
        {
            //Setup
            Pathfinder pathfinder = new Pathfinder();
            MapNode[,] gameMap = new MapNode[16, 16];
            gameMap = RetrieveMap();
            LineOfSight lineOfSight = new LineOfSight();
            MapNode allyCharacterLocation = IsOccupiedList.FirstOrDefault(l => l.OccupiedByCharacter == allyCharacter);
            MapNode enemyThorLocation = IsOccupiedList.FirstOrDefault(l => l.OccupiedByCharacter == EnemyThor);
            MapNode enemyIronManLocation = IsOccupiedList.FirstOrDefault(l => l.OccupiedByCharacter == EnemyIronMan);
            MapNode enemyCaptainAmericaLocation = IsOccupiedList.FirstOrDefault(l => l.OccupiedByCharacter == EnemyCaptainAmerica);

            int distance;
            int enemyClick;
            int LOS;
            int enemyThor;
            int enemyIronMan;
            int enemyCaptainAmerica;

            LinkedList<MapNode> distanceLinkedList = new LinkedList<MapNode>();

            //Calculate Thor's Threat number starting with distance
            if (EnemyThor._TotalClicks >= Convert.ToInt32(clickNumberOpposingThor.Text))
            {
                distanceLinkedList = pathfinder.FindShortestPath(allyCharacterLocation, enemyThorLocation, gameMap);

                if (distanceLinkedList == null)
                {
                    distance = 0;
                }
                else
                {
                    distance = distanceLinkedList.Count;
                }

                if (distance == 1)
                {
                    distance = 8;
                }
                else if (distance > 1 && distance <= EnemyThor.clicks[Convert.ToInt32(clickNumberOpposingThor.Text)].RangeValue)
                {
                    distance = 6 - distance;
                }
                else
                {
                    distance = 0;
                }

                //Click threat value
                enemyClick = Convert.ToInt32(clickNumberOpposingThor.Text);

                if (enemyClick > 6)
                {
                    enemyClick = enemyClick + 3;
                }

                //Line Of Sight threat value
                lineOfSight = IsThereLineOfSight(allyCharacterLocation, enemyThorLocation);

                if (lineOfSight.lineOfSight == true && lineOfSight.isHinderance == false)
                {
                    LOS = 2;
                }
                else if (lineOfSight.lineOfSight == true && lineOfSight.isHinderance == true)
                {
                    LOS = 1;
                }
                else
                {
                    LOS = 0;
                }

                enemyThor = distance + enemyClick + LOS;
            }
            else
            {
                enemyThor = 0;
            }

            EnemyThor.threatLevel = enemyThor;

            //Calculate Iron Man's Threat number starting with distance
            if (EnemyIronMan._TotalClicks >= Convert.ToInt32(clickNumberOpposingIronman.Text))
            {
                distanceLinkedList = pathfinder.FindShortestPath(allyCharacterLocation, enemyIronManLocation, gameMap);

                if (distanceLinkedList == null)
                {
                    distance = 0;
                }
                else
                {
                    distance = distanceLinkedList.Count;
                }

                if (distance == 1)
                {
                    distance = 6;
                }
                else if (distance > 1 && distance <= EnemyIronMan.clicks[Convert.ToInt32(clickNumberOpposingIronman.Text)].RangeValue)
                {
                    distance = 7 - distance;
                }
                else
                {
                    distance = 0;
                }

                //Click threat value
                enemyClick = Convert.ToInt32(clickNumberOpposingIronman.Text);

                if (enemyClick > 5)
                {
                    enemyClick = enemyClick + 3;
                }

                //Line Of Sight threat value
                lineOfSight = IsThereLineOfSight(allyCharacterLocation, enemyIronManLocation);

                if (lineOfSight.lineOfSight == true && lineOfSight.isHinderance == false)
                {
                    LOS = 2;
                }
                else if (lineOfSight.lineOfSight == true && lineOfSight.isHinderance == true)
                {
                    LOS = 1;
                }
                else
                {
                    LOS = 0;
                }

                enemyIronMan = distance + enemyClick + LOS;
            }
            else
            {
                enemyIronMan = 0;
            }

            EnemyIronMan.threatLevel = enemyIronMan;

            //Calculate Captain America's Threat number starting with distance
            if (EnemyCaptainAmerica._TotalClicks >= Convert.ToInt32(clickNumberOpposingCap.Text))
            {
                distanceLinkedList = pathfinder.FindShortestPath(allyCharacterLocation, enemyCaptainAmericaLocation, gameMap);

                if (distanceLinkedList == null)
                {
                    distance = 0;
                }
                else
                {
                    distance = distanceLinkedList.Count;
                }

                if (distance == 1)
                {
                    distance = 7;
                }
                else if (distance > 1 && distance <= EnemyCaptainAmerica.clicks[Convert.ToInt32(clickNumberOpposingCap.Text)].RangeValue)
                {
                    distance = 5 - distance;
                }
                else
                {
                    distance = 0;
                }

                //Click threat value
                enemyClick = Convert.ToInt32(clickNumberOpposingCap.Text);

                if (enemyClick > 6)
                {
                    enemyClick = enemyClick + 3;
                }

                //Line Of Sight threat value
                lineOfSight = IsThereLineOfSight(allyCharacterLocation, enemyCaptainAmericaLocation);

                if (lineOfSight.lineOfSight == true && lineOfSight.isHinderance == false)
                {
                    LOS = 2;
                }
                else if (lineOfSight.lineOfSight == true && lineOfSight.isHinderance == true)
                {
                    LOS = 1;
                }
                else
                {
                    LOS = 0;
                }

                enemyCaptainAmerica = distance + enemyClick + LOS;
            }
            else
            {
                enemyCaptainAmerica = 0;
            }

            EnemyCaptainAmerica.threatLevel = enemyCaptainAmerica;

            if (enemyThor >= enemyIronMan && enemyThor >= enemyCaptainAmerica)
            {
                return EnemyThor;
            }
            else if (enemyIronMan > enemyThor && enemyIronMan >= enemyCaptainAmerica)
            {
                return EnemyIronMan;
            }
            else if(enemyCaptainAmerica > enemyIronMan && enemyCaptainAmerica > enemyThor)
            {
                return EnemyCaptainAmerica;
            }

            return EnemyThor;
        }

        private void DetermineCharactersTurn(Character character)
        {
            //Calculate the threat levels of enemy characters as well as returning the one with the highest threat level
            //Threat Level is stored inside of character
            Character greatestThreat = new Character();
            greatestThreat = ReturnGreatestThreat(character);

            //Calculate the Clear Tokens score
            int clearActionTokenScore = 0;

            switch (character._CharacterName)
            {
                case "Thor":
                    {
                        if (Convert.ToInt32(actionTokensThorFriendly.Text) == 2)
                        {
                            clearActionTokenScore = 100;
                        }
                        else if (Convert.ToInt32(actionTokensThorFriendly.Text) == 1)
                        {
                            clearActionTokenScore = 1;
                        }
                        else
                        {
                            clearActionTokenScore = 0;
                        }
                        break;
                    }
                case "Iron Man":
                    {
                        if (Convert.ToInt32(actionTokensIronManFriendly.Text) == 2)
                        {
                            clearActionTokenScore = 100;
                        }
                        else if (Convert.ToInt32(actionTokensIronManFriendly.Text) == 1)
                        {
                            clearActionTokenScore = 1;
                        }
                        else
                        {
                            clearActionTokenScore = 0;
                        }
                        break;
                    }
                case "Captain America":
                    {
                        if (Convert.ToInt32(actionTokensCapFriendly.Text) == 2)
                        {
                            clearActionTokenScore = 100;
                        }
                        else if (Convert.ToInt32(actionTokensCapFriendly.Text) == 1)
                        {
                            clearActionTokenScore = 1;
                        }
                        else
                        {
                            clearActionTokenScore = 0;
                        }
                        break;
                    }
            }
            
            //Calculate the Pick Up Item score and what item to pick up
            //First figure out if in possession of a weapon
            LinkedList<ItemNode> AvailableItems = new LinkedList<ItemNode>();
            ItemNode closestItem = new ItemNode();
            int pickUpItemScore = 0;

            if (character._LightWeapon == true || character._HeavyWeapon == true)
            {
                pickUpItemScore = 0;
            }
            //Select the correct individual character
            else if (character == Thor)
            {
                //Collect all non-owned items
                foreach (ItemNode item in ItemList)
                {
                    if (item.character == null)
                    {
                        AvailableItems.AddLast(item);
                    }
                }

                //If list is empty return a 0 score otherwise continue
                if (AvailableItems.Count == 0)
                {
                    pickUpItemScore = 0;
                }
                else
                {
                    //Determine the closest Item and assign appropriate score
                    closestItem = CalculatePickUpItemScore(character, AvailableItems);

                    if (closestItem.itemDistanceScore == 0)
                    {
                        pickUpItemScore = 20;
                    }
                    else if (closestItem.itemDistanceScore > 0 && closestItem.itemDistanceScore <= character.clicks[Convert.ToInt32(clickNumberFriendlyThor.Text)].SpeedValue)
                    {
                        pickUpItemScore = character.clicks[Convert.ToInt32(clickNumberFriendlyThor.Text)].SpeedValue - closestItem.itemDistanceScore;
                    }
                    else
                    {
                        pickUpItemScore = 0;
                    }
                }
            }
            else if (character == IronMan)
            {
                //Collect all non-owned items that are light
                foreach (ItemNode item in ItemList)
                {
                    if (item.character == null && item.weaponType == "Light")
                    {
                        AvailableItems.AddLast(item);
                    }
                }

                //If list is empty return a 0 score otherwise continue
                if (AvailableItems.Count == 0)
                {
                    pickUpItemScore = 0;
                }
                else
                {
                    //Determine the closest Item and assign appropriate score
                    closestItem = CalculatePickUpItemScore(character, AvailableItems);

                    if (closestItem.itemDistanceScore == 0)
                    {
                        pickUpItemScore = 20;
                    }
                    else if (closestItem.itemDistanceScore > 0 && closestItem.itemDistanceScore <= character.clicks[Convert.ToInt32(clickNumberFriendlyIronMan.Text)].SpeedValue)
                    {
                        pickUpItemScore = character.clicks[Convert.ToInt32(clickNumberFriendlyIronMan.Text)].SpeedValue - closestItem.itemDistanceScore;
                    }
                    else
                    {
                        pickUpItemScore = 0;
                    }
                }
            }
            else if (character == CaptainAmerica)
            {
                //Collect all non-owned items that are Light
                foreach (ItemNode item in ItemList)
                {
                    if (item.character == null && item.weaponType == "Light")
                    {
                        AvailableItems.AddLast(item);
                    }
                }

                //If list is empty return a 0 score otherwise continue
                if (AvailableItems.Count == 0)
                {
                    pickUpItemScore = 0;
                }
                else
                {
                    //Determine the closest Item and assign appropriate score
                    closestItem = CalculatePickUpItemScore(character, AvailableItems);

                    if (closestItem.itemDistanceScore == 0)
                    {
                        pickUpItemScore = 20;
                    }
                    else if (closestItem.itemDistanceScore > 0 && closestItem.itemDistanceScore <= character.clicks[Convert.ToInt32(clickNumberFriendlyCap.Text)].SpeedValue)
                    {
                        pickUpItemScore = character.clicks[Convert.ToInt32(clickNumberFriendlyCap.Text)].SpeedValue - closestItem.itemDistanceScore;
                    }
                    else
                    {
                        pickUpItemScore = 0;
                    }
                }
            }

            //Determine which goal to take
            if (greatestThreat.threatLevel >= pickUpItemScore && greatestThreat.threatLevel > clearActionTokenScore)
            {
                HowToAttack(character, greatestThreat);
            }
            else if (pickUpItemScore > greatestThreat.threatLevel && pickUpItemScore > clearActionTokenScore)
            {
                //Setup
                Pathfinder pathfinder = new Pathfinder();
                LinkedList<MapNode> ItemPath = new LinkedList<MapNode>();
                MapNode CharacterLocation = new MapNode();
                MapNode[,] gameMap = new MapNode[16, 16];
                MapNode TargetNode = new MapNode();
                gameMap = RetrieveMap();
                CharacterLocation = IsOccupiedList.FirstOrDefault(l => l.OccupiedByCharacter._CharacterName == character._CharacterName);

                //Determine if character can reach item
                TargetNode = FindNearestMapNodeToTargetLocation(CharacterLocation, closestItem.mapLocation, 0, 0, true, false);

                if (TargetNode != null)
                {
                    ItemPath = pathfinder.FindShortestPath(CharacterLocation, closestItem.mapLocation, gameMap, false);

                    if (ItemPath.Count > 0)
                    {
                        string movement = "";
                        int max;
                        int characterSpeedValue = 0;

                        switch (character._CharacterName)
                        {
                            case "Thor":
                                {
                                    characterSpeedValue = character.clicks[Convert.ToInt32(clickNumberFriendlyThor.Text)].SpeedValue;
                                    break;
                                }
                            case "Iron Man":
                                {
                                    characterSpeedValue = character.clicks[Convert.ToInt32(clickNumberFriendlyIronMan.Text)].SpeedValue;
                                    break;
                                }
                            case "Captain America":
                                {
                                    characterSpeedValue = character.clicks[Convert.ToInt32(clickNumberFriendlyCap.Text)].SpeedValue;
                                    break;
                                }
                        }

                        if (characterSpeedValue >= ItemPath.Count)
                        {
                            max = ItemPath.Count - 1;
                        }
                        else
                        {
                            max = characterSpeedValue;
                        }

                        for (int i = 0; i < max; i++)
                        {
                            movement = movement + "=> " + "(" + ItemPath.ElementAt(i).x.ToString() + "," + ItemPath.ElementAt(i).y.ToString() + ")";
                        }

                        MessageBox.Show("Move to " + movement, character._CharacterName);

                        if (ItemPath.ElementAt(max).x == closestItem.mapLocation.x &&
                            ItemPath.ElementAt(max).y == closestItem.mapLocation.y ||
                            (ItemPath.ElementAt(max).x + 1 == closestItem.mapLocation.x + 1 &&
                            ItemPath.ElementAt(max).y == closestItem.mapLocation.y) ||
                            (ItemPath.ElementAt(max).x - 1== closestItem.mapLocation.x - 1 &&
                            ItemPath.ElementAt(max).y == closestItem.mapLocation.y) ||
                            (ItemPath.ElementAt(max).x == closestItem.mapLocation.x &&
                            ItemPath.ElementAt(max).y + 1 == closestItem.mapLocation.y + 1) ||
                            (ItemPath.ElementAt(max).x == closestItem.mapLocation.x &&
                            ItemPath.ElementAt(max).y - 1 == closestItem.mapLocation.y - 1) ||
                            (ItemPath.ElementAt(max).x + 1 == closestItem.mapLocation.x + 1 &&
                            ItemPath.ElementAt(max).y + 1 == closestItem.mapLocation.y + 1) ||
                            (ItemPath.ElementAt(max).x - 1 == closestItem.mapLocation.x - 1 &&
                            ItemPath.ElementAt(max).y - 1 == closestItem.mapLocation.y - 1) ||
                            (ItemPath.ElementAt(max).x + 1 == closestItem.mapLocation.x + 1 &&
                            ItemPath.ElementAt(max).y - 1 == closestItem.mapLocation.y - 1) ||
                            (ItemPath.ElementAt(max).x - 1 == closestItem.mapLocation.x - 1&&
                            ItemPath.ElementAt(max).y + 1 == closestItem.mapLocation.y + 1))
                        {
                            MessageBox.Show("Pick up item at " + "(" + closestItem.mapLocation.x.ToString() + "," + closestItem.mapLocation.y.ToString() + ")", character._CharacterName);
                        }
                        return;
                    }
                }
            }
            else
            {
                //Clear the correct character token and notify of the action
                switch (character._CharacterName)
                {
                    case "Thor":
                        {
                            MessageBox.Show("Clear Action Tokens.", character._CharacterName);
                            actionTokensThorFriendly.Text = "0";
                            return;
                        }
                    case "Iron Man":
                        {
                            MessageBox.Show("Clear Action Tokens.", character._CharacterName);
                            actionTokensIronManFriendly.Text = "0";
                            return;
                        }
                    case "Captain America":
                        {
                            MessageBox.Show("Clear Action Tokens.", character._CharacterName);
                            actionTokensCapFriendly.Text = "0";
                            return;
                        }
                }

                MessageBox.Show("Clear Action Tokens manually.",character._CharacterName);
                return;
            }
        }

        private void HowToAttack(Character character, Character enemy)
        {
            //Determine distance and line of sight
            MapNode[,] gameMap = new MapNode[16, 16];
            gameMap = RetrieveMap();
            Pathfinder pathfinder = new Pathfinder();
            MapNode characterLocation = new MapNode();
            MapNode enemyLocation = new MapNode();
            LinkedList<MapNode> Distance = new LinkedList<MapNode>();
            LineOfSight lineOfSight = new LineOfSight();
            int distance;

            characterLocation = IsOccupiedList.FirstOrDefault(l => l.OccupiedByCharacter._CharacterName == character._CharacterName);
            enemyLocation = IsOccupiedList.FirstOrDefault(l => l.OccupiedByCharacter._CharacterName == enemy._CharacterName);
            Distance = pathfinder.FindShortestPath(characterLocation, enemyLocation, gameMap, false);
            distance = Distance.Count;
            lineOfSight = IsThereLineOfSight(characterLocation, enemyLocation);

            //Select the appropriate character
            switch (character._CharacterName)
            {
                case "Thor":
                    {
                        //Decide Melee attack
                        if (distance == 1 && lineOfSight.lineOfSight == true)
                        {
                            //Check abilities
                            //Dealing with Lightning Smash
                            if (Convert.ToInt32(clickNumberFriendlyThor.Text) > 6 && 
                                character.SpecialAbilityUsed == false)
                            {
                                character.SpecialAbilityUsed = true;
                                MessageBox.Show("Use Lightning Smash.",character._CharacterName);
                                return;
                            }

                            character.SpecialAbilityUsed = false;
                            MessageBox.Show("Melee attack on " + enemy._CharacterName, character._CharacterName);

                            //Increase Token Count
                            actionTokensThorFriendly.Text = (Convert.ToInt32(actionTokensThorFriendly.Text) + 1).ToString();

                            return;
                        }

                        //Decide Range attack
                        if (distance > 1 && 
                            distance < character.clicks[Convert.ToInt32(clickNumberFriendlyThor.Text)].RangeValue &&
                            lineOfSight.lineOfSight == true &&
                            EngagementStatus(character) == false)
                        {
                            //Check abilities
                            //Dealing with Energy Explosion
                            if (Convert.ToInt32(clickNumberFriendlyThor.Text) > 3 && 
                                Convert.ToInt32(clickNumberFriendlyThor.Text) < 7 &&
                                EngagementStatus(enemy) == true)
                            {
                                character.SpecialAbilityUsed = false;
                                MessageBox.Show("Use Energy Explosion on " + enemy._CharacterName, character._CharacterName);

                                //Increase Token Count
                                actionTokensThorFriendly.Text = (Convert.ToInt32(actionTokensThorFriendly.Text) + 1).ToString();

                                return;
                            }
                        }

                        //Default Move to attack
                        if (distance > 1 &&
                            EngagementStatus(character) == false)
                        {
                            //Check Abilities
                            //Dealing with Charge
                            MapNode targetMapNode = new MapNode();
                            targetMapNode = FindNearestMapNodeToTargetLocation(characterLocation, enemyLocation, 1, 1, false, true);

                            if (targetMapNode != null)
                            {
                                LinkedList<MapNode> MovementList = new LinkedList<MapNode>();
                                LinkedList<MapNode> AvoidanceListCharge = new LinkedList<MapNode>();

                                AvoidanceListCharge = FindEnemyMovementThreatZoneForCharacter(targetMapNode, true);
                                MovementList = pathfinder.FindShortestPath(characterLocation, targetMapNode, gameMap, true, AvoidanceListCharge);

                                if (MovementList.Count < 6 &&
                                    character.abilities[Convert.ToInt32(clickNumberFriendlyThor.Text)]._Charge == true)
                                {
                                    string movementCharge = "";

                                    foreach (MapNode Node in MovementList)
                                    {
                                        movementCharge = movementCharge + "=> " + "(" + ConvertColumnNumberToLetter(Node.x) + "," + (Node.y + 1).ToString() + ")";
                                    }

                                    MessageBox.Show("Use Charge on " + enemy._CharacterName + "\n" +
                                        "Move to " + movementCharge + "\n" +
                                        "Melee attack on " + enemy._CharacterName, character._CharacterName);

                                    //Update Thor's Location
                                    locationAlphaFriendlyThorCB.Text = ConvertColumnNumberToLetter(MovementList.Last.Value.x);
                                    locationNumericFriendlyThorCB.Text = (MovementList.Last.Value.y + 1).ToString();
                                    UpdateCharacterLocations();

                                    //Increase Token Count
                                    actionTokensThorFriendly.Text = (Convert.ToInt32(actionTokensThorFriendly.Text) + 1).ToString();

                                    character.SpecialAbilityUsed = false;
                                    return;
                                }
                            }

                            //Dealing with Running Shot
                            LinkedList<MapNode> Movement = new LinkedList<MapNode>();
                            MapNode movementCheck = new MapNode();

                            targetMapNode = FindNearestMapNodeToTargetLocation(characterLocation, enemyLocation, 2, 4, false, true);

                            if (targetMapNode != null)
                            {
                                LinkedList<MapNode> AvoidanceListRunningShot = new LinkedList<MapNode>();

                                AvoidanceListRunningShot = FindEnemyMovementThreatZoneForCharacter(targetMapNode, true);
                                Movement = pathfinder.FindShortestPath(characterLocation, targetMapNode, gameMap, true, AvoidanceListRunningShot);
                                
                                if (Movement != null)
                                {
                                    if (Movement.Count <= 4 &&
                                        character.abilities[Convert.ToInt32(clickNumberFriendlyThor.Text)]._RunningShot == true)
                                    {
                                        string movementRunningShot = "";

                                        foreach (MapNode Node in Movement)
                                        {
                                            movementRunningShot = movementRunningShot + "=> " + "(" + ConvertColumnNumberToLetter(Node.x) + "," + (Node.y +1).ToString() + ")";
                                        }

                                        MessageBox.Show("Use Running Shot on " + enemy._CharacterName + "\n" +
                                            "Move to " + movementRunningShot + "\n" +
                                            "Range attack on " + enemy._CharacterName, character._CharacterName);

                                        //Update Thor's Location
                                        locationAlphaFriendlyThorCB.Text = ConvertColumnNumberToLetter(Movement.Last.Value.x);
                                        locationNumericFriendlyThorCB.Text = (Movement.Last.Value.y + 1).ToString();
                                        UpdateCharacterLocations();

                                        //Increase Token Count
                                        actionTokensThorFriendly.Text = (Convert.ToInt32(actionTokensThorFriendly.Text) + 1).ToString();

                                        character.SpecialAbilityUsed = false;
                                        return;
                                    }
                                }
                            }

                            //Dealing with Sidestep
                            if (distance < 3 && 
                                character.abilities[Convert.ToInt32(clickNumberFriendlyThor.Text)]._SideStep == true)
                            {
                                targetMapNode = FindNearestMapNodeToTargetLocation(characterLocation, enemyLocation, 1, 1, false, false);

                                if (targetMapNode != null)
                                {
                                    LinkedList<MapNode> AvoidanceListSideStep = new LinkedList<MapNode>();

                                    AvoidanceListSideStep = FindEnemyMovementThreatZoneForCharacter(targetMapNode, true);
                                    Movement = pathfinder.FindShortestPath(characterLocation, targetMapNode, gameMap, true, AvoidanceListSideStep);

                                    string MovementSideStep = "";

                                    foreach (MapNode Node in Movement)
                                    {
                                        MovementSideStep = MovementSideStep + "=> " + "(" + ConvertColumnNumberToLetter(Node.x) + "," + (Node.y + 1).ToString() + ")";
                                    }

                                    MessageBox.Show("Use SideStep\n" +
                                        "Move to " + MovementSideStep + "\n" +
                                        "Melee attack on " + enemy._CharacterName, character._CharacterName);

                                    //Update Thor's Location
                                    locationAlphaFriendlyThorCB.Text = ConvertColumnNumberToLetter(Movement.Last.Value.x);
                                    locationNumericFriendlyThorCB.Text = (Movement.Last.Value.y + 1).ToString();
                                    UpdateCharacterLocations();

                                    //Increase Token Count
                                    actionTokensThorFriendly.Text = (Convert.ToInt32(actionTokensThorFriendly.Text) + 1).ToString();

                                    character.SpecialAbilityUsed = false;
                                    return;
                                }
                            }

                            //Moving normally
                            targetMapNode = FindNearestMapNodeToTargetLocation(characterLocation, enemyLocation, 1, 1, false, true);

                            if (targetMapNode == null)
                            {
                                targetMapNode = FindNearestMapNodeToTargetLocation(characterLocation, enemyLocation, 1, 3, false, false);
                            }

                            LinkedList<MapNode> AvoidanceList = new LinkedList<MapNode>();

                            AvoidanceList = FindEnemyMovementThreatZoneForCharacter(targetMapNode, true);
                            Movement = pathfinder.FindShortestPath(characterLocation, targetMapNode, gameMap, true, AvoidanceList);
                            string movement = "";
                            int max;

                            if (character.clicks[Convert.ToInt32(clickNumberFriendlyThor.Text)].SpeedValue >= Movement.Count)
                            {
                                max = Movement.Count - 1;
                            }
                            else
                            {
                                max = character.clicks[Convert.ToInt32(clickNumberFriendlyThor.Text)].SpeedValue;
                            }

                            //Check to make sure the new ending spot is not Occupied, if not go back one MapNode and recheck
                            for (int i = max - 1; i > -1; i--)
                            {
                                if (Movement.ElementAt(i).IsOccupied == false)
                                {
                                    break;
                                }
                                max = max - 1;
                            }

                            //Prepare movement string for Message Box
                            for (int i = 0; i < max; i++)
                            {
                                movement = movement + "=> " + "(" + ConvertColumnNumberToLetter(Movement.ElementAt(i).x) + "," + (Movement.ElementAt(i).y + 1).ToString() + ")";
                            }

                            MessageBox.Show("Move to " + movement, character._CharacterName);

                            //Update Thor's Location
                            locationAlphaFriendlyThorCB.Text = ConvertColumnNumberToLetter(Movement.ElementAt(max - 1).x);
                            locationNumericFriendlyThorCB.Text = (Movement.ElementAt(max - 1).y + 1).ToString();
                            UpdateCharacterLocations();

                            //Increase Token Count
                            actionTokensThorFriendly.Text = (Convert.ToInt32(actionTokensThorFriendly.Text) + 1).ToString();

                            character.SpecialAbilityUsed = false;
                            return;
                        }
                        break;
                    }

                case "Iron Man":
                    {
                        //Decide Melee attack
                        if (distance == 1 && lineOfSight.lineOfSight == true)
                        {
                            //Check abilities
                            //Melee
                            MessageBox.Show("Melee attack on " + enemy._CharacterName, character._CharacterName);
                            
                            //Increase Token Count
                            actionTokensIronManFriendly.Text = (Convert.ToInt32(actionTokensIronManFriendly.Text) + 1).ToString();

                            return;
                        }

                        //Decide Range attack
                        if (distance > 1 &&
                            distance < character.clicks[Convert.ToInt32(clickNumberFriendlyIronMan.Text)].RangeValue &&
                            lineOfSight.lineOfSight == true)
                        {
                            //Check abilities
                            //Dealing with Energy Explosion
                            if (Convert.ToInt32(clickNumberFriendlyIronMan.Text) < 4 &&
                                EngagementStatus(enemy) == true)
                            {
                                MessageBox.Show("Use Energy Explosion on " + enemy._CharacterName, character._CharacterName);
                                
                                //Increase Token Count
                                actionTokensIronManFriendly.Text = (Convert.ToInt32(actionTokensIronManFriendly.Text) + 1).ToString();

                                return;
                            }

                            //See if Combat Expert is active
                            if (Convert.ToInt32(clickNumberFriendlyIronMan.Text) > 3)
                            {
                                //See what enemy it is and select the appropriate response
                                switch (enemy._CharacterName)
                                {
                                    case "Enemy Thor":
                                        {
                                            if (Convert.ToInt32(clickNumberOpposingThor.Text) < 7)
                                            {
                                                MessageBox.Show("Use Ranged Combat Expert\n" +
                                                    "Attack + 1 and Damage + 1\n" +
                                                    "Range attack on " + enemy._CharacterName, character._CharacterName);

                                                //Increase Token Count
                                                actionTokensIronManFriendly.Text = (Convert.ToInt32(actionTokensIronManFriendly.Text) + 1).ToString();

                                                return;
                                            }

                                            if (Convert.ToInt32(clickNumberOpposingThor.Text) > 6)
                                            {
                                                MessageBox.Show("Use Ranged Combat Expert\n" +
                                                    "Attack + 2\n" +
                                                    "Range attack on " + enemy._CharacterName, character._CharacterName);
                                                
                                                //Increase Token Count
                                                actionTokensIronManFriendly.Text = (Convert.ToInt32(actionTokensIronManFriendly.Text) + 1).ToString();

                                                return;
                                            }

                                            //Catch all
                                            MessageBox.Show("Use Ranged Combat Expert\n" +
                                                "Attack + 1 and Damage + 1\n" +
                                                "Range Attack on " + enemy._CharacterName, character._CharacterName);
                                            
                                            //Increase Token Count
                                            actionTokensIronManFriendly.Text = (Convert.ToInt32(actionTokensIronManFriendly.Text) + 1).ToString();

                                            return;
                                        }

                                    case "Enemy Iron Man":
                                        {
                                            if (Convert.ToInt32(clickNumberOpposingIronman.Text) < 4)
                                            {
                                                MessageBox.Show("Use Ranged Combat Expert\n" +
                                                    "Attack + 1 and + 1 Damage\n" +
                                                    "Range Attack on " + enemy._CharacterName, character._CharacterName);
                                                
                                                //Increase Token Count
                                                actionTokensIronManFriendly.Text = (Convert.ToInt32(actionTokensIronManFriendly.Text) + 1).ToString();

                                                return;
                                            }

                                            LineOfSight HinderanceCheck = new LineOfSight();
                                            HinderanceCheck = IsThereLineOfSight(characterLocation, enemyLocation);

                                            if (Convert.ToInt32(clickNumberOpposingIronman.Text) > 3 &&
                                                HinderanceCheck.isHinderance == true)
                                            {
                                                MessageBox.Show("Use Ranged Combat Expert\n" +
                                                    "Attack + 1 and + 1 Damage\n" +
                                                    "Range Attack on " + enemy._CharacterName, character._CharacterName);
                                                
                                                //Increase Token Count
                                                actionTokensIronManFriendly.Text = (Convert.ToInt32(actionTokensIronManFriendly.Text) + 1).ToString();

                                                return;
                                            }

                                            if (Convert.ToInt32(clickNumberOpposingIronman.Text) > 3 &&
                                                Convert.ToInt32(clickNumberOpposingIronman.Text) < 6 &&
                                                HinderanceCheck.isHinderance == false)
                                            {
                                                MessageBox.Show("Use Ranged Combat Expert\n" +
                                                    "Attack + 1 and Damage + 1\n" +
                                                    "Range Attack on " + enemy._CharacterName, character._CharacterName);
                                                
                                                //Increase Token Count
                                                actionTokensIronManFriendly.Text = (Convert.ToInt32(actionTokensIronManFriendly.Text) + 1).ToString();

                                                return;
                                            }

                                            if (Convert.ToInt32(clickNumberOpposingIronman.Text) == 6 &&
                                                HinderanceCheck.isHinderance == false)
                                            {
                                                MessageBox.Show("Use Ranged Combat Expert\n" +
                                                    "Damage + 2\n" +
                                                    "Range Attack on " + enemy._CharacterName, character._CharacterName);
                                                
                                                //Increase Token Count
                                                actionTokensIronManFriendly.Text = (Convert.ToInt32(actionTokensIronManFriendly.Text) + 1).ToString();

                                                return;
                                            }

                                            if (Convert.ToInt32(clickNumberOpposingIronman.Text) == 7 &&
                                                HinderanceCheck.isHinderance == false)
                                            {
                                                MessageBox.Show("Use Ranged Combat Expert\n" +
                                                    "Attack + 2\n" +
                                                    "Range Attack on " + enemy._CharacterName, character._CharacterName);

                                                //Increase Token Count
                                                actionTokensIronManFriendly.Text = (Convert.ToInt32(actionTokensIronManFriendly.Text) + 1).ToString();

                                                return;
                                            }

                                            //This is a catch all just in case
                                            MessageBox.Show("Use Ranged Combat Expert\n" +
                                                "Attack + 1 and Damage + 1\n" +
                                                "Range Attack on " + enemy._CharacterName, character._CharacterName);
                                            
                                            //Increase Token Count
                                            actionTokensIronManFriendly.Text = (Convert.ToInt32(actionTokensIronManFriendly.Text) + 1).ToString();

                                            return;
                                        }

                                    case "Enemy Captain America":
                                        {
                                            if (Convert.ToInt32(clickNumberOpposingCap.Text) < 4)
                                            {
                                                MessageBox.Show("Use Ranged Combat Expert\n" +
                                                    "Attack + 2\n" +
                                                    "Range Attack on " + enemy._CharacterName, character._CharacterName);
                                                
                                                //Increase Token Count
                                                actionTokensIronManFriendly.Text = (Convert.ToInt32(actionTokensIronManFriendly.Text) + 1).ToString();

                                                return;
                                            }

                                            LineOfSight HinderanceCheck = new LineOfSight();
                                            HinderanceCheck = IsThereLineOfSight(characterLocation, enemyLocation);

                                            if (Convert.ToInt32(clickNumberOpposingCap.Text) > 3 &&
                                                HinderanceCheck.isHinderance == true)
                                            {
                                                MessageBox.Show("Use Ranged Combat Expert\n" +
                                                    "Attack + 2\n" +
                                                    "Range Attack on " + enemy._CharacterName, character._CharacterName);
                                                
                                                //Increase Token Count
                                                actionTokensIronManFriendly.Text = (Convert.ToInt32(actionTokensIronManFriendly.Text) + 1).ToString();

                                                return;
                                            }

                                            if (Convert.ToInt32(clickNumberOpposingCap.Text) > 3 &&
                                                Convert.ToInt32(clickNumberOpposingCap.Text) < 6 &&
                                                HinderanceCheck.isHinderance == false)
                                            {
                                                MessageBox.Show("Use Ranged Combat Expert\n" +
                                                    "Attack + 1 and Damage + 1\n" +
                                                    "Range Attack on " + enemy._CharacterName, character._CharacterName);
                                                
                                                //Increase Token Count
                                                actionTokensIronManFriendly.Text = (Convert.ToInt32(actionTokensIronManFriendly.Text) + 1).ToString();

                                                return;
                                            }

                                            //Catch all
                                            MessageBox.Show("Use Ranged Combat Expert\n" +
                                                "Attack + 2\n" +
                                                "Range Attack on " + enemy._CharacterName, character._CharacterName);
                                            
                                            //Increase Token Count
                                            actionTokensIronManFriendly.Text = (Convert.ToInt32(actionTokensIronManFriendly.Text) + 1).ToString();

                                            return;
                                        }
                                }
                            }

                            //Normal Ranged Attack
                            MessageBox.Show("Range attack on " + enemy._CharacterName, character._CharacterName);
                            
                            //Increase Token Count
                            actionTokensIronManFriendly.Text = (Convert.ToInt32(actionTokensIronManFriendly.Text) + 1).ToString();

                            return;
                        }

                        //Default Move to attack
                        if (distance > 1 &&
                            EngagementStatus(character) == false)
                        {
                            //Check Abilities
                            //Dealing with Running Shot
                            MapNode targetMapNode = new MapNode();
                            LinkedList<MapNode> Movement = new LinkedList<MapNode>();
                            MapNode movementCheck = new MapNode();

                            targetMapNode = FindNearestMapNodeToTargetLocation(characterLocation, enemyLocation, 3, 6, false, true);

                            if (targetMapNode != null)
                            {
                                LinkedList<MapNode> AvoidanceListRunningShot = new LinkedList<MapNode>();

                                AvoidanceListRunningShot = FindEnemyMovementThreatZoneForCharacter(targetMapNode, true);
                                Movement = pathfinder.FindShortestPath(characterLocation, targetMapNode, gameMap, true, AvoidanceListRunningShot);

                                if (Movement != null)
                                {
                                    if (Movement.Count <= 4 &&
                                        character.abilities[Convert.ToInt32(clickNumberFriendlyIronMan.Text)]._RunningShot == true)
                                    {
                                        string movementRunningShot = "";

                                        foreach (MapNode Node in Movement)
                                        {
                                            movementRunningShot = movementRunningShot + "=> " + "(" + ConvertColumnNumberToLetter(Node.x) + "," + (Node.y + 1).ToString() + ")";
                                        }

                                        MessageBox.Show("Use Running Shot on " + enemy._CharacterName + "\n" +
                                            "Move to " + movementRunningShot + "\n" +
                                            "Range attack on " + enemy._CharacterName, character._CharacterName);

                                        //Update Character's Location
                                        locationAlphaFriendlyIronManCB.Text = ConvertColumnNumberToLetter(Movement.Last.Value.x);
                                        locationNumericFriendlyIronManCB.Text = (Movement.Last.Value.y + 1).ToString();
                                        UpdateCharacterLocations();

                                        //Increase Token Count
                                        actionTokensIronManFriendly.Text = (Convert.ToInt32(actionTokensIronManFriendly.Text) + 1).ToString();

                                        return;
                                    }
                                }
                            }

                            //Dealing with Sidestep
                            if (character.abilities[Convert.ToInt32(clickNumberFriendlyIronMan.Text)]._SideStep == true)
                            {
                                targetMapNode = FindNearestMapNodeToTargetLocation(characterLocation, enemyLocation, 2, 3, false, true);

                                if (targetMapNode != null)
                                {
                                    LinkedList<MapNode> AvoidanceListSideStep = new LinkedList<MapNode>();

                                    AvoidanceListSideStep = FindEnemyMovementThreatZoneForCharacter(targetMapNode, true);
                                    Movement = pathfinder.FindShortestPath(characterLocation, targetMapNode, gameMap, true, AvoidanceListSideStep);

                                    if (Movement.Count < 3)
                                    {
                                        string MovementSideStep = "";

                                        foreach (MapNode Node in Movement)
                                        {
                                            MovementSideStep = MovementSideStep + "=> " + "(" + ConvertColumnNumberToLetter(Node.x) + "," + (Node.y + 1).ToString() + ")";
                                        }

                                        MessageBox.Show("Use SideStep\n" +
                                            "Move to " + MovementSideStep + "\n" +
                                            "Click OK to continue", character._CharacterName);

                                        //Update character's Location
                                        locationAlphaFriendlyIronManCB.Text = ConvertColumnNumberToLetter(Movement.Last.Value.x);
                                        locationNumericFriendlyIronManCB.Text = (Movement.Last.Value.y + 1).ToString();
                                        UpdateCharacterLocations();

                                        //See if enemy is Thor and what click he is on
                                        switch (enemy._CharacterName)
                                        {
                                            case "Enemy Thor":
                                                {
                                                    if (Convert.ToInt32(clickNumberOpposingThor.Text) < 7)
                                                    {
                                                        MessageBox.Show("Use Ranged Combat Expert\n" +
                                                            "Attack + 1 and Damage + 1\n" +
                                                            "Range attack on " + enemy._CharacterName, character._CharacterName);

                                                        //Increase Token Count
                                                        actionTokensIronManFriendly.Text = (Convert.ToInt32(actionTokensIronManFriendly.Text) + 1).ToString();

                                                        return;
                                                    }

                                                    if (Convert.ToInt32(clickNumberOpposingThor.Text) > 6)
                                                    {
                                                        MessageBox.Show("Use Ranged Combat Expert\n" +
                                                            "Attack + 2\n" +
                                                            "Range attack on " + enemy._CharacterName, character._CharacterName);

                                                        //Increase Token Count
                                                        actionTokensIronManFriendly.Text = (Convert.ToInt32(actionTokensIronManFriendly.Text) + 1).ToString();

                                                        return;
                                                    }

                                                    //Catch all
                                                    MessageBox.Show("Use Ranged Combat Expert\n" +
                                                        "Attack + 1 and Damage + 1\n" +
                                                        "Range Attack on " + enemy._CharacterName, character._CharacterName);

                                                    //Increase Token Count
                                                    actionTokensIronManFriendly.Text = (Convert.ToInt32(actionTokensIronManFriendly.Text) + 1).ToString();

                                                    return;
                                                }

                                            case "Enemy Iron Man":
                                                {
                                                    if (Convert.ToInt32(clickNumberOpposingIronman.Text) < 4)
                                                    {
                                                        MessageBox.Show("Use Ranged Combat Expert\n" +
                                                            "Attack + 2\n" +
                                                            "Range Attack on " + enemy._CharacterName, character._CharacterName);

                                                        //Increase Token Count
                                                        actionTokensIronManFriendly.Text = (Convert.ToInt32(actionTokensIronManFriendly.Text) + 1).ToString();

                                                        return;
                                                    }

                                                    LineOfSight HinderanceCheck = new LineOfSight();
                                                    HinderanceCheck = IsThereLineOfSight(characterLocation, enemyLocation);

                                                    if (Convert.ToInt32(clickNumberOpposingIronman.Text) > 3 &&
                                                        HinderanceCheck.isHinderance == true)
                                                    {
                                                        MessageBox.Show("Use Ranged Combat Expert\n" +
                                                            "Attack + 1 and Damage + 1\n" +
                                                            "Range Attack on " + enemy._CharacterName, character._CharacterName);

                                                        //Increase Token Count
                                                        actionTokensIronManFriendly.Text = (Convert.ToInt32(actionTokensIronManFriendly.Text) + 1).ToString();

                                                        return;
                                                    }

                                                    if (Convert.ToInt32(clickNumberOpposingIronman.Text) > 3 &&
                                                        Convert.ToInt32(clickNumberOpposingIronman.Text) < 6 &&
                                                        HinderanceCheck.isHinderance == false)
                                                    {
                                                        MessageBox.Show("Use Ranged Combat Expert\n" +
                                                            "Attack + 1 and Damage + 1\n" +
                                                            "Range Attack on " + enemy._CharacterName, character._CharacterName);

                                                        //Increase Token Count
                                                        actionTokensIronManFriendly.Text = (Convert.ToInt32(actionTokensIronManFriendly.Text) + 1).ToString();

                                                        return;
                                                    }

                                                    if (Convert.ToInt32(clickNumberOpposingIronman.Text) == 6 &&
                                                        HinderanceCheck.isHinderance == false)
                                                    {
                                                        MessageBox.Show("Use Ranged Combat Expert\n" +
                                                            "Damage + 2\n" +
                                                            "Range Attack on " + enemy._CharacterName, character._CharacterName);

                                                        //Increase Token Count
                                                        actionTokensIronManFriendly.Text = (Convert.ToInt32(actionTokensIronManFriendly.Text) + 1).ToString();

                                                        return;
                                                    }

                                                    if (Convert.ToInt32(clickNumberOpposingIronman.Text) == 7 &&
                                                        HinderanceCheck.isHinderance == false)
                                                    {
                                                        MessageBox.Show("Use Ranged Combat Expert\n" +
                                                            "Attack + 2\n" +
                                                            "Range Attack on " + enemy._CharacterName, character._CharacterName);

                                                        //Increase Token Count
                                                        actionTokensIronManFriendly.Text = (Convert.ToInt32(actionTokensIronManFriendly.Text) + 1).ToString();

                                                        return;
                                                    }

                                                    //This is a catch all just in case
                                                    MessageBox.Show("Use Ranged Combat Expert\n" +
                                                        "Attack + 1 and Damage + 1\n" +
                                                        "Range Attack on " + enemy._CharacterName, character._CharacterName);

                                                    //Increase Token Count
                                                    actionTokensIronManFriendly.Text = (Convert.ToInt32(actionTokensIronManFriendly.Text) + 1).ToString();

                                                    return;
                                                }

                                            case "Enemy Captain America":
                                                {
                                                    if (Convert.ToInt32(clickNumberOpposingCap.Text) < 4)
                                                    {
                                                        MessageBox.Show("Use Ranged Combat Expert\n" +
                                                            "Attack + 2\n" +
                                                            "Range Attack on " + enemy._CharacterName, character._CharacterName);

                                                        //Increase Token Count
                                                        actionTokensIronManFriendly.Text = (Convert.ToInt32(actionTokensIronManFriendly.Text) + 1).ToString();

                                                        return;
                                                    }

                                                    LineOfSight HinderanceCheck = new LineOfSight();
                                                    HinderanceCheck = IsThereLineOfSight(characterLocation, enemyLocation);

                                                    if (Convert.ToInt32(clickNumberOpposingCap.Text) > 3 &&
                                                        HinderanceCheck.isHinderance == true)
                                                    {
                                                        MessageBox.Show("Use Ranged Combat Expert\n" +
                                                            "Attack + 2\n" +
                                                            "Range Attack on " + enemy._CharacterName, character._CharacterName);

                                                        //Increase Token Count
                                                        actionTokensIronManFriendly.Text = (Convert.ToInt32(actionTokensIronManFriendly.Text) + 1).ToString();

                                                        return;
                                                    }

                                                    if (Convert.ToInt32(clickNumberOpposingCap.Text) > 3 &&
                                                        Convert.ToInt32(clickNumberOpposingCap.Text) < 6 &&
                                                        HinderanceCheck.isHinderance == false)
                                                    {
                                                        MessageBox.Show("Use Ranged Combat Expert\n" +
                                                            "Attack + 1 and Damage + 1\n" +
                                                            "Range Attack on " + enemy._CharacterName, character._CharacterName);

                                                        //Increase Token Count
                                                        actionTokensIronManFriendly.Text = (Convert.ToInt32(actionTokensIronManFriendly.Text) + 1).ToString();

                                                        return;
                                                    }

                                                    //Catch all
                                                    MessageBox.Show("Use Ranged Combat Expert\n" +
                                                        "Attack + 2\n" +
                                                        "Range Attack on " + enemy._CharacterName, character._CharacterName);

                                                    //Increase Token Count
                                                    actionTokensIronManFriendly.Text = (Convert.ToInt32(actionTokensIronManFriendly.Text) + 1).ToString();

                                                    return;
                                                }
                                        }
                                    }
                                }
                            }

                            //Moving normally
                            //First check for an optimal position then any nearby position
                            targetMapNode = FindNearestMapNodeToTargetLocation(characterLocation, enemyLocation, 3, 6, false, true);

                            if (targetMapNode == null)
                            {
                                targetMapNode = FindNearestMapNodeToTargetLocation(characterLocation, enemyLocation, 2, 2, false, true);
                            }

                            if (targetMapNode == null)
                            {
                                targetMapNode = FindNearestMapNodeToTargetLocation(characterLocation, enemyLocation, 2, 3, false, false);
                            }

                            LinkedList<MapNode> AvoidanceList = new LinkedList<MapNode>();

                            AvoidanceList = FindEnemyMovementThreatZoneForCharacter(targetMapNode, true);
                            Movement = pathfinder.FindShortestPath(characterLocation, targetMapNode, gameMap, true, AvoidanceList);
                            string movement = "";
                            int max;

                            if (character.clicks[Convert.ToInt32(clickNumberFriendlyIronMan.Text)].SpeedValue >= Movement.Count)
                            {
                                max = Movement.Count - 1;
                            }
                            else
                            {
                                max = character.clicks[Convert.ToInt32(clickNumberFriendlyIronMan.Text)].SpeedValue;
                            }

                            //Check to make sure the new ending spot is not Occupied, if not go back one MapNode and recheck
                            for (int i = max - 1; i > -1; i--)
                            {
                                if (Movement.ElementAt(i).IsOccupied == false)
                                {
                                    break;
                                }
                                max = max - 1;
                            }

                            //Prepare movement string for Message Box
                            for (int i = 0; i < max; i++)
                            {
                                movement = movement + "=> " + "(" + ConvertColumnNumberToLetter(Movement.ElementAt(i).x) + "," + (Movement.ElementAt(i).y + 1).ToString() + ")";
                            }

                            MessageBox.Show("Move to " + movement, character._CharacterName);

                            //Update Character's Location
                            locationAlphaFriendlyIronManCB.Text = ConvertColumnNumberToLetter(Movement.ElementAt(max - 1).x);
                            locationNumericFriendlyIronManCB.Text = (Movement.ElementAt(max - 1).y + 1).ToString();
                            UpdateCharacterLocations();

                            //Increase Token Count
                            actionTokensIronManFriendly.Text = (Convert.ToInt32(actionTokensIronManFriendly.Text) + 1).ToString();

                            return;
                        }
                        break;
                    }

                case "Captain America":
                    {
                        {
                            //Decide Melee attack
                            if (distance == 1 && lineOfSight.lineOfSight == true)
                            {
                                //Check abilities
                                //Decide on Close Combat Expert
                                if (Convert.ToInt32(clickNumberFriendlyCap.Text) > 3)
                                {
                                    //Select the appropriate method of exterminating ones foe
                                    switch (enemy._CharacterName)
                                    {
                                        case "Enemy Thor":
                                            {
                                                if (Convert.ToInt32(clickNumberOpposingThor.Text) < 7)
                                                {
                                                    MessageBox.Show("Use Close Combat Expert\n" +
                                                        "Attack + 1 and Damage + 1\n" +
                                                        "Melee attack on " + enemy._CharacterName, character._CharacterName);

                                                    //Increase Token Count
                                                    actionTokensCapFriendly.Text = (Convert.ToInt32(actionTokensCapFriendly.Text) + 1).ToString();

                                                    return;
                                                }

                                                if (Convert.ToInt32(clickNumberOpposingThor.Text) > 6)
                                                {
                                                    MessageBox.Show("Use Close Combat Expert\n" +
                                                        "Attack + 2\n" +
                                                        "Melee attack on " + enemy._CharacterName, character._CharacterName);

                                                    //Increase Token Count
                                                    actionTokensCapFriendly.Text = (Convert.ToInt32(actionTokensCapFriendly.Text) + 1).ToString();

                                                    return;
                                                }

                                                //Catch all
                                                MessageBox.Show("Use Close Combat Expert\n" +
                                                    "Attack + 1 and Damage + 1\n" +
                                                    "Melee Attack on " + enemy._CharacterName, character._CharacterName);

                                                //Increase Token Count
                                                actionTokensCapFriendly.Text = (Convert.ToInt32(actionTokensCapFriendly.Text) + 1).ToString();

                                                return;
                                            }

                                        case "Enemy Iron Man":
                                            {
                                                if (Convert.ToInt32(clickNumberOpposingIronman.Text) < 4)
                                                {
                                                    MessageBox.Show("Use Close Combat Expert\n" +
                                                        "Attack + 2\n" +
                                                        "Melee Attack on " + enemy._CharacterName, character._CharacterName);

                                                    //Increase Token Count
                                                    actionTokensCapFriendly.Text = (Convert.ToInt32(actionTokensCapFriendly.Text) + 1).ToString();

                                                    return;
                                                }

                                                LineOfSight HinderanceCheck = new LineOfSight();
                                                HinderanceCheck = IsThereLineOfSight(characterLocation, enemyLocation);

                                                if (Convert.ToInt32(clickNumberOpposingIronman.Text) > 3 &&
                                                    HinderanceCheck.isHinderance == true)
                                                {
                                                    MessageBox.Show("Use Close Combat Expert\n" +
                                                        "Attack + 2\n" +
                                                        "Melee Attack on " + enemy._CharacterName, character._CharacterName);

                                                    //Increase Token Count
                                                    actionTokensCapFriendly.Text = (Convert.ToInt32(actionTokensCapFriendly.Text) + 1).ToString();

                                                    return;
                                                }

                                                if (Convert.ToInt32(clickNumberOpposingIronman.Text) > 3 &&
                                                    Convert.ToInt32(clickNumberOpposingIronman.Text) < 6 &&
                                                    HinderanceCheck.isHinderance == false)
                                                {
                                                    MessageBox.Show("Use Close Combat Expert\n" +
                                                        "Attack + 1 and Damage + 1\n" +
                                                        "Melee Attack on " + enemy._CharacterName, character._CharacterName);

                                                    //Increase Token Count
                                                    actionTokensCapFriendly.Text = (Convert.ToInt32(actionTokensCapFriendly.Text) + 1).ToString();

                                                    return;
                                                }

                                                if (Convert.ToInt32(clickNumberOpposingIronman.Text) == 6 &&
                                                    HinderanceCheck.isHinderance == false)
                                                {
                                                    MessageBox.Show("Use Close Combat Expert\n" +
                                                        "Damage + 2\n" +
                                                        "Melee Attack on " + enemy._CharacterName, character._CharacterName);

                                                    //Increase Token Count
                                                    actionTokensCapFriendly.Text = (Convert.ToInt32(actionTokensCapFriendly.Text) + 1).ToString();

                                                    return;
                                                }

                                                if (Convert.ToInt32(clickNumberOpposingIronman.Text) == 7 &&
                                                    HinderanceCheck.isHinderance == false)
                                                {
                                                    MessageBox.Show("Use Close Combat Expert\n" +
                                                        "Attack + 2\n" +
                                                        "Melee Attack on " + enemy._CharacterName, character._CharacterName);

                                                    //Increase Token Count
                                                    actionTokensCapFriendly.Text = (Convert.ToInt32(actionTokensCapFriendly.Text) + 1).ToString();

                                                    return;
                                                }

                                                //This is a catch all just in case
                                                MessageBox.Show("Use Close Combat Expert\n" +
                                                    "Attack + 1 and Damage + 1\n" +
                                                    "Melee Attack on " + enemy._CharacterName, character._CharacterName);

                                                //Increase Token Count
                                                actionTokensCapFriendly.Text = (Convert.ToInt32(actionTokensCapFriendly.Text) + 1).ToString();

                                                return;
                                            }

                                        case "Enemy Captain America":
                                            {
                                                if (Convert.ToInt32(clickNumberOpposingCap.Text) < 4)
                                                {
                                                    MessageBox.Show("Use Close Combat Expert\n" +
                                                        "Attack + 2\n" +
                                                        "Melee Attack on " + enemy._CharacterName, character._CharacterName);

                                                    //Increase Token Count
                                                    actionTokensCapFriendly.Text = (Convert.ToInt32(actionTokensCapFriendly.Text) + 1).ToString();

                                                    return;
                                                }

                                                LineOfSight HinderanceCheck = new LineOfSight();
                                                HinderanceCheck = IsThereLineOfSight(characterLocation, enemyLocation);

                                                if (Convert.ToInt32(clickNumberOpposingCap.Text) > 3 &&
                                                    HinderanceCheck.isHinderance == true)
                                                {
                                                    MessageBox.Show("Use Close Combat Expert\n" +
                                                        "Attack + 2\n" +
                                                        "Melee Attack on " + enemy._CharacterName, character._CharacterName);

                                                    //Increase Token Count
                                                    actionTokensCapFriendly.Text = (Convert.ToInt32(actionTokensCapFriendly.Text) + 1).ToString();

                                                    return;
                                                }

                                                if (Convert.ToInt32(clickNumberOpposingCap.Text) > 3 &&
                                                    Convert.ToInt32(clickNumberOpposingCap.Text) < 6 &&
                                                    HinderanceCheck.isHinderance == false)
                                                {
                                                    MessageBox.Show("Use Close Combat Expert\n" +
                                                        "Attack + 1 and Damage + 1\n" +
                                                        "Melee Attack on " + enemy._CharacterName, character._CharacterName);

                                                    //Increase Token Count
                                                    actionTokensCapFriendly.Text = (Convert.ToInt32(actionTokensCapFriendly.Text) + 1).ToString();

                                                    return;
                                                }

                                                //Catch all
                                                MessageBox.Show("Use Close Combat Expert\n" +
                                                    "Attack + 1 and Defense + 1\n" +
                                                    "Melee Attack on " + enemy._CharacterName, character._CharacterName);

                                                //Increase Token Count
                                                actionTokensCapFriendly.Text = (Convert.ToInt32(actionTokensCapFriendly.Text) + 1).ToString();

                                                return;
                                            }
                                    }
                                }

                                character.SpecialAbilityUsed = false;
                                MessageBox.Show("Melee attack on " + enemy._CharacterName, character._CharacterName);

                                //Increase Token Count
                                actionTokensCapFriendly.Text = (Convert.ToInt32(actionTokensCapFriendly.Text) + 1).ToString();

                                return;
                            }

                            //Decide Range attack
                            if (distance > 1 &&
                                distance < character.clicks[Convert.ToInt32(clickNumberFriendlyCap.Text)].RangeValue &&
                                lineOfSight.lineOfSight == true &&
                                EngagementStatus(character) == false)
                            {
                                //Check abilities
                            }

                            //Default Move to attack
                            if (distance > 1 &&
                                EngagementStatus(character) == false)
                            {
                                //Check Abilities
                                //Dealing with Charge
                                MapNode targetMapNode = new MapNode();
                                targetMapNode = FindNearestMapNodeToTargetLocation(characterLocation, enemyLocation, 1, 1, false, true);

                                if (targetMapNode != null)
                                {
                                    LinkedList<MapNode> MovementList = new LinkedList<MapNode>();
                                    LinkedList<MapNode> AvoidanceListCharge = new LinkedList<MapNode>();

                                    AvoidanceListCharge = FindEnemyMovementThreatZoneForCharacter(targetMapNode, false);
                                    MovementList = pathfinder.FindShortestPath(characterLocation, targetMapNode, gameMap, true, AvoidanceListCharge);

                                    if (MovementList.Count < 5 &&
                                        character.abilities[Convert.ToInt32(clickNumberFriendlyCap.Text)]._Charge == true)
                                    {
                                        string movementCharge = "";

                                        foreach (MapNode Node in MovementList)
                                        {
                                            movementCharge = movementCharge + "=> " + "(" + ConvertColumnNumberToLetter(Node.x) + "," + (Node.y + 1).ToString() + ")";
                                        }

                                        MessageBox.Show("Use Charge on " + enemy._CharacterName + "\n" +
                                            "Move to " + movementCharge + "\n" +
                                            "Melee attack on " + enemy._CharacterName, character._CharacterName);

                                        //Update Character's Location
                                        locationAlphaFriendlyCapCB.Text = ConvertColumnNumberToLetter(MovementList.Last.Value.x);
                                        locationNumericFriendlyCapCB.Text = (MovementList.Last.Value.y + 1).ToString();
                                        UpdateCharacterLocations();

                                        //Increase Token Count
                                        actionTokensCapFriendly.Text = (Convert.ToInt32(actionTokensCapFriendly.Text) + 1).ToString();

                                        return;
                                    }
                                }

                                LinkedList<MapNode> Movement = new LinkedList<MapNode>();
                                MapNode movementCheck = new MapNode();

                                //Dealing with Sidestep
                                if (distance < 3 &&
                                    character.abilities[Convert.ToInt32(clickNumberFriendlyCap.Text)]._SideStep == true)
                                {
                                    targetMapNode = FindNearestMapNodeToTargetLocation(characterLocation, enemyLocation, 1, 1, false, false);

                                    if (targetMapNode != null)
                                    {
                                        LinkedList<MapNode> AvoidanceListSideStep = new LinkedList<MapNode>();

                                        AvoidanceListSideStep = FindEnemyMovementThreatZoneForCharacter(targetMapNode, false);
                                        Movement = pathfinder.FindShortestPath(characterLocation, targetMapNode, gameMap, true, AvoidanceListSideStep);

                                        string MovementSideStep = "";

                                        foreach (MapNode Node in Movement)
                                        {
                                            MovementSideStep = MovementSideStep + "=> " + "(" + ConvertColumnNumberToLetter(Node.x) + "," + (Node.y + 1).ToString() + ")";
                                        }

                                        MessageBox.Show("Use SideStep\n" +
                                            "Move to " + MovementSideStep + "\n" +
                                            "Click OK to continue" , character._CharacterName);

                                        //Update Character's Location
                                        locationAlphaFriendlyCapCB.Text = ConvertColumnNumberToLetter(Movement.Last.Value.x);
                                        locationNumericFriendlyCapCB.Text = (Movement.Last.Value.y + 1).ToString();
                                        UpdateCharacterLocations();
                                                                                
                                        //Select the appropriate method of exterminating ones foe
                                        switch (enemy._CharacterName)
                                        {
                                            case "Enemy Thor":
                                                {
                                                    if (Convert.ToInt32(clickNumberOpposingThor.Text) < 7)
                                                    {
                                                        MessageBox.Show("Use Close Combat Expert\n" +
                                                            "Attack + 1 and Damage + 1\n" +
                                                            "Melee attack on " + enemy._CharacterName, character._CharacterName);

                                                        //Increase Token Count
                                                        actionTokensCapFriendly.Text = (Convert.ToInt32(actionTokensCapFriendly.Text) + 1).ToString();

                                                        return;
                                                    }

                                                    if (Convert.ToInt32(clickNumberOpposingThor.Text) > 6)
                                                    {
                                                        MessageBox.Show("Use Close Combat Expert\n" +
                                                            "Attack + 2\n" +
                                                            "Melee attack on " + enemy._CharacterName, character._CharacterName);

                                                        //Increase Token Count
                                                        actionTokensCapFriendly.Text = (Convert.ToInt32(actionTokensCapFriendly.Text) + 1).ToString();

                                                        return;
                                                    }

                                                    //Catch all
                                                    MessageBox.Show("Use Close Combat Expert\n" +
                                                        "Attack + 1 and Damage + 1\n" +
                                                        "Melee Attack on " + enemy._CharacterName, character._CharacterName);

                                                    //Increase Token Count
                                                    actionTokensCapFriendly.Text = (Convert.ToInt32(actionTokensCapFriendly.Text) + 1).ToString();

                                                    return;
                                                }

                                            case "Enemy Iron Man":
                                                {
                                                    if (Convert.ToInt32(clickNumberOpposingIronman.Text) < 4)
                                                    {
                                                        MessageBox.Show("Use Close Combat Expert\n" +
                                                            "Attack + 2\n" +
                                                            "Melee Attack on " + enemy._CharacterName, character._CharacterName);

                                                        //Increase Token Count
                                                        actionTokensCapFriendly.Text = (Convert.ToInt32(actionTokensCapFriendly.Text) + 1).ToString();

                                                        return;
                                                    }

                                                    LineOfSight HinderanceCheck = new LineOfSight();
                                                    HinderanceCheck = IsThereLineOfSight(characterLocation, enemyLocation);

                                                    if (Convert.ToInt32(clickNumberOpposingIronman.Text) > 3 &&
                                                        HinderanceCheck.isHinderance == true)
                                                    {
                                                        MessageBox.Show("Use Close Combat Expert\n" +
                                                            "Attack + 2\n" +
                                                            "Melee Attack on " + enemy._CharacterName, character._CharacterName);

                                                        //Increase Token Count
                                                        actionTokensCapFriendly.Text = (Convert.ToInt32(actionTokensCapFriendly.Text) + 1).ToString();

                                                        return;
                                                    }

                                                    if (Convert.ToInt32(clickNumberOpposingIronman.Text) > 3 &&
                                                        Convert.ToInt32(clickNumberOpposingIronman.Text) < 6 &&
                                                        HinderanceCheck.isHinderance == false)
                                                    {
                                                        MessageBox.Show("Use Close Combat Expert\n" +
                                                            "Attack + 1 and Damage + 1\n" +
                                                            "Melee Attack on " + enemy._CharacterName, character._CharacterName);

                                                        //Increase Token Count
                                                        actionTokensCapFriendly.Text = (Convert.ToInt32(actionTokensCapFriendly.Text) + 1).ToString();

                                                        return;
                                                    }

                                                    if (Convert.ToInt32(clickNumberOpposingIronman.Text) == 6 &&
                                                        HinderanceCheck.isHinderance == false)
                                                    {
                                                        MessageBox.Show("Use Close Combat Expert\n" +
                                                            "Damage + 2\n" +
                                                            "Melee Attack on " + enemy._CharacterName, character._CharacterName);

                                                        //Increase Token Count
                                                        actionTokensCapFriendly.Text = (Convert.ToInt32(actionTokensCapFriendly.Text) + 1).ToString();

                                                        return;
                                                    }

                                                    if (Convert.ToInt32(clickNumberOpposingIronman.Text) == 7 &&
                                                        HinderanceCheck.isHinderance == false)
                                                    {
                                                        MessageBox.Show("Use Close Combat Expert\n" +
                                                            "Attack + 2\n" +
                                                            "Melee Attack on " + enemy._CharacterName, character._CharacterName);

                                                        //Increase Token Count
                                                        actionTokensCapFriendly.Text = (Convert.ToInt32(actionTokensCapFriendly.Text) + 1).ToString();

                                                        return;
                                                    }

                                                    //This is a catch all just in case
                                                    MessageBox.Show("Use Close Combat Expert\n" +
                                                        "Attack + 1 and Damage + 1\n" +
                                                        "Melee Attack on " + enemy._CharacterName, character._CharacterName);

                                                    //Increase Token Count
                                                    actionTokensCapFriendly.Text = (Convert.ToInt32(actionTokensCapFriendly.Text) + 1).ToString();

                                                    return;
                                                }

                                            case "Enemy Captain America":
                                                {
                                                    if (Convert.ToInt32(clickNumberOpposingCap.Text) < 4)
                                                    {
                                                        MessageBox.Show("Use Close Combat Expert\n" +
                                                            "Attack + 2\n" +
                                                            "Melee Attack on " + enemy._CharacterName, character._CharacterName);

                                                        //Increase Token Count
                                                        actionTokensCapFriendly.Text = (Convert.ToInt32(actionTokensCapFriendly.Text) + 1).ToString();

                                                        return;
                                                    }

                                                    LineOfSight HinderanceCheck = new LineOfSight();
                                                    HinderanceCheck = IsThereLineOfSight(characterLocation, enemyLocation);

                                                    if (Convert.ToInt32(clickNumberOpposingCap.Text) > 3 &&
                                                        HinderanceCheck.isHinderance == true)
                                                    {
                                                        MessageBox.Show("Use Close Combat Expert\n" +
                                                            "Attack + 2\n" +
                                                            "Melee Attack on " + enemy._CharacterName, character._CharacterName);

                                                        //Increase Token Count
                                                        actionTokensCapFriendly.Text = (Convert.ToInt32(actionTokensCapFriendly.Text) + 1).ToString();

                                                        return;
                                                    }

                                                    if (Convert.ToInt32(clickNumberOpposingCap.Text) > 3 &&
                                                        Convert.ToInt32(clickNumberOpposingCap.Text) < 6 &&
                                                        HinderanceCheck.isHinderance == false)
                                                    {
                                                        MessageBox.Show("Use Close Combat Expert\n" +
                                                            "Attack + 1 and Damage + 1\n" +
                                                            "Melee Attack on " + enemy._CharacterName, character._CharacterName);

                                                        //Increase Token Count
                                                        actionTokensCapFriendly.Text = (Convert.ToInt32(actionTokensCapFriendly.Text) + 1).ToString();

                                                        return;
                                                    }

                                                    //Catch all
                                                    MessageBox.Show("Use Close Combat Expert\n" +
                                                        "Attack + 2\n" +
                                                        "Melee Attack on " + enemy._CharacterName, character._CharacterName);

                                                    //Increase Token Count
                                                    actionTokensCapFriendly.Text = (Convert.ToInt32(actionTokensCapFriendly.Text) + 1).ToString();

                                                    return;
                                                }
                                        }
                                    }
                                }

                                //Moving normally
                                targetMapNode = FindNearestMapNodeToTargetLocation(characterLocation, enemyLocation, 1, 1, false, true);

                                if (targetMapNode == null)
                                {
                                    targetMapNode = FindNearestMapNodeToTargetLocation(characterLocation, enemyLocation, 1, 3, false, false);
                                }
                                LinkedList<MapNode> AvoidanceList = new LinkedList<MapNode>();

                                AvoidanceList = FindEnemyMovementThreatZoneForCharacter(targetMapNode, false);
                                Movement = pathfinder.FindShortestPath(characterLocation, targetMapNode, gameMap, true, AvoidanceList);
                                string movement = "";
                                int max;

                                if (character.clicks[Convert.ToInt32(clickNumberFriendlyCap.Text)].SpeedValue >= Movement.Count)
                                {
                                    max = Movement.Count - 1;
                                }
                                else
                                {
                                    max = character.clicks[Convert.ToInt32(clickNumberFriendlyCap.Text)].SpeedValue;
                                }

                                //Check to make sure the new ending spot is not Occupied, if not go back one MapNode and recheck
                                for (int i = max - 1; i > -1; i--)
                                {
                                    if (Movement.ElementAt(i).IsOccupied == false)
                                    {
                                        break;
                                    }
                                    max = max - 1;
                                }

                                //Prepare movement string for Message Box
                                for (int i = 0; i < max; i++)
                                {
                                    movement = movement + "=> " + "(" + ConvertColumnNumberToLetter(Movement.ElementAt(i).x) + "," + (Movement.ElementAt(i).y + 1).ToString() + ")";
                                }

                                MessageBox.Show("Move to " + movement, character._CharacterName);

                                //Update Character's Location
                                locationAlphaFriendlyCapCB.Text = ConvertColumnNumberToLetter(Movement.ElementAt(max - 1).x);
                                locationNumericFriendlyCapCB.Text = (Movement.ElementAt(max - 1).y + 1).ToString();
                                UpdateCharacterLocations();

                                //Increase Token Count
                                actionTokensCapFriendly.Text = (Convert.ToInt32(actionTokensCapFriendly.Text) + 1).ToString();

                                return;
                            }
                            break;
                        }
                    }
            }
        }

        //Returns the closest ItemNode(item) from the character and assigns a distance score inside of the ItemNode
        private ItemNode CalculatePickUpItemScore(Character character, LinkedList<ItemNode> AvailableItems)
        {
            //Determine the closest item and score
            MapNode[,] gameMap = new MapNode[16, 16];
            gameMap = RetrieveMap();
            MapNode characterLocation = new MapNode();
            Pathfinder pathfinder = new Pathfinder();
            ItemNode closestItem = new ItemNode();
            //Get character's location
            characterLocation = IsOccupiedList.FirstOrDefault(l => l.OccupiedByCharacter == character);

            //Next check for distance avoiding hinderance if item not found
            closestItem = AvailableItems.First();

            foreach (ItemNode item in AvailableItems)
            {
                if (pathfinder.FindShortestPath(closestItem.mapLocation, characterLocation, gameMap, false).Count >
                    pathfinder.FindShortestPath(item.mapLocation, characterLocation, gameMap, false).Count)
                {
                    closestItem = item;
                    closestItem.itemDistanceScore = pathfinder.FindShortestPath(closestItem.mapLocation, characterLocation, gameMap, false).Count;
                    return closestItem;
                }
            }
            return closestItem;
        }

        //Returns true or false whether the character is currently engaged in melee with an enemy
        private Boolean EngagementStatus(Character character)
        {
            //Get the location of the character and determine if an enemy is engaged with them
            Pathfinder pathfinder = new Pathfinder();
            MapNode characterLocation = new MapNode();
            LinkedList<MapNode> LocationList = new LinkedList<MapNode>();
            Boolean engaged = false;
            MapNode[,] gameMap = new MapNode[16, 16];
            gameMap = RetrieveMap();

            //Create a seperate LinkedList that has the infromation in IsOccupiedList
            foreach (MapNode Node in IsOccupiedList)
            {
                LocationList.AddLast(Node);
            }

            //Find the character's MapNode in list and remove it and remove all ally's
            LinkedList<MapNode> NewLocationList = new LinkedList<MapNode>();
            characterLocation = LocationList.FirstOrDefault(l => l.OccupiedByCharacter._CharacterName == character._CharacterName);
            LocationList.Remove(characterLocation);

            foreach (MapNode Node in LocationList)
            {
                if (Node.OccupiedByCharacter != Thor &&
                    Node.OccupiedByCharacter != IronMan &&
                    Node.OccupiedByCharacter != CaptainAmerica)
                {
                    NewLocationList.AddLast(Node);
                }
            }

            //Discover if there is an enemy next to the character
            LinkedList<MapNode> Distance = new LinkedList<MapNode>();
            int distance;

            foreach (MapNode Node in NewLocationList)
            {
                Distance = pathfinder.FindShortestPath(characterLocation, Node, gameMap);

                if (Distance != null)
                {
                    distance = Distance.Count;
                    if (distance == 1)
                    {
                        engaged = true;
                    }
                }                
            }

            return engaged;
        }

        private void WhatToDoThor_Click(object sender, EventArgs e)
        {
            DetermineCharactersTurn(Thor);
            whatToDoThor.Enabled = false;
        }

        private void Button11_Click(object sender, EventArgs e)
        {
            int total;
            total = Convert.ToInt32(clickNumberOpposingCap.Text);
            total = total + 1;
            clickNumberOpposingCap.Text = total.ToString();
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            int total;
            total = Convert.ToInt32(clickNumberOpposingIronman.Text);
            total = total + 1;
            clickNumberOpposingIronman.Text = total.ToString();
        }

        private void Button7_Click(object sender, EventArgs e)
        {
            int total;
            total = Convert.ToInt32(clickNumberOpposingThor.Text);
            total = total + 1;
            clickNumberOpposingThor.Text = total.ToString();
        }

        private void Button12_Click(object sender, EventArgs e)
        {
            int total;
            total = Convert.ToInt32(clickNumberOpposingCap.Text);
            total = total - 1;
            clickNumberOpposingCap.Text = total.ToString();
        }

        private void Button10_Click(object sender, EventArgs e)
        {
            int total;
            total = Convert.ToInt32(clickNumberOpposingIronman.Text);
            total = total - 1;
            clickNumberOpposingIronman.Text = total.ToString();
        }

        private void Button8_Click(object sender, EventArgs e)
        {
            int total;
            total = Convert.ToInt32(clickNumberOpposingThor.Text);
            total = total - 1;
            clickNumberOpposingThor.Text = total.ToString();
        }

        private void WhatToDoIronMan_Click(object sender, EventArgs e)
        {
            DetermineCharactersTurn(IronMan);
            whatToDoIronMan.Enabled = false;
        }

        private string ConvertColumnNumberToLetter(int columnNumber)
        {
            int div = columnNumber + 1;
            string columnLetter = String.Empty;
            int mod = 0;

            while (div > 0)
            {
                mod = (div - 1) % 26;
                columnLetter = (char)(65 + mod) + columnLetter;
                div = (int)((div - mod) / 26);
            }

            return columnLetter;
        }

        //Builds a list for the pathfinder of MapNodes it needs to avoid except for the targetLocation
        private LinkedList<MapNode> FindEnemyMovementThreatZoneForCharacter(MapNode targetLocation, Boolean trueForFlyingCharacters)
        {
            //Setup
            Pathfinder pathfinder = new Pathfinder();
            MapNode characterLocation = new MapNode();
            LinkedList<MapNode> ThreatZoneList = new LinkedList<MapNode>();
            MapNode[,] gameMap = new MapNode[16, 16];
            gameMap = RetrieveMap();

            //Create new list of only enemy characters
            foreach (MapNode Node in IsOccupiedList)
            {
                if (Node.OccupiedByCharacter != Thor &&
                    Node.OccupiedByCharacter != IronMan &&
                    Node.OccupiedByCharacter != CaptainAmerica)
                {
                    ThreatZoneList.AddLast(Node);
                }
            }

            switch (trueForFlyingCharacters)
            {
                //Return occupied list for flyers
                case true:
                    {
                        return ThreatZoneList;
                    }

                //Return all viable nearby MapNodes for non flyers
                case false:
                    {
                        LinkedList<MapNode> NewThreatZoneList = new LinkedList<MapNode>();
                        LinkedList<MapNode> Distance = new LinkedList<MapNode>();
                        MapNode testingNode = new MapNode();

                        //Populate the new list
                        foreach (MapNode Node in ThreatZoneList)
                        {
                            for (int x = -1; x < 2; x++)
                            {
                                for (int y = -1; y < 2; y++)
                                {
                                    //Create first MapNode that is tested
                                    testingNode = gameMap[MaxCap(Node.x + x, 15, 0), MaxCap(Node.y + y, 15, 0)];
                                    Distance = pathfinder.FindShortestPath(testingNode, Node, gameMap);

                                    //Add MapNode to list if it is not a duplicate, is one step away from target, and not the target location
                                    if (Distance != null && 
                                        Distance.Count == 1 &&
                                        NewThreatZoneList.FirstOrDefault(l => l.x == testingNode.x && l.y == testingNode.y) == null &&
                                        (testingNode.x != targetLocation.x || testingNode.y != targetLocation.y) == true)
                                    {
                                        NewThreatZoneList.AddLast(testingNode);
                                    }
                                }
                            }
                        }

                        return NewThreatZoneList;
                    }
            }

            return null;
        }
    }
}
