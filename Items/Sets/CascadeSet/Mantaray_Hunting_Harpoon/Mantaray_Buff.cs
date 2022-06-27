using Terraria.ModLoader;
using Terraria;

namespace SpiritMod.Items.Sets.CascadeSet.Mantaray_Hunting_Harpoon
{
    public class Mantaray_Buff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
			DisplayName.SetDefault("Manta Ray");
			Description.SetDefault("Swift as the tides !");
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            player.mount.SetMount(ModContent.MountType<Mantaray_Mount>(), player);
            player.buffTime[buffIndex] = 10;
        }
    }
}