﻿using System;
using System.Collections.Generic;
using Rapr.Utils;
using System.Windows.Forms;

namespace Rapr
{
    class CSVExporter : IExport
    {
        private const string CSV_DELIM = ",";
        private const string CSV_DELIM_SUBST = " ";

        public string Export(List<DriverStoreEntry> ldse)
        {
            if (ldse.Count == 0)
            {
                MessageBox.Show("No entries to export", "Error");
                return null;
            }

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.CheckFileExists = false;
            saveFileDialog.DefaultExt = "csv";
            saveFileDialog.Filter = "CSV Files | *.csv";
            saveFileDialog.SupportMultiDottedExtensions = true;

            DialogResult dr = saveFileDialog.ShowDialog();
            if (dr == DialogResult.OK)
            {
                string csvFileName = saveFileDialog.FileName;   // Path                

                using (System.IO.StreamWriter file = new System.IO.StreamWriter(csvFileName))
                {
                    // Write the header once
                    string headerLine = String.Join(CSV_DELIM, ldse[0].GetFieldNames());
                    file.WriteLine(headerLine);

                    // Write the values
                    foreach (DriverStoreEntry dse in ldse)
                    {
                        string[] values = dse.GetFieldValues();
                        Sanitize(ref values);

                        string valueLine = String.Join(CSV_DELIM, values);
                        file.WriteLine(valueLine);
                    }
                }

                return csvFileName;
            }

            return null;
        }

        private void Sanitize(ref string[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (values[i].Contains(CSV_DELIM))
                {
                    values[i] = "\"" + values[i] + "\"";//.Replace(CSV_DELIM, CSV_DELIM_SUBST);
                }
            }
        }
    }
}
