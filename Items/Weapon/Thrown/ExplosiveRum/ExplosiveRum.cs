using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown.ExplosiveRum
{
	public class ExplosiveRum : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Explosive Rum");
			Tooltip.SetDefault("'Oh, there it is!'");
		}

		public override void SetDefaults()
		{
			Item.useStyle = ItemUseStyleID.Swing;
			Item.width = 24;
			Item.height = 24;
			Item.noUseGraphic = true;
			Item.UseSound = SoundID.Item1;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.consumable = true;
			Item.maxStack = 999;
			Item.shoot = ModContent.ProjectileType<ExplosiveRumProj>();
			Item.useAnimation = 29;
			Item.useTime = 29;
			Item.shootSpeed = 10.5f;
			Item.damage = 13;
			Item.knockBack = 1.5f;
			Item.value = Item.sellPrice(0, 0, 0, 50);
			Item.rare = ItemRarityID.LightRed;
			Item.autoReuse = true;
			Item.maxStack = 999;
			Item.consumable = true;
		}
	}

	public class ExplosiveRumProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Explosive Rum");
		}

		public override void SetDefaults()
		{
			Projectile.CloneDefaults(ProjectileID.Shuriken);
			Projectile.width = 20;
			Projectile.height = 20;
			Projectile.DamageType = DamageClass.Ranged;
			Projectile.penetrate = 1;
		}

		public override void AI()
		{
			var dust = Dust.NewDustPerfect(Projectile.Center + (15 * ((Projectile.rotation - 1.57f).ToRotationVector2())), 6);
			dust.noGravity = true;
			dust.scale = Main.rand.NextFloat(0.6f, .9f);
			dust.fadeIn = .75f;
		}

		public override void Kill(int timeLeft)
		{
			SoundEngine.PlaySound(new SoundStyle("SpiritMod/Sounds/rumboom"), Projectile.Center);
			SoundEngine.PlaySound(SoundID.Item27, Projectile.Center);

			for (int i = 1; i < 5; ++i)
				Gore.NewGore(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, Mod.Find<ModGore>("RumGore" + i).Type, 1f);

			Projectile.NewProjectile(Projectile.GetSource_Death(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<RumExplosion>(), Projectile.damage, Projectile.knockBack, Projectile.owner);

			Projectile.NewProjectileDirect(Projectile.GetSource_Death(), Projectile.Center - new Vector2(0, 15), new Vector2(0.25f, 15), ModContent.ProjectileType<RumFire>(), (int)(Projectile.damage * 0.75f), Projectile.knockBack, Projectile.owner, 1, 12).timeLeft = 60;
			Projectile.NewProjectileDirect(Projectile.GetSource_Death(), Projectile.Center - new Vector2(0, 15), new Vector2(-0.25f, 15), ModContent.ProjectileType<RumFire>(), (int)(Projectile.damage * 0.75f), Projectile.knockBack, Projectile.owner, -1, 12).timeLeft = 60;
		}
	}

	public class RumFire : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Explosive Rum");
			Main.projFrames[Projectile.type] = 5;
		}

		public override void SetDefaults()
		{
			Projectile.hostile = false;
			Projectile.width = 20;
			Projectile.height = 34;
			Projectile.aiStyle = -1;
			Projectile.friendly = false;
			Projectile.damage = 1;
			Projectile.penetrate = 3;
			Projectile.timeLeft = 26;
			Projectile.tileCollide = true;
			Projectile.ignoreWater = true;
			Projectile.scale = .85f;
			flipSprite = Main.rand.NextBool();
		}

		bool onGround = false;
		bool lightUp = false;
		bool flipSprite = false;
		public override void AI()
		{
			if (Projectile.scale <= 1f)
				Projectile.scale += .02f;

			if (Main.rand.Next(5) == 0 && onGround)
			{
				int d = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Torch);
				Main.dust[d].noGravity = true;
				Main.dust[d].velocity.Y = -1f;
			}
			if (Main.rand.Next(12) == 0)
			{
				int index3 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0.0f, 0f, 150, new Color(), 0.5f);
				Main.dust[index3].fadeIn = 1.25f;
				Main.dust[index3].velocity = new Vector2(0f, (float)Main.rand.Next(-2, -1));
				Main.dust[index3].noLight = true;
			}
			//projectile.scale = (float)Math.Sin((12 - projectile.timeLeft) / 4f);

			if (++Projectile.frameCounter > 4)
			{
				Projectile.frameCounter = 0;
				Projectile.frame++;
				if (Projectile.frame >= Main.projFrames[Projectile.type])
					Projectile.Kill();
			}

			if (Projectile.timeLeft == 23 && onGround && Projectile.ai[1] > 0)
			{
				lightUp = true;
				Vector2 pos = Projectile.Center + new Vector2(Projectile.ai[0] * 20, 0);
				Projectile.NewProjectile(Projectile.GetSource_Death(), pos, new Vector2(Projectile.ai[0] * 0.25f, 15), ModContent.ProjectileType<RumFire>(), Math.Max((int)(Projectile.damage * 0.98f), 1), Projectile.knockBack, Projectile.owner, Projectile.ai[0], Projectile.ai[1] - 1);
			}
			if (lightUp)
				Lighting.AddLight(Projectile.Center, 245 * 0.00361f, 99 * 0.00361f, 66 * 0.00361f);
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Tile tile = Framing.GetTileSafely(Projectile.position + (Vector2.UnitY * Projectile.height));
			Tile above = Framing.GetTileSafely(Projectile.position);

			if (tile.HasTile && Main.tileSolid[tile.TileType] && (!above.HasTile || !Main.tileSolid[above.TileType])) //Below 1 block; move me
				Projectile.position.Y -= 16;
			else if (tile.HasTile && Main.tileSolid[tile.TileType] && above.HasTile && Main.tileSolid[above.TileType]) //Inside of blocks; kill me
				Projectile.active = false;

			if (oldVelocity.X != Projectile.velocity.X) //Stopped moving; kill me
				Projectile.active = false;

			Projectile.velocity.Y = 0;
			onGround = true;
			Projectile.friendly = true;
			if (Projectile.timeLeft > 26)
				Projectile.timeLeft = 26;
			return false;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
		{
			fallThrough = false;
			return true;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
			int frameHeight = tex.Height / Main.projFrames[Projectile.type];
			Rectangle frame = new Rectangle(0, frameHeight * Projectile.frame, tex.Width, frameHeight);
			if (onGround || Projectile.timeLeft > 26)
			{
				SpriteEffects effect = flipSprite ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
				Vector2 pos = (Projectile.Center - Main.screenPosition + new Vector2(0, Projectile.gfxOffY)) + new Vector2(0, (frameHeight / 2) + 12);
				Main.spriteBatch.Draw(tex, pos, frame, Color.White, Projectile.rotation, new Vector2(tex.Width - (Projectile.width / 2), frameHeight), Projectile.scale, effect, 0);
			}
			return false;
		}

		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
			if (Main.rand.Next(3) == 0)
				target.AddBuff(BuffID.OnFire, 180);
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (Projectile.penetrate == 1)
				return false;
			return null;
		}
	}

	public class RumExplosion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rum Explosion");
			Main.projFrames[Projectile.type] = 6;
		}

		public override void SetDefaults()
		{
			Projectile.width = 64;
			Projectile.height = 64;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
			Projectile.hostile = false;
			Projectile.friendly = true;
		}

		public override void AI()
		{
			Lighting.AddLight(Projectile.Center, 245 * 0.0061f, 99 * 0.0061f, 66 * 0.0061f);
			Projectile.frameCounter++;
			if (Projectile.frameCounter > 3)
			{
				Projectile.frameCounter = 0;
				Projectile.frame++;
				if (Projectile.frame >= Main.projFrames[Projectile.type])
					Projectile.Kill();
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D tex = TextureAssets.Projectile[Projectile.type].Value;
			int frameHeight = tex.Height / Main.projFrames[Projectile.type];
			Rectangle frame = new Rectangle(0, frameHeight * Projectile.frame, tex.Width, frameHeight);
			Main.spriteBatch.Draw(tex, Projectile.Center - Main.screenPosition, frame, Color.White, Projectile.rotation, new Vector2(tex.Width / 2, frameHeight / 2), Projectile.scale, SpriteEffects.None, 0);
			return false;
		}
	}
}
