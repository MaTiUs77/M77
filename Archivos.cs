using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Data;

namespace M77
{
    public class Archivos
    {
        public string CARPETA = "";
        public char SEPARADOR = '\t';

        public string[] COLUMNAS = { };
        public char FILAS = '\n';
        public bool INDEXADO = false;

        public bool error = false;
        public string error_msg = "";

        private void SetError(string msg) {
            error = true;
            error_msg = msg;
        }

        // Listar archivos de la carpeta
        public string[] leer_carpeta()
        {
            string[] lista = { };

            if (Directory.Exists(CARPETA))
            {
                lista = Directory.GetDirectories(CARPETA);
            }
            else
            {
                SetError("No hay acceso a la carpeta: " + CARPETA);
            }
            return lista;
        }

        // Lista archivos de la carpeta
        public List<string> listar_archivos(string ruta_carpeta, string extencion)
        {
            FileInfo[] files = new DirectoryInfo(ruta_carpeta).GetFiles(extencion);
            List<string> ls = new List<string>();
            if (files.Length > 0)
            {
                var archivos = files.OrderByDescending(f => f.Name);
                foreach (FileInfo archivo in archivos)
                {
                    ls.Add(archivo.ToString());
                }
            }
            return ls;
        }

        // Lectura de archivo y devolucion en datatable
        public DataTable leer_archivo(string file)
        {
            DataTable dt = new DataTable(); // Creo una Datatable nueva.
            try
            {
                StreamReader reader = new StreamReader(file);
                string contenido = reader.ReadToEnd();
                reader.Close();

                string[] lineas = contenido.Split(FILAS); // Separo las lineas por filas.

                // Primer columna como header?
                bool first = true;
                bool customHeaderDone = false;

                bool customHeader = false;
                if ((COLUMNAS.Length > 0))
                {
                    customHeader = true;
                }

                foreach (string linea in lineas)
                {
                    string[] rows = linea.Split(SEPARADOR);

                    if (rows.Length > 1) // Me aseguro que las filas contengan mas de una columna.
                    {
                        if (first && !customHeader) // Si es la primer fila, y no defino columnas por defecto, ingreso la linea como columna.
                        {
                            int j = 0;
                            foreach (string row in rows)
                            {
                                if (INDEXADO) // Puedo asignar columnas con su respectivo INDEX.
                                {
                                    dt.Columns.Add(j.ToString());
                                }
                                else
                                {
                                    if (dt.Columns.Contains(row.ToLower()))
                                    {
                                        dt.Columns.Add(j + "_" +row.ToLower());
                                    }
                                    else
                                    {
                                        dt.Columns.Add(row.ToLower());
                                    }
                                }
                                j++;
                            }
                            first = false;
                        }
                        else
                        {
                            if (!customHeaderDone) // Si existen columnas personalizadas, y estas no se han seteado
                            {
                                for (int i = 0; i < COLUMNAS.Length; i++) // Cargo columnas personalizadas.
                                {
                                    dt.Columns.Add(COLUMNAS[i].ToString());
                                }
                                customHeaderDone = true;
                            }
                            dt.Rows.Add(rows); // Agrego filas.
                        }
                    }
                }
                return dt;
            }
            catch (Exception e)
            {
                SetError("leer_archivo(): " + e.Message);
                return dt;
            }
        }

        // Filtrar resultados en datatable
        public static DataTable filtrar_tabla(DataTable search, string filtro)
        {
            DataTable searchFiltrado = search.Clone();
            searchFiltrado.Clear(); // Copio columnas y vacio tabla.

            if (search.Rows.Count > 0)
            {
                DataRow[] result = search.Select(filtro); // Aplico filtro

                foreach (DataRow d in result)
                {
                    searchFiltrado.ImportRow(d);
                }
            }
            return searchFiltrado;
        }

        // Existe archivo
        public static bool existe(string archivo)
        {
            bool rs = false;
            try
            {
                if (File.Exists(archivo))
                {
                    rs = true;
                }
                else
                {
                    rs = false;
                }
            }
            catch (Exception e)
            {
                rs = false;
            }
            return rs;
        }

        // Crear carpeta
        public static bool crear_carpeta(string carpeta)
        {
            bool rs = false;
            string file = Path.GetDirectoryName(carpeta);
            try
            {
                if (Directory.Exists(file))
                {
                    rs = true;
                }
                else
                {
                    DirectoryInfo di = Directory.CreateDirectory(file);
                    rs = true;
                }
            }
            catch (Exception e)
            {
                rs = false;
            }
            return rs;
        }
    }
}
