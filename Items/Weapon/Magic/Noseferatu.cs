using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class Noseferatu : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Nosferatu");
			Tooltip.SetDefault("Summons a blood portal that heals the player for a large amount of health\n~Donator Item~");
		}


		public override void SetDefaults()
		{
			item.damage = 50;
			item.magic = true;
			item.mana = 50;
			item.width = 44;
			item.height = 44;
			item.useTime = 40;
			item.useAnimation = 40;
			item.useStyle = 5;
			Item.staff[item.type] = true;
			item.noMelee = true;
			item.knockBack = 6;
			item.value = Item.sellPrice(0, 0, 40, 0);
			item.rare = 5;
			item.UseSound = SoundID.Item8;
			item.autoReuse = true;
			item.shoot = mod.ProjectileType("NosPortal");
			item.shootSpeed = 0f;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			for (int i = 0; i < Main.projectile.Length; i++)
			{
				Projectile p = Main.projectile[i];
				if (p.active && p.type == item.shoot && p.owner == player.whoAmI)
				{
					p.active = false;
				}
			}
			//projectile spawns at mouse cursor
			Vector2 value18 = Main.screenPosition + new Vector2((float)Main.mouseX, (float)Main.mouseY);
			position = value18;
			return true;
		}
		public override void AddRecipes()
		{

			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(ItemID.SpellTome, 1);
			modRecipe.AddIngredient(ItemID.BrokenBatWing, 1);
			modRecipe.AddIngredient(ItemID.DarkShard, 1);
			modRecipe.AddIngredient(ItemID.LightShard, 1);
			modRecipe.AddIngredient(ItemID.SoulofNight, 5);
			modRecipe.AddIngredient(ItemID.SoulofLight, 5);
			modRecipe.AddTile(TileID.Bookcases);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();
		}
	}
}