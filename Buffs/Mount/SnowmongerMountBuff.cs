using Terraria.ModLoader;
using Terraria;

namespace SpiritMod.Buffs.Mount
{
    public class SnowmongerMountBuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
			DisplayName.SetDefault("Snowmech");
			Description.SetDefault("Powerful");
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            player.mount.SetMount(ModContent.MountType<Mounts.SnowMongerMount.SnowmongerMount>(), player);
            player.buffTime[buffIndex] = 10;
        }
    }
}