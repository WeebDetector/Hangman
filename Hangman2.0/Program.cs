using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

public class HangMan : Form
{
    private readonly string NL = Environment.NewLine;
    private TextBox tbInp;
    private PictureBox tbOut;
    private TextBox tbHigh;
    readonly string[] words1 = { "GĖLĖ", "RANKA", "GILZĖ", "AKIS", "BITĖ" };
    readonly string[] words2 = { "KIBIRAS", "VANAGAS", "KASTUVAS", "PROGRAMA", "SAKALAS" };
    readonly string[] words3 = { "KOMPIUTERIS", "KAILINIAI", "MONITORIUS", "PROGRAMAVIMAS", "ZVIMBALIUS" };
    readonly string[] words4 = { "BESIPASIKIŠKIAKOPŪSTELIAUDAMASIS", "OTORINOLARINGOLOGAS" };
    readonly string[] topic1 = { "GĖLĖ", "BITĖ", "VANAGAS", "SAKALAS" };
    readonly string[] topic2 = { "PROGRAMA", "KOMPIUTERIS", "MONITORIUS", "PROGRAMAVIMAS", "ZVIMBALIUS" };
    readonly string[] random = { "GĖLĖ", "RANKA", "GILZĖ", "AKIS", "BITĖ", "KIBIRAS", "VANAGAS", "KASTUVAS", "PROGRAMA", "SAKALAS",
                                 "KOMPIUTERIS", "KAILINIAI", "MONITORIUS", "PROGRAMAVIMAS", "ZVIMBALIUS", "BESIPASIKIŠKIAKOPŪSTELIAUDAMASIS", "OTORINOLARINGOLOGAS" };
    private int buttonsUD;
    private int buttonsLR;
    int wrongGuesses = 0;
    string chosenText;
    string chosenWord;
    char[] guess;
    int lettersIn = -1;
    int amountofGuesses = 0;
    int win = 0;
    List<Button> letterButtons;
    string dataFile = "GameUsageData.txt";
    string[] dataTexts = new string[10];
    int timesUsed;
    int timesWon;
    int timesLost;
    int times1;
    int times2;
    int times3;
    int times4;
    int topics1;
    int topics2;
    int randoms;
    int currentGames = 1;
    int currentWins = 0;
    int currentLosses = 0;
    int winsInARow = 0;
    int ifClicked = 0;

    public HangMan()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        this.tbInp = new TextBox();
        this.SuspendLayout();

        this.tbInp.Multiline = true;

        const int fontSize = 12;
        tbInp.Size = new Size(fontSize * 40, fontSize * 10);
        tbInp.Location = new Point(0, 0);
        this.Controls.Add(this.tbInp);
        this.Text = "TextBox Example";
        tbInp.BackColor = Color.Black;
        tbInp.ForeColor = Color.LightGray;
        tbInp.Font = new Font("Courier", fontSize);
        Width = tbInp.Width * 2 - 180;
        Height = tbInp.Height * 2 + 70;

        buttonsLR = fontSize * 40;
        buttonsUD = fontSize * 10;

        this.PerformLayout();

        this.tbOut = new PictureBox();
        this.SuspendLayout();

        tbOut.Size = new Size(fontSize * 6 + 10, fontSize * 15);
        tbOut.Location = new Point(fontSize * 40 + 20, fontSize * 5 + 30);
        this.Controls.Add(this.tbOut);
        tbOut.BackColor = Color.White;
        tbOut.ForeColor = Color.LightGray;
        tbOut.Font = new Font("Courier", fontSize);

        this.PerformLayout();

        this.tbHigh = new TextBox();
        this.SuspendLayout();

        this.tbHigh.Multiline = true;

        tbHigh.Size = new Size(fontSize * 13 + 11, fontSize * 15);
        tbHigh.Location = new Point(fontSize * 49, fontSize * 5 + 30);
        this.Controls.Add(this.tbHigh);
        tbHigh.BackColor = Color.Black;
        tbHigh.ForeColor = Color.LightGray;
        tbHigh.Font = new Font("Courier", fontSize);

        this.PerformLayout();

        AddButtons(Level_Click);

