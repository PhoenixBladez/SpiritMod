using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class CouchPotato : ModBuff
	{

		public override void SetDefaults()
		{
			DisplayName.SetDefault("Couch potato");
			Description.SetDefault("'Stop being so lazy!'");
			Main.debuff[Type] = true;
			Main.buffNoTimeDisplay[Type] = true;
			Main.pvpBuff[Type] = true;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = false;
			canBeCleared = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.statDefense += 6;
			player.moveSpeed *= 0.9f;
		}
	}
}
