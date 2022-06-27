using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Buffs.Summon;
using SpiritMod.Projectiles.BaseProj;
using SpiritMod.Utilities;
using System;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.StarjinxSet.JinxprobeWand
{
	[AutoloadMinionBuff("Tiny Jinxprobe", "The Jinxprobe will fight for you")]
	public class Jinxprobe : BaseMinion
	{
		public Jinxprobe() : base(800, 1600, new Vector2(32, 32)) { }

		public override void AbstractSetStaticDefaults() => DisplayName.SetDefault("Jinxprobe");

		Vector2 truePosition = Vector2.Zero;
		Vector2 newCenter = Vector2.Zero;

        public override bool PreAI()
        {
			if (Projectile.ai[0] == 0) //check for first tick
            {
				truePosition = Projectile.position - Player.Center;
				newCenter = Player.Center;
				Projectile.ai[0]++;
				Projectile.netUpdate = true;
			}

			if (Vector2.Distance(newCenter, Player.Center) > 500) //if too much distance from the center used for orbiting and the real player's center, set the new center to the player's center
				newCenter = Player.Center;
			else //otherwise slowly adjust it
				newCenter = Vector2.Lerp(newCenter, Player.Center, 0.066f);

			Projectile.position -= Projectile.velocity; //override default position updating, make it relative to player position
			truePosition += Projectile.velocity;
			Projectile.Center = newCenter + truePosition;
            return base.PreAI();
        }

		public override bool MinionContactDamage() => false;

		public override void IdleMovement(Player player)
		{
			OrbitingMovement();
			Projectile.rotation = Projectile.AngleFrom(player.Center);
		}

		private const float MaxDistFromCenter = 100;
		private void OrbitingMovement()
        {
			float distanceStrength = 0.002f;
			float mindistance = 30;

			if (Projectile.Distance(newCenter) > mindistance)
				Projectile.velocity += Projectile.DirectionTo(newCenter) * MathHelper.Clamp((Projectile.Distance(newCenter) - mindistance) * distanceStrength, 0, 0.5f);

			if (Projectile.Distance(newCenter) > MaxDistFromCenter)
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(newCenter) * 10, MathHelper.Clamp((Projectile.Distance(newCenter) - MaxDistFromCenter) * distanceStrength * 0.05f, 0, 0.01f));

			if (Projectile.velocity.Length() < 8)
				Projectile.velocity *= 1.03f;

			if (Projectile.velocity.Length() > 11)
				Projectile.velocity *= 0.98f;
		}

        public override void TargettingBehavior(Player player, NPC target)
		{
			OrbitingMovement();

			Projectile.rotation = Utils.AngleLerp(Projectile.rotation, Projectile.AngleTo(target.Center), 0.05f);

			Projectile.ai[1]++;
			if(Projectile.ai[1] > 50 && Collision.CanHit(Projectile.Center, 0, 0, target.Center, 0, 0))
            {
				if (Main.netMode != NetmodeID.Server)
					SoundEngine.PlaySound(Mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/starCast").WithPitchVariance(0.3f).WithVolume(0.6f), Projectile.position);

				Vector2 vel = Projectile.GetArcVel(target.Center, 0.1f, heightabovetarget : Main.rand.Next(50, 100));
				Projectile.velocity = Projectile.DirectionFrom(target.Center).RotatedByRandom(MathHelper.PiOver2) * 8;

				Projectile.NewProjectileDirect(Projectile.Center, vel, ModContent.ProjectileType<JinxprobeEnergy>(), Projectile.damage, Projectile.knockBack, Projectile.owner).netUpdate = true;
				Projectile.ai[1] = 0;
				Projectile.netUpdate = true;
            }
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D glow = Mod.Assets.Request<Texture2D>(Texture.Remove(0, Mod.Name.Length + 1).Value + "_glow");
			Texture2D glow2 = Mod.Assets.Request<Texture2D>(Texture.Remove(0, Mod.Name.Length + 1).Value + "_glow2");
			Rectangle rect = glow.Bounds;

			//draw beam to player
			Texture2D tex = Mod.Assets.Request<Texture2D>("Textures/Medusa_Ray").Value;
			Color beamcolor = SpiritMod.StarjinxColor(Main.GlobalTimeWrappedHourly * 4) * 0.5f * ((float)Math.Sin(Main.GlobalTimeWrappedHourly * 3) / 4 + 0.75f);
			Vector2 scale = new Vector2(Projectile.Distance(Player.Center) / tex.Width, 1) * 0.75f;
			spriteBatch.Draw(tex,
				Projectile.Center - Main.screenPosition + new Vector2(tex.Size().X * scale.X, 0).RotatedBy(Projectile.AngleTo(Player.Center)) / 2,
				null,
				SpiritMod.StarjinxColor(Main.GlobalTimeWrappedHourly * 4) * 0.5f * ((float)Math.Sin(Main.GlobalTimeWrappedHourly * 3) / 4 + 0.75f),
				Projectile.AngleTo(Player.Center),
				tex.Size() / 2,
				scale,
				SpriteEffects.None,
				0);

			float newrotation = (Math.Abs(Projectile.rotation) > MathHelper.Pi/2) ? Projectile.rotation - MathHelper.Pi : Projectile.rotation;
			SpriteEffects flip = (Math.Abs(Projectile.rotation) > MathHelper.Pi / 2) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;

			//draw big glow underneath projectile
			spriteBatch.Draw(glow2, Projectile.Center - Main.screenPosition, glow2.Bounds, beamcolor, newrotation,
				glow2.Size() / 2, Projectile.scale * 1.1f, flip, 0); 
			spriteBatch.Draw(glow2, Projectile.Center - Main.screenPosition, glow2.Bounds, beamcolor * 0.3f, newrotation,
				 glow2.Size() / 2, Projectile.scale * ((float)Math.Sin(Main.GlobalTimeWrappedHourly * 3) / 6 + 1.2f), flip, 0);

			//redraw projectile and glowmask
			spriteBatch.Draw(TextureAssets.Projectile[Projectile.type].Value, Projectile.Center - Main.screenPosition, rect, Projectile.GetAlpha(lightColor), newrotation, rect.Size() / 2, Projectile.scale, flip, 0);
			spriteBatch.Draw(glow, Projectile.Center - Main.screenPosition, rect, Projectile.GetAlpha(Color.White), newrotation, rect.Size() / 2, Projectile.scale, flip, 0);

			spriteBatch.Draw(glow, Projectile.Center - Main.screenPosition, rect, Projectile.GetAlpha(Color.White * 0.5f), newrotation, rect.Size() / 2, 
				Projectile.scale + (Projectile.scale * (0.3f + (float)Math.Sin(Main.GlobalTimeWrappedHourly * 3)/6)), flip, 0);

			return false;
		}
	}
}