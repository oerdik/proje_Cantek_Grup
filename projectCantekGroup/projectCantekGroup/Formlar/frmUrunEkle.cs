using DevExpress.XtraEditors;
using projectCantekGroup.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using wordeaktar = Microsoft.Office.Interop.Word;
using System.Reflection;
using System.IO;

namespace projectCantekGroup.Formlar
{
    public partial class frmUrunEkle : Form
    {
        dbCantekGrupEntities db = new dbCantekGrupEntities();
        public frmUrunEkle()
        {
            InitializeComponent();
            
            var degerler = (from x in db.tableUrun
                            select new
                            {
                                x.UrunId,
                                x.UrunAdi,
                                x.IslemAdi
                            }).ToList();
            gridControl1.DataSource = degerler;
            lstBilesen.Hide();
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {

        }

        private void btnUrunEkle_Click(object sender, EventArgs e)
        {
            tableUrun u = new tableUrun();
            u.UrunAdi = txtUrunAdi.Text;
            u.IslemAdi = txtIslemAdi.Text;
            db.tableUrun.Add(u);
            db.SaveChanges();
            XtraMessageBox.Show("Ürün Başarılı Bir Şekilde Kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnBilesenEkle_Click(object sender, EventArgs e)
        {
            lstBilesen.Items.Clear();
            int urunId;
            urunId = Convert.ToInt32(txtUrunId.Text);
            decimal bilesenOrani = Convert.ToDecimal(txtBilsenOrani.Text);
            tableBilesen b = new tableBilesen();
            b.UrunId = urunId;
            b.BilesenAdi = txtBilsesenAdi.Text;
            b.BilesenOrani = bilesenOrani;
            db.tableBilesen.Add(b);
            db.SaveChanges();
            XtraMessageBox.Show("Bileşen Başarılı Bir Şekilde Kaydedildi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);

            foreach (var item in db.tableBilesen)
            {
                if (item.UrunId == Convert.ToInt32(txtUrunId.Text))
                {
                    lstBilesen.Items.Add(item.BilesenAdi + "       " + item.BilesenOrani );

                }
            }
        }

        private void btnYazdir_Click(object sender, EventArgs e)
        {
            string template = @Path.GetDirectoryName(Application.ExecutablePath).Trim() + "\\cantekTemplate.docx";
            wordeaktar.Application wordapp = new wordeaktar.Application();
            wordapp.Visible = true;
            wordeaktar.Document worddoc = wordapp.Documents.OpenNoRepairDialog(template);
            worddoc.Activate();
            wordeaktar.Table table = worddoc.Tables[1];
            table.Cell(1, 2).Range.Text = txtUrunAdi.Text;
            table.Cell(2, 2).Range.Text = txtIslemAdi.Text;

            wordeaktar.Table table3 = worddoc.Tables[3];


            for (int r = 0; r < 7; r++)
            {

                table3.Cell(r, 1).Range.Text = lstBilesen.Items[r].ToString();
            }
        }
    }
}
