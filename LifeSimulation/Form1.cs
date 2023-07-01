using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LifeSimulation
{
    public partial class Form1 : Form
    {
        private Graphics graphics;
        private int resolution = 7;
        private string[,] field;
        private Area area;
        private MultiGrass multiGrass;
        private Grass grass;
        private Mushroom mushroom;
        private Predator predator;
        private Cancer cancer;
        private bool predatorFlag;
        private bool cancerFlag;
        private int rows;
        private int cols;
        private int numberOfGeneration;
        public Form1()
        {
            InitializeComponent();
        }
        
        private void StartSimulation()
        {
            if (timer1.Enabled)
                return;
            nudDensity.Enabled = false;
            cbPredator.Enabled = false;
            cbCancer.Enabled = false;
            buttonApply.Enabled = true;
            rows = pictureBox1.Height / resolution;
            cols = pictureBox1.Width / resolution;
            field = new string[cols, rows];
            area = new Area();
            multiGrass = new MultiGrass();
            grass = new Grass();
            mushroom = new Mushroom();
            predator = new Predator();
            cancer = new Cancer();
            //Генерация первого поколения
            Random random = new Random();
            for (int x = 0; x < cols; x++)
            {
                for(int y = 0; y < rows; y++)
                {
                    switch (random.Next(100 - (int)nudDensity.Value))
                    {
                        case 0: field[x, y] = "Grass"; break;
                        case 1 when predatorFlag: field[x, y] = "Predator"; break;
                        case 2: field[x, y] = "MultiGrass"; break;
                        case 3: field[x, y] = "Mushroom"; break;
                        case 4 when cancerFlag: field[x, y] = "Cancer"; break;
                        default: field[x, y] = "Empty"; break;
                    }
                }
            }
            numberOfGeneration = 1;
            //Считывание условий среды 
            ApplyButton();
            //Отрисовка первого поколения
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(pictureBox1.Image);
            timer1.Start();
        }

        private void StopSimulation()
        {
            if (!timer1.Enabled)
                return;
            timer1.Stop();
            //Активируем элементы интерфейса
            nudDensity.Enabled = true;
            cbCancer.Enabled = true;
            cbPredator.Enabled = true;
            MessageBox.Show("Экосистема вымерла или симуляция была остановлена.", "Симуляция окончена.", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void NextGeneration()
        {
            graphics.Clear(Color.LightGray);
            var newField = new string[cols, rows];
            int count = 0;
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    // подсчет соседей данной ячейки, соответствующими методами
                    var granei = CountNeighbours(x, y, "Grass");
                    var mulnei = CountNeighbours(x, y, "MultiGrass");
                    var mushnei = CountNeighbours(x, y, "Mushroom");
                    var cannei = CountNeighbours(x, y, "Cancer");
                    var prednei = CountNeighbours(x, y, "Predator");
                    var hasLife = field[x, y];
                    //Считаем количество не раковых и не пустых клеток
                    if (hasLife != "Empty" && hasLife != "Cancer")
                        count++;
                    //Проверяем не сгорело ли все
                    if(area.Burnt())
                        newField[x, y] = "Empty";
                    //Алгоритм действия рака
                    else if (hasLife != "Empty" && cannei > 0)
                    {
                        if(hasLife == "Predator" && cancer.CanCancered(predator.Immunity))
                            newField[x, y] = "Cancer";
                        else if (hasLife == "Grass" && cancer.CanCancered(grass.Immunity))
                            newField[x, y] = "Cancer";
                        else if (hasLife == "MultiGrass" && cancer.CanCancered(multiGrass.Immunity))
                            newField[x, y] = "Cancer";
                        else if (hasLife == "Mushroom" && cancer.CanCancered(mushroom.Immunity))
                            newField[x, y] = "Cancer";
                        else if(hasLife == "Cancer" && !cancer.IsAlive()) newField[x, y] = "Empty";
                        else newField[x, y] = field[x, y];
                    }
                    //Алгоритм действий хищника
                    else if(hasLife != "Empty" && prednei > 0)
                    {
                        if (hasLife == "Grass" && predator.CanEating(area.Energy))
                            newField[x, y] = "Predator";
                        else if (hasLife == "MultiGrass" && predator.CanEating((area.Energy + area.Organic)/2))
                            newField[x, y] = "Predator";
                        else if (hasLife == "Mushroom" && predator.CanEating(area.Organic))
                            newField[x, y] = "Predator";
                        else if (hasLife == "Predator" && !predator.IsAlive()) newField[x, y] = "Empty";
                        else newField[x, y] = field[x, y];
                    }
                    else if(hasLife == "Predator" && prednei > 5)
                    {
                        newField[x, y] = "Empty";
                    }
                    //Алгоритм действий травы
                    else if (hasLife == "Empty" && granei == 3 || (granei > mulnei && granei > mushnei))
                    {
                        if(grass.IsAlive() && grass.Live((int)area.Energy))
                            newField[x, y] = "Grass";
                    }
                    //Алгорит действий травы-гриба
                    else if(hasLife == "Empty" && mulnei == 3 || (mulnei > granei && mulnei > mushnei)){
                        if (multiGrass.IsAlive() && multiGrass.Live((int)area.Energy, (int)area.Organic))
                            newField[x, y] = "MultiGrass";
                    }
                    //Алгоритм действий гриба-падальщика
                    else if (hasLife == "Empty" && mushnei == 3 || (mushnei > granei && mushnei > mulnei))
                    {
                        if (mushroom.IsAlive() && mushroom.Live((int)area.Organic))
                            newField[x, y] = "Mushroom";
                    }
                    //Алгоритм освобождения клеток
                    else if(hasLife != "Empty" && (mushnei < 2|| mushnei >3|| mulnei < 2 || mulnei > 3 || granei < 2 || granei > 3))
                    {
                        newField[x, y] = "Empty";
                    }
                    else if (hasLife != "Empty")
                    {
                        newField[x, y] = field[x, y];
                    }
                    //Обрабатываем "жизнь" хищников в текущем поколении
                    cancer.Live(count);
                    predator.Live(count);
                    //Отрисовка текущего поколения
                    switch (hasLife)
                    {
                        case "Predator": graphics.FillRectangle(Brushes.Red, x * resolution, y * resolution, resolution - 1, resolution - 1); break;
                        case "MultiGrass": graphics.FillRectangle(Brushes.SpringGreen, x * resolution, y * resolution, resolution - 1, resolution - 1); break;
                        case "Mushroom": graphics.FillRectangle(Brushes.Blue, x * resolution, y * resolution, resolution - 1, resolution - 1); break;
                        case "Cancer": graphics.FillRectangle(Brushes.Black, x * resolution, y * resolution, resolution - 1, resolution - 1); break;
                        case "Grass": graphics.FillRectangle(Brushes.Green, x * resolution, y * resolution, resolution - 1, resolution - 1); break;
                        default: break;
                    }
                }
            }
            field = newField;
            pictureBox1.Refresh();
        }


        //Метод для подсчета ныне живущих и номера поколения 
        private void CheckSimInfo()
        {
            int countPredator = 0;
            int countGrass = 0;
            int countMultiGrass = 0;
            int countCancer = 0;
            int countMushroom = 0;
            for (int x = 0; x < cols; x++)
            {
                for (int y = 0; y < rows; y++)
                {
                    switch (field[x, y])
                    {
                        case "Grass": countGrass++; break;
                        case "Predator": countPredator++; break;
                        case "MultiGrass": countMultiGrass++; break;
                        case "Mushroom": countMushroom++; break;
                        case "Cancer":countCancer++; break;
                        default: break;
                    }
                }
            }
            numberOfGeneration++;
            labelCancer.Text = "Численность " + countCancer;
            labelGeneration.Text = "Номер поколения " + numberOfGeneration;
            labelGrass.Text = "Численность " + countGrass;
            labelMultiGrass.Text = "Численность " + countMultiGrass;
            labelMushroom.Text = "Численность " + countMushroom;
            labelPredator.Text = "Численность " + countPredator;
            if(countGrass == 0 && countMultiGrass == 0 && countMushroom == 0)
            {
                graphics.Clear(Color.LightGray);
                pictureBox1.Refresh();
                StopSimulation();
            }
        }

        //Метод подсчета соседей
        private int CountNeighbours(int x, int y, string neighboursType)
        {
            int count = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    var col = (x + i + cols) % cols;
                    var row = (y + j + rows) % rows;
                    var isSelfChecking = col == x && row == y;
                    var hasLife = field[col, row];
                    if (hasLife == neighboursType && !isSelfChecking)
                        count++;
                }
            }
            return count;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            NextGeneration();
            CheckSimInfo();
        }
        private void buttonStart_Click(object sender, EventArgs e)
        {
            StartSimulation();
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            StopSimulation();
        }

        private void rbSpeed1_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Interval = 400;
        }

        private void rbSpeed2_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Interval = 200;
        }

        private void rbSpeed3_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Interval = 100;
        }

        private void cbPredator_CheckedChanged(object sender, EventArgs e)
        {
            if (cbPredator.Checked)
                predatorFlag = true;
            else predatorFlag = false;
        }

        private void cbCancer_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCancer.Checked)
            {
                groupBox4.Enabled = true;
                cancerFlag = true;
            }
            else 
            {
                groupBox4.Enabled = false;
                cancerFlag = false; 
            }
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            ApplyButton();
        }

        //Метод для изменения состояния среды
        private void ApplyButton()
        {
            area.Organic = (uint)nudOrganic.Value;
            area.Energy = (uint)nudEnergy.Value;
            predator.Immunity = (uint)nudPredator.Value;
            grass.Immunity = (uint)nudGrass.Value;
            multiGrass.Immunity = (uint)nudMultiGrass.Value;
            mushroom.Immunity = (uint)nudMushroom.Value;
        }
    }
}
