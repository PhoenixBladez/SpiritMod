using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs.StarjinxEvent;
using System;
using Microsoft.Xna.Framework;

namespace SpiritMod.Items.Sets.StarjinxSet
{
	public class StarjinxSummoner : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starjinx Summoner");
			Tooltip.SetDefault("Placeholder! Summons the Starjinx Meteor.");
		}

		public override void SetDefaults()
		{
			item.maxStack = 99;
			item.width = item.height = 16;
			item.useTime = item.useAnimation = 20;

			item.useStyle = ItemUseStyleID.HoldingUp;
			item.rare = ItemRarityID.LightRed;
			item.UseSound = SoundID.Item43;

			item.noMelee = true;
			item.consumable = true;
			item.autoReuse = false;
		}

		public override bool CanUseItem(Player player) => !NPC.AnyNPCs(ModContent.NPCType<StarjinxMeteorite>());

		public override bool UseItem(Player player)
		{

			int width = 200 + (int)(((Main.maxTilesX / 4200f) - 1) * 75);
			int x = MyWorld.asteroidSide == 0 ? (width * 16) + 80 : (Main.maxTilesX * 16) - ((width * 16) + 80);
			Vector2 finalPos = GetOpenSpace(x, 1600);

			if (finalPos != Vector2.Zero)
			{
				Main.NewText("An enchanted comet has appeared in the asteroid field!", 252, 150, 255);
				NPC.NewNPC((int)finalPos.X, (int)finalPos.Y, ModContent.NPCType<StarjinxMeteorite>());
				StarjinxEventWorld.SpawnedStarjinx = true;
			}
			else
				Main.NewText("A comet soars overhead...", 202, 100, 205);
			return true;
		}

		/// <summary>Get an open space for the meteorite to spawn.</summary>
		/// <param name="x">Original X position.</param>
		/// <param name="y">Original Y position.</param>
		private Vector2 GetOpenSpace(int x, int y)
		{
			const int MinX = 1600;
			const int MinY = 1600;

			var temp = new NPC();
			temp.SetDefaults(ModContent.NPCType<StarjinxMeteorite>());

			int attempts = 0;

			while (true)
			{
				if (attempts++ >= 200)
					break;

				var spawnPos = new Vector2(x, y) + (new Vector2(Main.rand.Next(-1000, 1000), Main.rand.Next(-1000, 1000)) * (attempts / 25f));
				if (spawnPos.X < MinX) spawnPos.X = MinX;
				if (spawnPos.Y < MinY) spawnPos.Y = MinY;

				const float SizeArea = 8;

				if (!Collision.SolidCollision(spawnPos - (temp.Size * (SizeArea / 2)), (int)(temp.width * SizeArea), (int)(temp.height * SizeArea)))
					return spawnPos;
			}
			return Vector2.Zero;
		}
	}
}
