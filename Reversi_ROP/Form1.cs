using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reversi_ROP
{
    public partial class Form1 : Form
    {
        Deska deska = new Deska();

        Label lblCerny = new Label();
        Label lblBily = new Label();
        Label lblTah = new Label();

        Panel hraciDeska = new Panel();

        public Form1()
        {
            InitializeComponent();

            Text = "Reversi";
            Size = new Size(800, 700);
            MinimumSize = new Size(450, 450);

           
            BackColor = Color.FromArgb(40, 45, 50);

            // Skore
            lblCerny.Text = "Cerny: 2";
            lblCerny.Location = new Point(20, 20);
            lblCerny.AutoSize = true;
            lblCerny.ForeColor = Color.White;

            lblBily.Text = "Bily: 2";
            lblBily.Location = new Point(150, 20);
            lblBily.AutoSize = true;
            lblBily.ForeColor = Color.White;

            lblTah.Text = "Na tahu: Cerny";
            lblTah.Location = new Point(280, 20);
            lblTah.AutoSize = true;
            lblTah.ForeColor = Color.White;

            Controls.Add(lblCerny);
            Controls.Add(lblBily);
            Controls.Add(lblTah);

            // Hraci deska
            hraciDeska.BackColor = Color.Green;
            hraciDeska.Paint += VykresliDesku;

            Controls.Add(hraciDeska);

            Resize += ZmenaVelikosti;

            UpravVelikostDesky();
            AktualizujSkore();
        }

        private void ZmenaVelikosti(object sender, EventArgs e)
        {
            UpravVelikostDesky();
        }

        private void UpravVelikostDesky()
        {
            int velikost = Math.Min(ClientSize.Width,
                ClientSize.Height - 60) - 20;

            hraciDeska.Size = new Size(velikost, velikost);

            hraciDeska.Location = new Point(
                (ClientSize.Width - velikost) / 2, 60);

            hraciDeska.Invalidate();
        }

        private void VykresliDesku(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            float velikost = hraciDeska.Width / 8f;

            for (int radek = 0; radek < 8; radek++)
            {
                for (int sloupec = 0; sloupec < 8; sloupec++)
                {
                    float x = sloupec * velikost;
                    float y = radek * velikost;

                    // Stridani barev policek
                    if ((radek + sloupec) % 2 == 0)
                    {
                        g.FillRectangle(Brushes.SeaGreen,
                            x, y, velikost, velikost);
                    }
                    else
                    {
                        g.FillRectangle(Brushes.ForestGreen,
                            x, y, velikost, velikost);
                    }

                    // Policko
                    g.DrawRectangle(Pens.Black, x, y,
                        velikost, velikost);

                    // Kamen
                    StavKamene stav = deska.Policka[radek, sloupec].Stav;

                    if (stav == StavKamene.Cerny)
                    {
                        g.FillEllipse(Brushes.Black,
                            x + 5, y + 5,
                            velikost - 10, velikost - 10);
                    }

                    if (stav == StavKamene.Bily)
                    {
                        g.FillEllipse(Brushes.White,
                            x + 5, y + 5,
                            velikost - 10, velikost - 10);
                    }
                }
            }
        }

        private void AktualizujSkore()
        {
            lblCerny.Text = "Cerny: " +
                deska.SpocitejKameny(StavKamene.Cerny);

            lblBily.Text = "Bily: " +
                deska.SpocitejKameny(StavKamene.Bily);
        }
    }
}