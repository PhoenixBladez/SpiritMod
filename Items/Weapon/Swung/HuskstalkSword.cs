using SpiritMod.Buffs;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
	public class HuskstalkSword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Huskstalk Sword");
			Tooltip.SetDefault("Inflicts Withering Leaf");
		}


		public override void SetDefaults()
		{
			item.damage = 17;
			item.melee = true;
			item.width = 32;
			item.height = 32;
			item.useTime = 20;
			item.useAnimation = 20;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 4;
			item.rare = 1;
			item.UseSound = SoundID.Item1;
			item.autoReuse = false;
			item.value = Item.sellPrice(0, 0, 12, 0);
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<AncientBark>(), 8);
			recipe.AddIngredient(ModContent.ItemType<EnchantedLeaf>(), 4);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this, 1);
			recipe.AddRecipe();
		}
		public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
		{
			if(Main.rand.Next(2) == 0) {
				target.AddBuff(ModContent.BuffType<WitheringLeaf>(), 180);
			}
		}
	}
}