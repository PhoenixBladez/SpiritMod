using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Projectiles.Hostile;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Tide;
using SpiritMod.Items.Material;
using SpiritMod.Items.Weapon.Summon;
using SpiritMod.Items.Weapon.Magic;
using static Terraria.ModLoader.ModContent;

namespace SpiritMod.NPCs.Tides
{
	public class LargeCrustecean : ModNPC
	{
		internal ref float blockTimer => ref npc.ai[2];
		internal ref float blocking => ref npc.ai[3];

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bubble Brute");
			Main.npcFrameCount[npc.type] = 9;
		}

		public override void SetDefaults()
		{
			npc.width = 80;
			npc.height = 82;
			npc.damage = 25;
			npc.defense = 4;
			aiType = NPCID.WalkingAntlion;
			npc.aiStyle = 3;
			npc.lifeMax = 375;
			npc.knockBackResist = .2f;
			npc.value = 200f;
			npc.noTileCollide = false;
			npc.HitSound = SoundID.NPCHit18;
			npc.DeathSound = SoundID.NPCDeath5;
            banner = npc.type;
            bannerItem = ItemType<Items.Banners.BubbleBruteBanner>();
        }

		public override void AI()
		{
			npc.TargetClosest(true);
			Player player = Main.player[npc.target];
			npc.ai[0]++;

			if (npc.wet) {
				npc.noGravity = true;
				if (npc.velocity.Y > -7) npc.velocity.Y -= .085f;
				return;
			}
			else {
				npc.noGravity = false;
			}

			if(npc.ai[0] == 200) {
				npc.frameCounter = 0;
				npc.velocity.X = 0;
			}

			if(npc.ai[0] > 250) {
				blocking = 1;
				npc.netUpdate = true;
			}

			if(npc.ai[0] > 350) {
				blocking = 1;
				npc.ai[0] = 0;
				npc.frameCounter = 0;
				npc.netUpdate = true;
			}

			if(blocking == 1) {
				npc.aiStyle = 0;

				if(player.position.X > npc.position.X) npc.spriteDirection = 1;
				else npc.spriteDirection = -1;

				if (npc.frameCounter > 2 && npc.ai[0] % 5 == 0) {
					Main.PlaySound(SoundID.Item, npc.Center, 85);
					Projectile.NewProjectile(npc.Center.X + (npc.direction * 34), npc.Center.Y - 4, npc.direction * Main.rand.NextFloat(3, 6), -Main.rand.NextFloat(1), ProjectileType<LobsterBubbleSmall>(), npc.damage / 2, 1);
				}
			}
			else {
                npc.spriteDirection = npc.direction;
				npc.aiStyle = 3;
			}

		}

        public override void NPCLoot()
        {
            if (Main.rand.NextBool(15))
                Item.NewItem((int)npc.Center.X, (int)npc.Center.Y, npc.width, npc.height, ItemType<PumpBubbleGun>());

			 if (Main.rand.Next(3) != 0)
                Item.NewItem((int)npc.Center.X, (int)npc.Center.Y, npc.width, npc.height, ItemType<TribalScale>(), Main.rand.Next(2) + 1);
        }

        public override void FindFrame(int frameHeight)
		{
			if((npc.collideY || npc.wet) && blocking == 0) {
				npc.frameCounter += 0.2f;
				npc.frameCounter %= 6;
				int frame = (int)npc.frameCounter;
				npc.frame.Y = frame * frameHeight;
			}

			if (npc.wet) return;

			if(blocking == 1) {
				npc.frameCounter += 0.05f;
				npc.frameCounter = MathHelper.Clamp((float)npc.frameCounter, 0, 2.9f);
				int frame = (int)npc.frameCounter;
				npc.frame.Y = (frame + 6) * frameHeight;
			}
		}
		public override void HitEffect(int hitDirection, double damage)
        {
            for (int k = 0; k < 30; k++)
            {
                Dust.NewDust(npc.position, npc.width, npc.height, 5, 2.5f * hitDirection, -2.5f, 0, Color.White, 0.7f);
                Dust.NewDust(npc.position, npc.width, npc.height, 5, 2.5f * hitDirection, -2.5f, 0, default, 1.14f);
            }

            if (npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/LargeCrustacean/lobster1"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/LargeCrustacean/lobster2"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/LargeCrustacean/lobster3"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/LargeCrustacean/lobster4"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/LargeCrustacean/lobster5"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/LargeCrustacean/lobster6"), 1f);
				Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/LargeCrustacean/lobster7"), 1f);

                if (TideWorld.TheTide && TideWorld.TidePoints < 99) TideWorld.TidePoints += TideWorld.TidePoints < 97 ? 3 : 1;
            }
		}
	}
}
