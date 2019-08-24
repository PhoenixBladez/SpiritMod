using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.Buffs.Glyph
{
	public class DivineStrike : ModBuff
	{
		public static int _type;
		public static Texture2D[] _textures;

		public override void SetDefaults()
		{
			DisplayName.SetDefault("Divine Strike");
			Description.SetDefault("Your next attack will deal ");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
		}

		public override bool ReApply(Player player, int time, int buffIndex)
		{
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
			if (modPlayer.divineStacks < 6)
				modPlayer.divineStacks++;
			return false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
			if (player.whoAmI == Main.myPlayer && !Main.dedServ)
				Main.buffTexture[Type] = _textures[modPlayer.divineStacks - 1];
			if (modPlayer.glyph == GlyphType.Radiant)
				player.buffTime[buffIndex] = 2;
			else
				player.DelBuff(buffIndex--);
		}

		public override void ModifyBuffTip(ref string tip, ref int rare)
		{
			MyPlayer modPlayer = Main.player[Main.myPlayer].GetModPlayer<MyPlayer>();
			tip += modPlayer.divineStacks * 11 + "% more damage.";
		}
	}
}
