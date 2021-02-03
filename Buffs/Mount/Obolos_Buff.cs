using Terraria.ModLoader;
using Terraria;
 
namespace SpiritMod.Buffs.Mount
{
    public class Obolos_Buff : ModBuff
    {
        public override void SetDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
			DisplayName.SetDefault("Obolos");
			Description.SetDefault("Pretty grimacing...");
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            player.mount.SetMount(mod.MountType("Obolos_Mount"), player);
            player.buffTime[buffIndex] = 10;
        }
    }
}