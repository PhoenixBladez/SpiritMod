using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
namespace SpiritMod.Items.BossLoot.AtlasDrops.PrimalstoneArmor
{
	[AutoloadEquip(EquipType.Head)]
	public class PrimalstoneFaceplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Primalstone Faceplate");
			Tooltip.SetDefault("20% increased melee and magic damage\nIncreases maximum mana by 60\nReduces damage taken by 8%");
		}

		public override void SetDefaults()
		{
			Item.width = 40;
			Item.height = 30;
			Item.value = Item.buyPrice(gold: 1);
			Item.rare = ItemRarityID.Cyan;
			Item.defense = 14;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs) => body.type == ModContent.ItemType<PrimalstoneBreastplate>() && legs.type == ModContent.ItemType<PrimalstoneLeggings>();

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Melee and magic attacks inflict Unstable Affliction\n10% reduced movement speed";
			player.GetSpiritPlayer().primalSet = true;
			player.moveSpeed -= 0.10F;
			int dust1 = Dust.NewDust(player.position, player.width, player.height - 38, DustID.UnusedWhiteBluePurple);
			Main.dust[dust1].scale = 2f;
		}

		public override void UpdateEquip(Player player)
		{
			player.endurance += 0.08f;
			player.statManaMax2 += 60;
			player.GetDamage(DamageClass.Melee) += .2f;
			player.GetDamage(DamageClass.Magic) += .2f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<ArcaneGeyser>(), 10);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}