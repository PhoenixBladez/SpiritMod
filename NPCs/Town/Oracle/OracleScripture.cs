using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace SpiritMod.NPCs.Town.Oracle
{
	public class OracleScripture : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sacred Scripture");
			Tooltip.SetDefault("Calls the Oracle to you\nCan only be used while in the Marble Caverns");
		}

		public override void SetDefaults()
		{
			item.width = 46;
			item.height = 36;
			item.rare = ItemRarityID.LightRed;
			item.maxStack = 1;
			item.value = Item.sellPrice(0, 0, 3, 0);
			item.useStyle = ItemUseStyleID.HoldingUp;
			item.useTime = item.useAnimation = 50;
			item.noMelee = true;
			item.autoReuse = true;
			item.noUseGraphic = true;
		}

		public override bool UseItem(Player player)
		{
			if (player.GetSpiritPlayer().inMarble)
			{
				int w = NPC.FindFirstNPC(ModContent.NPCType<Oracle>());
				if (w != -1)
				{
					var oracle = Main.npc[w].modNPC as Oracle;
					oracle.Teleport = 200;
					oracle.TeleportX = player.Center.X;
					oracle.TeleportY = player.Center.Y;

					int glyphnum = Main.rand.Next(10);
					DustHelper.DrawDustImage(new Vector2(player.Center.X, player.Center.Y - 25), ModContent.DustType<Dusts.MarbleDust>(), 0.05f, "SpiritMod/Effects/Glyphs/Glyph" + glyphnum, 1f);
					Main.PlaySound(SoundID.DD2_DarkMageHealImpact, player.Center);


					return true;
				}
			}
			return false;
		}
	}
}
