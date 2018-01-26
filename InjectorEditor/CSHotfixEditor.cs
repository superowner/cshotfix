using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LCL
{
    public partial class CSHotfixEditor : Form
    {
        public CSHotfixEditor()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string delegatePath = "C:/GiteeSVN/ProtectGold/Research/MSILInject/Demo/Delegate";
            string dllPath = "C:/GiteeSVN/ProtectGold/Research/MSILInject/Demo/bin/Debug/Demo.exe";
            delegatePath = m_DelegatePath.Text;
            dllPath = m_DllPath.Text;
            try
            {
                DelegateGen delegateGen = new DelegateGen();
                delegateGen.Run(dllPath, delegatePath);
                m_log.Text = "生成完毕!";
            }
            catch(Exception exp)
            {
                m_log.Text = exp.Message;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string dllPath = "C:/GiteeSVN/ProtectGold/Research/MSILInject/Demo/bin/Debug/Demo.exe";
            dllPath = m_DllPath.Text;

            try
            {
                InjectorMain inject = new InjectorMain();
                inject.Run(dllPath);
                m_log.Text = "注入完毕完毕!";
            }
            catch(Exception exp)
            {
                m_log.Text = exp.Message + exp.StackTrace;
            }
        }

        private void OnFormLoad(object sender, EventArgs e)
        {
            LoadCfg();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadCfg();
        }
        private void LoadCfg()
        {
            try
            {
                FileStream file = new FileStream("ini.txt", FileMode.Open);
                StreamReader reader = new StreamReader(file);
                string dllPath = reader.ReadLine();
                dllPath = dllPath.Split('=')[1];
                string delegatePath = reader.ReadLine();
                delegatePath = delegatePath.Split('=')[1];
                m_DelegatePath.Text = delegatePath;
                m_DllPath.Text = dllPath;
                reader.Close();
                reader = null;
                file.Close();
                file = null;
            }
            catch(Exception e)
            {
                m_log.Text = e.Message;
            }
        }
    }
}
