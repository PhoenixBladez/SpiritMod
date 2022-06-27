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
		public List<int> IndestructiblesUngrounded = new List<int>();

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
						Indestructibles.Add(mod.Find<ModTile>(type.Name).Type);

					if (tag.Tags.Contains(TileTags.IndestructibleNoGround))
						IndestructiblesUngrounded.Add(mod.Find<ModTile>(type.Name).Type);
				}
			}
		}

		public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged)
		{
			Tile tileAbove = Framing.GetTileSafely(i, j - 1);

			if (Indestructibles.Contains(type) || Indestructibles.Contains(tileAbove.TileType)) //Check for indestructibles
				return false;
			if (IndestructiblesUngrounded.Contains(type)) //Check for floating indesctructibles
				return false;
			return true;
		}

		public override bool TileFrame(int i, int j, int type, ref bool resetFrame, ref bool noBreak)
		{
			Tile tileAbove = Framing.GetTileSafely(i, j - 1);

			if (Indestructibles.Contains(tileAbove.TileType) && type != tileAbove.TileType && TileID.Sets.Falling[type])
				return false;
			if (IndestructiblesUngrounded.Contains(type))
				return false;
			return true;
		}

		public override bool PreHitWire(int i, int j, int type)
		{
			Tile tileAbove = Framing.GetTileSafely(i, j - 1);

			if (Indestructibles.Contains(tileAbove.TileType) && type != tileAbove.TileType)
				Main.tile[i, j].IsActuated = false;
			return true;
		}

		public override bool Slope(int i, int j, int type) => !Indestructibles.Contains(type);

		public override bool CanExplode(int i, int j, int type)
		{
			Tile tileAbove = Framing.GetTileSafely(i, j - 1);

			if (Indestructibles.Contains(tileAbove.TileType) || Indestructibles.Contains(type))
				return false;
			if (IndestructiblesUngrounded.Contains(type))
				return false;
			return true;
		}
	}
}
