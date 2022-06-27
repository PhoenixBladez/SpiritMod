using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Dusts;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ScarabeusDrops.Khopesh
{
	public class RoyalKhopesh : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Royal Khopesh");
			Tooltip.SetDefault("Swings in a 3-hit combo\nThe last hit in the combo is bigger, does more damage, and ignores armor");
		}

		public override void SetDefaults()
		{
			Item.damage = 16;
			Item.DamageType = DamageClass.Melee;
			Item.width = 36;
			Item.height = 44;
			Item.useTime = 12;
			Item.useAnimation = 12;
			Item.reuseDelay = 4;
			Item.channel = true;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 5.5f;
			Item.value = Item.sellPrice(0, 1, 80, 0);
			Item.crit = 4;
			Item.rare = ItemRarityID.Blue;
			Item.shootSpeed = 14f;
			Item.autoReuse = false;
			Item.shoot = ModContent.ProjectileType<KhopeshSlash>();
			Item.noUseGraphic = true;
			Item.noMelee = true;
			Item.autoReuse = true;
		}

		public override bool CanUseItem(Player player) => player.GetModPlayer<KhopeshPlayer>().KhopeshDelay == 0;
	}

	internal class KhopeshSlash : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Royal Khopesh");
			Main.projFrames[Projectile.type] = 8;
		}

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.tileCollide = false;
			Projectile.Size = new Vector2(54, 54);
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 16;
			Projectile.ownerHitCheck = true;
		}

		Player Player => Main.player[Projectile.owner];

		private bool FirstTickOfSwingFrame {
			get => Projectile.ai[1] == 0;
			set => Projectile.ai[1] = value ? 0 : 1;
		}

		private bool Bigswing => (Projectile.frame >= 4);

		private bool SwingStart => Projectile.frame == 0 || Projectile.frame == 4 && Projectile.ai[0] > 0;
		public override bool PreAI()
		{
			Projectile.position -= Projectile.velocity;
			bool killproj()
			{
				Projectile.Kill();
				return false;
			}

			if (Player.dead || !Player.active)
				return killproj();

			if (SwingStart && Main.myPlayer == Projectile.owner && FirstTickOfSwingFrame) { //update the direction at the beginning of a slash
				if (!Player.channel)
					return killproj();

				LegacySoundStyle sound = new LegacySoundStyle(SoundID.Item, 1).WithPitchVariance(0.1f);

				if (Projectile.frame == 4) sound = sound.WithVolume(1.5f);

				SoundEngine.PlaySound(sound, Projectile.Center);

				Projectile.velocity = Player.DirectionTo(Main.MouseWorld);
				FirstTickOfSwingFrame = false;
				Projectile.netUpdate = true;
			}

			return true;
		}

		public override void AI()
		{
			Projectile.scale = (Bigswing) ? 1.75f : 1f;
			float dist = (Bigswing) ? 45f : 35f;

			Player.itemTime = 2;
			Player.itemAnimation = 2;
			Player.GetModPlayer<KhopeshPlayer>().KhopeshDelay = 20;
			Projectile.Center = Player.MountedCenter + Projectile.velocity * dist;
			Projectile.rotation = Player.AngleFrom(Projectile.Center) - ((Projectile.spriteDirection > 0) ? 0 : MathHelper.Pi);
			Player.ChangeDir(Math.Sign(Projectile.Center.X - Player.Center.X));
			Player.itemRotation = MathHelper.WrapAngle(Player.AngleFrom(Projectile.Center) - ((Player.direction < 0) ? 0 : MathHelper.Pi));
			//projectile.spriteDirection = player.direction;

			Projectile.frameCounter++;

			if (Projectile.frameCounter > 3) {
				FirstTickOfSwingFrame = true;
				Projectile.frameCounter = 0;
				Projectile.frame++;
				if (Projectile.frame == 4 && Projectile.ai[0] == 0) {
					Projectile.spriteDirection *= -1;
					Projectile.rotation -= MathHelper.Pi;
					Projectile.frame = 0;
					Projectile.ai[0]++;
				}

				if (Projectile.frame >= Main.projFrames[Projectile.type])
					Projectile.Kill();
			}
			if (SwingStart && Projectile.frameCounter == 1) {
				int dustamount = (Bigswing) ? 20 : 7;
				for (int i = 0; i < dustamount; i++) {
					float dustscale = (Bigswing) ? 2f : 1f;
					dustscale *= Main.rand.NextFloat(0.7f, 1.3f);
					float dusvel = dustscale * Main.rand.NextFloat(3, 6);
					Vector2 dustpos = Projectile.velocity.RotatedByRandom(MathHelper.Pi) * dist * Main.rand.NextFloat(0.8f, 1.2f);
					Dust dust = Dust.NewDustPerfect(Player.Center + dustpos, ModContent.DustType<SandDust>(), Projectile.velocity.RotatedByRandom(MathHelper.Pi / 6) * dusvel, Scale: dustscale);
					dust.noGravity = true;
				}
			}
		}

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			float scalemod = Projectile.scale - 0.8f;
			hitbox.Inflate((int)(scalemod * Projectile.width), (int)(scalemod * Projectile.height));
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (Bigswing) {
				damage = (int)(damage * 1.5f);
				damage += target.defense / 2;
				knockback *= 1.5f;

				if (!Main.player[Projectile.owner].noKnockback)
					Main.player[Projectile.owner].velocity = -Projectile.velocity * 4;
			}

			hitDirection = Player.direction;
		}

		public override Color? GetAlpha(Color lightColor) => Color.Lerp(lightColor, Color.White, 0.2f);

		public override bool PreDraw(ref Color lightColor)
		{
			if ((Projectile.frame == 0 || Projectile.frame == 4 && Projectile.ai[0] > 0) && !Player.channel)
				return false;

			Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
			Rectangle frame = new Rectangle(0, Projectile.frame * (tex.Height / Main.projFrames[Projectile.type]), tex.Width, (tex.Height / Main.projFrames[Projectile.type]));
			SpriteEffects effects = (Projectile.spriteDirection < 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, frame, Projectile.GetAlpha(lightColor), Projectile.rotation, frame.Size() / 2, Projectile.scale, effects, 0);
			return false;
		}
	}

	internal class KhopeshPlayer : ModPlayer
	{
		public int KhopeshDelay = 0;
		public override void ResetEffects() => KhopeshDelay = Math.Max(KhopeshDelay - 1, 0);
	}
}