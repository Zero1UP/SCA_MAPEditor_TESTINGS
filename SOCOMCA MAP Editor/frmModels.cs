using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SOCOMCA_MAP_Editor
{
    public partial class frmModels : Form
    {
        private CA_Node CurrentModel = new CA_Node();

        public frmModels()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "*.*|*.*";
            DialogResult dlgRet = openFileDialog1.ShowDialog();
            if (dlgRet.ToString() != "OK") { return; }

            txtFile.Text = openFileDialog1.FileName;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            byte[] fData;

            nodeTree.Nodes.Clear();

            if (System.IO.File.Exists(txtFile.Text) == false) { return; }
            try { fData = System.IO.File.ReadAllBytes(txtFile.Text); } catch { MessageBox.Show("File Read Error"); return; }

            int entry = int.Parse(txtAddr.Text, System.Globalization.NumberStyles.HexNumber);
            if (!CurrentModel.ExtractNode(fData, entry)) { MessageBox.Show("Load Error"); return; }

            //nodeTree.Nodes.Add(CurrentModel.NodeType);
            //textBox1.Text = CurrentModel.DumpTreeText("");
            
            FillTreeNode(CurrentModel);
        }

        private void FillTreeNode(CA_Node cNode)
        {
            TreeNode tNode = new TreeNode();
            
            tNode.Text = cNode.NodeType + ":" + cNode.StringID.ToString("X8");
            tNode.Tag = cNode;
            nodeTree.Nodes.Add(tNode);
            
            for (int i = 0; i < cNode.Nodes.Count(); i++)
            {
                AddChildNode(cNode.Nodes.ElementAt(i), tNode);
            }
        }
        private void AddChildNode(CA_Node cNode, TreeNode tNode)
        {
            TreeNode child = new TreeNode();
            child.Text = cNode.NodeType + ":" + cNode.StringID.ToString("X8");
            child.Tag = cNode;
            tNode.Nodes.Add(child);

            for (int i = 0; i < cNode.Nodes.Count(); i++)
            {
                AddChildNode(cNode.Nodes.ElementAt(i), child);
            }
        }

        private void nodeTree_AfterSelect_1(object sender, TreeViewEventArgs e)
        {
            CA_Node cNode = cNode = (CA_Node)e.Node.Tag;
            textBox1.Text = cNode.ToString();
            //MessageBox.Show(cNode.StringID.ToString("X8") + "\nParent: " + cNode.Parent.StringID.ToString("X8"));
        }
        
    }
}
