using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.Town
{
    public class BoundAdventurer : ModNPC
    {
        public static int _type;


        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Ensnared Adventurer");
            NPCID.Sets.TownCritter[npc.type] = true;
        }

        public override void SetDefaults()
        {
            npc.friendly = true;
            npc.townNPC = true;
            npc.dontTakeDamage = true;
            npc.width = 32;
            npc.height = 48;
            npc.aiStyle = 0;
            npc.damage = 0;
            npc.defense = 25;
            npc.lifeMax = 250;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0f;
            npc.rarity = 1;
        }
        public override string GetChat()
        {
            npc.Transform(mod.NPCType("Adventurer"));
            npc.dontTakeDamage = false; 
            return "I thought I was a real goner there! If you didn't butt in, I likley would have been fed to whatever those monsters were trying conjure up in that scary altar over there. I wouldn't touch it if I were you... Look, you have my thanks but between you and me, it's been a long few months and all I want is a vacation from adventuring for a while. Life is short, I'd rather not make it any shorter. I'll see you around sometime.";
        }
        public override void AI()
        {
            if (Main.netMode != 1)
            {
                npc.homeless = false;
                npc.homeTileX = -1;
                npc.homeTileY = -1;
                npc.netUpdate = true;
            }
            if (npc.wet)
            {
                npc.life = 250;

            }
        }
    }
}

