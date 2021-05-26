using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic.RealityQuill
{
	public class RealityQuill : ModItem
	{
		Vector2 lastSpawnPos;
		float lastTimeSpawned;
		public override void SetDefaults()
		{
			item.damage = 100;
			item.knockBack = 0.1f;
			item.magic = true;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.useAnimation = 30;
			item.useTime = 30;
			item.channel = true;
			item.width = 40;
			item.height = 60;
			item.mana = 1;
			item.noMelee = true;
			item.autoReuse = false;
			item.channel = true;
			item.value = Item.sellPrice(0, 10, 0, 0);
			item.rare = ItemRarityID.Red;
			item.shoot = ModContent.ProjectileType<RealityQuillProjectileTwo>();
		}

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Void Quill");
			Tooltip.SetDefault("Creates a tear in reality, damaging enemies\n'Write your own destiny'");
		}

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			Vector2 mouseDelta = Main.MouseWorld;
			Projectile.NewProjectile(Main.MouseWorld, Vector2.Zero, type, damage, knockBack, player.whoAmI);
			return false;
		}

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ItemID.FragmentNebula, 18);
            recipe.AddTile(TileID.LunarCraftingStation);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }

        /*public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
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
				Projectile.NewProjectile(Main.MouseWorld - mouseDelta * i * 16f, delta, type, damage, knockBack, player.whoAmI, Main.rand.NextFloat(0.125f, 0.5f));
			}
			lastTimeSpawned = Main.GlobalTime;

			lastSpawnPos = Main.MouseWorld;
			return false;
		}*/
    }
}
