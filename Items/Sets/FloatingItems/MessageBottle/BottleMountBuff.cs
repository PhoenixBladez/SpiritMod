using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.FloatingItems.MessageBottle
{
	public class BottleMountBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Raft");
			Description.SetDefault("Sending out an SOS");
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.mount.SetMount(ModContent.MountType<MessageBottleMount>(), player, false);
			player.buffTime[buffIndex] = 10;
		}
	}
}
