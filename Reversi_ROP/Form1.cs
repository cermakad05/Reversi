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

        FlowLayoutPanel horniPanel = new FlowLayoutPanel();
        Panel hraciDeska = new Panel();

        public Form1()
        {
            InitializeComponent();

            Text = "Reversi";
            Size = new Size(800, 700);
            MinimumSize = new Size(450, 450);
            BackColor = Color.FromArgb(40, 45, 50);

           
            horniPanel.Dock = DockStyle.Top;
            horniPanel.Height = 55;
            horniPanel.Padding = new Padding(10, 12, 0, 0);
            horniPanel.WrapContents = false;

            lblCerny.AutoSize = true;
            lblCerny.ForeColor = Color.White;
            lblCerny.Margin = new Padding(5, 5, 20, 0);

            lblBily.AutoSize = true;
            lblBily.ForeColor = Color.White;
            lblBily.Margin = new Padding(5, 5, 20, 0);

            lblTah.Text = "Na tahu: Černý";
            lblTah.AutoSize = true;
            lblTah.ForeColor = Color.White;
            lblTah.Margin = new Padding(5, 5, 0, 0);

            horniPanel.Controls.Add(lblCerny);
            horniPanel.Controls.Add(lblBily);
            horniPanel.Controls.Add(lblTah);
            Controls.Add(horniPanel);

          
            hraciDeska.BackColor = Color.ForestGreen;
            hraciDeska.Paint += VykresliDesku;
            Controls.Add(hraciDeska);

            Resize += ZmenaVelikosti;

            AktualizujSkore();
            UpravVelikostDesky();
        }

        private void ZmenaVelikosti(object sender, EventArgs e)
        {
            UpravVelikostDesky();
        }

        private void UpravVelikostDesky()
        {
            int volnaVyska = ClientSize.Height - horniPanel.Height;
            int velikost = Math.Max(8, Math.Min(ClientSize.Width - 30, volnaVyska - 30));

            hraciDeska.Size = new Size(velikost, velikost);
            hraciDeska.Location = new Point(
                (ClientSize.Width - velikost) / 2,
                horniPanel.Height + (volnaVyska - velikost) / 2);

            hraciDeska.Invalidate();
        }

        private void VykresliDesku(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            float velikost = hraciDeska.Width / 8f;
            float okraj = velikost * 0.1f;

            for (int radek = 0; radek < 8; radek++)
            {
                for (int sloupec = 0; sloupec < 8; sloupec++)
                {
                    float x = sloupec * velikost;
                    float y = radek * velikost;

                   
                    if ((radek + sloupec) % 2 == 0)
                        g.FillRectangle(Brushes.SeaGreen, x, y, velikost, velikost);
                    else
                        g.FillRectangle(Brushes.ForestGreen, x, y, velikost, velikost);

                    g.DrawRectangle(Pens.Black, x, y, velikost, velikost);

                    
                    StavKamene stav = deska.Policka[radek, sloupec].Stav;

                    if (stav == StavKamene.Cerny)
                        g.FillEllipse(Brushes.Black, x + okraj, y + okraj,
                            velikost - 2 * okraj, velikost - 2 * okraj);

                    if (stav == StavKamene.Bily)
                        g.FillEllipse(Brushes.White, x + okraj, y + okraj,
                            velikost - 2 * okraj, velikost - 2 * okraj);
                }
            }
        }

        private void AktualizujSkore()
        {
            lblCerny.Text = "Černý: " + deska.SpocitejKameny(StavKamene.Cerny);
            lblBily.Text = "Bílý: " + deska.SpocitejKameny(StavKamene.Bily);
        }
    }
}