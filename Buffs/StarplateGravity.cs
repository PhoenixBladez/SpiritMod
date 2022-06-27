using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
	public class StarplateGravity : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Starplate Gravity");
			Description.SetDefault("The Starplate Voyager pulls you toward the earth...");
			Main.buffNoTimeDisplay[Type] = true;
			Main.debuff[Type] = true;
			Main.pvpBuff[Type] = false;
			Main.buffNoSave[Type] = true;
			longerExpertDebuff = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.gravity = 0.4f;
		}
	}
}
