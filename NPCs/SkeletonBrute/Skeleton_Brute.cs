using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;

namespace SpiritMod.NPCs.SkeletonBrute
{
	public class Skeleton_Brute : ModNPC
	{
		public int attacking = 0;
		public bool frameRes = false;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Skeleton Brute");
			Main.npcFrameCount[npc.type] = 10;
		}

		public override void SetDefaults()
		{
			npc.lifeMax = 250;
			npc.defense = 10;
			npc.value = 400f;
			npc.knockBackResist = 0.3f;
			npc.width = 35;
			npc.height = 80;
			npc.damage = 26;
			npc.lavaImmune = false;
			npc.noTileCollide = false;
			npc.alpha = 0;
			npc.HitSound = SoundID.NPCHit2;
			npc.DeathSound = SoundID.NPCDeath2;

			banner = npc.type;
			bannerItem = ModContent.ItemType<Items.Banners.SkeletonBruteBanner>();
			aiType = 0;
		}

		public override void AI()
		{
			Player player = Main.player[npc.target];

			npc.TargetClosest(true);
			npc.spriteDirection = npc.direction;
			if (player.DistanceSQ(npc.Center) <= 120f * 120f)
			{
				attacking = 1;
				npc.aiStyle = 0;
				npc.velocity.X = 0f;
				npc.velocity.Y = 8f;				
			}
			else
			{
				npc.aiStyle = 3;
				frameRes = true;	
			}

			if (attacking == 1)
			{
				if (!frameRes)
				{
					frameRes = true;
					npc.frameCounter = 0;
				}	
				npc.velocity.X = 0f;
				npc.velocity.Y = 8f;				
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{		
			for (int k = 0; k < 10; k++)
			{
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Bone, 2.5f * hitDirection, -2.5f, 0, default, 1.2f);
				Dust.NewDust(npc.position, npc.width, npc.height, DustID.Iron, 2.5f * hitDirection, -2.5f, 0, default, 0.5f);
			}

			if (npc.life <= 0)
				for (int i = 1; i < 6; ++i)
					Gore.NewGore(npc.position, npc.velocity, mod.GetGoreSlot("Gores/SkeletonBrute/SkeletonBruteGore" + i), 1f);
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			bool downedAnyBoss = NPC.downedBoss1 || NPC.downedBoss2 || NPC.downedBoss3 || MyWorld.downedScarabeus || MyWorld.downedAncientFlier || MyWorld.downedMoonWizard || MyWorld.downedRaider;
			return spawnInfo.spawnTileY < Main.rockLayer && Main.bloodMoon && !NPC.AnyNPCs(ModContent.NPCType<Skeleton_Brute>()) && downedAnyBoss ? 0.05f : 0f;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor) => false;

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var effects = npc.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			Vector2 drawPos = new Vector2(npc.Center.X + 20 * npc.spriteDirection, npc.Center.Y - 22);
			spriteBatch.Draw(Main.npcTexture[npc.type], drawPos - Main.screenPosition + new Vector2(0, npc.gfxOffY), npc.frame, drawColor, npc.rotation, npc.frame.Size() / 2, npc.scale, effects, 0);
		}

        public override void FindFrame(int frameHeight)
        {
			npc.frame.Width = 240;
            npc.frameCounter++;

			void SetFrame(int frameX, int yOffset) => npc.frame.Location = new Point(frameX, frameHeight * yOffset); 

            if (npc.velocity.Y != 0f)
                npc.frame.Y = 2 * frameHeight;
            else if (attacking == 0 && npc.velocity.X == 0f)
				SetFrame(0, 2);
            else if (attacking == 1)
            {
                if (npc.frameCounter < 4)
					SetFrame(0, 3);
                else if (npc.frameCounter < 8)
					SetFrame(240, 3);
                else if (npc.frameCounter < 12)
					SetFrame(0, 4);
                else if (npc.frameCounter < 16)
                {
					SetFrame(240, 4);

					Player player = Main.player[npc.target];
					if (npc.frameCounter == 13 && Collision.CanHitLine(npc.Center, 0, 0, Main.player[npc.target].Center, 0, 0) && player.DistanceSQ(npc.Center) <= 150f * 150f)
					{
                        player.Hurt(PlayerDeathReason.LegacyDefault(), (int)(npc.damage * 1.5f), 0, false, false, false, -1);
                        Main.PlaySound(SoundID.Item, (int)npc.position.X, (int)npc.position.Y, 37, 1f, 0.3f);
                        player.velocity.X = npc.direction * 13f;
                        player.velocity.Y = -9f;
                    }
                }
                else if (npc.frameCounter < 20)
					SetFrame(0, 5);
                else if (npc.frameCounter < 24)
					SetFrame(240, 5);
                else if (npc.frameCounter < 28)
					SetFrame(0, 6);
                else if (npc.frameCounter < 32)
					SetFrame(240, 6);
                else if (npc.frameCounter < 36)
					SetFrame(0, 7);
                else if (npc.frameCounter < 40)
					SetFrame(240, 7);
                else if (npc.frameCounter < 44)
					SetFrame(0, 8);
                else if (npc.frameCounter < 48)
					SetFrame(240, 8);
                else if (npc.frameCounter < 52)
					SetFrame(0, 9);
                else if (npc.frameCounter < 70)
                {
					SetFrame(240, 9);
					if (npc.frameCounter == 69)
                    {
                        attacking = 0;
                        frameRes = false;
                    }
                }
                else
                    npc.frameCounter = 0;
            }
            else if (attacking == 0)
            {
                if (npc.frameCounter < 7)
					SetFrame(0, 0);
                else if (npc.frameCounter < 14)
					SetFrame(240, 0);
                else if (npc.frameCounter < 21)
					SetFrame(0, 1);
                else if (npc.frameCounter < 28)
					SetFrame(240, 1);
                else if (npc.frameCounter < 35)
					SetFrame(0, 2);
                else if (npc.frameCounter < 42)
					SetFrame(240, 2);
                else
                    npc.frameCounter = 0;
            }
        }
	}
}