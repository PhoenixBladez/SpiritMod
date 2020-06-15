using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs
{
    public class TrueMarksman : ModBuff
    {
        public override void SetDefaults() {
            DisplayName.SetDefault("True Marksman");
            Description.SetDefault("Gun damage is increased dramatically");
            Main.pvpBuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex) {
          player.bulletDamage+=1f;
        }
    }
}
