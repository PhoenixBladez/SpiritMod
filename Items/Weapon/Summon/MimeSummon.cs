using Microsoft.Xna.Framework;
using SpiritMod.Items.Armor;
using SpiritMod.Items.Material;
using SpiritMod.Projectiles.Summon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Summon
{
	public class MimeSummon : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Two-Faced Mask");
			Tooltip.SetDefault("Summons either a Soul of Happiness or Sadness at the cursor position with a left or right click\nThe Soul of Happiness shoots out beams at foes\nThe Soul of Sadness shoots out homing tears at foes");
		}

		public override void SetDefaults()
		{
			item.damage = 17;
			item.summon = true;
			item.mana = 10;
			item.width = 44;
			item.height = 48;
			item.useTime = 35;
			item.useAnimation = 35;
			item.useStyle = ItemUseStyleID.HoldingOut;
			Item.staff[item.type] = true;
			item.noMelee = true;
			item.knockBack = 5;
			item.value = 20000;
			item.rare = ItemRarityID.Orange;
			item.UseSound = SoundID.Item20;
			item.autoReuse = false;
			item.shoot = ModContent.ProjectileType<HappySoul>();
			item.shootSpeed = 0f;
		}

		public override bool AltFunctionUse(Player player) => true;

		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.altFunctionUse == 2)
				type = ModContent.ProjectileType<SadSoul>();
			else
				type = ModContent.ProjectileType<HappySoul>();
			position = Main.MouseWorld;
			Projectile.NewProjectile(position.X, position.Y, speedX, speedY, type, damage, knockBack, player.whoAmI);
			player.UpdateMaxTurrets();
			return false;
		}

		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(ModContent.ItemType<MimeMask>(), 1);
			modRecipe.AddIngredient(ModContent.ItemType<BloodFire>(), 8);
			modRecipe.AddTile(TileID.Anvils);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();
		}
	}
}