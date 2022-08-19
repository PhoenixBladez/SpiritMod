using SpiritMod.Items.BossLoot.DuskingDrops;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.BossLoot.DuskingDrops.DuskArmor
{
	[AutoloadEquip(EquipType.Head)]
	public class DuskHood : ModItem
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Dusk Hood");
			Tooltip.SetDefault("Increases magic damage by 10% and reduces mana cost by 10%");
		}

		public override void SetDefaults()
		{
			Item.width = 40;
			Item.height = 30;
			Item.value = 70000;
			Item.rare = ItemRarityID.Pink;
			Item.defense = 12;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			return body.type == ModContent.ItemType<DuskPlate>() && legs.type == ModContent.ItemType<DuskLeggings>();
		}
		public override void UpdateArmorSet(Player player)
		{

			player.setBonus = "8% Increased Magic and Ranged Damage at Night\nYou are surrounded by a rune that guides the way\nMagic attacks inflict Shadowflame ";
			{
				player.GetSpiritPlayer().duskSet = true;
			}
		}

		public override void UpdateEquip(Player player)
		{
			player.manaCost -= 0.10f;
			player.GetDamage(DamageClass.Magic) += 0.10f;
		}
		public virtual void OnHitByNPC(NPC npc, int damage, bool crit)
		{
			if (Main.rand.NextBool(4)) {
				npc.AddBuff(BuffID.ShadowFlame, 200, true);
			}
		}
		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe(1);
			recipe.AddIngredient(ModContent.ItemType<DuskStone>(), 8);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}