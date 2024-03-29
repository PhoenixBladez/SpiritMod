using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
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
			Main.npcFrameCount[npc.type] = 6;
		}

		public override void SetDefaults()
		{
			npc.width = 50;
			npc.height = 60;
			npc.damage = 0;
			npc.dontCountMe = true;
			npc.defense = 0;
			npc.lifeMax = 200;
			npc.DeathSound = SoundID.NPCDeath1;
			npc.knockBackResist = .095f;
			npc.aiStyle = 0;
			npc.npcSlots = 0;
			npc.noGravity = false;
            npc.chaseable = false;
            npc.HitSound = SoundID.NPCHit4;
			npc.friendly = false;
			
			for (int k = 0; k < npc.buffImmune.Length; k++)
				npc.buffImmune[k] = true;
		}
		public override void AI()
		{
			npc.spriteDirection = -1;

			float distanceToClosestSquared = -1f;
			if (Main.netMode == NetmodeID.SinglePlayer)
				distanceToClosestSquared = Main.LocalPlayer.DistanceSQ(npc.Center);
			else
			{
				foreach (Player player in Main.player)
				{
					if (player == null || !player.active || player.dead)
						continue;

					float distanceToPlayerSquared = player.DistanceSQ(npc.Center);
					if (distanceToPlayerSquared < distanceToClosestSquared || distanceToClosestSquared == -1f)
						distanceToClosestSquared = distanceToPlayerSquared;
				}		
			}

			if (distanceToClosestSquared < ActivationDistance * ActivationDistance || npc.ai[1] == 1800)
			{
				npc.ai[0] = 1;
				npc.netUpdate = true;
			}

			npc.ai[1]++;
            if (npc.ai[0] == 1)
            {
                npc.life--;

				if (Main.rand.Next(10) == 1)
                    Dust.NewDust(npc.position, npc.width, npc.height, DustID.Fire, 0, -5, 0, default, 1.5f);

                if (npc.life <= 0)
                    Explode();

				if (Main.netMode != NetmodeID.Server)
					npc.rotation += Main.rand.NextFloat(-0.01f,0.01f);
            }

			Lighting.AddLight(npc.Center, 0.242f / 4 * 3, 0.132f / 4 * 3, 0.068f / 4 * 3);
        }
        public override void OnHitByItem(Player player, Item item, int damage, float knockback, bool crit)
        {
            npc.ai[0] = 1;
            npc.netUpdate = true;
        }
        public override void OnHitByProjectile(Projectile projectile, int damage, float knockback, bool crit)
        {
            npc.ai[0] = 1;
            npc.netUpdate = true;
        }

        public void Explode()
		{
            Projectile.NewProjectile(new Vector2(npc.Center.X, npc.Center.Y - 48), Vector2.Zero, ModContent.ProjectileType<BarrelExplosionLarge>(), 100, 8, Main.myPlayer);
            npc.active = false;
        }

		public override void HitEffect(int hitDirection, double damage)
		{
			if (npc.life <= 0)
				Explode();
		}

		public override void FindFrame(int frameHeight)
		{
			npc.frameCounter += 0.14f;
			npc.frameCounter %= Main.npcFrameCount[npc.type];
			int frame = (int)npc.frameCounter;
			npc.frame.Y = frame * frameHeight;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			var orig = new Vector2(npc.width / 2, 68);
			spriteBatch.Draw(Main.npcTexture[npc.type], npc.position - Main.screenPosition + orig - new Vector2(0, 10), npc.frame, drawColor, npc.rotation, orig, 1f, SpriteEffects.None, 0f);
			return false;
		}
	}
}
