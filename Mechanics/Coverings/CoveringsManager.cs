using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;

namespace SpiritMod.Mechanics.Coverings
{
    public class CoveringsManager
    {
        private int _dataWidth;
        private int _dataHeight;
        private CoverChunk[,] _chunks;
        private Dictionary<Type, int> _coveringDict;
        private TileCovering[] _coveringTypes;

        public void Load(Mod mod)
        {
            Overlays.Scene["SpiritMod:CoveringsEntities"] = new CoveringOverlay(EffectPriority.High, RenderLayers.Entities);
			Overlays.Scene["SpiritMod:CoveringsFGW"] = new CoveringOverlay(EffectPriority.High, RenderLayers.ForegroundWater);
			//Overlays.Scene["SpiritMod:CoveringsBGW"] = new CoveringOverlay(EffectPriority.High, RenderLayers.BackgroundWater);

			// get my ass so i can pull all the coverings out of it
			Assembly myAss = mod.Code;

            _coveringDict = new Dictionary<Type, int>();
            Type coveringType = typeof(TileCovering);
            // get all of my ass's bits
            List<Type> assBits = myAss.GetTypes().Where(t => coveringType.IsAssignableFrom(t) && !t.IsAbstract).ToList();
            _coveringTypes = new TileCovering[assBits.Count];
            for (int i = 0; i < _coveringTypes.Length; i++)
            {
                _coveringDict[assBits[i]] = i;
                // convert ass bit to a cover
                _coveringTypes[i] = (TileCovering)Activator.CreateInstance(assBits[i]);
                _coveringTypes[i].Mod = mod;
                _coveringTypes[i].CoveringsManager = this;
                _coveringTypes[i].StaticLoad();
            }

            UpdateChunksArray();
        }

        public void Unload()
		{
			Overlays.Scene.Deactivate("SpiritMod:CoveringsEntities");
			Overlays.Scene.Deactivate("SpiritMod:CoveringsFGW");
			//Overlays.Scene.Deactivate("SpiritMod:CoveringsBGW");
		}

