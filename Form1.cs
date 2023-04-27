using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.CodeDom;

namespace puntoCSV
{
    public partial class Form1 : Form
    {
        string path = "archivo.csv";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (File.Exists(path))
            {
                File.Create(path).Dispose();
                using (TextWriter writer = new StreamWriter(path))
                {
                    writer.WriteLine("id,nombre,pais,edad");
                }
            }
            grid();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (File.Exists(path))
            {
                int count = 0;
                try
                {
                    count = Convert.ToInt32(File.ReadAllLines(path).Last().Split(',')[0]);
                    count = count + 1;
                }
                catch (Exception e1)
                {
                    count = 1;
                }
                File.AppendAllLines(path, new[] {count.ToString()+","+txtName.Text+","+txtPais.Text+","+txtPais.Text});
            }
            grid();
        }

        public void grid()
        {
            dgv.DataSource = null;
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("nombre");
            dt.Columns.Add("Pais");
            dt.Columns.Add("Edad");

            string[] lineas = File.ReadAllLines(path);
            lineas = lineas.Skip(1).ToArray();

            foreach (string linea in lineas)
            {
                DataRow fila = dt.NewRow();
                string[] valores = linea.Split(',');
                int i = 0;
                foreach (string valor in valores)
                {
                    fila[i] = valor;
                    i++;
                }
                dt.Rows.Add(fila);
            }   
            dgv.DataSource= dt;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string[] lineas = File.ReadAllLines(path);
            int j = 0;
            foreach (string linea in lineas) 
            {
                if (linea.Split(',')[0] == lblGuia.Text)
                {
                    break;
                }
                else
                {
                    j++;
                }
            }
            lineas[j]= lineas[j].Split(',')[0] + "," + txtName.Text + "," + txtPais.Text + "," + txtEdad.Text;
            File.WriteAllLines(path, lineas);
            grid();
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            string[] lineas = File.ReadAllLines (path);
            int j = 0;
            foreach (string linea in lineas)
            {
                if (linea.Split(',')[0]==lblGuia.Text) 
                { 
                    break;
                }
                else
                {
                    j++;
                }
            }
            string[] newlinea = lineas.Where(g => !g.Equals(lineas[j])).ToArray();
            File.WriteAllLines(path, newlinea);
            grid();
        }

        private void dgv_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow columnas = dgv.Rows[e.RowIndex];
                lblGuia.Text = columnas.Cells[0].Value.ToString();
            }
        }
    }
}
