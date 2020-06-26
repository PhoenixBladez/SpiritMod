using Microsoft.Xna.Framework;
using System;
using System.Net.Mail;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.World
{
    public abstract class AStructure
    {
		public virtual int OffsetX => 0;
		public virtual int OffsetY => 0;
		public virtual int[,] Tiles => new int[0, 0];
		public virtual int[,] Slopes => new int[0, 0];
		public virtual int[,] Walls => new int[0, 0];
		public virtual int[,] Furniture => new int[0, 0];

		protected virtual TileData TileMap(int tile, int x, int y) => null;
		protected virtual TileData FurnitureMap(int tile, int x, int y) => null;
		protected virtual WallData WallMap(int tile, int x, int y) => null;

		/// <summary>
		/// Place worldgen code for the structure in here
		/// </summary>
		public virtual bool Generate() => false;

		/// <summary>
		/// Places the structure at the given world position
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public virtual void Place(int x, int y)
		{
			PlaceTiles(x, y);
			PlaceWalls(x, y);
			PlaceFurniture(x, y);
		}

		protected virtual void PlaceTiles(int x, int y)
		{
			for(int i = 0; i < Tiles.GetLength(1); i++) {
				for(int j = 0; j < Tiles.GetLength(0); j++) {
					int wX = x + i + OffsetX;
					int wY = y + j + OffsetY;
					var data = TileMap(Tiles[j, i], i, j);
					if(data != null) {
						if(data.type == -1) Framing.GetTileSafely(wX, wY).ClearTile();
						else if(data.type >= 0) {
							WorldGen.PlaceTile(wX, wY, data.type, data.mute, data.force, -1, data.style);
							WorldGen.SlopeTile(wX, wY, Slopes[j, i]);
							if(TileID.Sets.Platforms[data.type]) WorldGen.SquareTileFrame(wX, wY);
						}
					}
				}
			}
		}

		protected virtual void PlaceWalls(int x, int y)
		{
			for(int i = 0; i < Walls.GetLength(1); i++) {
				for(int j = 0; j < Walls.GetLength(0); j++) {
					int wX = x + i + OffsetX;
					int wY = y + j + OffsetY;
					var data = WallMap(Walls[j, i], i, j);
					if(data != null) {
						if(data.type == -1) WorldGen.KillWall(wX, wY);
						else if(data.type >= 0) WorldGen.PlaceWall(wX, wY, data.type, data.mute);
					}
				}
			}
		}

		protected virtual void PlaceFurniture(int x, int y)
		{
			for(int i = 0; i < Furniture.GetLength(1); i++) {
				for(int j = 0; j < Furniture.GetLength(0); j++) {
					int wX = x + i + OffsetX;
					int wY = y + j + OffsetY;
					var data = FurnitureMap(Furniture[j, i], i, j);
					if(data != null) {
						if(data is ObjectData @objData) 
							WorldGen.PlaceObject(wX, wY, data.type, data.mute, data.style, objData.alternate, objData.random, objData.direction);
						else if(data is ChestData @chestData) 
							WorldGen.PlaceChest(wX, wY, (ushort)data.type, chestData.notNearOtherChests, style: data.style);
						else 
							WorldGen.PlaceTile(wX, wY, data.type, data.mute, data.force, -1, data.style);
					}
				}
			}
		}

		protected class TileData
		{
			public readonly int type;
			public readonly int style;
			public readonly bool mute;
			public readonly bool force;

			public TileData(int type = 0, int style = 0, bool force = true, bool mute = true)
			{
				this.type = type;
				this.style = style;
				this.mute = mute;
				this.force = force;
			}
		}

		protected class WallData
		{
			public readonly int type;
			public readonly bool mute;
			
			public WallData(int type = 0, bool mute = true)
			{
				this.type = type;
				this.mute = mute;
			}
		}
    
		protected class ObjectData : TileData
		{
			public readonly int alternate;
			public readonly int random;
			public readonly int direction;

			public ObjectData(int type = 0, int style = 0, bool force = true, bool mute = true, int alternate = 0, int random = -1, int direction = -1) : base(type, style, force, mute)
			{
				this.alternate = alternate;
				this.random = random;
				this.direction = direction;
			}
		}

		protected class ChestData : TileData
		{
			public readonly bool notNearOtherChests;

			public ChestData(int type = 21, int style = 0, bool force = true, bool mute = true, bool notNearOtherChests = false) : base(type, style, force, mute)
			{
				this.notNearOtherChests = notNearOtherChests;
			}
		}
	}
}