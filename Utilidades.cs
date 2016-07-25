using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Deployment.Application;
using System.Security.Cryptography;

namespace M77
{
    public class Utilidades
    {
        public static string creador = "Matias Flores";

        // List de clipboard
        public static List<string> GetClipboard()
        {
            List<string> clip = new List<string>();

            string s = Clipboard.GetText();
            string[] lines = s.Split('\n');

            foreach (string line in lines)
            {
                if (!line.Equals(""))
                {
                    string pclip = line.Replace("\r", "");// para string[] agregar .Split('\t');
                    clip.Add(pclip);
                }
            }
            return clip;
        }

        // Reiniciar aplicacion
        public static void Restart()
        {
            System.Diagnostics.Process.Start(Application.ExecutablePath);
            Application.Exit();
        }

        // Buscar Imagen
        public static string Buscar_imagen()
        {
            OpenFileDialog fdlg = new OpenFileDialog();

            fdlg.Title = "Buscar imagen";
            fdlg.InitialDirectory = Environment.SpecialFolder.DesktopDirectory.ToString();
            fdlg.Filter = "Imagenes (*.BMP;*.JPG;*.GIF,*.PNG,*.TIFF,*.JPEG)|*.BMP;*.JPG;*.GIF;*.PNG;*.TIFF,*.JPEG";
            fdlg.FilterIndex = 2;
            fdlg.RestoreDirectory = true;

            fdlg.ShowDialog();

            return fdlg.FileName;
        }

        // Actualiza la version de la aplicacion
        public static bool Actualizar_version()
        {
            bool ejecutado = true;
            try
            {
                ApplicationDeployment deploy = ApplicationDeployment.CurrentDeployment;
                UpdateCheckInfo update = deploy.CheckForDetailedUpdate();
                if (deploy.CheckForUpdate())
                {
                    MessageBox.Show("Se detecto la version " + update.AvailableVersion.ToString() + ", se actualizara la aplicacion!");
                    deploy.Update();
                    Application.Restart();
                }
                ejecutado = true;
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Error al actualizar aplicacion:\n " + ex.Message);
                ejecutado = true;
            }
            return ejecutado;
        }

        // Retorna version actual
        public static string Version()
        {
            string ver = "Release";

            try
            {
                ApplicationDeployment deploy = ApplicationDeployment.CurrentDeployment;
                ver = deploy.CurrentVersion.ToString();
            }
            catch (Exception ex)
            {
                ver = "Release";
            }

            return ver;
        }

        // Retorna version nueva
        public static string VersionNueva()
        {
            string version = "";
            try
            {
                ApplicationDeployment deploy = ApplicationDeployment.CurrentDeployment;
                UpdateCheckInfo update = deploy.CheckForDetailedUpdate();
                if (deploy.CheckForUpdate())
                {
                    version = update.AvailableVersion.ToString();
                }
            }
            catch (Exception ex)
            {
            }
            return version;
        }

        // Escapo caracteres para evitar RFI
        public static string Escape(string str)
        {
            string rp = str.Replace("'", "");
            rp = rp.Replace("\"", "");
            rp = rp.Replace("\\", "");
            return rp;
        }

        // Colorear Row, codigo HTML
        public static void rowColor(DataGridViewRow row, string texto, string fondo)
        {
            row.DefaultCellStyle.ForeColor = System.Drawing.ColorTranslator.FromHtml(texto);
            row.DefaultCellStyle.BackColor = System.Drawing.ColorTranslator.FromHtml(fondo);
        }

        // ENCODE/DECODE UN STRING A BYTE, simple proteccion de dato. 
        public static string Encrypt_byte(string texto)
        {
            string rs = "";
            try
            {
                System.Text.ASCIIEncoding codificador = new System.Text.ASCIIEncoding();
                byte[] tt = codificador.GetBytes(texto);
                rs = string.Join("-", tt);
            }
            catch (Exception ex)
            {
            }
            return rs;
        }
        public static string Decrypt_byte(string texto)
        {
            string rs = "";
            try
            {
                string[] ch = texto.Split('-');
                byte[] bytes = ch.Select(s => Convert.ToByte(s)).ToArray();
                rs = Encoding.UTF8.GetString(bytes);
            }
            catch (Exception ex)
            {
            }
            return rs;
        }

        // MD5
        public static string MD5(string input)
        {
            // Use input string to calculate MD5 hash
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
                // To force the hex string to lower-case letters instead of
                // upper-case, use he following line instead:
                // sb.Append(hashBytes[i].ToString("x2")); 
            }
            return sb.ToString();
        }
    }
}
