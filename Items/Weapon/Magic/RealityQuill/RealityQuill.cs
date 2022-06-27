using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;

namespace SpiritMod.Items.Weapon.Magic.RealityQuill
{
	public class RealityQuill : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Void Quill");
			Tooltip.SetDefault("Creates a tear in reality, damaging enemies \n Write faster to deal more damage\n'Write your own destiny'");
			SpiritGlowmask.AddGlowMask(Item.type, Texture + "_glow");
		}

		public override void SetDefaults()
		{
			Item.damage = 50;
			Item.knockBack = 0.1f;
			Item.DamageType = DamageClass.Magic;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.channel = true;
			Item.width = 40;
			Item.height = 60;
			Item.mana = 1;
			Item.noMelee = true;
			Item.autoReuse = false;
			Item.channel = true;
			Item.value = Item.sellPrice(0, 10, 0, 0);
			Item.rare = ItemRarityID.Red;
			Item.shoot = ModContent.ProjectileType<RealityQuillProjectileTwo>();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			Vector2 mouseDelta = Main.MouseWorld;
			Projectile.NewProjectile(Main.MouseWorld, Vector2.Zero, type, damage, knockback, player.whoAmI);
			return false;
		}

		public override void PostDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, float rotation, float scale, int whoAmI) => GlowmaskUtils.DrawItemGlowMaskWorld(spriteBatch, Item, ModContent.Request<Texture2D>(Texture + "_glow"), rotation, scale);

		public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ItemID.FragmentNebula, 18);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.Register();
        }

        /*public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback) 
		{
			Vector2 mouseDelta = Main.MouseWorld - lastSpawnPos;
			float mouseDeltaLength = mouseDelta.Length();
			if (Main.GlobalTime - lastTimeSpawned > 0.25f)
			{
				mouseDelta = Vector2.Zero;
				mouseDeltaLength = 1f;
			}
			mouseDelta.Normalize();

			for (int i = 0; i < mouseDeltaLength / 16f; i++)
			{
				Vector2 delta = mouseDelta * (float)Math.Sqrt(mouseDeltaLength) * 0.1f;
				delta = delta.RotatedBy(Main.rand.NextFloat(-0.2f, 0.2f));
				Projectile.NewProjectile(Main.MouseWorld - mouseDelta * i * 16f, delta, type, damage, knockback, player.whoAmI, Main.rand.NextFloat(0.125f, 0.5f));
			}
			lastTimeSpawned = Main.GlobalTime;

			lastSpawnPos = Main.MouseWorld;
			return false;
		}*/
    }
}
