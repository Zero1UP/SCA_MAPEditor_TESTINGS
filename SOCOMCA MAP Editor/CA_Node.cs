using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SOCOMCA_MAP_Editor
{
    class CA_Node
    {
        public struct Vect4
        {
            public Single x;
            public Single y;
            public Single z;
            public Single w;

            public Vect4(Single X, Single Y, Single Z, Single W)
            {
                x = X;
                y = Y;
                z = Z;
                w = W;
            }
        }


        private int NodeEntry = 0;
        public string NodeType = "CNode"; // CModel, CWorld, CCell

        // 0x00839a2c = Disc CModel
        // 0x0083dc7c = Disc CNode

        public Int32 NodePointer = 0;
        public Int32 StringID = 0;
        public Vect4[] Orientation = new Vect4[3];
        public Vect4 Coordinates = new Vect4(0f, 0f, 0f, 1f);


        // -------------------------- List Slot 1
        //public CA_Node[] Nodes;
        public List<CA_Node> Nodes = new List<CA_Node>();
        public int NodeCount = 0;

        private string List1Type;
        private int List1Start;
        private int List1End;
        private int List1Entry;
        private int List1Size;
        
        // -------------------------- List Slot 1
        // Need CDIPoly Listings
        private string List2Type;
        private int List2Start;
        private int List2End;
        private int List2Entry;
        private int List2Size;

        public int NodeSize = 0xd0; // _74 = Pointer -> End of Node
        // _78 = parent node
        public CA_Node Parent;

        public int _7c = -1;
        public int _84 = 0x0000beef;
        public int _90 = 0x0000ff06; // Subject to changes
        public int _94 = 0x0050c041; // Subject to changes

        // Need Mesh Listings
        // Need Visual Listings
        // Mesh/Visual listings contained in same spot..? different model types?
        private string List3Type;
        private int List3Start;
        private int List3End;
        private int List3Entry;
        private int List3Size;

        public Single Opacity;
        public Single[] unkFloats = {
                -7.76244f,
                -0.002f,
                -2.153161f,
                7.820006f,
                19.646551f,
                3.357163f };

        public int _c8 = 0x08060300; // Subject to changes
        public int _d0 = 0x00000010; // Subject to changes

        /*
        public void AddNode(CA_Node NewNode)
        {
            NodeCount++;
            CA_Node[] tmpNodes = new CA_Node[NodeCount];

            for (int i = 0; i < Nodes.Count(); i++)
            {
                
                tmpNodes[i] = Nodes[i];
            }
            tmpNodes[NodeCount] = NewNode;
            Nodes = tmpNodes;
        }
        */

        public override string ToString()
        {
            string ret = "";

            ret = StringID.ToString("X8") + "\r\n\r\n" +
                  "Orientation: (" + Orientation[0].x.ToString() + ", " + Orientation[0].y.ToString() + ", " + Orientation[0].z.ToString() + ")\r\n             ("
                                   + Orientation[1].x.ToString() + ", " + Orientation[1].y.ToString() + ", " + Orientation[1].z.ToString() + ")\r\n             ("
                                   + Orientation[2].x.ToString() + ", " + Orientation[2].y.ToString() + ", " + Orientation[2].z.ToString() + ")\r\n\r\n" + 
                  "Position: (" + Coordinates.x.ToString() + ", " + Coordinates.y.ToString() + ", " + Coordinates.z.ToString() + ")\r\n" + 
                  "";


            return ret;
        }
        
        public string GetTypeFromPointer(Int32 nPointer)
        {
            switch (nPointer)
            {
                //----------------------- Disc Files
                case 0x0083dc7c:
                    return "CNode";
                case 0x00839a2c:
                    return "CModel";
                
                //----------------------- Patch 1.4
                case 0x006e6980:
                    return "CWorld";
                case 0x006e5780:
                    return "CCell";
                case 0x006e68e0:
                    return "CNode";
                default:
                        return "Invalid";
            }
        }

        public bool ExtractNode(byte[] fData, int entry)
        {
            NodeEntry = entry;

            NodePointer = BitConverter.ToInt32(fData, entry);
            NodeType = GetTypeFromPointer(NodePointer);
            if (NodeType == "Invalid") { return false; }

            StringID = BitConverter.ToInt32(fData, entry + 0x04);
            // +0x08
            // +0x0c
            Orientation[0].x = BitConverter.ToSingle(fData, entry + 0x10);
            Orientation[0].y = BitConverter.ToSingle(fData, entry + 0x14);
            Orientation[0].z = BitConverter.ToSingle(fData, entry + 0x18);
            Orientation[0].w = BitConverter.ToSingle(fData, entry + 0x1c);
            Orientation[1].x = BitConverter.ToSingle(fData, entry + 0x20);
            Orientation[1].y = BitConverter.ToSingle(fData, entry + 0x24);
            Orientation[1].z = BitConverter.ToSingle(fData, entry + 0x28);
            Orientation[1].w = BitConverter.ToSingle(fData, entry + 0x2c);
            Orientation[2].x = BitConverter.ToSingle(fData, entry + 0x30);
            Orientation[2].y = BitConverter.ToSingle(fData, entry + 0x34);
            Orientation[2].z = BitConverter.ToSingle(fData, entry + 0x38);
            Orientation[2].w = BitConverter.ToSingle(fData, entry + 0x3c);
            Coordinates.x = BitConverter.ToSingle(fData, entry + 0x40);
            Coordinates.y = BitConverter.ToSingle(fData, entry + 0x44);
            Coordinates.z = BitConverter.ToSingle(fData, entry + 0x48);
            Coordinates.w = BitConverter.ToSingle(fData, entry + 0x4c);

            // 0x50 - List1 Start
            // 0x54 - List1 End
            // 0x58 - List1 Data Entry
            // 0x5c - List1 Data Size (replaced with allocator pointer after loaded)
            List1Start = BitConverter.ToInt32(fData, entry + 0x50);
            List1End = BitConverter.ToInt32(fData, entry + 0x54);
            List1Entry = BitConverter.ToInt32(fData, entry + 0x58);
            List1Size = BitConverter.ToInt32(fData, entry + 0x5c);
            ExtractList1(fData);

            // 0x60 - List2 Start
            // 0x64 - List2 End
            // 0x68 - List2 Data Entry
            // 0x6c - List2 Data Size (replaced with allocator pointer after loaded)
            List2Start = BitConverter.ToInt32(fData, entry + 0x60);
            List2End = BitConverter.ToInt32(fData, entry + 0x64);
            List2Entry = BitConverter.ToInt32(fData, entry + 0x68);
            List2Size = BitConverter.ToInt32(fData, entry + 0x6c);
            ExtractList2(fData);

            NodeSize = BitConverter.ToInt32(fData, entry + 0x74) - entry;

            _7c = BitConverter.ToInt32(fData, entry + 0x7c);
            _84 = BitConverter.ToInt32(fData, entry + 0x84);
            _90 = BitConverter.ToInt32(fData, entry + 0x90);
            _94 = BitConverter.ToInt32(fData, entry + 0x94);
            // 0x98 - List3 Start
            // 0x9c - List3 End
            // 0xa0 - List3 Data Entry
            // 0xa4 - List3 Data Size (replaced with allocator pointer after loaded)
            List3Start = BitConverter.ToInt32(fData, entry + 0x98);
            List3End = BitConverter.ToInt32(fData, entry + 0x9c);
            List3Entry = BitConverter.ToInt32(fData, entry + 0xa0);
            List3Size = BitConverter.ToInt32(fData, entry + 0xa4);
            ExtractList3(fData);

            Opacity = BitConverter.ToSingle(fData, entry + 0xac);
            unkFloats[0] = BitConverter.ToSingle(fData, entry + 0xb0);
            unkFloats[1] = BitConverter.ToSingle(fData, entry + 0xb4);
            unkFloats[2] = BitConverter.ToSingle(fData, entry + 0xb8);
            unkFloats[3] = BitConverter.ToSingle(fData, entry + 0xbc);
            unkFloats[4] = BitConverter.ToSingle(fData, entry + 0xc0);
            unkFloats[5] = BitConverter.ToSingle(fData, entry + 0xc4);
            _c8 = BitConverter.ToInt32(fData, entry + 0xc8);
            //_d0 = BitConverter.ToInt32(fData, entry + 0xd0);

            return true;
        }
        private void ExtractList1(byte[] fData)
        {
            if (List1Start <= 0) { return; }
            if (List1End <= 0) { return; }
            if (List1End <= List1Start) { return; }

            NodeCount = 0;
            Nodes.Clear();

            for (int lstPoint = List1Start; lstPoint < List1End; lstPoint += 4)
            {
                int entry = BitConverter.ToInt32(fData, lstPoint);
                string childType = GetTypeFromPointer(BitConverter.ToInt32(fData, entry));
                
                if ((childType == "CNode") || (childType == "CModel") || childType == "CWorld")
                {
                    CA_Node tNode = new CA_Node();
                    tNode.ExtractNode(fData, entry);
                    tNode.Parent = this;
                    //AddNode(tNode);
                    Nodes.Add(tNode);
                    NodeCount++;
                }
            }
        }
        private void ExtractList2(byte[] fData)
        {

        }
        private void ExtractList3(byte[] fData)
        {

        }

        public string DumpTreeText(string tSpace)
        {
            string ret = tSpace + NodeType + " @" + NodeEntry.ToString("X8") + "; ID: " + StringID.ToString("X8") + "; [" + NodeCount.ToString() + "] Child Nodes\r\n";

            for (int i = 0; i < NodeCount; i++)
            {
                //ret += tSpace + "|-" + Nodes.ElementAt(i).NodeType + "\r\n";
                ret += Nodes.ElementAt(i).DumpTreeText(tSpace + "|-");
            }

            return ret;
        }
    }
}
