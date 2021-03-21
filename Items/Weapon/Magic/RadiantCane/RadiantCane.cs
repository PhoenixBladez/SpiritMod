using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic.RadiantCane
{
	public class RadiantCane : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sunrise Sceptre");
			Tooltip.SetDefault("Conjures a slow, controllable sun orb");
			SpiritGlowmask.AddGlowMask(item.type, Texture + "_glow");
        }

        public override void SetDefaults()
		{
			item.damage = 28;
			Item.staff[item.type] = true;
			item.noMelee = true;
			item.magic = true;
			item.Size = new Vector2(34, 38);
			item.useTime = 25;
			item.channel = true;
			item.mana = 12;
            item.rare = ItemRarityID.Blue;
			item.useAnimation = 25;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 2;
			item.value = Item.sellPrice(0, 1, 80, 0);
			item.autoReuse = true;
			item.shootSpeed = 9;
			item.UseSound = SoundID.Item20;
			item.shoot = ModContent.ProjectileType<RadiantOrb>();
		}

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[item.shoot] == 0;

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) => GlowmaskUtils.DrawItemGlowMaskWorld(spriteBatch, item, mod.GetTexture(Texture.Remove(0, mod.Name.Length + 1) + "_glow"), rotation, scale);

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 offset = Vector2.UnitX.RotatedBy(player.AngleTo(Main.MouseWorld)) * 36;
			position += Collision.CanHit(player.MountedCenter, 0, 0, player.MountedCenter + offset, 0, 0) ? offset : Vector2.Zero;

			for (int k = 0; k < 10; k++) { // dust ring

				int dust = Dust.NewDust(player.MountedCenter + offset, player.width, player.height, 87, 0f, 0f, 0, default(Color), 1f);
				Main.dust[dust].shader = GameShaders.Armor.GetSecondaryShader(9, Main.LocalPlayer);
				Main.dust[dust].velocity *= -1f;
				Main.dust[dust].noGravity = true;
				Vector2 vector2_1 = new Vector2((float)Main.rand.Next(-100, 101), (float)Main.rand.Next(-100, 101));
				vector2_1.Normalize();
				Vector2 vector2_2 = vector2_1 * ((float)Main.rand.Next(50, 100) * 0.04f);
				Main.dust[dust].velocity = vector2_2;
				vector2_2.Normalize();
				Vector2 vector2_3 = vector2_2 * 34f;
				Main.dust[dust].position = (player.MountedCenter + offset) - vector2_3;
			}

			return true;
		}
	}

	internal class RadiantOrb : ModProjectile, IDrawAdditive
	{
		public override bool Autoload(ref string name)
		{
			name = name.Remove(name.Length - "RadiantOrb".Length, "RadiantOrb".Length) + "RadiantCane";
			return base.Autoload(ref name);
		}

		public override void SetStaticDefaults() => DisplayName.SetDefault("Radiant Orb");

		public override void SetDefaults()
		{
			projectile.Size = new Vector2(40, 40);
			projectile.friendly = true;
			projectile.magic = true;
			projectile.tileCollide = true;
			projectile.penetrate = -1;
		}

		public override void AI()
		{
			Player owner = Main.player[projectile.owner];
			owner.itemTime = 2;
			owner.itemAnimation = 2;
			owner.reuseDelay = owner.HeldItem.useTime;
			Lighting.AddLight(projectile.Center, Color.LightGoldenrodYellow.ToVector3() / 3);
			if (owner == Main.LocalPlayer && owner.channel && projectile.ai[0] == 0) {
				projectile.timeLeft++;
				projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(Main.MouseWorld) * Math.Min(projectile.Distance(Main.MouseWorld) / 75, 16), 0.05f);
				projectile.netUpdate = true;
			}

			if (!owner.channel)
				projectile.ai[0]++;

			if (++projectile.localAI[0] % 50 == 0 && owner.CheckMana(owner.HeldItem.mana, true) && projectile.ai[0] == 0) {
				Main.PlaySound(owner.HeldItem.UseSound, owner.MountedCenter); 
				
				for (int k = 0; k < 10; k++) { //dust ring
					Vector2 offset = Vector2.UnitX.RotatedBy(owner.AngleTo(projectile.Center)) * 36;
					int dust = Dust.NewDust(owner.MountedCenter + offset, owner.width, owner.height, 87, 0f, 0f, 0, default, 1f);
					Main.dust[dust].shader = GameShaders.Armor.GetSecondaryShader(9, Main.LocalPlayer);
					Main.dust[dust].velocity *= -1f;
					Main.dust[dust].noGravity = true;
					Vector2 vector2_1 = new Vector2(Main.rand.Next(-100, 101), Main.rand.Next(-100, 101));
					vector2_1.Normalize();
					Vector2 vector2_2 = vector2_1 * (Main.rand.Next(50, 100) * 0.04f);
					Main.dust[dust].velocity = vector2_2;
					vector2_2.Normalize();
					Vector2 vector2_3 = vector2_2 * 34f;
					Main.dust[dust].position = (owner.MountedCenter + offset) - vector2_3;
				}
			}
			else if (projectile.localAI[0] % 50 == 0)
				projectile.ai[0]++;

			if (projectile.ai[0] > 0) {
				projectile.timeLeft = Math.Min(projectile.timeLeft, 30);
				projectile.velocity = Vector2.Lerp(projectile.velocity, Vector2.Zero, 0.1f);
			}

			owner.ChangeDir(Math.Sign(owner.DirectionTo(projectile.Center).X));
			owner.itemRotation = MathHelper.WrapAngle(owner.AngleTo(projectile.Center) - owner.fullRotation - ((owner.direction < 0) ? MathHelper.Pi : 0));

			projectile.alpha = 255 - (projectile.timeLeft * 255 / 30);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			projectile.velocity = new Vector2((projectile.velocity.X != oldVelocity.X) ? -oldVelocity.X : projectile.velocity.X, (projectile.velocity.Y != oldVelocity.Y) ? -oldVelocity.Y : projectile.velocity.Y);
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => target.immune[projectile.owner] = 15;

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) => false;

		public void DrawAdditive(SpriteBatch spriteBatch)
		{
			int numrays = 7;
			for (int i = 0; i < numrays; i++) {
				Texture2D ray = mod.GetTexture("Textures/Medusa_Ray");
				float rotation = i * (MathHelper.TwoPi / numrays) + (Main.GlobalTime * (((i % 2) + 1)/2f));
				float length = 45 * (float)(Math.Sin((Main.GlobalTime + i) * 2) / 5 + 1);
				Vector2 rayscale = new Vector2(length / ray.Width, 0.8f);
				spriteBatch.Draw(ray, projectile.Center - Main.screenPosition, null, projectile.GetAlpha(new Color(255, 245, 158)) * projectile.Opacity * 0.5f, rotation, 
					new Vector2(0, ray.Height / 2), rayscale * projectile.scale, SpriteEffects.None, 0);
			}

			spriteBatch.End(); spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
			Texture2D bloom = Main.extraTexture[49];
			SpiritMod.SunOrbShader.Parameters["colorMod"].SetValue(new Color(255, 245, 158).ToVector4());
			SpiritMod.SunOrbShader.Parameters["colorMod2"].SetValue(Color.LightGoldenrodYellow.ToVector4());
			SpiritMod.SunOrbShader.Parameters["timer"].SetValue(Main.GlobalTime/3 % 1);
			SpiritMod.SunOrbShader.CurrentTechnique.Passes[0].Apply();

			float scale = MathHelper.Lerp(0.6f, 0.8f, (float)Math.Sin(Main.GlobalTime * 2) / 2 + 0.5f);
			Color drawcolor = projectile.GetAlpha(Color.White);
			Vector2 drawcenter = projectile.Center - Main.screenPosition;

			spriteBatch.Draw(bloom, drawcenter, null, drawcolor, projectile.rotation, bloom.Size() / 2, projectile.scale * 0.66f * MathHelper.Lerp(scale, 1, 0.25f), SpriteEffects.None, 0);

			spriteBatch.Draw(bloom, drawcenter, null, drawcolor * 0.33f, projectile.rotation, bloom.Size() / 2, projectile.scale * scale, SpriteEffects.None, 0);
		}
	}
}
