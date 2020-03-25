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
            Description.SetDefault("Your movement speed and melee speed are charged up!");
            Main.buffNoTimeDisplay[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.maxRunSpeed += 0.2f;
			player.meleeSpeed += 0.1f;
            player.runAcceleration += .04f;
        }
	}
}
