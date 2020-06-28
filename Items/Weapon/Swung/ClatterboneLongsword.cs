using SpiritMod.Buffs;
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Weapon.Swung
{
	public class ClatterboneLongsword : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Clatterbone Longsword");
			Tooltip.SetDefault("Attacks occasionally lowering enemy defense");
		}


		public override void SetDefaults()
		{
			item.damage = 20;
			item.melee = true;
			item.width = 34;
			item.height = 40;
			item.useTime = 31;
			item.useAnimation = 31;
			item.useStyle = ItemUseStyleID.SwingThrow;
			item.knockBack = 6;
			item.value = Terraria.Item.sellPrice(0, 1, 0, 0);
			item.rare = ItemRarityID.Green;
			item.UseSound = SoundID.Item1;
			item.autoReuse = true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<Carapace>(), 14);
			recipe.AddTile(TileID.Anvils);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			if(Main.rand.Next(6) == 0) {
				target.AddBuff(ModContent.BuffType<ClatterPierce>(), 180);
			}
		}
	}
}