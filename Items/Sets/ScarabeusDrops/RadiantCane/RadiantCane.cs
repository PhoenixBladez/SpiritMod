using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Sets.ScarabeusDrops.RadiantCane
{
	public class RadiantCane : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Sunrise Scepter");
			Tooltip.SetDefault("Conjures a slow, controllable sun orb");
			SpiritGlowmask.AddGlowMask(Item.type, Texture + "_glow");
        }

        public override void SetDefaults()
		{
			Item.damage = 28;
			Item.staff[Item.type] = true;
			Item.noMelee = true;
			Item.DamageType = DamageClass.Magic;
			Item.Size = new Vector2(34, 38);
			Item.useTime = 25;
			Item.channel = true;
			Item.mana = 12;
            Item.rare = ItemRarityID.Blue;
			Item.useAnimation = 25;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.knockBack = 2;
			Item.value = Item.sellPrice(0, 1, 80, 0);
			Item.autoReuse = true;
			Item.shootSpeed = 9;
			Item.UseSound = SoundID.Item20;
			Item.shoot = ModContent.ProjectileType<RadiantOrb>();
		}

		public override bool CanUseItem(Player player) => player.ownedProjectileCounts[Item.shoot] == 0;

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) => GlowmaskUtils.DrawItemGlowMaskWorld(spriteBatch, Item, Mod.Assets.Request<Texture2D>(Texture.Remove(0, Mod.Name.Length + 1) + "_glow", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value, rotation, scale);

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			Vector2 offset = Vector2.UnitX.RotatedBy(player.AngleTo(Main.MouseWorld)) * 36;
			position += Collision.CanHit(player.MountedCenter, 0, 0, player.MountedCenter + offset, 0, 0) ? offset : Vector2.Zero;

			for (int k = 0; k < 10; k++) { // dust ring

				int dust = Dust.NewDust(player.MountedCenter + offset, player.width, player.height, DustID.GemTopaz, 0f, 0f, 0, default, 1f);
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
		public override string Name => base.Name.Remove(base.Name.Length - "RadiantOrb".Length, "RadiantOrb".Length) + "RadiantCane";

		public override void SetStaticDefaults() => DisplayName.SetDefault("Radiant Orb");

		public override void SetDefaults()
		{
			Projectile.Size = new Vector2(40, 40);
			Projectile.friendly = true;
			Projectile.DamageType = DamageClass.Magic;
			Projectile.tileCollide = true;
			Projectile.penetrate = -1;
		}

		public override void AI()
		{
			Player owner = Main.player[Projectile.owner];
			owner.itemTime = 2;
			owner.itemAnimation = 2;
			owner.reuseDelay = owner.HeldItem.useTime;
			Lighting.AddLight(Projectile.Center, Color.LightGoldenrodYellow.ToVector3() / 3);
			if (owner == Main.LocalPlayer && owner.channel && Projectile.ai[0] == 0) {
				Projectile.timeLeft++;
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.DirectionTo(Main.MouseWorld) * Math.Min(Projectile.Distance(Main.MouseWorld) / 75, 16), 0.05f);
				Projectile.netUpdate = true;
			}

			if (!owner.channel)
				Projectile.ai[0]++;

			if (++Projectile.localAI[0] % 50 == 0 && owner.CheckMana(owner.HeldItem.mana, true) && Projectile.ai[0] == 0) {
				if (owner.HeldItem.UseSound.HasValue)
					SoundEngine.PlaySound(owner.HeldItem.UseSound.Value, owner.MountedCenter); 
				
				for (int k = 0; k < 10; k++) { //dust ring
					Vector2 offset = Vector2.UnitX.RotatedBy(owner.AngleTo(Projectile.Center)) * 36;
					int dust = Dust.NewDust(owner.MountedCenter + offset, owner.width, owner.height, DustID.GemTopaz, 0f, 0f, 0, default, 1f);
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
			else if (Projectile.localAI[0] % 50 == 0)
				Projectile.ai[0]++;

			if (Projectile.ai[0] > 0) {
				Projectile.timeLeft = Math.Min(Projectile.timeLeft, 30);
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, Vector2.Zero, 0.1f);
			}

			owner.ChangeDir(Math.Sign(owner.DirectionTo(Projectile.Center).X));
			owner.itemRotation = MathHelper.WrapAngle(owner.AngleTo(Projectile.Center) - owner.fullRotation - ((owner.direction < 0) ? MathHelper.Pi : 0));

			Projectile.alpha = 255 - (Projectile.timeLeft * 255 / 30);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Projectile.velocity = new Vector2((Projectile.velocity.X != oldVelocity.X) ? -oldVelocity.X : Projectile.velocity.X, (Projectile.velocity.Y != oldVelocity.Y) ? -oldVelocity.Y : Projectile.velocity.Y);
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => target.immune[Projectile.owner] = 15;

		public override bool PreDraw(ref Color lightColor)
		{
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
			Texture2D bloom = TextureAssets.Extra[49].Value;
			SpiritMod.SunOrbShader.Parameters["colorMod"].SetValue(new Color(255, 245, 158).ToVector4());
			SpiritMod.SunOrbShader.Parameters["colorMod2"].SetValue(Color.LightGoldenrodYellow.ToVector4());
			SpiritMod.SunOrbShader.Parameters["timer"].SetValue(Main.GlobalTimeWrappedHourly / 3 % 1);
			SpiritMod.SunOrbShader.CurrentTechnique.Passes[0].Apply();

			float scale = MathHelper.Lerp(0.6f, 0.8f, (float)Math.Sin(Main.GlobalTimeWrappedHourly * 2) / 2 + 0.5f);
			Color drawcolor = Projectile.GetAlpha(Color.White);
			Vector2 drawcenter = Projectile.Center - Main.screenPosition;

			Main.spriteBatch.Draw(bloom, drawcenter, null, drawcolor, Projectile.rotation, bloom.Size() / 2, Projectile.scale * 0.66f * MathHelper.Lerp(scale, 1, 0.25f), SpriteEffects.None, 0);

			Main.spriteBatch.Draw(bloom, drawcenter, null, drawcolor * 0.33f, Projectile.rotation, bloom.Size() / 2, Projectile.scale * scale, SpriteEffects.None, 0);
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, null, null, Main.GameViewMatrix.ZoomMatrix);
			return false;
		}

		public void AdditiveCall(SpriteBatch spriteBatch)
		{
			int numrays = 7;
			for (int i = 0; i < numrays; i++) {
				Texture2D ray = Mod.Assets.Request<Texture2D>("Textures/Medusa_Ray").Value;
				float rotation = i * (MathHelper.TwoPi / numrays) + (Main.GlobalTimeWrappedHourly * (((i % 2) + 1)/2f));
				float length = 45 * (float)(Math.Sin((Main.GlobalTimeWrappedHourly + i) * 2) / 5 + 1);
				Vector2 rayscale = new Vector2(length / ray.Width, 0.8f);
				spriteBatch.Draw(ray, Projectile.Center - Main.screenPosition, null, Projectile.GetAlpha(new Color(255, 245, 158)) * Projectile.Opacity * 0.5f, rotation, 
					new Vector2(0, ray.Height / 2), rayscale * Projectile.scale, SpriteEffects.None, 0);
			}
		}
	}
}