        tbInp.Text = "Pasirinkite norimą lygį arba temą \r\n";
        tbInp.AppendText("Lengvas lygis: žodžiai iš 4-5 raidžių \r\n");
        tbInp.AppendText("Vidutinis lygis: žodžiai 6-8 raidžių \r\n");
        tbInp.AppendText("Sunkus lygis: žodžiai 9-15 raidžių \r\n");
        tbInp.AppendText("Neįmanomas lygis: painūs ir ilgi žodžiai \r\n");

        string[] lines = File.ReadAllLines(dataFile);
        for (int i = 0; i < lines.Count(); i++)
        {
            string[] values = lines[i].Split(' ');
            for (int j = 0; j < values.Count(); j++)
            {
                if (j != values.Count() - 1)
                    dataTexts[i] += values[j] + " ";
                switch (i)
                {
                    case 0:
                        timesUsed = int.Parse(values[values.Count() - 1]);
                        break;
                    case 1:
                        timesWon = int.Parse(values[values.Count() - 1]);
                        break;
                    case 2:
                        timesLost = int.Parse(values[values.Count() - 1]);
                        break;
                    case 3:
                        times1 = int.Parse(values[values.Count() - 1]);
                        break;
                    case 4:
                        times2 = int.Parse(values[values.Count() - 1]);
                        break;
                    case 5:
                        times3 = int.Parse(values[values.Count() - 1]);
                        break;
                    case 6:
                        times4 = int.Parse(values[values.Count() - 1]);
                        break;
                    case 7:
                        topics1 = int.Parse(values[values.Count() - 1]);
                        break;
                    case 8:
                        topics2 = int.Parse(values[values.Count() - 1]);
                        break;
                    case 9:
                        randoms = int.Parse(values[values.Count() - 1]);
                        break;
                }
            }
        }
        timesUsed++;
    }

    [STAThread]
    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new HangMan());
    }
    protected void Level_Click(object sender, System.EventArgs e)
    {
        tbOut.Load("Resources/Hangman0.png");
        if (ifClicked == 1)
        {
            currentGames++;
            ifClicked = 0;
        }
        tbHigh.Text = "Current Session \r\n";
        tbHigh.AppendText("Times Played: " + currentGames + "\r\n");
        tbHigh.AppendText("Times Won: " + currentWins + "\r\n");
        tbHigh.AppendText("Times Lost: " + currentLosses + "\r\n");
        tbHigh.AppendText("Wins in a row : " + winsInARow + "\r\n");
        if (wrongGuesses == 6 || win == 1)
        {
            timesUsed++;
            foreach (Button button in letterButtons)
                button.Enabled = true;
            wrongGuesses = 0;
            win = -1;
            amountofGuesses = 0;
        }
        Random randGen = new Random();

        string s = (sender as Button).Text;
        if (s == "Lengvas lygis")
        {
            times1++;
            chosenText = "Pasirinktas lengvas lygis: žodžiai iš 4 - 5 raidžių \r\n";
            tbInp.Text = chosenText;
            tbInp.AppendText("Jūsų žodis: ");

            var wordNumber = randGen.Next(0, words1.Count());

            chosenWord = words1[wordNumber];

            guess = new char[chosenWord.Length];

            for (int i = 0; i < chosenWord.Length; i++)
                guess[i] = '*';

            foreach (char letter in guess)
            {
                tbInp.AppendText(letter.ToString());
            }
        }
        if (s == "Vidutinis lygis")
        {
            times2++;
            chosenText = "Pasirinktas vidutinis lygis: žodžiai 6 - 8 raidžių \r\n";
            tbInp.Text = chosenText;
            tbInp.AppendText("Jūsų žodis: ");

            var wordNumber = randGen.Next(0, words2.Count());

            chosenWord = words2[wordNumber];

            guess = new char[chosenWord.Length];

            for (int i = 0; i < chosenWord.Length; i++)
                guess[i] = '*';

            foreach (char letter in guess)
            {
                tbInp.AppendText(letter.ToString());
            }
        }
        if (s == "Sunkus lygis")
        {
            times3++;
            chosenText = "Pasirinktas sunkus lygis: žodžiai 9 - 15 raidžių \r\n";
            tbInp.Text = chosenText;
            tbInp.AppendText("Jūsų žodis: ");

            var wordNumber = randGen.Next(0, words3.Count());

            chosenWord = words3[wordNumber];

            guess = new char[chosenWord.Length];

            for (int i = 0; i < chosenWord.Length; i++)
                guess[i] = '*';

            foreach (char letter in guess)
            {
                tbInp.AppendText(letter.ToString());
            }
        }
        if (s == "Neįmanomas lygis")
        {
            times4++;
            chosenText = "Pasirinktas neįmanomas lygis: painūs ir ilgi žodžiai \r\n";
            tbInp.Text = chosenText;
            tbInp.AppendText("Jūsų žodis: ");

            var wordNumber = randGen.Next(0, words4.Count());

            chosenWord = words4[wordNumber];

            guess = new char[chosenWord.Length];

            for (int i = 0; i < chosenWord.Length; i++)
                guess[i] = '*';

            foreach (char letter in guess)
            {
                tbInp.AppendText(letter.ToString());
            }
        }
        if (s == "Gamta")
        {
            topics1++;
            chosenText = "Pasirinkta tema:  gamta \r\n";
            tbInp.Text = chosenText;
            tbInp.AppendText("Jūsų žodis: ");

            var wordNumber = randGen.Next(0, topic1.Count());

            chosenWord = topic1[wordNumber];

            guess = new char[chosenWord.Length];

            for (int i = 0; i < chosenWord.Length; i++)
                guess[i] = '*';

            foreach (char letter in guess)
            {
                tbInp.AppendText(letter.ToString());
            }
        }
        if (s == "Technologijos")
        {
            topics2++;
            chosenText = "Pasirinkta tema: technologijos \r\n";
            tbInp.Text = chosenText;
            tbInp.AppendText("Jūsų žodis: ");

            var wordNumber = randGen.Next(0, topic2.Count());

            chosenWord = topic2[wordNumber];

            guess = new char[chosenWord.Length];

            for (int i = 0; i < chosenWord.Length; i++)
                guess[i] = '*';

            foreach (char letter in guess)
            {
                tbInp.AppendText(letter.ToString());
            }
        }
        if (s == "Atsitiktiniai žodžiai")
        {
            randoms++;
            chosenText = "Pasirinkta tema: atsitiktiniai žodžiai \r\n";
            tbInp.Text = chosenText;
            tbInp.AppendText("Jūsų žodis: ");

            var wordNumber = randGen.Next(0, random.Count());

            chosenWord = random[wordNumber];

            guess = new char[chosenWord.Length];

            for (int i = 0; i < chosenWord.Length; i++)
                guess[i] = '*';

            foreach (char letter in guess)
            {
                tbInp.AppendText(letter.ToString());
            }
        }
        if (lettersIn == -1)
        {
            AddLetters(Guess_Click);
            lettersIn = 1;
        }

    }

    protected void Guess_Click(object sender, System.EventArgs e)
    {
        int verifyGuess = -1;
        ifClicked = 1;
        if (wrongGuesses != 6 && guess.Contains('*'))
        {
            (sender as Button).Enabled = false;
            string s = (sender as Button).Text;
            for (int i = 0; i < chosenWord.Length; i++)
            {
                string letter = chosenWord[i].ToString();
                if (s == letter)
                {
                    guess[i] = chosenWord[i];
                    verifyGuess = 1;
                    amountofGuesses++;
                }
            }
            tbInp.Text = chosenText;
            tbInp.AppendText("Paskutinė spėta raidė yra " + s);

            if (verifyGuess == 1)
            {
                tbInp.AppendText(" yra teisinga \r\n");
            }
            else
            {
                tbInp.AppendText(" yra neteisinga \r\n");
                wrongGuesses++;
                tbOut.Load("Resources/Hangman" + wrongGuesses + ".png");
            }
            tbInp.AppendText("Jūsų žodis: ");
            foreach (char symbol in guess)
            {
                tbInp.AppendText(symbol.ToString());
            }
            if (amountofGuesses == chosenWord.Length)
            {
                tbInp.AppendText("\r\nŽodis atspėtas! Norėdami žaisti dar kartą pasirinkite lygį");
                win = 1;
                timesWon++;
                currentWins++;
                winsInARow++;
                tbHigh.Text = "Current Session \r\n";
                tbHigh.AppendText("Times Played: " + currentGames + "\r\n");
                tbHigh.AppendText("Times Won: " + currentWins + "\r\n");
                tbHigh.AppendText("Times Lost: " + currentLosses + "\r\n");
                tbHigh.AppendText("Wins in a row : " + winsInARow + "\r\n");
            }
            else if (wrongGuesses == 6 && guess.Contains('*'))
            {
                tbInp.AppendText("\r\nŽodžio atspėti nepavyko! Teisingas žodis: " + chosenWord +
                                 " Norėdami žaisti dar kartą pasirinkite lygį");
                timesLost++;
                currentLosses++;
                winsInARow = 0;
                tbHigh.Text = "Current Session \r\n";
                tbHigh.AppendText("Times Played: " + currentGames + "\r\n");
                tbHigh.AppendText("Times Won: " + currentWins + "\r\n");
                tbHigh.AppendText("Times Lost: " + currentLosses + "\r\n");
                tbHigh.AppendText("Wins in a row: " + winsInARow + "\r\n");
            }
        }
        else if (guess.Contains('*') && wrongGuesses == 6)
        {
            tbInp.Text = "Žaidimas jau pralaimėtas, norėdami žaisti dar kartą pasirinkite lygį";
        }
        else
        {
            tbInp.Text = "Žaidimas jau laimėtas, norėdami žaisti dar kartą pasirinkite lygį";
        }
        string[] lines = new string[dataTexts.Count()];
        for (int i = 0; i < lines.Count(); i++)
        {
            switch (i)
            {
                case 0:
                    lines[i] = dataTexts[i] + timesUsed;
                    break;
                case 1:
                    lines[i] = dataTexts[i] + timesWon;
                    break;
                case 2:
                    lines[i] = dataTexts[i] + timesLost;
                    break;
                case 3:
                    lines[i] = dataTexts[i] + times1;
                    break;
                case 4:
                    lines[i] = dataTexts[i] + times2;
                    break;
                case 5:
                    lines[i] = dataTexts[i] + times3;
                    break;
                case 6:
                    lines[i] = dataTexts[i] + times4;
                    break;
                case 7:
                    lines[i] = dataTexts[i] + topics1;
                    break;
                case 8:
                    lines[i] = dataTexts[i] + topics2;
                    break;
                case 9:
                    lines[i] = dataTexts[i] + randoms;
                    break;
            }
        }
        File.WriteAllLines(dataFile, lines);
    }

    void AddButtons(EventHandler ev)
    {
        buttonsLR += 5;

        Button btn = new Button();
        btn.Text = "Lengvas lygis";
        btn.Location = new Point(buttonsLR, 0);
        btn.Size = new Size(90, 30);
        btn.Click += ev;
        this.Controls.Add(btn);
        buttonsLR += btn.Width;

        btn = new Button();
        btn.Text = "Vidutinis lygis";
        btn.Location = new Point(buttonsLR, 0);
        btn.Size = new Size(90, 30);
        btn.Click += ev;
        this.Controls.Add(btn);
        buttonsLR += btn.Width;

        btn = new Button();
        btn.Text = "Sunkus lygis";
        btn.Location = new Point(buttonsLR, 0);
        btn.Size = new Size(90, 30);
        btn.Click += ev;
        this.Controls.Add(btn);
        buttonsLR = buttonsLR - (2 * btn.Width);

        btn = new Button();
        btn.Text = "Neįmanomas lygis";
        btn.Location = new Point(buttonsLR, 30);
        btn.Size = new Size(90, 30);
        btn.Click += ev;
        this.Controls.Add(btn);
        buttonsLR += btn.Width;

        btn = new Button();
        btn.Text = "Gamta";
        btn.Location = new Point(buttonsLR, 30);
        btn.Size = new Size(90, 30);
        btn.Click += ev;
        this.Controls.Add(btn);
        buttonsLR += btn.Width;

        btn = new Button();
        btn.Text = "Technologijos";
        btn.Location = new Point(buttonsLR, 30);
        btn.Size = new Size(90, 30);
        btn.Click += ev;
        this.Controls.Add(btn);
        buttonsLR = buttonsLR - (2 * btn.Width);

        btn = new Button();
        btn.Text = "Atsitiktiniai žodžiai";
        btn.Location = new Point(buttonsLR, 60);
        btn.Size = new Size(270, 30);
        btn.Click += ev;
        this.Controls.Add(btn);

    }

    void AddLetters(EventHandler ev)
    {
        letterButtons = new List<Button>();
        Button btn = new Button();
        buttonsLR = 0;
        for (char i = 'A'; i <= 'Z'; i++)
        {
            string name = i.ToString();
            
            btn = new Button();
            btn.Text = name;
            btn.Location = new Point(buttonsLR, buttonsUD + 10);
            btn.Size = new Size(40, 30);
            btn.Click += ev;
            this.Controls.Add(btn);
            buttonsLR += btn.Width;
            if (buttonsLR == tbInp.Width)
            {
                buttonsUD += btn.Height;
                buttonsLR = 0;
            }
            letterButtons.Add(btn);
        }

        btn = new Button();
        btn.Text = "Ą";
        btn.Location = new Point(buttonsLR, buttonsUD + 10);
        btn.Size = new Size(40, 30);
        btn.Click += ev;
        this.Controls.Add(btn);
        buttonsLR += btn.Width;
        if (buttonsLR == tbInp.Width)
        {
            buttonsUD += btn.Height;
            buttonsLR = 0;
        }
        letterButtons.Add(btn);

        btn = new Button();
        btn.Text = "Č";
        btn.Location = new Point(buttonsLR, buttonsUD + 10);
        btn.Size = new Size(40, 30);
        btn.Click += ev;
        this.Controls.Add(btn);
        buttonsLR += btn.Width;
        if (buttonsLR == tbInp.Width)
        {
            buttonsUD += btn.Height;
            buttonsLR = 0;
        }
        letterButtons.Add(btn);

        btn = new Button();
        btn.Text = "Ę";
        btn.Location = new Point(buttonsLR, buttonsUD + 10);
        btn.Size = new Size(40, 30);
        btn.Click += ev;
        this.Controls.Add(btn);
        buttonsLR += btn.Width;
        if (buttonsLR == tbInp.Width)
        {
            buttonsUD += btn.Height;
            buttonsLR = 0;
        }
        letterButtons.Add(btn);

        btn = new Button();
        btn.Text = "Ė";
        btn.Location = new Point(buttonsLR, buttonsUD + 10);
        btn.Size = new Size(40, 30);
        btn.Click += ev;
        this.Controls.Add(btn);
        buttonsLR += btn.Width;
        if (buttonsLR == tbInp.Width)
        {
            buttonsUD += btn.Height;
            buttonsLR = 0;
        }
        letterButtons.Add(btn);

        btn = new Button();
        btn.Text = "Į";
        btn.Location = new Point(buttonsLR, buttonsUD + 10);
        btn.Size = new Size(40, 30);
        btn.Click += ev;
        this.Controls.Add(btn);
        buttonsLR += btn.Width;
        if (buttonsLR == tbInp.Width)
        {
            buttonsUD += btn.Height;
            buttonsLR = 0;
        }
        letterButtons.Add(btn);

        btn = new Button();
        btn.Text = "Š";
        btn.Location = new Point(buttonsLR, buttonsUD + 10);
        btn.Size = new Size(40, 30);
        btn.Click += ev;
        this.Controls.Add(btn);
        buttonsLR += btn.Width;
        if (buttonsLR == tbInp.Width)
        {
            buttonsUD += btn.Height;
            buttonsLR = 0;
        }
        letterButtons.Add(btn);

        btn = new Button();
        btn.Text = "Ų";
        btn.Location = new Point(buttonsLR, buttonsUD + 10);
        btn.Size = new Size(40, 30);
        btn.Click += ev;
        this.Controls.Add(btn);
        buttonsLR += btn.Width;
        if (buttonsLR == tbInp.Width)
        {
            buttonsUD += btn.Height;
            buttonsLR = 0;
        }
        letterButtons.Add(btn);

        btn = new Button();
        btn.Text = "Ū";
        btn.Location = new Point(buttonsLR, buttonsUD + 10);
        btn.Size = new Size(40, 30);
        btn.Click += ev;
        this.Controls.Add(btn);
        buttonsLR += btn.Width;
        if (buttonsLR == tbInp.Width)
        {
            buttonsUD += btn.Height;
            buttonsLR = 0;
        }
        letterButtons.Add(btn);

        btn = new Button();
        btn.Text = "Ž";
        btn.Location = new Point(buttonsLR, buttonsUD + 10);
        btn.Size = new Size(40, 30);
        btn.Click += ev;
        this.Controls.Add(btn);
        buttonsLR += btn.Width;
        letterButtons.Add(btn);
    }
}