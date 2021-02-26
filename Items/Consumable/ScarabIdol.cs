
using SpiritMod.NPCs.Boss.Scarabeus;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace SpiritMod.Items.Consumable
{
	public class ScarabIdol : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Scarab Idol");
			Tooltip.SetDefault("Use in the desert at daytime to summon Scarabeus");
		}


		public override void SetDefaults()
		{
			item.width = item.height = 16;
			item.rare = ItemRarityID.Green;
			item.maxStack = 99;

			item.useStyle = ItemUseStyleID.HoldingUp;
			item.useTime = item.useAnimation = 20;

			item.noMelee = true;
			item.consumable = false;
			item.autoReuse = false;

			item.UseSound = SoundID.Item43;
		}

		public override bool CanUseItem(Player player)
		{
			if (!NPC.AnyNPCs(ModContent.NPCType<Scarabeus>()) && player.ZoneDesert && Main.dayTime)
				return true;
			return false;
		}

		public override bool UseItem(Player player)
		{
			NPC.SpawnOnPlayer(player.whoAmI, ModContent.NPCType<Scarabeus>());
			Main.PlaySound(SoundID.Roar, player.position, 0);
			return true;
		}
		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ItemID.Sapphire, 1);
			recipe.AddIngredient(ItemID.AntlionMandible, 3);
            recipe.AddIngredient(ItemID.DesertFossil, 1);
            recipe.AddTile(TileID.DemonAltar);
			recipe.SetResult(this);
			recipe.AddRecipe();

			ModRecipe recipe2 = new ModRecipe(mod);
			recipe2.AddIngredient(ItemID.Topaz, 1);
			recipe2.AddIngredient(ItemID.AntlionMandible, 3);
            recipe.AddIngredient(ItemID.DesertFossil, 1);
            recipe2.AddTile(TileID.DemonAltar);
			recipe2.SetResult(this);
			recipe2.AddRecipe();

		}
	}
}
