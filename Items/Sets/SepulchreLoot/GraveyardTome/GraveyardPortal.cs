using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Particles;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.SepulchreLoot.GraveyardTome
{
	public class GraveyardPortal : ModProjectile
	{
		public override string Texture => "SpiritMod/Textures/StardustPillarStar";
		public override void SetStaticDefaults() => DisplayName.SetDefault("Graveyard Portal");

		public const float MaxScale = 1.2f;

		public override void SetDefaults()
		{
			projectile.Size = Vector2.One;
			projectile.tileCollide = false;
			projectile.friendly = true;
			projectile.ignoreWater = true;
			projectile.magic = true;
			projectile.scale = 0f;
		}

		public override bool CanDamage() => false;

		private ref float Direction => ref projectile.ai[0];
		private ref float Timer => ref projectile.ai[1];

		public override void AI()
		{
			Player player = Main.player[projectile.owner];

			if (player == Main.LocalPlayer) //Make projectile rotate towards the cursor and change player direction, if the client is the owner
			{
				int temp = player.direction;
				player.ChangeDir(player.DirectionTo(Main.MouseWorld).X > 0 ? 1 : -1);
				projectile.rotation = projectile.AngleTo(Main.MouseWorld);// MathHelper.WrapAngle(projectile.AngleTo(Main.MouseWorld) - ((Direction < 0) ? MathHelper.Pi : 0)) / 2;
				if (temp != player.direction && Main.netMode == NetmodeID.MultiplayerClient)
					NetMessage.SendData(MessageID.SyncPlayer, -1, -1, null, player.whoAmI);
			}

			//Make the player locked to the item
			player.itemTime = 2;
			player.itemAnimation = 2;
			player.itemRotation = 0;
			projectile.timeLeft = 20;

			//Gradually make projectile move behind the player and flip
			Direction = MathHelper.Lerp(Direction, player.direction, 0.045f);
			projectile.Center = player.MountedCenter - new Vector2(50 * Direction, 50 + (float)(Math.Sin(Main.GameUpdateCount / 30f) * 7));
			float particlerate = 0.15f;

			if (!player.channel) //Die if player isn't channelling 
			{
				projectile.scale -= 0.025f;
				particlerate = 0.3f;
				if (projectile.scale <= 0)
					projectile.Kill();
			}
			else
			{
				projectile.scale = Math.Min(projectile.scale + 0.025f, MaxScale);
				if (projectile.scale == MaxScale) //First, check if the projectile's scale is at max
				{
					if (++Timer % player.HeldItem.useTime == 0) //Then, increment timer and check if it's time to fire a projectile
					{
						if (player.CheckMana(player.HeldItem.mana, true)) //Fire a projectile if player has enough mana
						{
							if (!Main.dedServ) //Dont do sounds on server
								Main.PlaySound(SoundID.Item104.WithPitchVariance(0.3f).WithVolume(0.5f), projectile.Center);

							if (player == Main.LocalPlayer) //Spawn in projectiles only if the client is the owner, due to using the mouse position
							{
								bool straightline = Main.rand.NextBool(3);
								var p = Projectile.NewProjectileDirect(projectile.Center, projectile.DirectionTo(Main.MouseWorld) * (straightline ? 1.5f : 1) * Main.rand.NextFloat(10, 12), ModContent.ProjectileType<GraveyardSkull>(), projectile.damage, projectile.knockBack, projectile.owner);
								if (p.modProjectile is GraveyardSkull)
									(p.modProjectile as GraveyardSkull).Movement = new GraveyardSkull.SkullMovement(straightline ? 0 : Main.rand.NextFloat(20, 30), Main.rand.NextFloat(60, 120));

								if (Main.netMode != NetmodeID.SinglePlayer)
									NetMessage.SendData(MessageID.SyncProjectile, -1, -1, null, p.whoAmI);
							}
						}
						else //If player doesn't have enough mana, kill projectile
							player.channel = false;
					}
				}
				else
					particlerate = 0.3f;
			}

			//Spawn in particles
			if(Main.rand.NextFloat() < particlerate && !Main.dedServ)
			{
				Vector2 offset = Main.rand.NextVector2CircularEdge(15, 30) * projectile.scale;
				Vector2 velocity = Vector2.Normalize(projectile.Center + new Vector2(Direction * 3, 0).RotatedBy(projectile.rotation) - offset).RotatedBy(MathHelper.Pi * Direction) * Main.rand.NextFloat(0.2f, 0.4f);
				ParticleHandler.SpawnParticle(new GraveyardPortalParticle(projectile, offset, velocity, Main.rand.NextFloat(0.02f, 0.035f), Main.rand.Next(20, 30)));
			}
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D bloom = mod.GetTexture("Effects/Masks/Extra_A1");
			spriteBatch.Draw(bloom, projectile.Center - Main.screenPosition, null, Color.Black, 0, bloom.Size() / 2, 
				projectile.scale * 1.3f, SpriteEffects.None, 0); //draw a dark bloom beneath the portal

			spriteBatch.End(); spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, default, default, default, default, Main.GameViewMatrix.ZoomMatrix);
			Texture2D tex = Main.projectileTexture[projectile.type];
			SpiritMod.ShaderDict["PortalShader"].Parameters["PortalNoise"].SetValue(mod.GetTexture("Utilities/Noise/SpiralNoise"));
			SpiritMod.ShaderDict["PortalShader"].Parameters["DistortionNoise"].SetValue(mod.GetTexture("Utilities/Noise/noise"));
			SpiritMod.ShaderDict["PortalShader"].Parameters["Timer"].SetValue(MathHelper.WrapAngle(Main.GlobalTime / 3));
			SpiritMod.ShaderDict["PortalShader"].Parameters["DistortionStrength"].SetValue(0.1f);
			SpiritMod.ShaderDict["PortalShader"].Parameters["Rotation"].SetValue(MathHelper.WrapAngle(Main.GlobalTime / 2));
			SpiritMod.ShaderDict["PortalShader"].CurrentTechnique.Passes[0].Apply();

			float opacitymod = (float)Math.Pow(projectile.scale / MaxScale, 2);

			float numtodraw = 3;
			for (float i = 0; i < numtodraw; i++) //pulsating bloom type effect, draws multiple of the texture that grow in scale over time and fade out
			{
				float Timer = (Main.GlobalTime / 2 + i / numtodraw) % 1;
				float Opacity = 0.33f * (float)Math.Pow(Math.Sin(Timer * MathHelper.Pi), 3);
				float Scale = 0.6f * (projectile.scale + (Timer * projectile.scale));
				float Rotation = Main.GlobalTime / 2 * ((i % 2 == 0) ? -1 : 1) * (MathHelper.TwoPi * i / numtodraw);
				spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, new Color(94, 25, 25) * Opacity * opacitymod,
					Rotation, tex.Size() / 2, Scale, SpriteEffects.None, 0);
			}

			Vector2 ellipticalscale = new Vector2(projectile.scale * 0.5f, projectile.scale);
			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, null, new Color(15, 4, 4) * opacitymod, projectile.rotation, tex.Size() / 2, //the main "body" of the portal
				ellipticalscale, SpriteEffects.None, 0);
			spriteBatch.Draw(tex, projectile.Center + new Vector2(Direction * 3, 0).RotatedBy(projectile.rotation) - Main.screenPosition, null, new Color(212, 8, 8) * opacitymod, 
				projectile.rotation, tex.Size() / 2, ellipticalscale * 0.66f, SpriteEffects.None, 0);  //the center, moves slightly depending on direction for illusion of facing the player

			spriteBatch.End(); spriteBatch.Begin(default, BlendState.AlphaBlend, default, default, default, default, Main.GameViewMatrix.ZoomMatrix);
			return false;
		}
	}
}