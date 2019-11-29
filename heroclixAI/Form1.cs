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
            MapNode current = new MapNode();

            current = gameMap[ConvertColumnLetter(locationAlphaFriendlyCapCB.Text), Convert.ToInt32(locationNumericFriendlyCapCB.Text) - 1];
            gameMap[current.x, current.y].IsOccupied = true;
            current.IsOccupied = true;
            IsOccupiedList.AddLast(current);

            current = gameMap[ConvertColumnLetter(locationAlphaFriendlyIronManCB.Text), Convert.ToInt32(locationNumericFriendlyIronManCB.Text) - 1];
            gameMap[current.x, current.y].IsOccupied = true;
            current.IsOccupied = true;
            IsOccupiedList.AddLast(current);

            current = gameMap[ConvertColumnLetter(locationAlphaFriendlyThorCB.Text), Convert.ToInt32(locationNumericFriendlyThorCB.Text) - 1];
            gameMap[current.x, current.y].IsOccupied = true;
            current.IsOccupied = true;
            IsOccupiedList.AddLast(current);

            current = gameMap[ConvertColumnLetter(locationAlphaOpposingCapCB.Text), Convert.ToInt32(locationNumericOpposingCapCB.Text) - 1];
            gameMap[current.x, current.y].IsOccupied = true;
            current.IsOccupied = true;
            IsOccupiedList.AddLast(current);

            current = gameMap[ConvertColumnLetter(locationAlphaOpposingIronManCB.Text), Convert.ToInt32(locationNumericOpposingIronManCB.Text) - 1];
            gameMap[current.x, current.y].IsOccupied = true;
            current.IsOccupied = true;
            IsOccupiedList.AddLast(current);

            current = gameMap[ConvertColumnLetter(locationAlphaOpposingThorCB.Text), Convert.ToInt32(locationNumericOpposingThorCB.Text) - 1];
            gameMap[current.x, current.y].IsOccupied = true;
            current.IsOccupied = true;
            IsOccupiedList.AddLast(current);

            //Save the map data
            SaveMap(gameMap);

        }

        private void whatToDoCaptainAmerica_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This is what you should do");
        }

        private void mainForm_Load(object sender, EventArgs e)
        {
            actionTokensCapFriendly.SelectedIndex = 0;
            actionTokensIronManFriendly.SelectedIndex = 0;
            actionTokensThorFriendly.SelectedIndex = 0;

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
            Thor._characterName = "Thor";
            EnemyThor._characterName = "Enemy Thor";

            //Click 1
            chAbility.Charge = true;
            chAbility.SuperStrength = true;
            chAbility.Impervious = true;

            Thor.AddClick(1, 11, 11, 18, 4, 6, chAbility);
            EnemyThor.AddClick(1, 11, 11, 18, 4, 6, chAbility);

            //Click 2
            Thor.AddClick(2, 11, 11, 17, 4, 6, chAbility);
            EnemyThor.AddClick(2, 11, 11, 17, 4, 6, chAbility);

            //Click 3
            Thor.AddClick(3, 11, 11, 17, 3, 6, chAbility);
            EnemyThor.AddClick(3, 11, 11, 17, 3, 6, chAbility);

            //Click 4
            chAbility.Charge = false;
            chAbility.RunningShot = true;
            chAbility.SuperStrength = false;
            chAbility.EnergyExplosion = true;
            chAbility.Impervious = false;
            chAbility.Invulnerability = true;

            Thor.AddClick(4, 11, 10, 17, 3, 6, chAbility);
            EnemyThor.AddClick(4, 11, 10, 17, 3, 6, chAbility);

            //Click 5
            Thor.AddClick(5, 11, 10, 17, 3, 6, chAbility);
            EnemyThor.AddClick(5, 11, 10, 17, 3, 6, chAbility);

            //Click 6
            Thor.AddClick(6, 11, 10, 17, 3, 6, chAbility);
            EnemyThor.AddClick(6, 11, 10, 17, 3, 6, chAbility);

            //Click 7
            chAbility.RunningShot = false;
            chAbility.SideStep = true;
            chAbility.EnergyExplosion = false;
            chAbility.LightningSmash = true;
            chAbility.Invulnerability = false;
            chAbility.WillPower = true;

            Thor.AddClick(7, 10, 9, 17, 3, 6, chAbility);
            EnemyThor.AddClick(7, 10, 9, 17, 3, 6, chAbility);

            //Click 8
            Thor.AddClick(8, 10, 9, 17, 3, 6, chAbility);
            EnemyThor.AddClick(8, 10, 9, 17, 3, 6, chAbility);

            //Click 9
            Thor.AddClick(9, 10, 9, 16, 3, 6, chAbility);
            EnemyThor.AddClick(9, 10, 9, 16, 3, 6, chAbility);

            //Setup Iron Man
            IronMan._characterName = "Iron Man";
            EnemyIronMan._characterName = "Enemy Iron Man";

            //Click 1 Added ClearAbilites Method to help clear abilites in the chAbility class
            chAbility.ClearAbilites();
            chAbility.RunningShot = true;
            chAbility.EnergyExplosion = true;
            chAbility.Invulnerability = true;

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
            chAbility.SideStep = true;
            chAbility.Toughness = true;
            chAbility.RangedCombatExpert = true;

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
            CaptainAmerica._characterName = "Captain America";
            EnemyCaptainAmerica._characterName = "Captain America";

            //Click 1
            chAbility.ClearAbilites();
            chAbility.Deflection = true;
            chAbility.Charge = true;
            chAbility.CombatReflexes = true;
            chAbility.Leadership = true;

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
            chAbility.Deflection = true;
            chAbility.SideStep = true;
            chAbility.WillPower = true;
            chAbility.CloseCombatExpert = true;

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
            string filePath = textBoxMapFilePath.Text;
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
                string filePath = textBoxMapFilePath.Text;
                BinaryFormatter bf = new BinaryFormatter();
                MapNode[,] gameMap = new MapNode[16, 16];
                Stream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                gameMap = (MapNode[,])bf.Deserialize(stream);
                stream.Close();
                return gameMap;
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Map data file not found.", "File Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private void SaveMap(MapNode[,] gameMap)
        {
            //Creates or overwrites file at specified file path with the array given.
            string filePath = textBoxMapFilePath.Text;
            BinaryFormatter bf = new BinaryFormatter();
            Stream stream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            bf.Serialize(stream, gameMap);
            stream.Close();
        }

        private void Button13_Click(object sender, EventArgs e)
        {
            MapNode attacker = new MapNode();
            MapNode defender = new MapNode();
            attacker.x = 0;
            attacker.y = 0;
            defender.x = 15;
            defender.y = 0;
            LineOfSight LOS = new LineOfSight();
            LOS = IsThereLineOfSight(attacker, defender);

            MessageBox.Show("Line Of Sight: " + LOS.lineOfSight.ToString() +
                "\nBlocked: " + LOS.IsBlocked.ToString() +
                "\nHinderance: " + LOS.isHinderance.ToString() +
                "\nOccupied: " + LOS.isOccupied.ToString());

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
                    Item.SubItems.Add(Node.character._characterName.ToString());
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
                            Item.character._lightWeapon = false;
                        }
                        
                        ItemList.Remove(Item);
                        break;
                    }

                case "Heavy":
                    {
                        if (Item.character != null)
                        {
                            Item.character._heavyWeapon = false;
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
                        if (Item.weaponType == "Light" && Thor._lightWeapon == false && Thor._heavyWeapon == false)
                        {
                            Thor._lightWeapon = true;
                            Item.character = Thor;
                            break;
                        }

                        if (Item.weaponType == "Heavy" && Thor._lightWeapon == false && Thor._heavyWeapon == false)
                        {
                            Thor._heavyWeapon = true;
                            Item.character = Thor;
                            break;
                        }

                        MessageBox.Show("Character already has a weapon.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                case "Iron Man":
                    {
                        if (Item.weaponType == "Light" && IronMan._lightWeapon == false && IronMan._heavyWeapon == false)
                        {
                            IronMan._lightWeapon = true;
                            Item.character = IronMan;
                            break;
                        }

                        if (Item.weaponType == "Heavy" && IronMan._lightWeapon == false && IronMan._heavyWeapon == false)
                        {
                            IronMan._heavyWeapon = true;
                            Item.character = IronMan;
                            break;
                        }

                        MessageBox.Show("Character already has a weapon.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                case "Captain America":
                    {
                        if (Item.weaponType == "Light" && CaptainAmerica._lightWeapon == false && CaptainAmerica._heavyWeapon == false)
                        {
                            CaptainAmerica._lightWeapon = true;
                            Item.character = CaptainAmerica;
                            break;
                        }

                        if (Item.weaponType == "Heavy" && CaptainAmerica._lightWeapon == false && CaptainAmerica._heavyWeapon == false)
                        {
                            CaptainAmerica._heavyWeapon = true;
                            Item.character = CaptainAmerica;
                            break;
                        }

                        MessageBox.Show("Character already has a weapon.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                case "Enemy Thor":
                    {
                        if (Item.weaponType == "Light" && EnemyThor._lightWeapon == false && EnemyThor._heavyWeapon == false)
                        {
                            EnemyThor._lightWeapon = true;
                            Item.character = EnemyThor;
                            break;
                        }

                        if (Item.weaponType == "Heavy" && EnemyThor._lightWeapon == false && EnemyThor._heavyWeapon == false)
                        {
                            EnemyThor._heavyWeapon = true;
                            Item.character = EnemyThor;
                            break;
                        }

                        MessageBox.Show("Character already has a weapon.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                case "Enemy Iron Man":
                    {
                        if (Item.weaponType == "Light" && EnemyIronMan._lightWeapon == false && EnemyIronMan._heavyWeapon == false)
                        {
                            EnemyIronMan._lightWeapon = true;
                            Item.character = EnemyIronMan;
                            break;
                        }

                        if (Item.weaponType == "Heavy" && EnemyIronMan._lightWeapon == false && EnemyIronMan._heavyWeapon == false)
                        {
                            EnemyIronMan._heavyWeapon = true;
                            Item.character = EnemyIronMan;
                            break;
                        }

                        MessageBox.Show("Character already has a weapon.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                case "Enemy Captain America":
                    {
                        if (Item.weaponType == "Light" && EnemyCaptainAmerica._lightWeapon == false && EnemyCaptainAmerica._heavyWeapon == false)
                        {
                            EnemyCaptainAmerica._lightWeapon = true;
                            Item.character = EnemyCaptainAmerica;
                            break;
                        }

                        if (Item.weaponType == "Heavy" && EnemyCaptainAmerica._lightWeapon == false && EnemyCaptainAmerica._heavyWeapon == false)
                        {
                            EnemyCaptainAmerica._heavyWeapon = true;
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
            Item.character._lightWeapon = false;
            Item.character._heavyWeapon = false;
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
            //Create the object that will hold the answers
            LineOfSight lineOfSight = new LineOfSight();

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
                MapNode current = new MapNode();
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

                    //If no wall is detected return true
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

                    //If no wall is detected return true
                    lineOfSight.lineOfSight = true;
                    return lineOfSight;
                }
            }

            //Determine if the line of sight is horizontal
            if (attacker.y == defender.y)
            {
                MapNode current = new MapNode();
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

                    //If no wall is detected return true
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

                    //If no wall is detected return true
                    lineOfSight.lineOfSight = true;
                    return lineOfSight;
                }
            }

            //Determine if the Line of Sight is diagonal


            lineOfSight.lineOfSight = true;
            return lineOfSight;
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
    }
}
