using Microsoft.Xna.Framework;
using SpiritMod.NPCs;
using System;
using Terraria;
using Terraria.ModLoader;

namespace SpiritMod.Buffs.Glyph
{
    public class SanguineBleed : ModBuff
    {
        public override void SetDefaults() {
            DisplayName.SetDefault("Crimson Bleed");
            Description.SetDefault("You are rapidly losing blood.");
            Main.buffNoSave[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.debuff[Type] = true;
        }

        public override bool ReApply(NPC npc, int time, int buffIndex) {
            if(time < 357) {
                return false;
            }

            npc.buffTime[buffIndex] = 357;

            return true;
        }

        public override void Update(NPC npc, ref int buffIndex) {
            GNPC modNPC = npc.GetGlobalNPC<GNPC>();

            double chance = Math.Max(0.02, npc.width * npc.height * 0.00003);
            if(!modNPC.sanguinePrev) {
                for(int i = 0; i < 4; i++) {
                    Vector2 offset = Main.rand.NextVec2CircularEven(npc.width >> 1, npc.height >> 1);
                    Dust.NewDustPerfect(npc.Center + offset, ModContent.DustType<Dusts.Blood>()).customData = npc;
                }
            }

            if(Main.rand.NextDouble() < chance && npc.buffTime[buffIndex] > 60) {
                Vector2 offset = Main.rand.NextVec2CircularEven(npc.width >> 1, npc.height >> 1);
                Dust.NewDustPerfect(npc.Center + offset, ModContent.DustType<Dusts.Blood>()).customData = npc;
            }

            modNPC.sanguineBleed = true;
        }
    }
}
