using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.NPCs.Town;
using static Terraria.ModLoader.ModContent;

namespace SpiritMod.NPCs.Town
{
	public class BoundGambler : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bound Gambler");
			NPCID.Sets.TownCritter[NPC.type] = true;
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
		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			return false;
		}
		public override string GetChat()
		{
			NPC.Transform(NPCType<Gambler>());
			NPC.dontTakeDamage = false;
			return "Must be my lucky day to see a friendly face around here!\nThese goblins didn't take too kindly to me offering a, uh, rigged deal.\nAnyway, d'you have a place to stay? Let's flip for it.";
		}
		public override void AI()
		{
			if (Main.netMode != NetmodeID.MultiplayerClient) {
				NPC.homeless = false;
				NPC.homeTileX = -1;
				NPC.homeTileY = -1;
				NPC.netUpdate = true;
			}

			if (NPC.wet) {
				NPC.life = 250;
			}
            foreach (var player in Main.player)
            {
                if (!player.active) continue;
                if (player.talkNPC == NPC.whoAmI)
                {
                    Rescue();
                    return;
                }
            }
        }
        public void Rescue()
        {
            NPC.Transform(NPCType<Gambler>());
            NPC.dontTakeDamage = false;
        }
    }
}

