using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.NPCs.ExplosiveBarrel
{
	public class ExplosiveBarrel : ModNPC
	{
		private int ActivationDistance => 100;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Explosive Barrel");
			Main.npcFrameCount[NPC.type] = 6;
		}

		public override void SetDefaults()
		{
			NPC.width = 50;
			NPC.height = 60;
			NPC.damage = 0;
			NPC.dontCountMe = true;
			NPC.defense = 0;
			NPC.lifeMax = 200;
			NPC.DeathSound = SoundID.NPCDeath1;
			NPC.knockBackResist = .095f;
			NPC.aiStyle = 0;
			NPC.npcSlots = 0;
			NPC.noGravity = false;
            NPC.chaseable = false;
            NPC.HitSound = SoundID.NPCHit4;
			NPC.friendly = false;
			
			for (int k = 0; k < NPC.buffImmune.Length; k++)
				NPC.buffImmune[k] = true;
		}
		public override void AI()
		{
			NPC.spriteDirection = -1;

			float distanceToClosestSquared = -1f;
			if (Main.netMode == NetmodeID.SinglePlayer)
				distanceToClosestSquared = Main.LocalPlayer.DistanceSQ(NPC.Center);
			else
			{
				foreach (Player player in Main.player)
				{
					if (player == null || !player.active || player.dead)
						continue;

					float distanceToPlayerSquared = player.DistanceSQ(NPC.Center);
					if (distanceToPlayerSquared < distanceToClosestSquared || distanceToClosestSquared == -1f)
						distanceToClosestSquared = distanceToPlayerSquared;
				}		
			}

			if (distanceToClosestSquared < ActivationDistance * ActivationDistance || NPC.ai[1] == 1800)
			{
				NPC.ai[0] = 1;
				NPC.netUpdate = true;
			}

			NPC.ai[1]++;
            if (NPC.ai[0] == 1)
            {
                NPC.life--;

				if (Main.rand.Next(10) == 1)
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, DustID.Torch, 0, -5, 0, default, 1.5f);

                if (NPC.life <= 0)
                    Explode();

				if (Main.netMode != NetmodeID.Server)
					NPC.rotation += Main.rand.NextFloat(-0.01f,0.01f);
            }

			Lighting.AddLight(NPC.Center, 0.242f / 4 * 3, 0.132f / 4 * 3, 0.068f / 4 * 3);
        }
        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
        {
            NPC.ai[0] = 1;
            NPC.netUpdate = true;
        }
        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
            NPC.ai[0] = 1;
            NPC.netUpdate = true;
        }

        public void Explode()
		{
            Projectile.NewProjectile(new Vector2(NPC.Center.X, NPC.Center.Y - 48), Vector2.Zero, ModContent.ProjectileType<BarrelExplosionLarge>(), 100, 8, Main.myPlayer);
            NPC.active = false;
        }

		public override void HitEffect(int hitDirection, double damage)
		{
			if (NPC.life <= 0)
				Explode();
		}

		public override void FindFrame(int frameHeight)
		{
			NPC.frameCounter += 0.14f;
			NPC.frameCounter %= Main.npcFrameCount[NPC.type];
			int frame = (int)NPC.frameCounter;
			NPC.frame.Y = frame * frameHeight;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			var orig = new Vector2(NPC.width / 2, 68);
			spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.position - Main.screenPosition + orig - new Vector2(0, 10), NPC.frame, drawColor, NPC.rotation, orig, 1f, SpriteEffects.None, 0f);
			return false;
		}
	}
}
