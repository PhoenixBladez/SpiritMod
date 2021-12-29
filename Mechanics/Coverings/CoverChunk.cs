using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;

namespace SpiritMod.Mechanics.Coverings
{
    public class CoverChunk
    {
        private CoveringsManager _manager;
        public int CoverCount { get; private set; }

        private CoverData[,] _data;

        public CoverChunk(CoveringsManager manager, int size)
        {
            _manager = manager;
            _data = new CoverData[size, size];
        }

        public void SetOrientation(Point pos, int orientation) => SetOrientation(pos.X, pos.Y, orientation);
        public void SetOrientation(int x, int y, int orientation)
        {
            ref CoverData data = ref _data[x, y];
            int prevOrientation = data.Orientation;
            data.Orientation = orientation;

            // update count
            // kind of an awkward implementation to do this but I think it helps overall
            if (prevOrientation == 0 && orientation > 0) CoverCount++;
            if (prevOrientation > 0 && orientation == 0) CoverCount--;
        }

        public void SetVariation(Point pos, int variation) => SetVariation(pos.X, pos.Y, variation);
        public void SetVariation(int x, int y, int variation)
        {
            ref CoverData data = ref _data[x, y];
            data.Variation = variation;
        }

        public void SetType(Point pos, int variation) => SetType(pos.X, pos.Y, variation);
        public void SetType(int x, int y, int type)
        {
            ref CoverData data = ref _data[x, y];
            data.Type = (byte)type;
        }

        public void SetData(Point pos, int variation, int orientation, int type) => SetData(pos.X, pos.Y, variation, orientation, type);
        public void SetData(int x, int y, int variation, int orientation, int type)
        {
            SetVariation(x, y, variation);
            SetOrientation(x, y, orientation);
            SetType(x, y, type);
        }

        public void SetData(Point pos, CoverData data) => SetData(pos.X, pos.Y, data);
        public void SetData(int x, int y, CoverData data)
        {
            _data[x, y] = data;
        }

        public void Save(BinaryWriter writer)
        {
            for (int y = 0; y < 64; y++)
            {
                // is empty row?
                bool emptyRow = true;
                for (int x = 0; x < 64; x++)
                {
                    if (_data[x, y].Orientation > 0 && _manager.GetCoveringFromID(_data[x, y].Type).RequiresSaving)
                    {
                        emptyRow = false;
                        break;
                    }
                }

                writer.Write(emptyRow);
                if (!emptyRow)
                {
                    for (int x = 0; x < 64; x++)
                    {
                        writer.Write(_data[x, y].Type);
                        writer.Write(_data[x, y].Data);
                    }
                }
            }
        }

        public void Load(BinaryReader reader, Dictionary<int, int> conversions)
        {
            for (int y = 0; y < 64; y++)
            {
                // if not empty row
                if (!reader.ReadBoolean())
                {
                    for (int x = 0; x < 64; x++)
                    {
                        int type = reader.ReadByte();
                        // if the type no longer exists, or the converted type is -1, this covering should be removed.
                        if (!conversions.TryGetValue(type, out int convertedType) || convertedType == -1)
                        {
                            _data[x, y].Data = 0;
                            reader.ReadByte();
                            continue;
                        }

                        _data[x, y].Type = (byte)convertedType;
                        _data[x, y].Data = reader.ReadByte();
                    }
                }
                else
                {
                    // clear that row, to be sure it has no remnants from previously loaded worlds
                    for (int x = 0; x < 64; x++)
                    {
                        _data[x, y].Data = 0;
                    }
                }
            }
        }

        public CoverData GetData(int x, int y)
        {
            return _data[x, y];
        }

        public CoverData this[int x, int y] => GetData(x, y);
    }
}
