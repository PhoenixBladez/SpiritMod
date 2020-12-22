using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Bullet;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.Weapon.Gun
{
	public class OrionPistol : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Orion's Quickdraw");
			Tooltip.SetDefault("Right click to shoot an energy bomb \nShoot this bomb to overcharge Orion's Quickdraw\n'Historically accurate'");
			SpiritGlowmask.AddGlowMask(item.type, "SpiritMod/Items/Weapon/Gun/OrionPistol_Glow");

		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI)
		{
			Texture2D texture;
			texture = Main.itemTexture[item.type];
			spriteBatch.Draw
			(
				ModContent.GetTexture("SpiritMod/Items/Weapon/Gun/OrionPistol_Glow"),
				new Vector2
				(
					item.position.X - Main.screenPosition.X + item.width * 0.5f,
					item.position.Y - Main.screenPosition.Y + item.height - texture.Height * 0.5f + 2f
				),
				new Rectangle(0, 0, texture.Width, texture.Height),
				Color.White,
				rotation,
				texture.Size() * 0.5f,
				scale,
				SpriteEffects.None,
				0f
			);
		}
		public override void SetDefaults()
		{
			item.damage = 10;
			item.ranged = true;
			item.width = 24;
			item.height = 24;
			item.useTime = 9;
			item.useAnimation = 9;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.noMelee = true;
			item.knockBack = 0;
			item.useTurn = false;
			item.value = Terraria.Item.sellPrice(0, 2, 0, 0);
			item.rare = ItemRarityID.Orange;
			item.UseSound = SoundID.Item41;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<OrionOrb>();
			item.shootSpeed = 12f;
			item.useAmmo = AmmoID.Bullet;
		}
		public override void HoldItem(Player player)
		{
			if (cooldown > 0)
			{
				cooldown--;
				if (cooldown == 0)
				{
					Main.PlaySound(25, (int)player.position.X, (int)player.position.Y);
				}
			}
		}
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.altFunctionUse == 2)
            {
				type = ModContent.ProjectileType<OrionOrb>();
				Vector2 direction = new Vector2(speedX, speedY);
				direction = direction.RotatedBy(0 - (player.direction * 0.5f));
				int proj = Projectile.NewProjectile(position, direction, type, 0, knockBack, player.whoAmI);
				cooldown = 500;
				return false;
			}
			else
			{
				Vector2 direction = new Vector2(speedX, speedY);
				float shootRotation = Main.rand.NextFloat(-0.05f,0.05f);
				if (player.HasBuff(mod.BuffType("OverchargedOrion"))) 
				{
					shootRotation = Main.rand.NextFloat(-0.09f,0.09f);
					direction = direction.RotatedBy(shootRotation);
					Vector2 dustUnit = direction.RotatedBy(Main.rand.NextFloat(-1,1)) * 0.05f;
					Vector2 dustOffset = player.Center + (direction * 1.5f) + player.velocity;
					Dust dust = Dust.NewDustPerfect(dustOffset + (dustUnit * 30), 226);
					dust.velocity = Vector2.Zero - (dustUnit * 3);
					dust.noGravity = true;
				}
				else
				{
					direction = direction.RotatedBy(shootRotation);
				}
				player.itemRotation+= shootRotation;
				speedX = direction.X;
				speedY = direction.Y;
				Gore.NewGore(player.Center, new Vector2(player.direction * -1, -0.5f) * 4, mod.GetGoreSlot("Gores/BulletCasing"), 1f);
			}
			return base.Shoot(player,ref position,ref speedX,ref speedY,ref type,ref damage,ref knockBack);
		}
		int cooldown = 0;
		public override bool CanUseItem(Player player)
		{
			if (player.altFunctionUse == 2)
			{
				item.shootSpeed = 9f;
				item.useTime = 8;
				item.useAnimation = 8;
				item.autoReuse = false;
				if (cooldown > 0)
				{
					return false;
				}
			}
			else
			{
				item.shootSpeed = 12f;
				item.useTime = 8;
				item.useAnimation = 8;
				item.autoReuse = true;
				if (player.HasBuff(mod.BuffType("OverchargedOrion"))) 
				{
					item.useTime = 4;
					item.useAnimation = 4;
					item.shootSpeed = 18f;
				}
			}
			return base.CanUseItem(player);
		}
		public override Vector2? HoldoutOffset()
		{
			return new Vector2(-10, 0);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.FlintlockPistol, 1);
			recipe.AddIngredient(ModContent.ItemType<CosmiliteShard>(), 16);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}