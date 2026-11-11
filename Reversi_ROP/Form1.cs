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

        StavKamene hracNaTahu = StavKamene.Cerny;

        Label lblCerny = new Label();
        Label lblBily = new Label();
        Label lblTah = new Label();

        FlowLayoutPanel horniPanel = new FlowLayoutPanel();
        HerniPanel hraciDeska = new HerniPanel();

        Panel panelTah = new Panel();
        HerniPanel koleckoTah = new HerniPanel();

        public Form1()
        {
            InitializeComponent();

            Text = "Reversi";
            Size = new Size(800, 700);
            MinimumSize = new Size(450, 450);
            BackColor = Color.FromArgb(40, 45, 50);

      
            horniPanel.Dock = DockStyle.Top;
            horniPanel.Height = 60;
            horniPanel.Padding = new Padding(15, 12, 0, 0);
            horniPanel.WrapContents = false;

            lblCerny.AutoSize = true;
            lblCerny.ForeColor = Color.White;
            lblCerny.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblCerny.Margin = new Padding(5, 8, 25, 0);

            lblBily.AutoSize = true;
            lblBily.ForeColor = Color.White;
            lblBily.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblBily.Margin = new Padding(5, 8, 25, 0);

          
            panelTah.Size = new Size(150, 35);
            panelTah.BackColor = Color.FromArgb(35, 93, 72);
            panelTah.Margin = new Padding(5, 0, 0, 0);

            koleckoTah.Size = new Size(20, 20);
            koleckoTah.Location = new Point(12, 8);
            koleckoTah.Paint += VykresliKolecko;

            lblTah.Text = "Na tahu";
            lblTah.AutoSize = true;
            lblTah.ForeColor = Color.White;
            lblTah.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            lblTah.Location = new Point(42, 7);

            panelTah.Controls.Add(koleckoTah);
            panelTah.Controls.Add(lblTah);

            horniPanel.Controls.Add(lblCerny);
            horniPanel.Controls.Add(lblBily);
            horniPanel.Controls.Add(panelTah);
            Controls.Add(horniPanel);

          
            hraciDeska.BackColor = Color.ForestGreen;
            hraciDeska.Paint += VykresliDesku;
            hraciDeska.MouseClick += KliknutiNaDesku;
            Controls.Add(hraciDeska);

            Resize += ZmenaVelikosti;

            AktualizujSkore();
            AktualizujTah();
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

                
                    g.FillRectangle(Brushes.ForestGreen, x, y, velikost, velikost);

                  
                    

                   
                    StavKamene stav = deska.Policka[radek, sloupec].Stav;

                    if (stav != StavKamene.Prazdne)
                    {
                        Brush barva = Brushes.Black;

                        if (stav == StavKamene.Bily)
                            barva = Brushes.White;

                        g.FillEllipse(barva, x + okraj, y + okraj,
                            velikost - 2 * okraj, velikost - 2 * okraj);
                    }
                }
            }
            g.SmoothingMode = SmoothingMode.None;

            for (int i = 1; i < 8; i++)
            {
                float pozice = (float)Math.Round(i * velikost);

                g.DrawLine(Pens.Black, pozice, 0, pozice, hraciDeska.Height);
                g.DrawLine(Pens.Black, 0, pozice, hraciDeska.Width, pozice);
            }
        }

        private void VykresliKolecko(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Brush barva = Brushes.Black;
            Pen okraj = Pens.Gray;

            if (hracNaTahu == StavKamene.Bily)
            {
                barva = Brushes.White;
                okraj = Pens.LightGray;
            }

            g.FillEllipse(barva, 1, 1, 18, 18);
            g.DrawEllipse(okraj, 1, 1, 18, 18);
        }

        private void KliknutiNaDesku(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;

            int sloupec = (int)(e.X / (hraciDeska.Width / 8f));
            int radek = (int)(e.Y / (hraciDeska.Height / 8f));

            if (radek < 0 || radek >= 8 || sloupec < 0 || sloupec >= 8)
                return;

           
            if (!deska.PolozKamen(radek, sloupec, hracNaTahu))
                return;

            
            if (hracNaTahu == StavKamene.Cerny)
                hracNaTahu = StavKamene.Bily;
            else
                hracNaTahu = StavKamene.Cerny;

            AktualizujSkore();
            AktualizujTah();
            hraciDeska.Invalidate();
        }

        private void AktualizujSkore()
        {
            lblCerny.Text = "Černý: " + deska.SpocitejKameny(StavKamene.Cerny);
            lblBily.Text = "Bílý: " + deska.SpocitejKameny(StavKamene.Bily);
        }

        private void AktualizujTah()
        {
            koleckoTah.Invalidate();
        }
    }

    public class HerniPanel : Panel
    {
        public HerniPanel()
        {
            DoubleBuffered = true;
        }
    }
}