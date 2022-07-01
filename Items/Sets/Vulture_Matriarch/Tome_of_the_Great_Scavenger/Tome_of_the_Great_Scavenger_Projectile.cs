using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
 
namespace SpiritMod.Items.Sets.Vulture_Matriarch.Tome_of_the_Great_Scavenger
{
    public class Tome_of_the_Great_Scavenger_Projectile : ModProjectile, IDrawAdditive
    {
 		public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gilded Feather");
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 10; 
			ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
        } 
        public override void SetDefaults()
        {
            Projectile.width = 18;
            Projectile.height = 18;
            Projectile.aiStyle = -1;
			Projectile.penetrate = 1;
            Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
            Projectile.hostile = false;
			Projectile.timeLeft = 180;
			Projectile.tileCollide = true;
			Projectile.scale = 0.1f;
			Projectile.rotation = Main.rand.NextFloat(MathHelper.TwoPi);
        }
		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			width = 8;
			height = 8;
			return true;
		}

		private float RotationDirection => Projectile.ai[0];

		private ref float AiState => ref Projectile.ai[1];
		private ref float Timer => ref Projectile.localAI[0];
		private ref float RotationToCursor => ref Projectile.localAI[1];

        public override void AI()
        {
			Lighting.AddLight(Projectile.position, 0.3f, .15f, 0f);
			Player player = Main.player[Projectile.owner];
			Projectile.scale = Math.Min(Projectile.scale + 0.05f, 1);

			switch (AiState)
			{
				case 0:
					Projectile.rotation += 0.3f * RotationDirection * Math.Max(Projectile.velocity.Length()/7, 0.25f);
					Projectile.velocity *= 0.94f;
					if(++Timer > 35 && Main.LocalPlayer == player)
					{
						Timer = 0;
						RotationToCursor = Projectile.AngleTo(Main.MouseWorld);
						AiState++;
						Projectile.netUpdate = true;
					}
					break;

				case 1:
					if (Math.Abs(MathHelper.WrapAngle(RotationToCursor + MathHelper.PiOver2 - Projectile.rotation)) > MathHelper.PiOver4)
						Projectile.rotation += 0.15f * RotationDirection;

					else
						Projectile.rotation = Utils.AngleLerp(Projectile.rotation, RotationToCursor + MathHelper.PiOver2, 0.07f);

					if(++Timer > 20)
					{
						AiState++;
						Projectile.velocity = Vector2.UnitX.RotatedBy(RotationToCursor) * Main.rand.NextFloat(6, 8);
						if (!Main.dedServ)
							SoundEngine.PlaySound(SoundID.Item1 with { PitchVariance = 0.2f }, Projectile.Center);

						Projectile.netUpdate = true;
					}
					else if(Timer > 7) 
						Projectile.velocity = Vector2.Lerp(Projectile.velocity, -Vector2.UnitX.RotatedBy(RotationToCursor) * 3, 0.2f);

					break;

				case 2:
					Projectile.extraUpdates = 1;
					Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
					if (Projectile.velocity.Length() < 15)
						Projectile.velocity *= 1.02f;
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

		public override bool PreDraw(ref Color lightColor)
		{
			Projectile.QuickDraw(Main.spriteBatch);
			return false;
		}

		public override void PostDraw(Color lightColor)
		{
			if (!Projectile.wet)
			{
				Texture2D bloom = Mod.Assets.Request<Texture2D>("Effects/Desert_Shadow").Value;

				Color color = Color.LightGoldenrodYellow;
				color.A = (byte)(color.A * 2);
				Main.spriteBatch.Draw(bloom, Projectile.Center - Main.screenPosition, null, color * 0.3f, Projectile.rotation, bloom.Size() / 2, Projectile.scale * 1.5f, SpriteEffects.None, 0);
			}
		}

		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			if (!Projectile.wet)
			{
				Projectile.QuickDrawGlowTrail(spriteBatch);

				Projectile.QuickDrawGlow(spriteBatch);
			}
		}

		public override void Kill(int timeLeft)
		{
			if (Main.dedServ)
				return;

			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center);

			for(int i = 0; i < 5; i++)
			{
				Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Sandnado, Projectile.velocity.RotatedByRandom(MathHelper.Pi / 12) * Main.rand.NextFloat(0.5f, 0.7f), 100, default, Main.rand.NextFloat(0.7f, 1f));
				dust.fadeIn = 0.75f;
				dust.noGravity = true;
			}

			for (int j = 0; j < 10; j++)
			{
				Dust dust = Dust.NewDustPerfect(Projectile.Center, DustID.Sandnado, Projectile.velocity.RotatedByRandom(MathHelper.Pi / 3) * Main.rand.NextFloat(0.1f, 0.3f), 100, default, Main.rand.NextFloat(0.2f, 0.4f));
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