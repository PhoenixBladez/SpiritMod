using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
    public class OverDrive : ModBuff
	{
		public override void SetDefaults()
		{
            DisplayName.SetDefault("Overdrive");
            Description.SetDefault("Your movement speed and throwing damage are charged up!");
            Main.buffNoTimeDisplay[Type] = false;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.maxRunSpeed += 0.2f;
			player.thrownDamage += 0.09f;

			Dust.NewDust(player.position, player.width, player.height, DustID.Electric);
		}
	}
}
