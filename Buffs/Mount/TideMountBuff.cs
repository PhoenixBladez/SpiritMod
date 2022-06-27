using SpiritMod.Mounts;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Mount
{
	public class TideMountBuff : ModBuff
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crocodrillo Mount");
			Description.SetDefault("This cute lil' Crocodillo is your new best friend!");
			Main.buffNoTimeDisplay[Type] = true;
			Main.buffNoSave[Type] = true;
		}

		public override void Update(Player player, ref int buffIndex)
		{
			player.mount.SetMount(ModContent.MountType<TideMount>(), player, false);
			player.buffTime[buffIndex] = 10;
		}
	}
}
