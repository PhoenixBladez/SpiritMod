using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using SpiritMod.Prim;

namespace SpiritMod.Items.Sets.OlympiumSet.Thyrsus
{

	//TODO: Make it so you cant throw one if theres one active
	//TODO: Make it so right click destroys the currently alive one
	public class Thyrsus : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thyrsus");
			Tooltip.SetDefault("Throw at a surface to grow damaging vines");
		}

		public override void SetDefaults()
		{
			item.damage = 50;
			item.noMelee = true;
			item.rare = ItemRarityID.LightRed;
			item.width = 18;
			item.height = 18;
			item.useTime = 30;
			item.useAnimation = 30;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.useTime = item.useAnimation = 24;
			item.knockBack = 8;
			item.magic = true;
			item.noMelee = true;
			item.UseSound = SoundID.Item20;
			item.autoReuse = false;
			item.noUseGraphic = true;
			item.shoot = ModContent.ProjectileType<ThyrsusProj>();
			item.shootSpeed = 10f;
			item.value = Item.sellPrice(0, 2, 0, 0);
		}
	}
	public class ThyrsusProj : ModProjectile
	{
		bool stuck = false;
		float shrinkCounter = 0.25f;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Thyrsus");
			ProjectileID.Sets.TrailCacheLength[projectile.type] = 9;
			ProjectileID.Sets.TrailingMode[projectile.type] = 0;
		}
		public override void SetDefaults()
		{
			projectile.width = 8;
			projectile.height = 8;
			projectile.aiStyle = 1;
			projectile.friendly = false;
			projectile.minion = true;
			projectile.penetrate = 1;
			projectile.timeLeft = 600;
			aiType = ProjectileID.ThrowingKnife;
		}
		public override bool PreAI()
		{
			if (stuck)
			{
				projectile.velocity = Vector2.Zero;
				if (projectile.timeLeft < 40)
				{
					shrinkCounter += 0.1f;
					projectile.scale = 0.75f + (float)(Math.Sin(shrinkCounter));
					if (projectile.scale < 0.3f)
					{
						projectile.active = false;
					}
					if (projectile.scale > 1)
						projectile.scale = ((projectile.scale - 1) / 2f) + 1;
				}
				return false;
			}
			return true;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			//TODO: Playsound here
			if (!stuck)
			{
				stuck = true;
				projectile.tileCollide = false;
				projectile.velocity = Vector2.Zero;
				projectile.timeLeft = 100;
			}
			return false;
		}
		//public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
		//{
		//    Vector2 drawOrigin = new Vector2(Main.projectileTexture[projectile.type].Width * 0.5f, projectile.height * 0.5f);
		//    for (int k = 0; k < projectile.oldPos.Length; k++)
		//    {
		//        Vector2 drawPos = projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, projectile.gfxOffY);
		//        Color color = projectile.GetAlpha(lightColor) * ((float)(projectile.oldPos.Length - k) / (float)projectile.oldPos.Length);
		//        spriteBatch.Draw(Main.projectileTexture[projectile.type], drawPos, null, color, projectile.rotation, drawOrigin, projectile.scale, SpriteEffects.None, 0f);
		//    }
		//    return true;
		//}
	}
}