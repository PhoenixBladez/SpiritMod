using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Items.Consumable.Food;

namespace SpiritMod.NPCs.SamuraiGhost
{
	public class SamuraiHostile : ModNPC
	{
		int chargeTime = 200; //how many frames between charges
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Phantom Samurai");
			Main.npcFrameCount[npc.type] = 9;
			NPCID.Sets.TownCritter[npc.type] = true;
			NPCID.Sets.TrailCacheLength[npc.type] = 3;
			NPCID.Sets.TrailingMode[npc.type] = 0;
		}

		public override void SetDefaults()
		{
			if (NPC.downedBoss2) {
				npc.width = 30;
				npc.height = 40;
				npc.damage = 35;
				npc.noGravity = true;
				npc.defense = 8;
				npc.lifeMax = 130;
			}
			else if (NPC.downedBoss3) {
				npc.width = 30;
				npc.height = 40;
				npc.damage = 38;
				npc.noGravity = true;
				npc.defense = 11;
				npc.lifeMax = 180;
			}
			else {
				npc.width = 30;
				npc.height = 40;
				npc.damage = 33;
				npc.noGravity = true;
				npc.defense = 4;
				npc.lifeMax = 90;
			}
			npc.HitSound = SoundID.NPCHit3;
			npc.DeathSound = SoundID.NPCDeath6;
			npc.value = 120f;
			npc.knockBackResist = .1f;
			npc.noTileCollide = true;
            banner = npc.type;
            bannerItem = ModContent.ItemType<Items.Banners.PhantomSamuraiBanner>();
			for (int k = 0; k < npc.buffImmune.Length; k++)
			{
				npc.buffImmune[k] = true;
			}
		}
		float frameCounter = 0;
		public override void FindFrame(int frameHeight)
		{
			frameCounter += 0.15f;
			frameCounter %= 3f;
			if (npc.ai[2] == chargeTime) {
				frameCounter = 0;
			}
			if (npc.ai[3] == 1) {
				npc.frameCounter = frameCounter + 6;
			}
			else if (npc.ai[2] > chargeTime - 50) {
				npc.frameCounter = frameCounter + 3;
			}
			else {
				npc.frameCounter = frameCounter;
			}
			if (npc.frameCounter == 8) {
				npc.ai[3] = 0;
			}
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override void HitEffect(int hitDirection, double damage)
		{
			for (int k = 0; k < 20; k++) {
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Wraith, 2.5f * hitDirection, -2.5f, 54, new Color(0, 255, 142), .6f);
			}
			if (npc.life <= 0) {
				Gore.NewGore(npc.position, npc.velocity, 99);
				Gore.NewGore(npc.position, npc.velocity, 99);
				Gore.NewGore(npc.position, npc.velocity, 99);
				for (int i = 0; i < 40; i++) {
					int num = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Wraith, 0f, -2f, 117, new Color(0, 255, 142), .6f);
					Main.dust[num].noGravity = true;
					Dust dust = Main.dust[num];
					dust.position.X += ((Main.rand.Next(-50, 51) / 20) - 1.5f);
					dust.position.Y += ((Main.rand.Next(-50, 51) / 20) - 1.5f);
					if (Main.dust[num].position != npc.Center) {
						Main.dust[num].velocity = npc.DirectionTo(Main.dust[num].position) * 6f;
					}
				}
			}
		}
        public override void NPCLoot()
        {
            if (Main.rand.NextBool(16))
            {
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, ModContent.ItemType<Ramen>());
            }
        }
        private static int[] SpawnTiles = { };
		Vector2 targetLocation = Vector2.Zero;
		float chargeRotation = 0;
		public override void AI()
		{
			npc.TargetClosest(true);
			Player player = Main.player[npc.target];
			float num395 = Main.mouseTextColor / 200f - 0.35f;
			num395 *= 0.14f;
			npc.scale = num395 + 0.95f;
			float velMax = 1f;
			float acceleration = 0.011f;
			Vector2 center = npc.Center;
			float deltaX = player.position.X + (player.width / 2) - center.X;
			float deltaY = player.position.Y + (player.height / 2) - center.Y;
			float distance = (float)Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
			npc.ai[1]++;
			if (npc.ai[3] == 0) {
				if (npc.ai[1] > 600.0) {
					acceleration *= 8f;
					velMax = 4f;
					if (npc.ai[1] > 650.0) {
						npc.ai[1] = 0f;
					}
				}
				else if (distance < 250.0) {
					npc.ai[0] += 0.9f;
					if (npc.ai[0] > 0f) {
						npc.velocity.Y = npc.velocity.Y + 0.019f;
					}
					else {
						npc.velocity.Y = npc.velocity.Y - 0.019f;
					}
					if (npc.ai[0] < -100f || npc.ai[0] > 100f) {
						npc.velocity.X = npc.velocity.X + 0.019f;
					}
					else {
						npc.velocity.X = npc.velocity.X - 0.019f;
					}
					if (npc.ai[0] > 200f) {
						npc.ai[0] = -200f;
						npc.netUpdate = true;
					}
				}
				if (distance > 350.0) {
					velMax = 5f;
					acceleration = 0.3f;
				}
				else if (distance > 300.0) {
					velMax = 3f;
					acceleration = 0.2f;
				}
				else if (distance > 250.0) {
					velMax = 1.5f;
					acceleration = 0.1f;
				}
				float stepRatio = velMax / distance;
				float velLimitX = deltaX * stepRatio;
				float velLimitY = deltaY * stepRatio;
				if (Main.player[npc.target].dead) {
					velLimitX = (float)(npc.direction * velMax / 2.0);
					velLimitY = (float)((-velMax) / 2.0);
				}
				if (npc.velocity.X < velLimitX) {
					npc.velocity.X = npc.velocity.X + acceleration;
				}
				else if (npc.velocity.X > velLimitX) {
					npc.velocity.X = npc.velocity.X - acceleration;
				}
				if (npc.velocity.Y < velLimitY) {
					npc.velocity.Y = npc.velocity.Y + acceleration;
				}
				else if (npc.velocity.Y > velLimitY) {
					npc.velocity.Y = npc.velocity.Y - acceleration;
				}
				if (velLimitX > 0.0) {
					npc.rotation = (float)Math.Atan2(velLimitY, velLimitX);
				}
				if (velLimitX < 0.0) {
					npc.rotation = (float)Math.Atan2(velLimitY, velLimitX) + 3.14f;
				}
				if (npc.ai[2] < chargeTime - 50) {
					npc.rotation = 0;
				}
				else if (npc.ai[2] > chargeTime - 40) {
					npc.rotation = chargeRotation;
				}
				if (npc.ai[2] == chargeTime - 40) {
					Main.PlaySound(SoundLoader.customSoundType, npc.position, mod.GetSoundSlot(SoundType.Custom, "Sounds/SamuraiUnsheathe"));
				}
				if (npc.ai[2] > chargeTime - 50 && npc.ai[2] < chargeTime - 40) {
					chargeRotation = npc.rotation;
				}
			}
			npc.ai[2]++;
			if (npc.ai[2] == chargeTime)  {
				if(Main.netMode != NetmodeID.MultiplayerClient) {
					Vector2 direction = Main.player[npc.target].Center - npc.Center;
					direction.Normalize();
					direction.X *= 18;
					direction.Y *= 9;
					npc.velocity.X = direction.X;
					npc.velocity.Y = direction.Y;
					npc.ai[3] = 1;
					npc.netUpdate = true;
				}
				Main.PlaySound(SoundID.DD2_WyvernDiveDown, npc.Center);

				for (int i = 0; i < 20; i++) {
					int num = Dust.NewDust(npc.position, npc.width, npc.height, DustID.Demonite, 0f, -2f, 0, default, .8f);
					Main.dust[num].noGravity = true;
					Main.dust[num].position.X += Main.rand.Next(-50, 51) * .05f - 1.5f;
					Main.dust[num].position.Y += Main.rand.Next(-50, 51) * .05f - 1.5f;
					if (Main.dust[num].position != npc.Center)
						Main.dust[num].velocity = npc.DirectionTo(Main.dust[num].position) * 6f;
				}
			}
			if (npc.ai[2] >= chargeTime + 20 && Main.netMode != NetmodeID.MultiplayerClient) {
				npc.ai[2] = 0;
				npc.ai[3] = 0;
				npc.netUpdate = true;
			}
			npc.spriteDirection = npc.direction;
		}
	}
}
