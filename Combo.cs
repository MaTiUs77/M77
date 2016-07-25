using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace M77
{
    public class Combo
    {
        public string Nombre;
        public string Valor;

        public Combo(string nombre, string valor)
        {
            this.Nombre = nombre;
            this.Valor = valor;
        }

        public override string ToString()
        {
            return this.Nombre;
        }

        public static string nombre(ComboBox cm)
        {
            string rt = "";
            try {
                rt = (((Combo)cm.Items[cm.SelectedIndex]).Nombre).ToString();
            } catch(Exception ex) {
                rt = "";
            }
            return rt;
        }
        public static string valor(ComboBox cm)
        {
            string rt = "";
            try {
                rt = (((Combo)cm.Items[cm.SelectedIndex]).Valor).ToString();
            }
            catch (Exception ex)
            {
                rt = "";
            }
            return rt;
        }
    }
}
