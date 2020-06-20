
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using SpiritMod.NPCs.Tides;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Tides
{
    public class KakamoraRunner : ModNPC
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Kakamora");
            Main.npcFrameCount[npc.type] = 4;
        }

        public override void SetDefaults() {
            npc.width = 42;
            npc.height = 38;
            npc.damage = 24;
            npc.defense = 4;
            aiType = NPCID.SnowFlinx;
            npc.aiStyle = 3;
            npc.lifeMax = 120;
            npc.knockBackResist = .70f;
            npc.value = 200f;
            npc.noTileCollide = false;
             npc.HitSound = SoundID.NPCHit2;
            npc.DeathSound = SoundID.NPCDeath1;
        }

        public override void AI() {
            npc.spriteDirection = npc.direction;
            if (Main.rand.NextBool(1500))
            {
                Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/Kakamora/KakamoraIdle3"));
            }
        }

        public override void FindFrame(int frameHeight) {
            if (npc.collideY)
            {
            npc.frameCounter += 0.2f;
            npc.frameCounter %= Main.npcFrameCount[npc.type];
            int frame = (int)npc.frameCounter;
            npc.frame.Y = frame * frameHeight;
            }
        }
        public override void HitEffect(int hitDirection, double damage) {
            if(npc.life <= 0) {
                 Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/Kakamora/KakamoraDeath"));
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Kakamora_Gore1"), 1f);
                Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Kakamora_Gore2"), 1f);
                 Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/Kakamora_Gore3"), 1f);
            }
            else
            {
                 Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/Kakamora/KakamoraHit"));
            }
        }
    }
}
