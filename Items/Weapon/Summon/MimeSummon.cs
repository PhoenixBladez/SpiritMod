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
			Tooltip.SetDefault("Summons either a Soul of Happiness or Sadness at the cursor position with a left or right click\nThe Soul of Happiness shoots out beams at foes\nThe Soul of Sadness shoots out homing tears at foes\nOnly one of either soul can exist at once");
		}


		public override void SetDefaults()
		{
			item.damage = 21;
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
		public override bool AltFunctionUse(Player player)
		{
			return true;
		}
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			if (player.altFunctionUse == 2) {
				type = ModContent.ProjectileType<SadSoul>();
			}
			else {
				type = ModContent.ProjectileType<HappySoul>();
			}
			//remove any other owned SpiritBow projectiles, just like any other sentry minion
			for (int i = 0; i < Main.projectile.Length; i++) {
				Projectile p = Main.projectile[i];
				if (p.active && (p.type == item.shoot || p.type == ModContent.ProjectileType<SadSoul>()) && p.owner == player.whoAmI) {
					p.active = false;
				}
			}
			//projectile spawns at mouse cursor
			Vector2 value18 = Main.screenPosition + new Vector2(Main.mouseX, Main.mouseY);
			position = value18;
			return true;
		}
		public override void AddRecipes()
		{
			ModRecipe modRecipe = new ModRecipe(mod);
			modRecipe.AddIngredient(ModContent.ItemType<MimeMask>(), 1);
			modRecipe.AddIngredient(ModContent.ItemType<BloodFire>(), 5);
			modRecipe.AddTile(TileID.Anvils);
			modRecipe.SetResult(this, 1);
			modRecipe.AddRecipe();
		}
	}
}