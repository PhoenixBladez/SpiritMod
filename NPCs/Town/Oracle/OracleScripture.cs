using Terraria;
using Terraria.Audio;
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
			Item.width = 46;
			Item.height = 36;
			Item.rare = ItemRarityID.LightRed;
			Item.maxStack = 1;
			Item.value = Item.sellPrice(0, 0, 3, 0);
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.useTime = Item.useAnimation = 50;
			Item.noMelee = true;
			Item.autoReuse = true;
			Item.noUseGraphic = true;
		}

		public override bool CanUseItem(Player player) => player.GetSpiritPlayer().inMarble;

		public override bool? UseItem(Player player)/* tModPorter Suggestion: Return null instead of false */
		{
			int w = NPC.FindFirstNPC(ModContent.NPCType<Oracle>());
			if (w != -1)
			{
				var oracle = Main.npc[w].ModNPC as Oracle;
				oracle.Teleport = 200;
				oracle.TeleportX = player.Center.X;
				oracle.TeleportY = player.Center.Y;

				int glyphnum = Main.rand.Next(10);
				DustHelper.DrawDustImage(new Vector2(player.Center.X, player.Center.Y - 25), ModContent.DustType<Dusts.MarbleDust>(), 0.05f, "SpiritMod/Effects/Glyphs/Glyph" + glyphnum, 1f);
				SoundEngine.PlaySound(SoundID.DD2_DarkMageHealImpact, player.Center);

				return true;
			}
			return false;
		}
	}
}
