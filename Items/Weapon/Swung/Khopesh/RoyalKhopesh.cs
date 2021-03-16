using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json.Schema;
using SpiritMod.Dusts;
using System;
using System.IO;
using System.Linq;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung.Khopesh
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
			item.damage = 16;
			item.melee = true;
			item.width = 36;
			item.height = 44;
			item.useTime = 12;
			item.useAnimation = 12;
			item.reuseDelay = 20;
			item.channel = true;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 5.5f;
			item.value = Item.sellPrice(0, 1, 50, 0);
			item.crit = 4; 
			item.rare = ItemRarityID.Blue;
			item.shootSpeed = 14f;
			item.autoReuse = false;
			item.shoot = ModContent.ProjectileType<KhopeshSlash>();
			item.noUseGraphic = true;
			item.noMelee = true;
			item.autoReuse = true;
		}

		public override bool CanUseItem(Player player) => player.GetModPlayer<KhopeshPlayer>().KhopeshDelay == 0;
	}

	class KhopeshSlash : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Royal Khopesh");
			Main.projFrames[projectile.type] = 8;
		}

		public override void SetDefaults()
		{
			projectile.friendly = true;
			projectile.melee = true;
			projectile.tileCollide = false;
			projectile.Size = new Vector2(54, 54);
			projectile.penetrate = -1;
			projectile.usesLocalNPCImmunity = true;
			projectile.localNPCHitCooldown = 16;
		}

		Player Player => Main.player[projectile.owner];

		private bool FirstTickOfSwingFrame {
			get => projectile.ai[1] == 0;
			set => projectile.ai[1] = value ? 0 : 1;
		}

		bool Bigswing => (projectile.frame >= 4);

		bool SwingStart => projectile.frame == 0 || projectile.frame == 4 && projectile.ai[0] > 0;
		public override bool PreAI()
		{
			projectile.position -= projectile.velocity;
			bool killproj()
			{
				projectile.Kill();
				return false;
			}

			if (Player.dead || !Player.active)
				return killproj();

			if (SwingStart && Main.myPlayer == projectile.owner && FirstTickOfSwingFrame) { //update the direction at the beginning of a slash
				if (!Player.channel)
					return killproj();

				LegacySoundStyle sound = new LegacySoundStyle(SoundID.Item, 1).WithPitchVariance(0.1f);

				if (projectile.frame == 4) sound = sound.WithVolume(1.5f);

				Main.PlaySound(sound, projectile.Center);

				projectile.velocity = Player.DirectionTo(Main.MouseWorld);
				FirstTickOfSwingFrame = false;
				projectile.netUpdate = true;
			}

			return true;
		}

		public override void AI()
		{
			projectile.scale = (Bigswing) ? 1.75f : 1f;
			float dist = (Bigswing) ? 45f : 35f;

			Player.itemTime = 2;
			Player.itemAnimation = 2;
			Player.GetModPlayer<KhopeshPlayer>().KhopeshDelay = 20;
			projectile.Center = Player.MountedCenter + projectile.velocity * dist;
			projectile.rotation = Player.AngleFrom(projectile.Center) - ((projectile.spriteDirection > 0) ? 0 : MathHelper.Pi);
			Player.ChangeDir(Math.Sign(projectile.Center.X - Player.Center.X));
			Player.itemRotation = MathHelper.WrapAngle(Player.AngleFrom(projectile.Center) - ((Player.direction < 0) ? 0 : MathHelper.Pi));
			//projectile.spriteDirection = player.direction;

			projectile.frameCounter++;

			if(projectile.frameCounter > 3) {
				FirstTickOfSwingFrame = true;
				projectile.frameCounter = 0;
				projectile.frame++;
				if(projectile.frame == 4 && projectile.ai[0] == 0) {
					projectile.spriteDirection *= -1;
					projectile.rotation -= MathHelper.Pi;
					projectile.frame = 0;
					projectile.ai[0]++;
				}

				if (projectile.frame >= Main.projFrames[projectile.type])
					projectile.Kill();
			}
			if(SwingStart && projectile.frameCounter == 1) {
				int dustamount = (Bigswing) ? 20 : 7;
				for(int i = 0; i < dustamount; i++) {
					float dustscale = (Bigswing) ? 2f : 1f;
					dustscale *= Main.rand.NextFloat(0.7f, 1.3f);
					float dusvel = dustscale * Main.rand.NextFloat(3, 6);
					Vector2 dustpos = projectile.velocity.RotatedByRandom(MathHelper.Pi) * dist * Main.rand.NextFloat(0.8f, 1.2f);
					Dust dust = Dust.NewDustPerfect(Player.Center + dustpos, ModContent.DustType<SandDust>(), projectile.velocity.RotatedByRandom(MathHelper.Pi / 6) * dusvel, Scale: dustscale);
					dust.noGravity = true;
				}
			}
		}

		public override void ModifyDamageHitbox(ref Rectangle hitbox)
		{
			float scalemod = projectile.scale - 0.8f;
			hitbox.Inflate((int)(scalemod * projectile.width), (int)(scalemod * projectile.height));
		}

		public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
		{
			if (Bigswing) {
				damage = (int)(damage * 1.5f);
				damage += target.defense / 2;
				knockback *= 1.5f;

				if(!Main.player[projectile.owner].noKnockback)
					Main.player[projectile.owner].velocity = -projectile.velocity * 4;
			}

			hitDirection = Player.direction;
		}
		public override Color? GetAlpha(Color lightColor) => Color.Lerp(lightColor, Color.White, 0.2f);

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			if((projectile.frame == 0 || projectile.frame == 4 && projectile.ai[0] > 0) && !Player.channel)
				return false;

			Texture2D tex = Main.projectileTexture[projectile.type];
			Rectangle frame = new Rectangle(0, projectile.frame * (tex.Height / Main.projFrames[projectile.type]), tex.Width, (tex.Height / Main.projFrames[projectile.type]));
			SpriteEffects effects = (projectile.spriteDirection < 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, frame, projectile.GetAlpha(lightColor), projectile.rotation, frame.Size() / 2, projectile.scale, effects, 0);
			return false;
		}
	}

	class KhopeshPlayer : ModPlayer
	{
		public int KhopeshDelay = 0;
		public override void ResetEffects() => KhopeshDelay = Math.Max(KhopeshDelay - 1, 0);
	}
}