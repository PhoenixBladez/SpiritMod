
using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using SpiritMod.Items.Placeable.Furniture;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class Contraption : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Crazed Contraption");
			Tooltip.SetDefault("'What does it do? No one knows!'");
		}



		public override void SetDefaults()
		{
			item.damage = 120;
			Item.staff[item.type] = true;
			item.noMelee = true;
			item.magic = true;
			item.width = 28;
			item.height = 28;
			item.useTime = 9;
			item.mana = 10;
			item.useAnimation = 9;
			item.useStyle = ItemUseStyleID.HoldingOut;
			item.knockBack = 10;
			item.value = Item.buyPrice(gold: 20);
			item.rare = ItemRarityID.Cyan;
			item.UseSound = SoundID.Item92;
			item.autoReuse = true;
			item.shootSpeed = 15;
			item.UseSound = SoundID.Item20;
			item.shoot = ModContent.ProjectileType<Starshock1>();
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			int p = Main.rand.Next(1, 714);
			{
				if(!Main.projectile[p].minion) {
					int pl = Projectile.NewProjectile(position.X, position.Y, speedX, speedY, p, damage, knockBack, player.whoAmI, 0f, 0f);
					Main.projectile[pl].friendly = true;
					Main.projectile[pl].hostile = false;
				}
			}
			return false;
		}
		public override void AddRecipes()
		{

			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(ModContent.ItemType<SteamParts>(), 10);
			modRecipe.AddIngredient(ModContent.ItemType<TechDrive>(), 10);
			modRecipe.AddIngredient(ModContent.ItemType<PrintPrime>(), 1);
			modRecipe.AddIngredient(ModContent.ItemType<PrintProbe>(), 1);
			modRecipe.AddIngredient(ModContent.ItemType<BlueprintTwins>(), 1);
			modRecipe.AddIngredient(ModContent.ItemType<SpiritBar>(), 10);
			modRecipe.AddIngredient(ModContent.ItemType<StellarBar>(), 10);
			modRecipe.AddIngredient(ItemID.FragmentVortex, 2);
			modRecipe.AddIngredient(ItemID.FragmentNebula, 2);
			modRecipe.AddIngredient(ItemID.FragmentStardust, 2);
			modRecipe.AddIngredient(ItemID.FragmentSolar, 2);
			modRecipe.AddIngredient(ItemID.Cog, 25);
			modRecipe.AddIngredient(ItemID.Ectoplasm, 6);
			modRecipe.AddIngredient(ItemID.LihzahrdPowerCell, 2);
			modRecipe.AddTile(TileID.MythrilAnvil);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();
		}
	}
}
