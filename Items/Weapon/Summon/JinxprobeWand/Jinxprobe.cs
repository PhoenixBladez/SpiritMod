using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs.Summon;
using SpiritMod.Projectiles.BaseProj;
using SpiritMod.Utilities;
using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Summon.JinxprobeWand
{
	[AutoloadMinionBuff("Tiny Jinxprobe", "The Jinxprobe will fight for you")]
	public class Jinxprobe : BaseMinion
	{
		public Jinxprobe() : base(800, 1600, new Vector2(32, 32), false) { }

		public override void AbstractSetStaticDefaults() => DisplayName.SetDefault("Jinxprobe");

		Player Player => Main.player[projectile.owner];
		Vector2 truePosition = Vector2.Zero;
		Vector2 newCenter = Vector2.Zero;

        public override bool PreAI()
        {
			if (projectile.ai[0] == 0) //check for first tick
            {
				truePosition = projectile.position - Player.Center;
				newCenter = Player.Center;
				projectile.ai[0]++;
				projectile.netUpdate = true;
			}

			if (Vector2.Distance(newCenter, Player.Center) > 500) //if too much distance from the center used for orbiting and the real player's center, set the new center to the player's center
				newCenter = Player.Center;
			else //otherwise slowly adjust it
				newCenter = Vector2.Lerp(newCenter, Player.Center, 0.05f);

			projectile.position -= projectile.velocity; //override default position updating, make it relative to player position
			truePosition += projectile.velocity;
			projectile.Center = newCenter + truePosition;
            return base.PreAI();
        }

		public override void IdleMovement(Player player)
		{
			OrbitingMovement();
			projectile.rotation = projectile.AngleFrom(player.Center);
		}

		private void OrbitingMovement()
        {
			float distanceStrength = 0.001f;
			float mindistance = 50;
			float maxdistance = 200;

			if (projectile.Distance(newCenter) > mindistance)
				projectile.velocity += projectile.DirectionTo(newCenter) * MathHelper.Clamp((projectile.Distance(newCenter) - mindistance) * distanceStrength, 0, 0.5f);

			if (projectile.Distance(newCenter) > maxdistance)
				projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(newCenter) * 10, MathHelper.Clamp((projectile.Distance(newCenter) - maxdistance) * distanceStrength * 0.05f, 0, 0.01f));

			if (projectile.velocity.Length() < 8)
				projectile.velocity *= 1.03f;

			if (projectile.velocity.Length() > 11)
				projectile.velocity *= 0.98f;
		}

        public override void TargettingBehavior(Player player, NPC target)
		{
			OrbitingMovement();

			projectile.rotation = Utils.AngleLerp(projectile.rotation, projectile.AngleTo(target.Center), 0.05f);

			projectile.ai[1]++;
			if(projectile.ai[1] > 60 && projectile.Distance(newCenter) < 300)
            {
				if (Main.netMode != NetmodeID.Server)
					Main.PlaySound(mod.GetLegacySoundSlot(Terraria.ModLoader.SoundType.Custom, "Sounds/starCast").WithPitchVariance(0.3f).WithVolume(0.6f), projectile.position);

				Vector2 vel = projectile.GetArcVel(target.Center, 0.1f, heightabovetarget : Main.rand.Next(50, 100));
				projectile.velocity = projectile.DirectionFrom(target.Center).RotatedByRandom(MathHelper.PiOver2) * 15;

				Projectile.NewProjectileDirect(projectile.Center, vel, ModContent.ProjectileType<JinxprobeEnergy>(), projectile.damage, projectile.knockBack, projectile.owner).netUpdate = true;
				projectile.ai[1] = 0;
				projectile.netUpdate = true;
            }
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D glow = mod.GetTexture(Texture.Remove(0, mod.Name.Length + 1) + "_glow");
			Texture2D glow2 = mod.GetTexture(Texture.Remove(0, mod.Name.Length + 1) + "_glow2");
			Rectangle rect = glow.Bounds;

			//draw beam to player
			Texture2D tex = mod.GetTexture("Extras/Medusa_Ray");
			Color beamcolor = SpiritMod.StarjinxColor(Main.GlobalTime * 4) * 0.5f * ((float)Math.Sin(Main.GlobalTime * 3) / 4 + 0.75f);
			Vector2 scale = new Vector2(projectile.Distance(Player.Center) / tex.Width, 1) * 0.75f;
			spriteBatch.Draw(tex,
				projectile.Center - Main.screenPosition + new Vector2(tex.Size().X * scale.X, 0).RotatedBy(projectile.AngleTo(Player.Center)) / 2,
				null,
				SpiritMod.StarjinxColor(Main.GlobalTime * 4) * 0.5f * ((float)Math.Sin(Main.GlobalTime * 3) / 4 + 0.75f),
				projectile.AngleTo(Player.Center),
				tex.Size() / 2,
				scale,
				SpriteEffects.None,
				0);

			float newrotation = (Math.Abs(projectile.rotation) > MathHelper.Pi/2) ? projectile.rotation - MathHelper.Pi : projectile.rotation;
			SpriteEffects flip = (Math.Abs(projectile.rotation) > MathHelper.Pi / 2) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			//draw big glow underneath projectile
			spriteBatch.Draw(glow2, projectile.Center - Main.screenPosition, glow2.Bounds, beamcolor, newrotation,
				glow2.Size() / 2, projectile.scale * 1.1f, flip, 0); 
			spriteBatch.Draw(glow2, projectile.Center - Main.screenPosition, glow2.Bounds, beamcolor * 0.3f, newrotation,
				 glow2.Size() / 2, projectile.scale * ((float)Math.Sin(Main.GlobalTime * 3) / 6 + 1.2f), flip, 0);

			//redraw projectile and glowmask
			spriteBatch.Draw(Main.projectileTexture[projectile.type], projectile.Center - Main.screenPosition, rect, projectile.GetAlpha(lightColor), newrotation, rect.Size() / 2, projectile.scale, flip, 0);
			spriteBatch.Draw(glow, projectile.Center - Main.screenPosition, rect, projectile.GetAlpha(Color.White), newrotation, rect.Size() / 2, projectile.scale, flip, 0);

			spriteBatch.Draw(glow, projectile.Center - Main.screenPosition, rect, projectile.GetAlpha(Color.White * 0.5f), newrotation, rect.Size() / 2, 
				projectile.scale + (projectile.scale * (0.3f + (float)Math.Sin(Main.GlobalTime * 3)/6)), flip, 0);

			return false;
		}
	}
}