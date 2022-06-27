using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Mounts.Minecarts.MarbleMinecart
{
	public class MarbleMinecartBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nemean Chariot"); //name tbd?
			Description.SetDefault("'You feel powerful!'");
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.mount.SetMount(ModContent.MountType<MarbleMinecart>(), player);
			player.buffTime[buffIndex] = 10;

			//if (Math.Abs(player.velocity.X) > 3)
			//	player.armorEffectDrawShadow = true;
		}
	}
}
