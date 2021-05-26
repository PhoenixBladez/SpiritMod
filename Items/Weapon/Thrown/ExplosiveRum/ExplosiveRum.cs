using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Thrown.ExplosiveRum
{ 
	public class ExplosiveRum : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Explosive Rum");
			Tooltip.SetDefault("'Oh, there it is'");
		}

		public override void SetDefaults()
		{
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.width = 24;
			item.height = 24;
			item.noUseGraphic = true;
			item.UseSound = SoundID.Item1;
			item.ranged = true;
			item.noMelee = true;
			item.consumable = true;
			item.maxStack = 999;
			item.shoot = ModContent.ProjectileType<ExplosiveRumProj>();
			item.useAnimation = 25;
			item.useTime = 25;
			item.shootSpeed = 10.5f;
			item.damage = 50;
			item.knockBack = 1.5f;
			item.value = Item.sellPrice(0, 0, 0, 20);
			item.crit = 8;
			item.rare = ItemRarityID.Blue;
			item.autoReuse = true;
			item.maxStack = 999;
			item.consumable = true;
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
			projectile.CloneDefaults(ProjectileID.Shuriken);
			projectile.width = 20;
			projectile.height = 20;
			projectile.ranged = true;
			projectile.penetrate = 1;
		}

		public override void AI()
		{
			Dust dust = Dust.NewDustPerfect(projectile.Center + (15 * ((projectile.rotation - 1.57f).ToRotationVector2())), 6);
			dust.noGravity = true;
			dust.scale = Main.rand.NextFloat(0.8f,1.15f);
			dust.fadeIn = 1.5f;
		}

		public override void Kill(int timeLeft)
		{
			Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Custom, "Sounds/rumboom"), projectile.Center);
			Main.PlaySound(SoundID.Item, (int)projectile.position.X, (int)projectile.position.Y, 27);

			for (int i = 0; i < 15; i++) 
				Dust.NewDust(new Vector2(projectile.position.X, projectile.position.Y), projectile.width, projectile.height, 13, projectile.velocity.X * 0.1f, projectile.velocity.Y * 0.1f, 0, default, 0.75f);

            for (int i = 1; i < 5; ++i)
			    Gore.NewGore(projectile.Center, Vector2.Zero, mod.GetGoreSlot("Gores/Rum/RumGore" + i), 1f);

			Projectile.NewProjectile(projectile.Center, Vector2.Zero, ModContent.ProjectileType<RumExplosion>(), projectile.damage, projectile.knockBack, projectile.owner);

            Projectile.NewProjectileDirect(projectile.Center - new Vector2(0, 15), new Vector2(0.25f, 15), ModContent.ProjectileType<RumFire>(), projectile.damage, projectile.knockBack, projectile.owner, 1, 12).timeLeft = 60;
            Projectile.NewProjectileDirect(projectile.Center - new Vector2(0, 15), new Vector2(-0.25f, 15), ModContent.ProjectileType<RumFire>(), projectile.damage, projectile.knockBack, projectile.owner, -1, 12).timeLeft = 60;
		}
	}

	public class RumFire : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Explosive Rum");
			Main.projFrames[projectile.type] = 5;
		}

		public override void SetDefaults()
        {
            projectile.hostile = false;
            projectile.width = 20;
            projectile.height = 34;
            projectile.aiStyle = -1;
            projectile.friendly = false;
            projectile.damage = 1;
            projectile.penetrate = 3;
            projectile.timeLeft = 26;
            projectile.tileCollide = true;
            projectile.ignoreWater = true;
            trueScale = new Vector2(Main.rand.NextFloat(1f, 1.25f), Main.rand.NextFloat(1f, 1.5f));
			flipSprite = Main.rand.NextBool();
        }

		bool onGround = false;
		bool lightUp = false;
		Vector2 trueScale;
		bool flipSprite = false;

		public override void AI()
		{
			if (Main.rand.Next(5) == 0 && onGround)
				Dust.NewDust(projectile.position, projectile.width, projectile.height, 6);
			//projectile.scale = (float)Math.Sin((12 - projectile.timeLeft) / 4f);
			
			if (++projectile.frameCounter > 4) {
				projectile.frameCounter = 0;
				projectile.frame++;
				if (projectile.frame >= Main.projFrames[projectile.type]) {
					projectile.Kill();
				}
			}

			if (lightUp || projectile.timeLeft > 26)
				Lighting.AddLight(projectile.Center, Color.Orange.R * 0.007f, Color.Orange.G * 0.007f, Color.Orange.B * 0.007f);

			if (projectile.timeLeft == 23 && onGround && projectile.ai[1] > 0)
			{
				lightUp = true;
                Vector2 pos = projectile.Center + new Vector2(projectile.ai[0] * 20, 0);
                Projectile.NewProjectile(pos, new Vector2(projectile.ai[0] * 0.25f, 15), ModContent.ProjectileType<RumFire>(), projectile.damage, projectile.knockBack, projectile.owner, projectile.ai[0], projectile.ai[1] - 1);
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Tile tile = Framing.GetTileSafely(projectile.position + (Vector2.UnitY * projectile.height));
            Tile above = Framing.GetTileSafely(projectile.position);

            if (tile.active() && Main.tileSolid[tile.type] && (!above.active() || !Main.tileSolid[above.type])) //Below 1 block; move me
                projectile.position.Y -= 16;
            else if (tile.active() && Main.tileSolid[tile.type] && above.active() && Main.tileSolid[above.type]) //Inside of blocks; kill me
                projectile.active = false;

            if (oldVelocity.X != projectile.velocity.X) //Stopped moving; kill me
				projectile.active = false;

			projectile.velocity.Y = 0;
			onGround = true;
			projectile.friendly = true;
			if (projectile.timeLeft > 26)
				projectile.timeLeft = 26;
			return false;
		}

		public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough)
        {
            fallThrough = false;
            return true;
        }

		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];
			int frameHeight = tex.Height / Main.projFrames[projectile.type];
			Rectangle frame = new Rectangle(0, frameHeight * projectile.frame, tex.Width, frameHeight);
			if (onGround || projectile.timeLeft > 26)
			{
				SpriteEffects effect = flipSprite ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
                Vector2 pos = (projectile.Center - Main.screenPosition + new Vector2(0, projectile.gfxOffY)) + new Vector2(0, (frameHeight / 2) + 6);
                spriteBatch.Draw(tex, pos, frame, lightColor, projectile.rotation, new Vector2(tex.Width - (projectile.width / 2), frameHeight), trueScale, effect, 0);
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
			if (projectile.penetrate == 1)
				return false;
			return base.CanHitNPC(target);
		}
    }

	public class RumExplosion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Rum Explosion");
			Main.projFrames[projectile.type] = 6;
		}

		public override void SetDefaults()
		{
			projectile.width = 64;
			projectile.height = 64;
			projectile.penetrate = -1;
			projectile.ignoreWater = true;
			projectile.tileCollide = false;
			projectile.hostile = false;
			projectile.friendly = true;
		}

		public override void AI()
		{
			Lighting.AddLight(projectile.Center, Color.Orange.R * 0.011f, Color.Orange.G * 0.011f, Color.Orange.B * 0.011f);
			projectile.frameCounter++;
			if (projectile.frameCounter > 3) {
				projectile.frameCounter = 0;
				projectile.frame++;
				if (projectile.frame >= Main.projFrames[projectile.type]) {
					projectile.Kill();
				}
			}
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];
			int frameHeight = tex.Height / Main.projFrames[projectile.type];
			Rectangle frame = new Rectangle(0, frameHeight * projectile.frame, tex.Width, frameHeight);
			spriteBatch.Draw(tex, projectile.Center - Main.screenPosition, frame, Color.White, projectile.rotation, new Vector2(tex.Width / 2, frameHeight / 2), projectile.scale, SpriteEffects.None, 0);
			return false;
		}
	}
}
