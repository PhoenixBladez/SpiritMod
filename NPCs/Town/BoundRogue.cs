using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;
using Microsoft.Xna.Framework;

namespace SpiritMod.NPCs.Town
{
    public class BoundRogue : ModNPC
    {
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Bound Rogue");
            NPCID.Sets.TownCritter[npc.type] = true;
        }

        public override void SetDefaults() {
            npc.friendly = true;
            npc.townNPC = true;
            npc.dontTakeDamage = true;
            npc.width = 32;
            npc.height = 48;
            npc.aiStyle = 0;
            npc.damage = 0;
            npc.defense = 25;
            npc.lifeMax = 10000;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0f;
            npc.rarity = 1;
        }
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
        {
            return false;
        }
        public override string GetChat() {
            npc.Transform(NPCType<Rogue>());
            npc.dontTakeDamage = false;
            return MyWorld.gennedTower
                ? "Thanks for saving me from these goblins!"
                : "Hey! Thanks for saving me- Now, mind getting us out of this pickle and killing these bandits? They duped me and took all my cash! Don't think it means I'll discount my wares for you, though. Just kidding! Not.";
        }
        public override void AI() {
            if(Main.netMode != NetmodeID.MultiplayerClient) {
                npc.homeless = false;
                npc.homeTileX = -1;
                npc.homeTileY = -1;
                npc.netUpdate = true;
            }

            if(npc.wet) {
                npc.life = 250;
            }
        }
    }
}

