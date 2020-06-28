using Microsoft.Xna.Framework;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Magic
{
	public class TalonPiercer : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Talon's Fury");
			Tooltip.SetDefault("Shoots feathers from off screen");
		}


		int charger;
		private Vector2 newVect;
		public override void SetDefaults()
		{
			item.damage = 25;
			item.magic = true;
			item.mana = 10;
			item.width = 46;
			item.height = 46;
			item.useTime = 16;
			item.useAnimation = 16;
			item.useStyle = ItemUseStyleID.HoldingOut;
			Item.staff[item.type] = true;
			item.noMelee = true;
			item.knockBack = 3.5f;
			item.useTurn = false;
			item.value = Terraria.Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item20;
			item.autoReuse = true;
			item.shoot = ModContent.ProjectileType<BoneFeatherFriendly>();
			item.shootSpeed = 14f;
		}

		public override bool Shoot(Player player, ref Microsoft.Xna.Framework.Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			position -= new Vector2(speedX, speedY) * 100;
			speedX += (Main.rand.Next(-3, 4) / 5f);
			speedY += (Main.rand.Next(-3, 4) / 5f);
			return true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Talon>(), 14);
			recipe.AddIngredient(ModContent.ItemType<FossilFeather>(), 1);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
