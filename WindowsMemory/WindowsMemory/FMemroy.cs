using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using dllLoto;

namespace WindowsMemory
{
    public partial class FMemory : Form
    {
        // Déclaration des variables globales du jeu
        int nbCartesDansSabot;  // Nombre de cartes dans le sabot (en fait nombre
                                // d'images dans le réservoir)
        int nbCartesSurTapis;   // Nombre de cartes sur le tapis

        int[] tImagesCartes;    // On veut une série de nbCartesSurTapis cartes parmi celles 
                                // du réservoir
        int score;

        int nb_cartes;

        bool carte_retournees;

        PictureBox Image_1, Image_2;

        Timer tmr = new Timer();

        public FMemory()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btn_Distribuer_Click(object sender, EventArgs e)
        {
            // On récupère le nombre d'images dans le réservoir :
            nbCartesDansSabot = ilSabotDeCartes.Images.Count - 1;
            // On enlève 1 car :
            // -> la l'image 0 ne compte pas c’est l’image du dos de carte 
            // -> les indices vont de 0 à N-1, donc les indices vont jusqu’à 39
            //    s’il y a 40 images au total *

            // On récupère également le nombre de cartes à distribuées sur la tapis
            // autrement dit le nombre de contrôles présents sur le conteneur
            nbCartesSurTapis = tlpTapisDeCartes.Controls.Count;

            // On effectue la distribution (aléatoire) proprement dite
            Distribution_Aleatoire();

            // On active les boutons retourner   les cartes et jouer
            btn_Jouer.Enabled = true;

            carte_retournees = true;
        }


        private void btn_Test_Click(object sender, EventArgs e)
        {
            // On utilise la LotoMachine pour générer une série aléatoire
            // On fixe à 49 le nombre maxi que retourne la machine
            LotoMachine hasard = new LotoMachine(49);
            // On veut une série de 6 numéros distincts parmi 49 (comme quand on joue au loto)
            int[] tirageLoto = hasard.TirageAleatoire(6, false);
            // false veut dire pas de doublon : une fois qu'une boule est sortie, 
            // elle ne peut pas sortir à nouveau ;-)
            // La série d'entiers retournée par la LotoMachine correspond au loto
            // affiché sur votre écran TV ce soir...
            string grilleLoto = "* ";
            for (int i = 1; i <= 6; i++)
            {
                grilleLoto = grilleLoto + tirageLoto[i] + " * ";
            }
            MessageBox.Show(grilleLoto, "Tirage du LOTO ce jour !");
        }

        private void Distribution_Aleatoire()
        {
            // On utilise la LotoMachine pour générer une série aléatoire
            LotoMachine hasard = new LotoMachine(nbCartesDansSabot);
            // On veut une série de nbCartesSurTapis cartes parmi celles 
            // du réservoir
            tImagesCartes = hasard.TirageAleatoire(nbCartesSurTapis, false);
            // La série d'entiers retournée par la LotoMachine correspondra
            // aux indices des cartes dans le "sabot"

            // Affectation des images aux picturebox
            PictureBox carte, carte2 ;
            int i_image;
            int nb_carteTapis = nbCartesSurTapis;

            for (int i_carte = 0; i_carte != nb_carteTapis ; i_carte++)
            {
      
                carte = (PictureBox)tlpTapisDeCartes.Controls[i_carte];
                carte2 = (PictureBox)tlpTapisDeCartes.Controls[nb_carteTapis-1];

                i_image = tImagesCartes[i_carte + 1]; // i_carte + 1 à cause
                                                      // des pbs d'indices
                carte.Image = ilSabotDeCartes.Images[i_image];
                carte2.Image = ilSabotDeCartes.Images[i_image];

                nb_carteTapis--;
            }
        }

        private void pb_XX_Click(object sender, EventArgs e)
        {
            nb_cartes++;

            PictureBox carte;
            int i_carte, i_image;
            //if (Image_1 == null)
            //    MessageBox.Show("L'image 1 n'est pas référencée");
            //if (Image_2 == null)
            //    MessageBox.Show("L'image 2 n'est pas référencée");

            /*if (nb_cartes < 2)
            {*/
                carte = (PictureBox)sender;
                i_carte = Convert.ToInt32(carte.Tag);
                i_image = tImagesCartes[i_carte];

                RetournerLaCarte(carte, i_carte, i_image);
            /*
                if (nb_cartes == 0)
                {
                    Image_1 = carte;
                }
                else if (nb_cartes == 1)
                {
                    Image_2 = carte;
                }
                RetournerLaCarte(carte);
            }
            else if(nb_cartes == 2)
            {
                /*if (Image_1.Image == Image_2.Image)//if (i_image == i_hasard)
                {
                    MessageBox.Show("Bravo !");
                }
                else
                {
                    MessageBox.Show("DOMMAGE !");
                }
            }
            else
            {
                MessageBox.Show("Deux cartes sont déjà retournées !");
                RetournerLesCartes(sender, e);
                nbCartesSurTapis = 0;
                Image_1 = null;
                Image_2 = null;
            }*/

        }

        private void RetournerLaCarte(PictureBox carte, int i_carte, int i_image)
        {
            carte.Image = ilSabotDeCartes.Images[i_image];
        }

        private void RetournerLesCartes(object sender, EventArgs e)
        {
            if (carte_retournees)
            {
                PictureBox carte;
                int i_image;

                for (int i_carte = 0; i_carte < nbCartesSurTapis; i_carte++)
                {
                    carte = (PictureBox)tlpTapisDeCartes.Controls[i_carte];
                    i_image = tImagesCartes[i_carte]; // i_carte + 1 à cause
                                                // des pbs d'indices
                    carte.Image = ilSabotDeCartes.Images[i_image];
                }
            }
        }

        private void Jouer(object sender, EventArgs e)
        {
            score = 0;
            nb_cartes = 0;
            RetournerLesCartes(sender, e);
            btn_Distribuer.Enabled = false;
            btn_Abandonner.Enabled = true;
            btn_Retourner.Enabled = true;
        }

        private void Abandonner(object sender, EventArgs e)
        {
            btn_Distribuer.Enabled = true;
            btn_Abandonner.Enabled = false;
            btn_Retourner.Enabled = false;
        }

    }

}
