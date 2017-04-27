﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DeScammerControlPanel.API;

namespace DeScammerControlPanel
{
    public partial class FileManager : Form
    {
        #region Variables
        private string path = "C:\\"; // The path
        #endregion

        public FileManager()
        {
            InitializeComponent();
        }

        #region Public Functions
        public void AddFile(string file)
        {
            ListViewItem lvi = new ListViewItem(file); // Create the list view item

            listView1.Items.Add(lvi); // Add the list view item
        }
        #endregion

        #region Private Functions
        private void RefreshFiles()
        {
            listView1.Items.Clear(); // Clear the items

            Variables.Client.Send(CommandManager.FormatCommand("getfiles", new string[] { path })); // Ask for files
        }
        #endregion

        #region Form Functions
        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshFiles();
        }
        #endregion
    }
}
