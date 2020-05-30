using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace SpiritMod.Items.Armor.BloodCourt
{
    [AutoloadEquip(EquipType.Head)]
    public class BloodCourtHead : ModItem
    {
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Bloodcourt's Visage");
            Tooltip.SetDefault("Increases damage 4%\nIncreases your maximum number of minions by 1");

        }


        int timer = 0;
        public override void SetDefaults()
        {
            item.width = 22;
            item.height = 20;
            item.value = 3000;
            item.rare = 2;
            item.defense = 1;
        }
         public override void UpdateEquip(Player player)
        {
            player.GetSpiritPlayer().bloodCourtHead = true;
            player.magicDamage += 0.04f;
            player.meleeDamage += 0.04f;
            player.rangedDamage += 0.04f;
			player.minionDamage += 0.04f;
			player.maxMinions += 1;
        }
		public override void DrawHair(ref bool drawHair, ref bool drawAltHair)
        {
            drawHair = true;
        }
		public override void UpdateArmorSet(Player player)
        {
            player.GetSpiritPlayer().bloodcourtSet = true;
            player.setBonus = "Press the 'Armor Bonus' hotkey to sacrifice 8% of your maximum health\nand launch a bolt of Dark Anima dealing high damage in a radius\nThis bolt siphons 10 additional health over 5 seconds back to the attacker"; 
        }
        public override void ArmorSetShadows(Player player)
        {
            player.armorEffectDrawShadow = true;
        }
        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            return body.type == mod.ItemType("BloodCourtChestplate") && legs.type == mod.ItemType("BloodCourtLeggings");
        }
		 public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "BloodFire", 6);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
