using Terraria.ModLoader;
using Terraria;
 
namespace SpiritMod.Buffs.Mount
{
    public class Obolos_Buff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
			DisplayName.SetDefault("Obolos");
			Description.SetDefault("Pretty grimacing...");
        }
 
        public override void Update(Player player, ref int buffIndex)
        {
            player.mount.SetMount(Mod.Find<ModMount>("Obolos_Mount").Type, player);
            player.buffTime[buffIndex] = 10;
        }
    }
}