		public void Update(GameTime gameTime)
        {
            // update array if necessary
            if (Main.tile.GetLength(0) != _dataWidth || Main.tile.GetLength(1) != _dataHeight)
                UpdateChunksArray();

            // padding area of 20 for updates
            Rectangle area = GetScreenArea(20);

            for (int x = area.Left; x <= area.Right; x++)
            {
                for (int y = area.Top; y <= area.Bottom; y++)
                {
                    CoverData data = GetData(x, y);

                    // don't do anything
                    if (data.Orientation == 0) continue;

                    var type = _coveringTypes[data.Type];

                    if (!type.IsValidAt(x, y))
                    {
                        RemoveAt(x, y);
                        continue;
                    }

                    type.Update(gameTime, x, y, data.Variation, data.Orientation);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, RenderLayers layer)
        {
            // padding area of 5 for draws
            Rectangle area = GetScreenArea(5);

            for (int x = area.Left; x <= area.Right; x++)
            {
                for (int y = area.Top; y <= area.Bottom; y++)
                {
                    CoverData data = GetData(x, y);

                    // don't do anything
                    if (data.Orientation == 0) continue;

					if (_coveringTypes[data.Type].Layer != layer) continue;

                    _coveringTypes[data.Type].Draw(spriteBatch, x, y, data.Variation, data.Orientation);
                }
            }
        }

        public T GetCovering<T>() where T : TileCovering
        {
            if (_coveringDict.TryGetValue(typeof(T), out int index)) return (T)_coveringTypes[index];
            return null;
        }

        public TileCovering GetCoveringFromID(int id)
        {
            if (id < 0 || id >= _coveringTypes.Length) return null;

            return _coveringTypes[id];
        }

        public int GetCoveringID<T>() where T : TileCovering
        {
            if (_coveringDict.TryGetValue(typeof(T), out int index)) return index;
            return -1;
        }

        private Rectangle GetScreenArea(int padding = 2)
        {
            int startX = (int)(Main.screenPosition.X / 16f);
            int endX = (int)((Main.screenPosition.X + Main.screenWidth) / 16f);
            int startY = (int)(Main.screenPosition.Y / 16f);
            int endY = (int)((Main.screenPosition.Y + Main.screenHeight) / 16f);

            startX = Math.Max(startX, 5) - padding;
            startY = Math.Max(startY, 5) - padding;
            endX = Math.Min(endX, Main.maxTilesX - 5) + padding;
            endY = Math.Min(endY, Main.maxTilesY - 5) + padding;

            return new Rectangle(startX, startY, endX - startX, endY - startY);
        }

        private void UpdateChunksArray()
        {
            int width = Main.tile.GetLength(0);
            int height = Main.tile.GetLength(1);

            _dataWidth = width;
            _dataHeight = height;

            _chunks = new CoverChunk[width >> 6, height >> 6];
        }

        public void SetOrientation(int x, int y, int orientation)
        {
            ValidateExists(x, y);
            CoverChunk chunk = GetChunkAt(x, y);
            chunk.SetOrientation(GetPositionInChunk(x, y), orientation);

            // if our new count is 0, remove the chunk
            if (chunk.CoverCount == 0)
            {
                Point chunkPos = GetChunkFor(x, y);
                _chunks[chunkPos.X, chunkPos.Y] = null;
            }
        }

        public void SetVariation(int x, int y, int variation)
        {
            ValidateExists(x, y);
            GetChunkAt(x, y).SetVariation(GetPositionInChunk(x, y), variation);
        }

        public void SetType(int x, int y, int type)
        {
            ValidateExists(x, y);
            GetChunkAt(x, y).SetType(GetPositionInChunk(x, y), type);
        }

        public void SetData(int x, int y, int variation, int orientation, int type)
        {
            if (!_coveringTypes[type].IsValidAt(x, y)) return;

            ValidateExists(x, y);
            GetChunkAt(x, y).SetData(GetPositionInChunk(x, y), variation, orientation, type);
        }

        public void SetData(int x, int y, CoverData data)
        {
            if (!_coveringTypes[data.Type].IsValidAt(x, y)) return;

            ValidateExists(x, y);
            GetChunkAt(x, y).SetData(GetPositionInChunk(x, y), data);
        }

        public CoverData GetData(int x, int y)
        {
            CoverChunk chunk = GetChunkAt(x, y);
            if (chunk == null) return default; // no data to return

            Point posInChunk = GetPositionInChunk(x, y);
            return chunk[posInChunk.X, posInChunk.Y];
        }

        public void RemoveAt(int x, int y)
        {
            SetOrientation(x, y, 0);
        }

        /// <summary>
        /// Returns an orientation value that covers the entire tile.
        /// </summary>
        public int FullCoverOrientation(int x, int y)
        {
            Tile left = Framing.GetTileSafely(x - 1, y);
            Tile right = Framing.GetTileSafely(x + 1, y);
            Tile up = Framing.GetTileSafely(x, y - 1);
            Tile down = Framing.GetTileSafely(x, y + 1);

            byte b = new BitsByte(!WorldGen.SolidOrSlopedTile(up), !WorldGen.SolidOrSlopedTile(right), !WorldGen.SolidOrSlopedTile(down), !WorldGen.SolidOrSlopedTile(left), false, false, false, false);

            return b;
        }

        public int GetOrientationFor(bool coverUp, bool coverLeft, bool coverRight, bool coverDown)
        {
            return new BitsByte(coverUp, coverRight, coverDown, coverLeft, false, false, false, false);
        }

        public void SaveWorld(BinaryWriter writer)
        {
            // write all the types, their ID will be their index in this array
            writer.Write(_coveringTypes.Length);
            for (int i = 0; i < _coveringTypes.Length; i++)
            {
                writer.Write(_coveringTypes[i].GetType().FullName);
            }

            int chunksWidth = _chunks.GetLength(0);
            int chunksHeight = _chunks.GetLength(1);
            writer.Write(chunksWidth);
            writer.Write(chunksHeight);
            for (int y = 0; y < chunksHeight; y++)
            {
                for (int x = 0; x < chunksWidth; x++)
                {
                    if (_chunks[x, y] == null)
                    {
                        writer.Write(false);
                        continue;
                    }
                    writer.Write(true);
                    _chunks[x, y].Save(writer);
                }
            }
        }

        public void LoadWorld(BinaryReader reader)
        {
            Dictionary<string, int> currentTypes = new Dictionary<string, int>();
            for (int i = 0; i < _coveringTypes.Length; i++)
            {
                string typeName = _coveringTypes[i].GetType().FullName;
                currentTypes[typeName] = i;
            }

            Dictionary<int, int> typeConversions = new Dictionary<int, int>();
            int types = reader.ReadInt32();
            for (int i = 0; i < types; i++)
            {
                string type = reader.ReadString();
                // if this type no longer exists, we need to ensure that it's removed on load, so we'll set it's type to -1
                if (!currentTypes.TryGetValue(type, out int currentTypeIndex))
                {
                    typeConversions[i] = -1;
                    continue;
                }

                typeConversions[i] = currentTypeIndex;
            }

            int chunksWidth = reader.ReadInt32();
            int chunksHeight = reader.ReadInt32();
            for (int y = 0; y < chunksHeight; y++)
            {
                for (int x = 0; x < chunksWidth; x++)
                {
                    if (reader.ReadBoolean())
                    {
                        _chunks[x, y] = new CoverChunk(this, 64);
                        _chunks[x, y].Load(reader, typeConversions);
                    }
                    else
                    {
                        _chunks[x, y] = null;
                    }
                }
            }
        }

        private void ValidateExists(int x, int y)
        {
            Point chunkPos = GetChunkFor(x, y);
            if (_chunks[chunkPos.X, chunkPos.Y] == null) _chunks[chunkPos.X, chunkPos.Y] = new CoverChunk(this, 64);
        }

        private Point GetChunkFor(int worldX, int worldY) => new Point(worldX >> 6, worldY >> 6);

        private CoverChunk GetChunkAt(int worldX, int worldY)
        {
            Point chunkPos = GetChunkFor(worldX, worldY);
            return _chunks[chunkPos.X, chunkPos.Y];
        }

        private Point GetPositionInChunk(int worldX, int worldY) => new Point(worldX % 64, worldY % 64);
    }
}
