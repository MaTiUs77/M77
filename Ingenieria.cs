using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Data;

namespace M77
{
    public static class Ingenieria
    {
        public static string CARPETA = "";
        public static Archivos ingenieria;
        public static string extencion = "*.txt";

        public static void iniciar() {
            ingenieria = new Archivos();
            ingenieria.CARPETA = CARPETA;
            ingenieria.SEPARADOR = '\t';
        }

        // Listo lotes en carpeta del modelo
        public static List<string> lotes(string archivo)
        {
            return ingenieria.listar_archivos(archivo, extencion);
        }

        // Leo archivo Lotes
        public static DataTable leer_lote(string archivo) {
            return ingenieria.leer_archivo(archivo);
        }

        // Listo modelos en listas
        public static string[] lista_modelos()
        {
            return ingenieria.leer_carpeta();
        }

        // Carga comboModelos
        public static bool combo_Modelos(ComboBox combo)
        {
            combo.Items.Clear();
            foreach (string carpeta in lista_modelos())
            {
                combo.Items.Add(new Combo(Path.GetFileNameWithoutExtension(carpeta), carpeta));
            }
            return true;
        }

        // Carga comboLotes
        public static bool combo_Lotes(string modelo_path, ComboBox combo)
        {
            combo.Items.Clear();
            List<string> dt = ingenieria.listar_archivos(modelo_path,"*.txt");

            foreach (string lote in dt)
            {
                combo.Items.Add(new Combo(Path.GetFileNameWithoutExtension(lote), modelo_path + @"\" + lote));
            }
            return true;
        }

        // Carga paneles
        public static bool combo_Placas(DataTable dt, ComboBox combo)
        {
            combo.Items.Clear();

            List<string> pcblist = new List<string>();
            pcblist = pcb(dt);

            foreach (string panel in pcblist)
            {
                combo.Items.Add(panel);
            }

            return true;
        }

        // Lee el archivo y devuelve un datatable.
        public static DataTable info(string modelo, string lote)
        {
            string archivo_lote = ingenieria.CARPETA + @"\" + modelo + @"\" + lote + ".txt";
            DataTable dt = ingenieria.leer_archivo(archivo_lote);
            return dt;
        }

        // Verifica si es PCB
        public static List<string> pcb(DataTable dt)
        {
            List<string> pcb = new List<string>();

            foreach (DataRow row in dt.Rows)
            {
                string logop = row.ItemArray[4].ToString();

                if (!pcb.Contains(logop) && !logop.Equals(""))
                {
                    pcb.Add(logop);
                }
            }
            return pcb;
        }

        
    }
}
