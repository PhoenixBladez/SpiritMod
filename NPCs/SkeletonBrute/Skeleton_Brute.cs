using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
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
			Main.npcFrameCount[NPC.type] = 10;
		}

		public override void SetDefaults()
		{
			NPC.lifeMax = 250;
			NPC.defense = 10;
			NPC.value = Item.buyPrice(0, 0, 30, 0);
			NPC.knockBackResist = 0.3f;
			NPC.width = 35;
			NPC.height = 80;
			NPC.damage = 26;
			NPC.lavaImmune = false;
			NPC.noTileCollide = false;
			NPC.alpha = 0;
			NPC.HitSound = SoundID.NPCHit2;
			NPC.DeathSound = SoundID.NPCDeath2;

			Banner = NPC.type;
			BannerItem = ModContent.ItemType<Items.Banners.SkeletonBruteBanner>();
			AIType = 0;
		}

		public override void AI()
		{
			Player player = Main.player[NPC.target];

			NPC.TargetClosest(true);
			NPC.spriteDirection = NPC.direction;
			if (player.DistanceSQ(NPC.Center) <= 120f * 120f)
			{
				attacking = 1;
				NPC.aiStyle = 0;
				NPC.velocity.X = 0f;
				NPC.velocity.Y = 8f;				
			}
			else
			{
				NPC.aiStyle = 3;
				frameRes = true;	
			}

			if (attacking == 1)
			{
				if (!frameRes)
				{
					frameRes = true;
					NPC.frameCounter = 0;
				}	
				NPC.velocity.X = 0f;
				NPC.velocity.Y = 8f;				
			}
		}

		public override void HitEffect(int hitDirection, double damage)
		{		
			for (int k = 0; k < 10; k++)
			{
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Bone, 2.5f * hitDirection, -2.5f, 0, default, 1.2f);
				Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Iron, 2.5f * hitDirection, -2.5f, 0, default, 0.5f);
			}

			if (NPC.life <= 0)
				for (int i = 1; i < 6; ++i)
					Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("SpiritMod/Gores/SkeletonBrute/SkeletonBruteGore" + i).Type, 1f);
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			bool downedAnyBoss = NPC.downedBoss1 || NPC.downedBoss2 || NPC.downedBoss3 || MyWorld.downedScarabeus || MyWorld.downedAncientFlier || MyWorld.downedMoonWizard || MyWorld.downedRaider;
			return spawnInfo.SpawnTileY < Main.rockLayer && Main.bloodMoon && !NPC.AnyNPCs(ModContent.NPCType<Skeleton_Brute>()) && downedAnyBoss ? 0.05f : 0f;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) => false;

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var effects = NPC.direction == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			Vector2 drawPos = new Vector2(NPC.Center.X + 20 * NPC.spriteDirection, NPC.Center.Y - 22);
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, drawPos - Main.screenPosition + new Vector2(0, NPC.gfxOffY), NPC.frame, drawColor, NPC.rotation, NPC.frame.Size() / 2, NPC.scale, effects, 0);
		}

        public override void FindFrame(int frameHeight)
        {
			NPC.frame.Width = 240;
            NPC.frameCounter++;

			void SetFrame(int frameX, int yOffset) => NPC.frame.Location = new Point(frameX, frameHeight * yOffset); 

            if (NPC.velocity.Y != 0f)
                NPC.frame.Y = 2 * frameHeight;
            else if (attacking == 0 && NPC.velocity.X == 0f)
				SetFrame(0, 2);
            else if (attacking == 1)
            {
                if (NPC.frameCounter < 4)
					SetFrame(0, 3);
                else if (NPC.frameCounter < 8)
					SetFrame(240, 3);
                else if (NPC.frameCounter < 12)
					SetFrame(0, 4);
                else if (NPC.frameCounter < 16)
                {
					SetFrame(240, 4);

					Player player = Main.player[NPC.target];
					if (NPC.frameCounter == 13 && Collision.CanHitLine(NPC.Center, 0, 0, Main.player[NPC.target].Center, 0, 0) && player.DistanceSQ(NPC.Center) <= 150f * 150f)
					{
                        player.Hurt(PlayerDeathReason.LegacyDefault(), (int)(NPC.damage * 1.5f), 0, false, false, false, -1);
                        SoundEngine.PlaySound(SoundID.Item37, NPC.Center);
                        player.velocity.X = NPC.direction * 13f;
                        player.velocity.Y = -9f;
                    }
                }
                else if (NPC.frameCounter < 20)
					SetFrame(0, 5);
                else if (NPC.frameCounter < 24)
					SetFrame(240, 5);
                else if (NPC.frameCounter < 28)
					SetFrame(0, 6);
                else if (NPC.frameCounter < 32)
					SetFrame(240, 6);
                else if (NPC.frameCounter < 36)
					SetFrame(0, 7);
                else if (NPC.frameCounter < 40)
					SetFrame(240, 7);
                else if (NPC.frameCounter < 44)
					SetFrame(0, 8);
                else if (NPC.frameCounter < 48)
					SetFrame(240, 8);
                else if (NPC.frameCounter < 52)
					SetFrame(0, 9);
                else if (NPC.frameCounter < 70)
                {
					SetFrame(240, 9);
					if (NPC.frameCounter == 69)
                    {
                        attacking = 0;
                        frameRes = false;
                    }
                }
                else
                    NPC.frameCounter = 0;
            }
            else if (attacking == 0)
            {
                if (NPC.frameCounter < 7)
					SetFrame(0, 0);
                else if (NPC.frameCounter < 14)
					SetFrame(240, 0);
                else if (NPC.frameCounter < 21)
					SetFrame(0, 1);
                else if (NPC.frameCounter < 28)
					SetFrame(240, 1);
                else if (NPC.frameCounter < 35)
					SetFrame(0, 2);
                else if (NPC.frameCounter < 42)
					SetFrame(240, 2);
                else
                    NPC.frameCounter = 0;
            }
        }
	}
}