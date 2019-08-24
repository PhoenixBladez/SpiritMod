using System;

using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Glyph
{
	public class CollapsingVoid : ModBuff
	{
		public static int _type;
		public static Texture2D[] _textures;

		public override void SetDefaults()
		{
			DisplayName.SetDefault("Collapsing Void");
			Description.SetDefault("");
			Main.pvpBuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override bool ReApply(Player player, int time, int buffIndex)
		{
			if (time >= 60)
			{
				MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
				if (modPlayer.voidStacks < 3)
					modPlayer.voidStacks++;
				Main.buffNoTimeDisplay[Type] = false;
			}
			return false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			MyPlayer modPlayer = player.GetModPlayer<MyPlayer>();
			player.endurance += modPlayer.voidStacks * 0.05f;
			if (modPlayer.voidStacks > 1 && player.buffTime[buffIndex] <= 2)
			{
				modPlayer.voidStacks--;
				player.buffTime[buffIndex] = 299;
			}
			if (modPlayer.voidStacks <= 1)
				Main.buffNoTimeDisplay[Type] = true;
			
			if (player.whoAmI == Main.myPlayer && !Main.dedServ)
				Main.buffTexture[Type] = _textures[modPlayer.voidStacks];
		}

		public override void ModifyBuffTip(ref string tip, ref int rare)
		{
			MyPlayer modPlayer = Main.player[Main.myPlayer].GetModPlayer<MyPlayer>();
			tip = "Damage taken is reduced by " + modPlayer.voidStacks * 5 + "%";
			rare = modPlayer.voidStacks >> 1;
		}
	}
}
