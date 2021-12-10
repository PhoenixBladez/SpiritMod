using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Dusts;
using SpiritMod.Items.Material;

namespace SpiritMod.Items.Sets.MagicMisc.MagicDeck
{
	public class MagicDeck : ModItem
	{
		public override void SetStaticDefaults() => DisplayName.SetDefault("Magic Deck");

		public override void SetDefaults()
		{
			item.damage = 45;
			item.magic = true;
			item.mana = 9;
			item.width = 40;
			item.height = 40;
			item.useTime = 6;
			item.useAnimation = 6;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 2;
			item.useTurn = false;
			item.value = Item.sellPrice(0, 5, 0, 0);
			item.rare = ItemRarityID.LightRed;
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<MagicDeckProj>();
			item.shootSpeed = 15;
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 direction = new Vector2(speedX, speedY);
			direction = direction.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f));
			speedX = direction.X;
			speedY = direction.Y;
			position += direction;
			return true;
		}
	}

	public class MagicDeckProj : ModProjectile
	{
		private const int NUMBEROFXFRAMES = 4;

		private int xFrame = 0;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Magic Card");
			Main.projFrames[projectile.type] = 4;
		}

		public override void SetDefaults()
		{
			projectile.penetrate = 1;
			projectile.tileCollide = true;
			projectile.hostile = false;
			projectile.friendly = true;
			projectile.width = projectile.height = 14;
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 5;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
			projectile.frame = Main.rand.Next(4);
		}

		int counter;
		public override void AI()
		{
			projectile.rotation = projectile.velocity.ToRotation() + 1.57f;
			counter++;
			if (counter > 15)
				projectile.alpha += 25;
			if (projectile.alpha > 255)
				projectile.active = false;

			projectile.frameCounter++;
			if (projectile.frameCounter % 2 == 0)
				xFrame++;
			xFrame %= NUMBEROFXFRAMES;
		}
		public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		{
			Texture2D tex = Main.projectileTexture[projectile.type];
			int frameWidth = tex.Width / NUMBEROFXFRAMES;
			int frameHeight = tex.Height / Main.projFrames[projectile.type];
			Rectangle frame = new Rectangle(frameWidth * xFrame, frameHeight * projectile.frame, frameWidth, frameHeight);
			Vector2 origin = new Vector2(frameWidth / 2, frameHeight / 2);
			for (int k = projectile.oldPos.Length - 1; k > 0; k--)
			{
				Vector2 drawPos = projectile.oldPos[k] + (new Vector2(projectile.width, projectile.height) / 2);
				Color color = lightColor * (float)(((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length)) * (1 - (projectile.alpha / 255f));
				spriteBatch.Draw(tex, drawPos - Main.screenPosition, frame, color, projectile.rotation, origin, projectile.scale, SpriteEffects.None, 0f);
			}
			return false;
		}
	}
}