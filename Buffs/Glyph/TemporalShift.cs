using System;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.Buffs.Glyph
{
	public class TemporalShift : ModBuff
	{
		public static int _type;
		public static Texture2D[] _textures;

		public override void SetDefaults()
		{
			DisplayName.SetDefault("Temporal Shift");
			Description.SetDefault("Double tap to dash and gain a temporary speed boost");
			Main.buffNoSave[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			Main.pvpBuff[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();

			if (player.whoAmI != Main.myPlayer && player.buffTime[buffIndex] > 1)
				player.buffTime[buffIndex]--;
			if (player.buffTime[buffIndex] > 1)
			{
				modPlayer.phaseShift = true;
				Main.buffNoTimeDisplay[Type] = false;
			}
			else if (modPlayer.phaseStacks > 0)
			{
				player.buffTime[buffIndex] = 2;
				Main.buffNoTimeDisplay[Type] = true;
			}
			else if (player.whoAmI == Main.myPlayer)
			{
				player.DelBuff(buffIndex--);
			}

			if (player.whoAmI == Main.myPlayer && !Main.dedServ)
				Main.buffTexture[Type] = _textures[modPlayer.phaseStacks];
		}

		public override void ModifyBuffTip(ref string tip, ref int rare)
		{
			MyPlayer modPlayer = Main.LocalPlayer.GetModPlayer<MyPlayer>();
			if (modPlayer.phaseShift)
				tip = "High speed and immunity to all movement impairment";
		}
	}
}
