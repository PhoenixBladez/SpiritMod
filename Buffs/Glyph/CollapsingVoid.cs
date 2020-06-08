using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Glyph
{
    public class CollapsingVoid : ModBuff
    {
        public override void SetDefaults() {
            DisplayName.SetDefault("Collapsing Void");
            Description.SetDefault("");
            Main.pvpBuff[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
            Main.buffNoSave[Type] = true;
        }

        public override bool ReApply(Player player, int time, int buffIndex) {
            if(time >= 60) {
                MyPlayer modPlayer = player.GetSpiritPlayer();
                if(modPlayer.voidStacks < 3) {
                    modPlayer.voidStacks++;
                }

                Main.buffNoTimeDisplay[Type] = false;
            }

            return false;
        }

        public override void Update(Player player, ref int buffIndex) {
            MyPlayer modPlayer = player.GetSpiritPlayer();
            player.endurance += modPlayer.voidStacks * 0.05f;

            if(modPlayer.voidStacks > 1 && player.buffTime[buffIndex] <= 2) {
                modPlayer.voidStacks--;
                player.buffTime[buffIndex] = 299;
            }

            if(modPlayer.voidStacks <= 1) {
                Main.buffNoTimeDisplay[Type] = true;
            }

            if(player.whoAmI == Main.myPlayer && !Main.dedServ) {
                if(modPlayer.voidStacks == 0) {
                    Main.buffTexture[Type] = mod.GetTexture("CollapsingVoid");
                } else {
                    Main.buffTexture[Type] = mod.GetTexture("CollapsingVoid_" + modPlayer.voidStacks.ToString());
                }
            }
        }

        public override void ModifyBuffTip(ref string tip, ref int rare) {
            MyPlayer modPlayer = Main.LocalPlayer.GetSpiritPlayer();
            tip = $"Damage taken is reduced by {modPlayer.voidStacks * 5}%";
            rare = modPlayer.voidStacks >> 1;
        }
    }
}
