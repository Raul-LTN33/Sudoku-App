using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data.SqlServerCe;

namespace Tester2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            CustomizeMenu();
            ArrangeLayout();
            InitializeSudokuGrid();
            InitializeTimer();
        }

        private bool isPasswordVisible = false;
        private bool isGameEnded=false;
        private bool isGameStarted = false;
        private string CurrentUsername;
        private DateTime startTime;
        private SudokuCell[,] cells = new SudokuCell[9, 9];
        private Random random = new Random();
        private SudokuCell selectedCell;
        class SudokuCell : Button
        {
            public int Value { get; set; }
            public bool IsLocked { get; set; }
            public int X { get; set; }
            public int Y { get; set; }

            public void Clear()
            {
                this.Text = string.Empty;
                this.IsLocked = false;
            }
        }

        private void CustomizeMenu()
        {
            this.BackColor = Color.Beige;
            Color lightGrey = Color.FromArgb(220, 220, 220);

            tabPage1.BackColor = lightGrey;
            tabPage2.BackColor = lightGrey;
            tabPage3.BackColor = lightGrey;
            tabPage4.BackColor = lightGrey;
            tabPage5.BackColor = lightGrey;

            tabControl1.Appearance = TabAppearance.FlatButtons;
            tabControl1.ItemSize = new Size(0, 1);
            tabControl1.SizeMode = TabSizeMode.Fixed;
            tabControl1.TabStop = false;

            CustomizeButton(button1);
            CustomizeButton(button2);
            CustomizeButton(button3);
            CustomizeButton(button4);
            CustomizeButton(button5);
            CustomizeButton(button6);
            CustomizeButton(button7);
            CustomizeButton(button8);
            CustomizeButton(button9);
            CustomizeButton(button10);
            CustomizeButton(button11);
            CustomizeButton(button12);
            CustomizeButton(button13);
            CustomizeButton(button14);
            CustomizeButton(button15);
            button1.Text = "Conectare";
            button2.Text = "Înregistrare";
            button3.Text = "Conectare";
            button4.Text = "Arată parola";
            button5.Text = "Închidere aplicație";
            button6.Text = "Înregistrare";
            button7.Text = "Arată parola";
            button8.Text = "Pagina principală";
            button9.Text = "Pagina principală";
            button10.Text="Verifică datele";
            button11.Text = "Golește caseta";
            button12.Text = "Pagina principală";
            button13.Text = "Începe joc nou";
            button14.Text = "Clasament";
            button15.Text = "Pagina principală";
        }

        private void CustomizeButton(Button button)
        {
            button.BackColor = Color.FromArgb(0, 122, 204);
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button.TextAlign = ContentAlignment.MiddleCenter;
            button.AutoSize = false;
            button.Size = new Size(150, 30);
        }

        private void ArrangeLayout()
        {
            // Tab 1: Main Menu
            label2.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Text = "Sudoku";
            label2.AutoSize = true;
            label2.Location = new Point((tabPage1.Width - label2.Width) / 2, 100);

            CenterButton(button1, tabPage1, label2.Bottom + 50);
            CenterButton(button2, tabPage1, button1.Bottom + 20);
            CenterButton(button5, tabPage1, button2.Bottom + 20);
            CenterButton(button14, tabPage1, button5.Bottom + 20);

            // Tab 2: Login
            label3.Text = "Introdu numele de utilizator:";
            label3.AutoSize = true;
            label3.Location = new Point((tabPage2.Width - textBox1.Width) / 2 - 100, 100);

            textBox1.Location = new Point((tabPage2.Width - textBox1.Width) / 2 - 100, label3.Bottom + 10);
            textBox1.Width = 200;

            label4.Text = "Introdu parola:";
            label4.AutoSize = true;
            label4.Location = new Point((tabPage2.Width - textBox2.Width) / 2 - 100, textBox1.Bottom + 20);

            textBox2.Location = new Point((tabPage2.Width - textBox2.Width) / 2 - 100, label4.Bottom + 10);
            textBox2.Width = 200;
            textBox2.PasswordChar = '*';

            button4.Text = "Arată parola";
            button4.Location = new Point(textBox2.Right + 10, textBox2.Top);

            CenterButton(button3, tabPage2, textBox2.Bottom + 40);
            CenterButton(button8, tabPage2, button3.Bottom + 20);

            // Tab 3: Register
            label5.Text = "Introdu numele de utilizator:";
            label5.AutoSize = true;
            label5.Location = new Point((tabPage3.Width - textBox3.Width) / 2 - 100, label5.Top);

            textBox3.Location = new Point((tabPage3.Width - textBox3.Width) / 2 - 100, label5.Bottom + 10);
            textBox3.Width = 200;

            label6.Text = "Introdu parola (trebuie să conțină cel puțin 8 caractere, o literă mică și una mare, o cifră și un caracter special):";
            label6.AutoSize = true;
            label6.Location = new Point(textBox3.Left, label6.Top);

            textBox4.Location = new Point((tabPage3.Width - textBox4.Width) / 2 - 100, label6.Bottom + 10);
            textBox4.Width = 200;
            textBox4.PasswordChar = '*';

            label7.Text = "Confirmă parola:";
            label7.AutoSize = true;
            label7.Location = new Point(textBox4.Left, label7.Top);

            textBox5.Location = new Point((tabPage3.Width - textBox5.Width) / 2 - 100, label7.Bottom + 10);
            textBox5.Width = 200;
            textBox5.PasswordChar = '*';

            button7.Location = new Point(textBox5.Right + 10, textBox5.Top);

            CenterButton(button6, tabPage3, textBox5.Bottom + 40);
            CenterButton(button9, tabPage3, button6.Bottom + 20);

            // Tab 4: Sudoku
            int buttonRightMargin = 70;
            label8.Text = "Alege nivelul de dificultate";
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            int labelY = 90; // Adjust the Y coordinate here
            label8.Location = new Point(tabPage4.Width - button10.Width - buttonRightMargin, labelY);

            //Tab 5: Leaderboard
            label10.Text = "Clasament";
            label10.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            int labelX = (tabPage5.Width - label10.Width) / 2;
            label10.Location = new Point(labelX, 30);
            int button = (tabPage5.Width - button15.Width) / 2;
            button15.Location = new Point(button, tabPage5.Height - button15.Height - 30);
            int dataGridViewX = (tabPage5.Width - dataGridView1.Width) / 2;
            int dataGridViewY = (tabPage5.Height - dataGridView1.Height) / 2;
            dataGridView1.Location = new Point(dataGridViewX, dataGridViewY);


            // Radio buttons
            Font radioButtonFont = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0); // Increased font size
            radioButton1.Font = radioButtonFont;
            radioButton2.Font = radioButtonFont;
            radioButton3.Font = radioButtonFont;
            int radioY = labelY + label8.Height + 10; // 10 is the spacing between label8 and radio buttons
            radioButton1.Location = new Point(tabPage4.Width - button10.Width - buttonRightMargin, radioY);
            radioButton2.Location = new Point(tabPage4.Width - button10.Width - buttonRightMargin, radioButton1.Bottom + 10);
            radioButton3.Location = new Point(tabPage4.Width - button10.Width - buttonRightMargin, radioButton2.Bottom + 10);
            radioButton1.Text = "Începător";
            radioButton2.Text = "Intermediar";
            radioButton3.Text = "Avansat";

            // Buttons
            int buttonY = radioButton3.Bottom + 20; // 20 is the spacing between radio buttons and buttons
            button10.Location = new Point(tabPage4.Width - button10.Width - buttonRightMargin, buttonY);
            button11.Location = new Point(tabPage4.Width - button11.Width - buttonRightMargin, button10.Bottom + 10);
            button12.Location = new Point(tabPage4.Width - button12.Width - buttonRightMargin, button11.Bottom + 10);
            button13.Location = new Point(tabPage4.Width - button13.Width - buttonRightMargin, button12.Bottom + 10);
        }

        private void CenterButton(Button button, TabPage tabPage, int top)
        {
            button.Left = (tabPage.Width - button.Width) / 2;
            button.Top = top;
        }


        private bool IsPasswordStrong(string password)
        {
            // At least 8 characters long
            if (password.Length < 8)
                return false;

            // Contains uppercase letters
            if (!Regex.IsMatch(password, @"[A-Z]"))
                return false;

            // Contains lowercase letters
            if (!Regex.IsMatch(password, @"[a-z]"))
                return false;

            // Contains digits
            if (!Regex.IsMatch(password, @"[0-9]"))
                return false;

            // Contains special characters
            if (!Regex.IsMatch(password, @"[\W_]"))
                return false;

            return true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            // Get the username, password, and confirm password from the TextBoxes
            string username = textBox3.Text;
            string password = textBox4.Text;
            string confirmPassword = textBox5.Text;

            // Collect error messages
            StringBuilder errorMessage = new StringBuilder();

            // Check if the username is empty
            if (string.IsNullOrWhiteSpace(username))
            {
                errorMessage.AppendLine("Nume de utilizator invalid. Introdu un alt nume.");
            }

            //Check if username is already used
            userTableAdapter.Fill(databaseDataSet.User);
            DataTable d = databaseDataSet.User;
            bool ok = true;
            for (int i = 0; i < d.Rows.Count; i++)
            {
                if (d.Rows[i]["Username"].ToString() == username)
                    ok = false;
            }
            if (!ok)
            {
                errorMessage.AppendLine("Numele de utilizator există deja.");
            }

            // Check if the password is strong enough
            if (!IsPasswordStrong(password))
            {
                errorMessage.AppendLine("Parola trebuie să aibă cel puțin 8 caractere și să includă litere mari, litere mici, cifre și caractere speciale.");
            }

            // Check if the passwords match
            if (password != confirmPassword)
            {
                errorMessage.AppendLine("Parolele nu se potrivesc. Reintroduceți parola.");
            }

            // If there are any errors, show the message box and return
            if (errorMessage.Length > 0)
            {
                MessageBox.Show(errorMessage.ToString(), "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            else
            {
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                userTableAdapter.Insert(username, password, "00:00");
                MessageBox.Show("Cont creat cu succes.");
                CurrentUsername = username;
                tabControl1.SelectedTab = tabPage4;
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            // Toggle password visibility
            if (isPasswordVisible)
            {
                // Hide the password
                textBox4.PasswordChar = '*';
                textBox5.PasswordChar = '*';
                button7.Text = "Arată parola";
                isPasswordVisible = false;
            }
            else
            {
                // Show the password
                textBox4.PasswordChar = '\0';
                textBox5.PasswordChar = '\0';
                button7.Text = "Ascunde parola";
                isPasswordVisible = true;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage2;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage3;
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;
            userTableAdapter.Fill(databaseDataSet.User);
            DataTable d = databaseDataSet.User;
            bool ok = false;
            for (int i = 0; i < d.Rows.Count; i++)
            {
                if (d.Rows[i]["Username"].ToString() == username && d.Rows[i]["Password"].ToString()==password)
                    ok = true;
            }
            if (ok)
            {
                MessageBox.Show("Conectat cu succes.");
                CurrentUsername = username;
                textBox1.Text = "";
                textBox2.Text = "";
                tabControl1.SelectedTab = tabPage4;
            }
            else
            {
                MessageBox.Show("Date invalide.", "Eroare", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            // Toggle password visibility
            if (isPasswordVisible)
            {
                // Hide the password
                textBox2.PasswordChar = '*';
                button4.Text = "Arată parola";
                isPasswordVisible = false;
            }
            else
            {
                // Show the password
                textBox2.PasswordChar = '\0';
                button4.Text = "Ascunde parola";
                isPasswordVisible = true;
            }
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            // Toggle password visibility
            if (isPasswordVisible)
            {
                // Hide the password
                textBox4.PasswordChar = '*';
                textBox5.PasswordChar = '*';
                button7.Text = "Arată parola";
                isPasswordVisible = false;
            }
            else
            {
                // Show the password
                textBox4.PasswordChar = '\0';
                textBox5.PasswordChar = '\0';
                button7.Text = "Ascunde parola";
                isPasswordVisible = true;
            }
        }

        private void button8_Click_1(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            tabControl1.SelectedTab = tabPage1;
        }

        private void button9_Click_1(object sender, EventArgs e)
        {
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
            tabControl1.SelectedTab = tabPage1;
        }

        //Sudoku
        #region Initialization
        private void InitializeSudokuGrid()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    cells[i, j] = new SudokuCell
                    {
                        Font = new Font(SystemFonts.DefaultFont.FontFamily, 20),
                        Size = new Size(40, 40),
                        ForeColor = SystemColors.ControlDarkDark,
                        Location = new Point(i * 40, j * 40),
                        BackColor = ((i / 3) + (j / 3)) % 2 == 0 ? SystemColors.Control : Color.LightGray,
                        FlatStyle = FlatStyle.Flat,
                        FlatAppearance = { BorderColor = Color.Black },
                        X = i,
                        Y = j
                    };

                    cells[i, j].Click += Cell_Click;
                    cells[i, j].KeyPress += Cell_KeyPressed;
                    panel1.Controls.Add(cells[i, j]);
                }
            }
        }

        private void InitializeTimer()
        {
            timer1.Interval = 1000; // Set the interval to 1 second
            timer1.Tick += Timer1_Tick;
            label1.Text = "00:00"; // Initialize time display
            label1.AutoSize = true;
            label1.Font = new Font(label1.Font.FontFamily, 24, label1.Font.Style); // Increase font size
            label1.Location = new Point(this.ClientSize.Width - label1.Width - 10, 10); // Top right corner
            label1.TextAlign = ContentAlignment.TopRight;
        }
        #endregion

        #region Game Logic

        private bool IsDifficultySelected()
        {
            return radioButton1.Checked || radioButton2.Checked || radioButton3.Checked;
        }

        private void StartNewGame()
        {
            if (!IsDifficultySelected())
            {
                MessageBox.Show("Alege un nivel de dificultate.", "Sudoku", MessageBoxButtons.OK, 
                MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000); 
                return;
            }
            StartTimer();
            LoadValues();
            int hintsCount = GetHintsCount();
            ShowRandomValueHints(hintsCount);
        }

        private int GetHintsCount()
        {
            if (radioButton1.Checked) return 45;
            if (radioButton2.Checked) return 30;
            if (radioButton3.Checked) return 15;
            return 0;
        }

        private void LoadValues()
        {
            foreach (var cell in cells)
            {
                cell.Value = 0;
                cell.Clear();
            }
            GenerateSudokuSolution(0, -1);
        }

        private bool GenerateSudokuSolution(int row, int col)
        {
            if (++col > 8)
            {
                col = 0;
                if (++row > 8) return true;
            }

            var nums = Enumerable.Range(1, 9).OrderBy(n => random.Next()).ToList();
            foreach (var num in nums)
            {
                if (IsValidNumber(num, row, col))
                {
                    cells[row, col].Value = num;
                    if (GenerateSudokuSolution(row, col)) return true;
                    cells[row, col].Value = 0;
                }
            }
            return false;
        }

        private bool IsValidNumber(int value, int row, int col)
        {
            for (int i = 0; i < 9; i++)
            {
                if (cells[row, i].Value == value || cells[i, col].Value == value)
                    return false;
            }

            int startRow = row / 3 * 3;
            int startCol = col / 3 * 3;
            for (int i = startRow; i < startRow + 3; i++)
            {
                for (int j = startCol; j < startCol + 3; j++)
                {
                    if (cells[i, j].Value == value)
                        return false;
                }
            }
            return true;
        }

        private void ShowRandomValueHints(int hintsCount)
        {
            for (int i = 0; i < hintsCount; i++)
            {
                int rX, rY;
                do
                {
                    rX = random.Next(9);
                    rY = random.Next(9);
                } while (cells[rX, rY].IsLocked);

                cells[rX, rY].Text = cells[rX, rY].Value.ToString();
                cells[rX, rY].ForeColor = Color.Black;
                cells[rX, rY].IsLocked = true;
            }
        }

        private void EndGame()
        {
            // Stop the timer
            StopTimer();

            // Clear the Sudoku grid
            foreach (var cell in cells)
            {
                cell.Clear();
            }

            // Reset the timer display
            label1.Text = "00:00";

        }
        #endregion

        #region Timer Logic
        private void StartTimer()
        {
            startTime = DateTime.Now;
            timer1.Start();
        }

        private void StopTimer()
        {
            timer1.Stop();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            TimeSpan elapsed = DateTime.Now - startTime;
            label1.Text = string.Format("{0:mm\\:ss}", elapsed); // Display minutes and seconds only
        }
        #endregion

        #region Event Handlers
        private void Cell_KeyPressed(object sender, KeyPressEventArgs e)
        {
            var cell = sender as SudokuCell;
            if (cell.IsLocked) return;

            int value;
            if (int.TryParse(e.KeyChar.ToString(), out value))
            {
                if (value == 0)
                    cell.Clear();
                else
                    cell.Text = value.ToString();

                cell.ForeColor = SystemColors.ControlDarkDark;
            }
        }

        private void Cell_Click(object sender, EventArgs e)
        {
            if (selectedCell != null)
            {
                selectedCell.BackColor = ((selectedCell.X / 3) + (selectedCell.Y / 3)) % 2 == 0 ? SystemColors.Control : Color.LightGray;
            }

            selectedCell = sender as SudokuCell;
            selectedCell.BackColor = Color.LightBlue;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            var wrongCells = new List<SudokuCell>();

            foreach (var cell in cells)
            {
                if (!string.Equals(cell.Value.ToString(), cell.Text) && !cell.IsLocked)
                {
                    cell.ForeColor = Color.Red; // Color wrong and unlocked cells red
                    wrongCells.Add(cell);
                }
                else if (!cell.IsLocked)
                {
                    cell.ForeColor = SystemColors.ControlDarkDark; // Reset color of correct and unlocked cells
                }
            }

            if (wrongCells.Any())
            {
                MessageBox.Show("Date invalide", "Sudoku", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000); 
            }
            else
            {
                StopTimer();
                MessageBox.Show(string.Format("Ai rezolvat corect! Timp: {0}", label1.Text), "Sudoku", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, (MessageBoxOptions)0x40000);
                UpdateHighScore(label1.Text);
                isGameEnded = true;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (selectedCell != null && !selectedCell.IsLocked)
            {
                selectedCell.Clear();
                selectedCell.BackColor = ((selectedCell.X / 3) + (selectedCell.Y / 3)) % 2 == 0 ? SystemColors.Control : Color.LightGray;
                selectedCell = null;
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (isGameStarted == false)
            {
                tabControl1.SelectedTab = tabPage1;
                foreach (var cell in cells)
                {
                    cell.Clear();
                }
            }
            else
            {
                if (isGameEnded == false)
                {
                    isGameStarted = false;
                    var result = MessageBox.Show("Ești sigur că vrei să închei jocul? Vei pierde tot progresul.", "Sfârșește jocul", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2, (MessageBoxOptions)0x40000);
                    if (result == DialogResult.Yes)
                    {
                        EndGame();
                        tabControl1.SelectedTab = tabPage1;
                    }
                }
                else
                {
                    isGameStarted = false;
                    tabControl1.SelectedTab = tabPage1;
                    foreach (var cell in cells)
                    {
                        cell.Clear();
                    }
                }
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            StartNewGame();
            isGameStarted = true;
            isGameEnded = false;
        }

        private string GetShorterTimer(string timer1, string timer2)
        {
            TimeSpan timeSpan1 = ParseTimer(timer1);
            TimeSpan timeSpan2 = ParseTimer(timer2);

            return timeSpan1 < timeSpan2 ? timer1 : timer2;
        }

        private TimeSpan ParseTimer(string timer)
        {
            string[] parts = timer.Split(':');
            int minutes = int.Parse(parts[0]);
            int seconds = int.Parse(parts[1]);
            return new TimeSpan(0, minutes, seconds);
        }

        public void UpdateHighScore(string userTimer)
        {
            // Get the current high score from the database
            userTableAdapter.Fill(databaseDataSet.User);
            DataTable d = databaseDataSet.User;
            string time="";
            for (int i = 0; i < d.Rows.Count; i++)
            {
                if (d.Rows[i]["Username"].ToString() == CurrentUsername)
                    time = d.Rows[i]["Highscore"].ToString();
            }
            if (time == "00:00")
            {
                userTableAdapter.Update1(label1.Text,CurrentUsername);
            }
            else
            {
                string besttime = GetShorterTimer(time, label1.Text);
                userTableAdapter.Update1(besttime,CurrentUsername);
            }
        }
        #endregion

        private void button14_Click(object sender, EventArgs e)
        {
            userTableAdapter.Clasament(databaseDataSet.User);
            DataTable d = databaseDataSet.User;
            DataTable leaderboardData = new DataTable();
            leaderboardData.Columns.Add("Loc", typeof(int));
            leaderboardData.Columns.Add("Nume de utilizator", typeof(string));
            leaderboardData.Columns.Add("Timp", typeof(string));
            int loc = 1;
            for (int i = 0; i < d.Rows.Count; i++)
            {
                if (d.Rows[i]["Highscore"].ToString() != "00:00")
                {
                    leaderboardData.Rows.Add(loc, d.Rows[i]["Username"].ToString(), d.Rows[i]["Highscore"].ToString());
                }
                else
                {
                    loc--;
                }
                loc++;
            }
            
            dataGridView1.DataSource = leaderboardData;
            tabControl1.SelectedTab = tabPage5;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage1;
        }


    }
}
