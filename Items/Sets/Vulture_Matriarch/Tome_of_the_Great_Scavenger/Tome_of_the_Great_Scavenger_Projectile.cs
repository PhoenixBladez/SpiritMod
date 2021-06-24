using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SpiritMod.Items.Sets.Vulture_Matriarch.Tome_of_the_Great_Scavenger
{
    public class Tome_of_the_Great_Scavenger_Projectile : ModProjectile, IDrawAdditive
    {
 		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gilded Feather");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 10; 
			ProjectileID.Sets.TrailingMode[projectile.type] = 2;
        } 
        public override void SetDefaults()
        {
            projectile.width = 18;
            projectile.height = 18;
            projectile.aiStyle = -1;
			projectile.penetrate = 1;
            projectile.friendly = true;
			projectile.magic = true;
            projectile.hostile = false;
			projectile.timeLeft = 180;
			projectile.tileCollide = true;
			projectile.scale = 0.1f;
			projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
        }
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
		{
			width = 8;
			height = 8;
			return true;
		}

		private float RotationDirection => projectile.ai[0];

		private ref float AiState => ref projectile.ai[1];
		private ref float Timer => ref projectile.localAI[0];
		private ref float RotationToCursor => ref projectile.localAI[1];

        public override void AI()
        {
			Lighting.AddLight(projectile.position, 0.3f, .15f, 0f);
			Player player = Main.player[projectile.owner];
			projectile.scale = Math.Min(projectile.scale + 0.05f, 1);

			switch (AiState)
			{
				case 0:
					projectile.rotation += 0.3f * RotationDirection * Math.Max(projectile.velocity.Length()/7, 0.25f);
					projectile.velocity *= 0.94f;
					if(++Timer > 35 && Main.LocalPlayer == player)
					{
						Timer = 0;
						RotationToCursor = projectile.AngleTo(Main.MouseWorld);
						AiState++;
						projectile.netUpdate = true;
					}
					break;

				case 1:
					if (Math.Abs(MathHelper.WrapAngle(RotationToCursor + MathHelper.PiOver2 - projectile.rotation)) > MathHelper.PiOver4)
						projectile.rotation += 0.15f * RotationDirection;

					else
						projectile.rotation = Utils.AngleLerp(projectile.rotation, RotationToCursor + MathHelper.PiOver2, 0.07f);

					if(++Timer > 20)
					{
						AiState++;
						projectile.velocity = Vector2.UnitX.RotatedBy(RotationToCursor) * Main.rand.NextFloat(6, 8);
						if (!Main.dedServ)
							Main.PlaySound(SoundID.Item1.WithPitchVariance(0.2f), projectile.Center);

						projectile.netUpdate = true;
					}
					else if(Timer > 7) 
						projectile.velocity = Vector2.Lerp(projectile.velocity, -Vector2.UnitX.RotatedBy(RotationToCursor) * 3, 0.2f);

					break;

				case 2:
					projectile.extraUpdates = 1;
					projectile.rotation = projectile.velocity.ToRotation() + MathHelper.PiOver2;
					if (projectile.velocity.Length() < 15)
						projectile.velocity *= 1.02f;
					break;
			}
        }

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) => damage += Math.Min(target.defense / 2, 15);

		public override void ModifyHitPlayer(Player target, ref int damage, ref bool crit) => damage += Math.Min(target.statDefense / 2, 15);

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(Timer);
			writer.Write(RotationToCursor);
		}
		public override void ReceiveExtraAI(BinaryReader reader)
		{
			Timer = reader.ReadSingle();
			RotationToCursor = reader.ReadSingle();
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			projectile.QuickDraw(spriteBatch);
			return false;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
		{
			if (!projectile.wet)
			{
				Texture2D bloom = mod.GetTexture("Effects/Desert_Shadow");

				Color color = Color.LightGoldenrodYellow;
				color.A = (byte)(color.A * 2);
				spriteBatch.Draw(bloom, projectile.Center - Main.screenPosition, null, color * 0.3f, projectile.rotation, bloom.Size() / 2, projectile.scale * 1.5f, SpriteEffects.None, 0);
			}
		}

		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			if (!projectile.wet)
			{
				projectile.QuickDrawGlowTrail(spriteBatch);

				projectile.QuickDrawGlow(spriteBatch);
			}
		}

		public override void Kill(int timeLeft)
		{
			if (Main.dedServ)
				return;

			Main.PlaySound(SoundID.Dig, projectile.Center);

			for(int i = 0; i < 5; i++)
			{
				Dust dust = Dust.NewDustPerfect(projectile.Center, DustID.Sandnado, projectile.velocity.RotatedByRandom(MathHelper.Pi / 12) * Main.rand.NextFloat(0.5f, 0.7f), 100, default, Main.rand.NextFloat(0.7f, 1f));
				dust.fadeIn = 0.75f;
				dust.noGravity = true;
			}

			for (int j = 0; j < 10; j++)
			{
				Dust dust = Dust.NewDustPerfect(projectile.Center, DustID.Sandnado, projectile.velocity.RotatedByRandom(MathHelper.Pi / 3) * Main.rand.NextFloat(0.1f, 0.3f), 100, default, Main.rand.NextFloat(0.2f, 0.4f));
				dust.fadeIn = 0.75f;
				dust.noGravity = true;
			}
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(4) == 0 && !target.SpawnedFromStatue) 
				target.AddBuff(BuffID.Midas, 180);
		}
	}
}