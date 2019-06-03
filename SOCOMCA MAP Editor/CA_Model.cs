using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOCOMCA_MAP_Editor
{
    class CA_Model
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

        public Int32 StringID = 0;
        public Vect4[] Orientation = new Vect4[3];
        public Vect4 Coordinates = new Vect4(0f, 0f, 0f, 1f);
        public CA_Node[] Nodes;
        public int NodeCount = 0;
        // Need CDIPoly Listings

        // _78 = parent node
        
        public int _7c = -1;
        public int _84 = 0x0000beef;
        public int _90 = 0x0000ff06; // Subject to changes
        public int _94 = 0x0050c041; // Subject to changes

        // Need Mesh Listings
        // Need Visual Listings
        // Mesh/Visual listings contained in same spot..? different model types?

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
    }
}
