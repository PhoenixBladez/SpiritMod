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
			NPCID.Sets.TownCritter[NPC.type] = true;

			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
			{
				Hide = true
			};
			NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
		}

		public override void SetDefaults()
		{
			NPC.friendly = true;
			NPC.townNPC = true;
			NPC.dontTakeDamage = true;
			NPC.width = 32;
			NPC.height = 48;
			NPC.aiStyle = 0;
			NPC.damage = 0;
			NPC.defense = 25;
			NPC.lifeMax = 10000;
			NPC.HitSound = SoundID.NPCHit1;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = 0f;
			NPC.rarity = 1;
		}

		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) => false;

		public override string GetChat() => "I thought I was a real goner there! If you didn't butt in, I probably would've been fed to whatever those monsters were trying to conjure up over there. I wouldn't touch it if I were you... Look, you have my thanks; but just between you and me, it's been a long few months, and all I want is a vacation from adventuring for a while. Life is short, and I'd rather not make it shorter. D'you have a place to stay?";

		public override void AI()
        {
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.homeless = false;
                NPC.homeTileX = -1;
                NPC.homeTileY = -1;
                NPC.netUpdate = true;
            }

            if (NPC.wet)
                NPC.life = 250;

            foreach (var player in Main.player)
            {
                if (!player.active)
					continue;

                if (player.talkNPC == NPC.whoAmI)
                {
                    Rescue();
                    return;
                }
            }
        }

		public void Rescue()
        {
            NPC.Transform(NPCType<Adventurer>());
            NPC.dontTakeDamage = false;
        }
	}
}

