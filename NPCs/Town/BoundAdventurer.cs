using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SpiritMod.NPCs.Town
{
	public class BoundAdventurer : ModNPC
	{
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
			npc.lifeMax = 10000;
			npc.HitSound = SoundID.NPCHit1;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = 0f;
			npc.rarity = 1;
		}

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) => false;

		public override string GetChat() => "I thought I was a real goner there! If you didn't butt in, I probably would've been fed to whatever those monsters were trying to conjure up over there. I wouldn't touch it if I were you... Look, you have my thanks; but just between you and me, it's been a long few months, and all I want is a vacation from adventuring for a while. Life is short, and I'd rather not make it shorter. D'you have a place to stay?";

		public override void AI()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                npc.homeless = false;
                npc.homeTileX = -1;
                npc.homeTileY = -1;
                npc.netUpdate = true;
            }

            if (npc.wet)
                npc.life = 250;

            foreach (var player in Main.player)
            {
                if (!player.active)
					continue;

                if (player.talkNPC == npc.whoAmI)
                {
                    Rescue();
                    return;
                }
            }
        }

		public void Rescue()
        {
            npc.Transform(NPCType<Adventurer>());
            npc.dontTakeDamage = false;
        }
	}
}

