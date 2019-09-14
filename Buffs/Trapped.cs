using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
    public class Trapped : ModBuff
    {
        public override void SetDefaults()
        {
            DisplayName.SetDefault("Trapped");
            Description.SetDefault("It's a trap!");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.velocity.X *= 0.93f;
            Dust.NewDust(player.position, player.width, player.height, DustID.Dirt);
        }
    }
}
