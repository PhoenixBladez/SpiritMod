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
		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			return false;
		}
		public override string GetChat()
		{
			npc.Transform(NPCType<Gambler>());
			npc.dontTakeDamage = false;
			return "Must be my lucky day to see a friendly face around here!\nThese goblins didn't take too kindly to me offering a, uh, rigged deal.\nAnyway, d'you have a place to stay? Let's flip for it.";
		}
		public override void AI()
		{
			if (Main.netMode != NetmodeID.MultiplayerClient) {
				npc.homeless = false;
				npc.homeTileX = -1;
				npc.homeTileY = -1;
				npc.netUpdate = true;
			}

			if (npc.wet) {
				npc.life = 250;
			}
            foreach (var player in Main.player)
            {
                if (!player.active) continue;
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

