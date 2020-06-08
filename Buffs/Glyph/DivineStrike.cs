using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Glyph
{
    public class DivineStrike : ModBuff
    {
        public override void SetDefaults() {
            DisplayName.SetDefault("Divine Strike");
            Description.SetDefault("Your next attack will deal ");
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override bool ReApply(Player player, int time, int buffIndex) {
            MyPlayer modPlayer = player.GetSpiritPlayer();
            if(modPlayer.divineStacks < 6) {
                modPlayer.divineStacks++;
            }

            return false;
        }

        public override void Update(Player player, ref int buffIndex) {
            MyPlayer modPlayer = player.GetSpiritPlayer();
            if(player.whoAmI == Main.myPlayer && !Main.dedServ) {
                if(modPlayer.divineStacks == 1) {
                    Main.buffTexture[Type] = mod.GetTexture("DivineStrike");
                } else {
                    Main.buffTexture[Type] = mod.GetTexture("DivineStrike_" + (modPlayer.divineStacks - 1).ToString());
                }
            }

            if(modPlayer.glyph == GlyphType.Radiant) {
                player.buffTime[buffIndex] = 2;
            } else {
                player.DelBuff(buffIndex--);
            }
        }

        public override void ModifyBuffTip(ref string tip, ref int rare) {
            MyPlayer modPlayer = Main.LocalPlayer.GetSpiritPlayer();
            tip += $"{modPlayer.divineStacks * 11}% more damage.";
        }
    }
}
