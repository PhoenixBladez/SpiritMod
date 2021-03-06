
using SpiritMod.Items.Material;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.OverseerArmor
{
	[AutoloadEquip(EquipType.Head)]
	public class ShadowMagic : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Shadowspirit Headdress");
			Tooltip.SetDefault("27% increased magic and minion damage\n16% increased magic crit chance\nIncreases your max number of minions by 4\n25% increased movement speed\nIncreases max mana by 120\n7% reduced mana usage");
		}

		public override void SetDefaults()
		{
			item.width = 40;
			item.height = 30;
			item.value = 200000;
			item.rare = ItemRarityID.Purple;

			item.defense = 16;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<ShadowSBody>() && legs.type == ModContent.ItemType<ShadowLegs>();
		}
		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Magic hits spawn Soul Shards to chase down foes! \n 'You have become the Guardian' \n Double tap to dash repeatedly \n You are surrounded by protective Spirits, which deflect projectiles";
			player.GetSpiritPlayer().magicshadowSet = true;
			player.GetSpiritPlayer().shadowSet = true;

			if (Main.rand.Next(4) == 1) {

				Dust.NewDust(player.position, player.width, player.height, 187);
			}
		}
		public override void ArmorSetShadows(Player player)
		{
			player.armorEffectDrawShadow = true;
		}

		public override void UpdateEquip(Player player)
		{
			player.magicCrit += 16;
			player.minionDamage += 0.27f;
			player.maxMinions += 4;
			player.magicDamage += 0.27F;
			player.moveSpeed += 0.25F;
			player.manaCost -= 0.07f;
			player.statManaMax2 += 120;
		}

		public override void AddRecipes()
		{
			ModRecipe recipe = new ModRecipe(mod);
			recipe.AddIngredient(ModContent.ItemType<EternityEssence>(), 6);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}
	}
}
