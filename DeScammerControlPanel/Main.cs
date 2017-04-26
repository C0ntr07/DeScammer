﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DeScammerControlPanel.API;

namespace DeScammerControlPanel
{
    public partial class Main : Form
    {
        #region Form Properties
        public bool IsClientConnected
        {
            get
            {
                return lblClientStatus.Text == "Connected";
            }
            set
            {
                if (value)
                {
                    if (InvokeRequired)
                    {
                        lblClientStatus.Invoke(new MethodInvoker(delegate { lblClientStatus.Text = "Connected"; }));
                        lblClientStatus.Invoke(new MethodInvoker(delegate { lblClientStatus.ForeColor = Color.Green; }));
                        btnDisconnectClient.Invoke(new MethodInvoker(delegate () { btnDisconnectClient.Enabled = true; }));
                        btnShutdownClient.Invoke(new MethodInvoker(delegate () { btnShutdownClient.Enabled = true; }));
                    }
                    else
                    {
                        lblClientStatus.Text = "Connected";
                        lblClientStatus.ForeColor = Color.Green;
                        btnDisconnectClient.Enabled = true;
                        btnShutdownClient.Enabled = true;
                    }
                }
                else
                {
                    if (InvokeRequired)
                    {
                        lblClientStatus.Invoke(new MethodInvoker(delegate { lblClientStatus.Text = "Disconnected"; }));
                        lblClientStatus.Invoke(new MethodInvoker(delegate { lblClientStatus.ForeColor = Color.Red; }));
                        btnDisconnectClient.Invoke(new MethodInvoker(delegate () { btnDisconnectClient.Enabled = false; }));
                        btnShutdownClient.Invoke(new MethodInvoker(delegate () { btnShutdownClient.Enabled = false; }));
                        this.Invoke(new MethodInvoker(delegate () { RemoveClient(); }));
                    }
                    else
                    {
                        lblClientStatus.Text = "Disconnected";
                        lblClientStatus.ForeColor = Color.Red;
                        btnDisconnectClient.Enabled = false;
                        btnShutdownClient.Enabled = false;
                        RemoveClient();
                    }
                }
            }
        }
        #endregion

        public Main()
        {
            InitializeComponent();
        }

        #region Private Functions
        private void RemoveClient()
        {
            try
            {
                Variables.Client.Disconnect(); // Try to disconnect first
            }
            catch (Exception ex) { }

            Variables.Client = null; // Set the client to null
            txtIP.ReadOnly = false; // Set the textbox to be writtable
            btnConnect.Enabled = true; // Enable connect button
            btnClear.Enabled = true; // Enable the clear button
        }
        #endregion

        #region Form Functions
        private void btnClear_Click(object sender, EventArgs e)
        {
            if (txtIP.ReadOnly)
                return; // Don't clear if it is in read only mode

            txtIP.Text = ""; // Clear the text
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            if(Variables.Client != null && Variables.Client.IsConnected)
                return; // Client is already connected don't bother

            Variables.Client = Client.Create(txtIP.Text); // Create a client instance
            txtIP.ReadOnly = true; // Set the textbox to read only
            btnConnect.Enabled = false; // Disable the connect button
            btnClear.Enabled = false; // Disable the clear button

            if (!Variables.Client.IsConnected)
                RemoveClient(); // Restore if the client couldn't connect

            MessageBox.Show("You are now connected!", "DeScammer Control Panel");
        }

        private void btnDisconnectClient_Click(object sender, EventArgs e)
        {
            if (Variables.Client == null || !Variables.Client.IsConnected)
                return; // Client isn't connected don't bother

            RemoveClient();

            MessageBox.Show("You are now disconnected!", "DeScammer Control Panel");
        }

        private void btnShutdownClient_Click(object sender, EventArgs e)
        {
            if (Variables.Client == null || !Variables.Client.IsConnected)
                return; // Client isn't connected don't bother

            Variables.Client.Send(CommandManager.FormatCommand("shutdown", new string[0]));
        }

        private void Main_Load(object sender, EventArgs e)
        {
            if(!CommandManager.IsLoaded)
                CommandManager.Load(); // Load the command manager
        }
        #endregion
    }
}