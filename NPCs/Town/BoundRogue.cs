using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;

namespace SpiritMod.NPCs.Town
{
	public class BoundRogue : ModNPC
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bound Bandit");
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
		public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			return false;
		}
		public override string GetChat()
		{
			NPC.Transform(NPCType<Rogue>());
			NPC.dontTakeDamage = false;
			return "Hey! Thanks for saving me- Now, mind getting us out of this pickle? They duped me, took all my cash and left me for dead here! Don't think it means I'll discount my wares for you, though. Just kidding! Not.";
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
            NPC.Transform(NPCType<Rogue>());
            NPC.dontTakeDamage = false;
        }
    }
}

