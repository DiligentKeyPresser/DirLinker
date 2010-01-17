﻿using System;
using System.Windows.Forms;
using JunctionPointer.Interfaces.Views;

namespace JunctionPointer.Views
{
    public partial class ProgessView : Form, IWorkingView
    {
        public ProgessView()
        {
            InitializeComponent();

            BindToData();
        }

        private void BindToData()
        {
            m_Title.DataBindings.Add("Text", this, "FeedTitle", false, DataSourceUpdateMode.OnPropertyChanged);
            m_CurrentItem.DataBindings.Add("Text", this, "FeedbackCaption", false, DataSourceUpdateMode.OnPropertyChanged);
        }

        public string FeedbackTitle 
        { 
            set { UpdateLabel(m_Title, value); }
        }

        public string FeedbackCaption 
        { 
            set { UpdateLabel(m_CurrentItem, value); }
        }

        private void UpdateLabel(Label label, string value)
        {
            MethodInvoker updateLabelMethod = () => label.Text = value;
            if (label.InvokeRequired)
            {
                label.Invoke(updateLabelMethod);
            }
            else
            {
                updateLabelMethod();
            }
        }

        public DialogResult AskUser(string message, MessageBoxButtons options)
        {

            //Exception is being thrown here because invoked requires returns false as we are asking before the dialog is up and 
            //as such we don't have a handle to the window.  So we need to wait for a handle to it

            while (!IsHandleCreated)
            {
                System.Threading.Thread.Sleep(100);
            }

            Func<DialogResult> invoke = () => MessageBox.Show(this, message, "Directory Linker", options, MessageBoxIcon.None);
            if (InvokeRequired)
            {

                return (DialogResult)Invoke(invoke);
            }
            else
            {
                return invoke();
            }
        }


        new public DialogResult ShowDialog(IWin32Window owner)
        {
            progressBar1.Style = ProgressBarStyle.Marquee;
            progressBar1.MarqueeAnimationSpeed = 100;
            return base.ShowDialog(owner);
        }
    }
}