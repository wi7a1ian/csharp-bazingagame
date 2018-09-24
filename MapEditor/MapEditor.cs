using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MapFileModel;

namespace MapEditor
{
    public partial class MapEditor : Form
    {
        private MapSpriteTile[][] _mapSpriteTile;

        public MapEditor()
        {
            InitializeComponent();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var serializer = new BinaryFormatter();

            using (var stream = File.OpenWrite("map.baz"))
            {
                serializer.Serialize(stream, _mapSpriteTile);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Deserialize the list from a file
            var serializer = new BinaryFormatter();
            using (var stream = File.OpenRead("map.baz"))
            {
                _mapSpriteTile = (MapSpriteTile[][])serializer.Deserialize(stream);
            }

            SetComponents();
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _mapSpriteTile = new MapSpriteTile[200][];

            for (int i = 0; i < _mapSpriteTile.Length; i++)
            {
                _mapSpriteTile[i] = new MapSpriteTile[100];
            }

            for (int i = 0; i < _mapSpriteTile.Length; i++)
            {
                for (int j = 0; j < _mapSpriteTile[0].Length; j++)
                {
                    _mapSpriteTile[i][j] = MapSpriteTile.None;
                }
            }

            SetComponents();
        }

        private void SetComponents()
        {
            for (int i = 0; i < _mapSpriteTile.Length; i++)
            {
                for (int j = 0; j < _mapSpriteTile[0].Length; j++)
                {
                }
            }
        }
    }
}
