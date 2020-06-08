using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
    public class ClockBuff : ModBuff
    {
        public override void SetDefaults() {
            DisplayName.SetDefault("Time power");
            Description.SetDefault("ZAWARUDO!!");
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex) {
            MyPlayer modPlayer = player.GetSpiritPlayer();
            player.GetModPlayer<MyPlayer>().clockActive = true;
        }
    }
}