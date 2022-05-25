using SpiritMod.Tiles;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.GlobalClasses.Tiles
{
	class IndestructibleGlobalTile : GlobalTile
	{
		public List<int> Indestructibles = new List<int>();

		public void Load(Mod mod)
		{
			var types = typeof(IndestructibleGlobalTile).Assembly.GetTypes();
			foreach (var type in types)
			{
				if (typeof(ModTile).IsAssignableFrom(type))
				{
					var tag = (TileTagAttribute)Attribute.GetCustomAttribute(type, typeof(TileTagAttribute));

					if (tag == null || tag.Tags.Length == 0)
						continue;

					if (tag.Tags.Contains(TileTags.Indestructible))
						Indestructibles.Add(mod.TileType(type.Name));
				}
			}
		}

		public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged)
		{
			Tile tileAbove = Framing.GetTileSafely(i, j - 1);

			if (Indestructibles.Contains(type) || Indestructibles.Contains(tileAbove.type))
				return false;
			return true;
		}

		public override bool TileFrame(int i, int j, int type, ref bool resetFrame, ref bool noBreak)
		{
			Tile tileAbove = Framing.GetTileSafely(i, j - 1);

			if (Indestructibles.Contains(tileAbove.type) && type != tileAbove.type && TileID.Sets.Falling[type])
				return false;
			return true;
		}

		public override bool PreHitWire(int i, int j, int type)
		{
			Tile tileAbove = Framing.GetTileSafely(i, j - 1);

			if (Indestructibles.Contains(tileAbove.type) && type != tileAbove.type)
				Main.tile[i, j].inActive(false);
			return true;
		}

		public override bool Slope(int i, int j, int type)
		{
			Tile tileAbove = Framing.GetTileSafely(i, j - 1);

			if (Indestructibles.Contains(type) || Indestructibles.Contains(tileAbove.type))
				return false;
			return true;
		}

		public override bool CanExplode(int i, int j, int type)
		{
			Tile tileAbove = Framing.GetTileSafely(i, j - 1);

			if (Indestructibles.Contains(tileAbove.type) || Indestructibles.Contains(type))
				return false;
			return true;
		}
	}
}
