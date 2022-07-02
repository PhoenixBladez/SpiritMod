using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.EvilBiomeDrops.GastricGusher
{
	public class GastricGusherProjectile : ModProjectile
	{
		private int _charge = 0;
		private int _endCharge = -1;
		private float _finalRotation = 0f;

		const int MinimumCharge = 0; //How long it takes for a minimum charge - 1/2 second by default

		private float Scaling => ((_charge - MinimumCharge) * 0.03f) + 1f; //Scale factor for projectile damage, spread and speed
		private float ScalingCapped => Scaling >= 4f ? 4f : Scaling; //Cap for scaling so there's not super OP charging lol

		public override void SetStaticDefaults() => DisplayName.SetDefault("Gastric Gusher");

		public override void SetDefaults()
		{
			Projectile.width = 42;
			Projectile.height = 24;
			Projectile.friendly = true;
			Projectile.penetrate = -1;
			Projectile.tileCollide = false;
			Projectile.ignoreWater = true;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.aiStyle = -1;

			DrawHeldProjInFrontOfHeldItemAndArms = true;
		}

		public override bool? CanDamage()/* tModPorter Suggestion: Return null instead of false */ => false;

		public override void AI()
		{
			Player p = Main.player[Projectile.owner];
			p.heldProj = Projectile.whoAmI;

			if (_endCharge == -1) //Wait until the player has fired to let go & set position
			{
				p.itemTime = p.HeldItem.useTime;
				p.itemAnimation = p.HeldItem.useAnimation;
				Projectile.Center = p.Center - (Vector2.Normalize(p.MountedCenter - Main.MouseWorld) * 27) + new Vector2(21, 12);
			}
			else
				Projectile.Center = p.Center - (new Vector2(1, 0).RotatedBy(_finalRotation) * 27) + new Vector2(21, 12);

			if (p.whoAmI != Main.myPlayer)
				return; //mp check (hopefully)

			if (!p.channel && _endCharge == -1) //Fire (if possible)
			{
				_endCharge = _charge;
				_finalRotation = (Vector2.Normalize(p.MountedCenter - Main.MouseWorld) * 27).ToRotation();
				if (_endCharge >= MinimumCharge)
					Fire(p);
			}

			if (p.channel) //Use turn functionality
				p.direction = Main.MouseWorld.X >= p.MountedCenter.X ? 1 : -1;

			if (_charge > _endCharge && _endCharge != -1) //Kill projectile when done shooting - does nothing special but allowed for a cooldown timer before polish
				Projectile.active = false;

			_charge++; //Increase charge timer...
			Projectile.timeLeft++; //...and dont die

			Projectile.rotation = Vector2.Normalize(p.MountedCenter - Main.MouseWorld).ToRotation() - MathHelper.Pi; //So it looks like the player is holding it properly
			GItem.ArmsTowardsMouse(p);
		}

		private void Fire(Player p)
		{
			SoundEngine.PlaySound(SoundID.NPCDeath13, Projectile.Center);

			Vector2 vel = Vector2.Normalize(Main.MouseWorld - p.Center) * 10f * (ScalingCapped * 0.8f);
			int inc = 3 + (int)ScalingCapped;

			p.PickAmmo(p.HeldItem, out int _, out float _, out int damage, out float kb, out int ammo, false);

			for (int i = 0; i < inc; i++) //Projectiles
			{
				Vector2 velocity = vel.RotatedBy((i - (inc / 2f)) * 0.16f) * Main.rand.NextFloat(0.85f, 1.15f);
				Projectile.NewProjectile(p.GetSource_ItemUse_WithPotentialAmmo(), p.Center, velocity, ModContent.ProjectileType<GastricAcid>(), (int)(damage * ScalingCapped), 1f, Projectile.owner);
			}
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Player p = Main.player[Projectile.owner];
			Texture2D t = TextureAssets.Projectile[Projectile.type].Value;
			SpriteEffects e = Main.MouseWorld.X >= p.MountedCenter.X ? SpriteEffects.None : SpriteEffects.FlipHorizontally;

			float realRot = Projectile.rotation; //Rotate towards mouse
			if (_endCharge != -1) realRot = _finalRotation + MathHelper.Pi;
			if (e == SpriteEffects.FlipHorizontally)
				realRot -= MathHelper.Pi;

			Vector2 drawPos = Projectile.position - Main.screenPosition; //Draw position + charge shaking
			if (_charge > MinimumCharge && _endCharge == -1)
				drawPos += new Vector2(Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f)) * (ScalingCapped * 0.75f);

			Main.spriteBatch.Draw(t, drawPos, new Rectangle(0, 0, 42, 24), lightColor, realRot, new Vector2(21, 12), 1f, e, 1f);
			return false;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(_endCharge);
			writer.Write(_charge);
			writer.Write(_finalRotation);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			_endCharge = reader.ReadInt32();
			_charge = reader.ReadInt32();
			_finalRotation = reader.ReadSingle();
		}
	}
}
