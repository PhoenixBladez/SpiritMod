using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
    public class Crunk : ModBuff
	{
		public override void SetDefaults()
		{
            DisplayName.SetDefault("Drunk");
            Description.SetDefault("Go home, you're drunk");
            Main.buffNoTimeDisplay[Type] = false;
			Main.pvpBuff[Type] = false;
			Main.debuff[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.statDefense -= 10;
			player.meleeSpeed += 0.12f;
			player.meleeDamage += 0.12f;
			player.meleeCrit += 5;
		}
	}
}