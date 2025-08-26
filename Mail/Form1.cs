using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mail
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-0NNHPCV\SQLEXPRESS;Initial Catalog=REHBER;Integrated Security=True");
        void listele()
        {
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM KISILER", baglanti);
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
        void temizle()
        {
            TxtId.Text = "";
            TxtAd.Text = "";
            TxtSoyad.Text = "";
            TxtMail.Text = "";
            MskTel.Text = "";
            TxtAd.Focus();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            listele();
        }

        private void BtnEkle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("insert into KISILER (AD,SOYAD,TELEFON,MAIL) values (@p1,@p2,@p3,@p4)", baglanti);
            komut.Parameters.AddWithValue("@p1", TxtAd.Text);
            komut.Parameters.AddWithValue("@p2", TxtSoyad.Text);
            komut.Parameters.AddWithValue("@p3", MskTel.Text);
            komut.Parameters.AddWithValue("@p4", TxtMail.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Kişi sisteme eklendi", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
            listele();
            temizle();


        }

        private void BtnTemizle_Click(object sender, EventArgs e)
        {
            temizle();
        }

        /*private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;

            TxtId.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
            TxtAd.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
            TxtSoyad.Text = dataGridView1.Rows[secilen].Cells[2].Value.ToString();
            MskTel.Text = dataGridView1.Rows[secilen].Cells[3].Value.ToString();
            TxtMail.Text = dataGridView1.Rows[secilen].Cells[4].Value.ToString();
        }*/
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;
            TxtId.Text = dataGridView1.Rows[secilen].Cells[0].Value.ToString();
            TxtAd.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
            TxtSoyad.Text = dataGridView1.Rows[secilen].Cells[2].Value.ToString();
            MskTel.Text = dataGridView1.Rows[secilen].Cells[3].Value.ToString();
            TxtMail.Text = dataGridView1.Rows[secilen].Cells[4].Value.ToString();

            // Resim verisini al
            if (dataGridView1.Rows[secilen].Cells[5].Value != DBNull.Value)
            {
                byte[] resimBytes = (byte[])dataGridView1.Rows[secilen].Cells[5].Value;
                using (MemoryStream ms = new MemoryStream(resimBytes))
                {
                    pictureBox1.Image = Image.FromStream(ms);
                }
            }
            else
            {
                pictureBox1.Image = null;
            }
        }


        private void BtnSil_Click(object sender, EventArgs e)
        {
            
            DialogResult result = MessageBox.Show("Kişi silinsin mi?", "Kişi silme", MessageBoxButtons.YesNo,MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("DELETE FROM KISILER  where ID=" + TxtId.Text, baglanti);
                komut.ExecuteNonQuery();
                baglanti.Close();
                listele();
                temizle();
                MessageBox.Show("Kişi başarı ile silindi","İşlem başarılı");
            }
            else
            {
                MessageBox.Show("Kişi silinmedi");
            }
           
            
         }

        private void BtnGuncelle_Click(object sender, EventArgs e)
        {
            /* baglanti.Open();
             SqlCommand komut = new SqlCommand("update KISILER set AD=@P1,SOYAD=@P2,TELEFON=@P3,MAIL=@P4 where ID=@P5", baglanti);
             komut.Parameters.AddWithValue("@P1", TxtAd.Text);
             komut.Parameters.AddWithValue("@P2", TxtSoyad.Text);
             komut.Parameters.AddWithValue("@P3", MskTel.Text);
             komut.Parameters.AddWithValue("@P4", TxtMail.Text);
             komut.Parameters.AddWithValue("@P5", TxtId.Text);
             komut.ExecuteNonQuery();
             baglanti.Close();
             MessageBox.Show("Kişi Bilgisi güncellendi", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
             listele();
             temizle();*/
            byte[] resimBytes = null;

            if (pictureBox1.Image != null)
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
                    resimBytes = ms.ToArray();
                }
            }

            baglanti.Open();
            SqlCommand komut = new SqlCommand("UPDATE KISILER SET AD=@p1, SOYAD=@p2, TELEFON=@p3, MAIL=@p4, RESIM=@p5 WHERE ID=@p6", baglanti);
            komut.Parameters.AddWithValue("@p1", TxtAd.Text);
            komut.Parameters.AddWithValue("@p2", TxtSoyad.Text);
            komut.Parameters.AddWithValue("@p3", MskTel.Text);
            komut.Parameters.AddWithValue("@p4", TxtMail.Text);
            komut.Parameters.Add("@p5", SqlDbType.VarBinary).Value = (object)resimBytes ?? DBNull.Value;
            komut.Parameters.AddWithValue("@p6", TxtId.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();

            MessageBox.Show("Kişi güncellendi");
            listele();
            temizle();


        }

        private void BtnResim_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Resim Dosyaları|*.jpg;*.jpeg;*.png;*.bmp";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(ofd.FileName);
            }
        }
    }
}

//Data Source=DESKTOP-0NNHPCV\SQLEXPRESS;Initial Catalog=BonusOkul;Integrated Security=True;Trust Server Certificate=True
